using LinqToDB;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O24OpenAPI.Web.CMS.Models.ContextModels;
using O24OpenAPI.Web.CMS.Models.Digital;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using O24OpenAPI.Web.CMS.Services.Interfaces.Digital;

namespace O24OpenAPI.Web.CMS.Services.Services;

/// <summary>
/// The smart otp service class
/// </summary>
/// <seealso cref="ISmartOTPService"/>
public class SmartOTPService : ISmartOTPService
{
    /// <summary>
    /// The digital user
    /// </summary>
    private readonly IRepository<D_DIGITALBANKINGUSER> _digitalUser;

    /// <summary>
    /// The smart otp info
    /// </summary>
    private readonly IRepository<SmartOTPInfo> _smartOTPInfo;

    /// <summary>
    /// The web api settings
    /// </summary>
    private readonly WebApiSettings _setting = EngineContext.Current.Resolve<WebApiSettings>();

    /// <summary>
    /// The raise error
    /// </summary>
    private readonly IRaiseErrorService _raiseError;

    private readonly INotificationService _notificationService;

    /// <summary>
    ///
    /// </summary>
    public SmartOTPService(
        IRepository<D_DIGITALBANKINGUSER> digitalUser,
        IRepository<SmartOTPInfo> smartOtpInfo,
        INotificationService notificationService,
        IRaiseErrorService raiseError
    )
    {
        _digitalUser = digitalUser;
        _smartOTPInfo = smartOtpInfo;
        _raiseError = raiseError;
        _notificationService = notificationService;
    }

    /// <summary>
    ///
    /// </summary>
    public JWebUIObjectContextModel Context { get; set; } =
        EngineContext.Current.Resolve<JWebUIObjectContextModel>();

    /// <summary>
    ///
    /// </summary>
    /// <param name="workflow"></param>
    public async Task<JToken> RegisterOTP(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<RegisterOTPModel>();
        var user = await _digitalUser.GetByFields(
            new Dictionary<string, string>()
            {
                { nameof(D_DIGITALBANKINGUSER.UserCode), model.UserInfo.UserCode },
            }
        );
        if (string.IsNullOrEmpty(user.Email))
        {
            throw await _raiseError.RaiseErrorWithKeyResource("Digital.UserNotHaveEmail");
        }

        var decrypt = DigitalUtility.EncryptWithRSA(
            JsonConvert.SerializeObject(
                new RegisterOTPEncryptModel()
                {
                    UserCode = model.UserInfo.UserCode,
                    DeviceId = model.DeviceId,
                    Time = DateTime.UtcNow,
                }
            )
        );
        var qr = DigitalUtility.GenerateQrCodeBase64(decrypt);
        return new JObject() { { "qr", qr }, { "email", user.Email } };
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="workflow"></param>
    public async Task<JToken> ActiveOTP(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<ActiveOTPModel>();
        await ValidateActiveCode(model);
        await _smartOTPInfo
            .Table.Where(e =>
                e.UserCode == model.UserInfo.UserCode && e.Status == model.ActiveStatus.ToString()
            )
            .Set(e => e.Status, model.DeActiveStatus)
            .UpdateAsync();

        var privateKey = DigitalUtility.TotpExecute<string>(
            "generatePrivateKey",
            model.UserInfo.UserCode + model.DeviceId + DateTimeOffset.UtcNow.ToString()
        );
        var newSmartOtp = new SmartOTPInfo()
        {
            Status = model.ActiveStatus,
            UserCode = model.UserInfo.UserCode,
            PrivateKey = privateKey,
            DeviceId = model.DeviceId,
            PinCode = model.Pincode,
        };
        await _smartOTPInfo.Insert(newSmartOtp);
        return new JObject() { { "privatekey", privateKey } };
    }

    /// <summary>
    /// Validates the active code using the specified model
    /// </summary>
    /// <param name="model">The model</param>
    private async Task ValidateActiveCode(ActiveOTPModel model)
    {
        if (string.IsNullOrEmpty(model.Pincode) || model.Pincode.Length != _setting.PinCodeLength)
        {
            throw await _raiseError.RaiseErrorWithKeyResource("Digital.PinCodeNotValid");
        }

        var encode = "";
        if (!DigitalUtility.TryDecryptWithRSA(model.ActiveCode, ref encode))
        {
            throw await _raiseError.RaiseErrorWithKeyResource("Digital.ActiveCodeNotValid");
        }

        var parser = JsonConvert.DeserializeObject<RegisterOTPEncryptModel>(encode);
        if (parser?.UserCode != model.UserInfo.UserCode)
        {
            throw await _raiseError.RaiseErrorWithKeyResource("Digital.ActiveCodeNotValid");
        }

        if (parser.DeviceId != model.DeviceId)
        {
            throw await _raiseError.RaiseErrorWithKeyResource("Digital.ActiveCodeNotMatchDevice");
        }

        if ((DateTime.UtcNow.Date - parser.Time).TotalSeconds > _setting.TimeoutValidateOtp)
        {
            throw await _raiseError.RaiseErrorWithKeyResource("Digital.ActiveCodeExpire");
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    public async Task<JToken> VerifyOTP(VerifyOtpModel model)
    {
        var notification = await _notificationService.GetById(model.NotificationId);
        if (notification == null)
        {
            throw await _raiseError.RaiseErrorWithKeyResource("Digital.NotificationNotFound");
        }
        if (notification.IsProcessed)
        {
            throw await _raiseError.RaiseErrorWithKeyResource("Digital.OtpAlreadyProcessed");
        }

        var smartOtpInfo = await _smartOTPInfo.GetByFields(
            new Dictionary<string, string>()
            {
                { nameof(SmartOTPInfo.UserCode), model.UserInfo.UserCode },
                { nameof(SmartOTPInfo.DeviceId), model.DeviceId },
                { nameof(SmartOTPInfo.Status), model.ActiveStatus },
            }
        );

        var currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds() / 60;
        var genOtp = DigitalUtility.TotpExecute<string>(
            "generateOTPWithTransId",
            model.TransId,
            smartOtpInfo.PrivateKey,
            currentTime
        );
        if (model.Otp != genOtp)
        {
            throw await _raiseError.RaiseErrorWithKeyResource(
                "Digital.OtpNotValidWithTranId",
                model.TransId
            );
        }
        return new JObject() { { "isValid", true } };
    }
}

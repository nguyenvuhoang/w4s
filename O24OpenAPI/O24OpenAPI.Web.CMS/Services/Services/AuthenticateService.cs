using Newtonsoft.Json.Linq;
using O24OpenAPI.Core.Domain.Users;
using O24OpenAPI.Web.CMS.Constant;
using O24OpenAPI.Web.CMS.Models;
using O24OpenAPI.Web.CMS.Models.ContextModels;
using O24OpenAPI.Web.CMS.Models.Request;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using O24OpenAPI.Web.Framework.Services;

namespace O24OpenAPI.Web.CMS.Services.Services;

public class AuthenticateService(
    IUserSessionsService userSessionsService,
    IJwtTokenService jwtTokenService,
    IRepository<D_DIGITALBANKINGUSER> digitalUser,
    IRepository<S_USERPORTAL> userPortal,
    IRaiseErrorService raiseError
) : IAuthenticateService
{
    private readonly IJwtTokenService _jwtTokenService = jwtTokenService;
    private readonly IUserSessionsService _userSessionsService = userSessionsService;
    private readonly IRepository<D_DIGITALBANKINGUSER> _digitalUser = digitalUser;
    private readonly IRepository<S_USERPORTAL> _userPortal = userPortal;
    private readonly IRaiseErrorService _raiseError = raiseError;
    private readonly JWebUIObjectContextModel _context =
        EngineContext.Current.Resolve<JWebUIObjectContextModel>();

    /// <summary>
    ///
    /// </summary>
    /// <param name="workflow"></param>
    public async Task<JToken> AuthenJwt(AuthenJWTModel model, long transactionDate, string ip)
    {
        try
        {
            if (string.IsNullOrEmpty(model.DeviceId))
            {
                throw await ErrorUtils.Required(nameof(model.DeviceId));
            }
            var expireTime = GlobalVariable.ExpireTime();
            var token = _jwtTokenService.GetNewJwtToken(
                new User
                {
                    Username = model.UserName,
                    UserCode = model.UserCode,
                    BranchCode = model.BranchCode,
                    LoginName = model.LoginName,
                    DeviceId = model.DeviceId,
                },
                expireTime.ToUnixTimeSeconds()
            );
            var jsonResponse = new JObject { { "expireTime", expireTime }, { "token", token } };

            if (model.IsTemporaryToken)
            {
                return jsonResponse;
            }

            UserSessions userSessions = new()
            {
                Usrid = 0,
                UserCode = model.UserCode,
                Ssesionid = "",
                UsrBranch = model.BranchCode,
                UsrBranchid = 0,
                Mac = model.DeviceId,
                Usrname = model.UserName,
                Lang = _context.InfoUser.GetUserLogin().Lang,
                Txdt = DateTimeOffset.FromUnixTimeMilliseconds(transactionDate).DateTime,
                Exptime = expireTime.DateTime,
                Token = token,
                Acttype = "I",
                Wsip = ip,
                CommandList = "",
                ResetPassword = false,
                LoginName = model.LoginName,
                ApplicationCode = model.AppCode,
            };
            await Logout(true);

            await _userSessionsService.Insert(userSessions);

            return jsonResponse;
        }
        catch (Exception)
        {
            throw await _raiseError.RaiseErrorWithKeyResource(
                "Common.Value.UsernamePasswordIncorrect"
            );
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="workflow"></param>
    public async Task<JToken> DigitalHashPassword(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<AuthenJWTModel>();
        if (model.ChannelId.Equals("IB") || model.ChannelId.Equals("MB"))
        {
            var user = await _digitalUser.GetByFields(
                new Dictionary<string, string>()
                {
                    { nameof(D_DIGITALBANKINGUSER.UserName), model.UserName },
                }
            );
            if (user == null)
            {
                throw await _raiseError.RaiseErrorWithKeyResource(
                    "Digital.UserNotFound",
                    model.UserName
                );
            }

            var result = new JObject
            {
                { "password", O9Encrypt.sha_sha256(model.PassWord, user.UserCode) },
            };
            return result;
        }
        else
        {
            var user = await _userPortal.GetByFields(
                new Dictionary<string, string>()
                {
                    { nameof(S_USERPORTAL.UserName), model.UserName },
                }
            );
            if (user == null)
            {
                throw await _raiseError.RaiseErrorWithKeyResource(
                    "Digital.UserNotFound",
                    user.UserName
                );
            }

            var result = new JObject
            {
                { "password", O9Encrypt.sha_sha256(model.PassWord, user.UserCode) },
            };
            return result;
        }
    }

    public async Task<JToken> Logout(bool isLockoutOldSession = false)
    {
        var userSessions = SessionUtils.GetUserSession(_context);
        if (userSessions == null)
        {
            return null;
        }

        await _userSessionsService.UpdateActtype(
            userSessions.Token,
            SessionStatus.Logout,
            isLockoutOldSession
        );
        return new StatusResponse(TranStatus.COMPLETED).ToJToken();
    }
}

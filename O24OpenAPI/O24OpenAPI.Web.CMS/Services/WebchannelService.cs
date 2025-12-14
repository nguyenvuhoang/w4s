using System.Text;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Core.Logging.Helpers;
using O24OpenAPI.Core.Utils;
using O24OpenAPI.GrpcContracts.GrpcClientServices.CTH;
using O24OpenAPI.Web.CMS.Configuration;
using O24OpenAPI.Web.CMS.Models;
using O24OpenAPI.Web.CMS.Models.ContextModels;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using O24OpenAPI.Web.Framework.Extensions;
using O24OpenAPI.Web.Framework.Localization;
using O24OpenAPI.Web.Framework.Services;
using O24OpenAPI.Web.Framework.Utils;
using Constants = O24OpenAPI.Web.CMS.Utils.Constants;

namespace O24OpenAPI.Web.CMS.Services;

public partial class WebChannelService(
    IUserSessionsService userSessions,
    ILocalizationService localizationService,
    WebApiSettings webApiSettings,
    IPostService postService,
    IO24OpenAPIFileProvider fileProvider,
    JWebUIObjectContextModel context,
    CMSSetting cmsSetting,
    IJwtTokenService jwtTokenService
) : IWebChannelService
{
    #region Field injection
    private readonly WebApiSettings _webApiSettings = webApiSettings;
    private readonly IO24OpenAPIFileProvider _fileProvider = fileProvider;
    private readonly IUserSessionsService _userSessions = userSessions;
    private readonly ILocalizationService _localizationService = localizationService;
    private readonly JWebUIObjectContextModel _context = context;
    private readonly IPostService _postService = postService;
    private readonly CMSSetting _cmsSetting = cmsSetting;
    private readonly WorkContext _workContext = EngineContext.Current.Resolve<WorkContext>();
    private readonly IJwtTokenService _jwtTokenService = jwtTokenService;
    private bool NeedCheckSignature { get; set; } = true;
    private bool IsNeedCheckSession { get; set; } = true;
    private bool IsRefreshToken { get; set; } = false;
    #endregion

    #region sub methods
    private (bool isNeedCheckSession, bool isRefreshToken) CheckInput(List<BoRequest> boArrays)
    {
        const string keyLogin = "LOGIN";
        bool isNeedCheckSession = true;
        bool isRefreshToken = true;

        foreach (var e in boArrays)
        {
            e.Input.TryGetValue("workflowid", out var workflowId);
            var workflowStr = workflowId as string ?? string.Empty;

            e.Input.TryGetValue("learn_api", out var learnApi);
            var learnApiStr = learnApi as string ?? string.Empty;

            if (
                learnApiStr.Contains(keyLogin)
                || workflowStr.Contains(keyLogin)
                || _cmsSetting.ListWorkflowNotCheckSession.Contains(workflowStr)
            )
            {
                isNeedCheckSession = false;
            }

            if (!workflowStr.Contains("REFRESH_TOKEN"))
            {
                isRefreshToken = false;
            }

            if (_cmsSetting.ListWorkflowNotCheckSignature.Contains(workflowStr))
            {
                NeedCheckSignature = false;
            }

            if (!isNeedCheckSession && !isRefreshToken)
            {
                return (false, false);
            }
        }

        return (isNeedCheckSession, isRefreshToken);
    }
    #endregion

    public virtual async Task<ActionsResponseModel<object>> StartRequest(
        BoRequestModel request,
        HttpContext httpContext
    )
    {
        if (request.Bo == null)
        {
            return new ActionsResponseModel<object>();
        }

        (IsNeedCheckSession, IsRefreshToken) = CheckInput(request.Bo);

        var infoHeaderDictionary = Utils.Utils.GetHeaders(httpContext);
        RequestHeaderModel infoHeaderModel = new();
        if (infoHeaderDictionary is not null)
        {
            infoHeaderModel = new RequestHeaderModel()
            {
                Token = infoHeaderDictionary.GetValueOrDefault("uid"),
                Domain = infoHeaderDictionary.GetValueOrDefault("d"),
                Lang = infoHeaderDictionary.GetValueOrDefault("lang"),
                App = infoHeaderDictionary.GetValueOrDefault("app"),
                MyDevice = infoHeaderDictionary.GetValueOrDefault("my_device") is not null
                    ? infoHeaderDictionary
                        .GetValueOrDefault("my_device")
                        ?.MapToModel<Dictionary<string, object>>()
                    : [],
                Url = infoHeaderDictionary.GetValueOrDefault("u"),
                Signature = infoHeaderDictionary.GetValueOrDefault("signature"),
                Timestamp = infoHeaderDictionary.GetValueOrDefault("timestamp"),
                Nonce = infoHeaderDictionary.GetValueOrDefault("nonce"),
            };
        }
        _workContext.SetWorkingLanguage(infoHeaderModel.Lang);

        if (
            _cmsSetting.ListChannelCheckSignature != null
            && _cmsSetting.ListChannelCheckSignature.Count > 0
        )
        {
            NeedCheckSignature =
                NeedCheckSignature
                && _cmsSetting.ListChannelCheckSignature.Any(x =>
                    x.EqualsOrdinalIgnoreCase(infoHeaderModel.App)
                );
        }
        else
        {
            NeedCheckSignature = false;
        }

        if (NeedCheckSignature)
        {
            if (string.IsNullOrWhiteSpace(infoHeaderModel.Signature))
            {
                throw new Exception("Signature is null or empty.");
            }
            if (string.IsNullOrWhiteSpace(infoHeaderModel.Timestamp))
            {
                throw new Exception("Timestamp is null or empty.");
            }
            if (string.IsNullOrWhiteSpace(infoHeaderModel.Nonce))
            {
                throw new Exception("Nonce is null or empty.");
            }
        }
        return await RunBo(request, infoHeaderModel, httpContext);
    }

    public virtual async Task<ActionsResponseModel<object>> RunBo(
        BoRequestModel request_json,
        RequestHeaderModel infoHeader,
        HttpContext httpContext
    )
    {
        #region Validate token
        if (IsNeedCheckSession && !string.IsNullOrEmpty(infoHeader.Token))
        {
            var validateTokenResponse = _jwtTokenService.ValidateToken(infoHeader.Token);
            if (!validateTokenResponse.IsValid)
            {
                var error = await AddErrorSystem("CMS.string.invalidtoken", "401");
                return new ActionsResponseModel<object> { error = error };
            }
            infoHeader.UserId = validateTokenResponse.UserId;
            _workContext.UserContext.SetUserId(infoHeader.UserId);
        }
        #endregion

        List<BoRequest> BoArrays = request_json.Bo;
        ActionsResponseModel<object> result = new();
        try
        {
            string appPost = infoHeader.App;
            _workContext.SetCurrentChannel(appPost);
            var requestCookies = Utils.Utils.GetCookies(httpContext);
            var getSsidFromCookies = "";
            var sSIDStr = "device_id";
            if (requestCookies.TryGetValue(sSIDStr, out var value))
            {
                getSsidFromCookies = value;
            }
            var portalToken = "";
            _context.InfoUser.SetUserLogin(infoHeader);

            #region Check session
            var checkedSession = !IsNeedCheckSession;
            try
            {
                if (!string.IsNullOrEmpty(infoHeader.Token) && !checkedSession && !IsRefreshToken)
                {
                    var isValid = false;
                    UserSessions userSessions = null;
                    if (!request_json.Bo[0].UseMicroservice)
                    {
                        (isValid, userSessions) = await _userSessions.CheckValidSession(
                            infoHeader.Token
                        );
                        if (userSessions == null)
                        {
                            try
                            {
                                var cthSession = await EngineContext
                                    .Current.Resolve<ICTHGrpcClientService>()
                                    .GetUserSessionAsync(infoHeader.Token);
                                if (cthSession != null)
                                {
                                    userSessions = new UserSessions(cthSession);
                                    isValid = true;
                                }
                            }
                            catch { }
                        }
                    }
                    else
                    {
                        try
                        {
                            var cthSession = await EngineContext
                                .Current.Resolve<ICTHGrpcClientService>()
                                .GetUserSessionAsync(infoHeader.Token);
                            if (cthSession == null)
                            {
                                var error = await AddErrorSystem("Invalid Token", "400");
                                result.error.AddRange(error);
                                return result;
                            }
                            else
                            {
                                userSessions = new UserSessions(cthSession);
                                isValid = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            await ex.LogErrorAsync();
                            var error = await AddErrorSystem(
                                "Can not connect to Control Hub",
                                "401"
                            );
                            result.error.AddRange(error);
                            return result;
                        }
                    }

                    if (isValid)
                    {
                        if (
                            NeedCheckSignature
                            && !(
                                SignatureUtils.VerifySignature(
                                    requestObject: request_json.Bo[0],
                                    timestamp: infoHeader.Timestamp,
                                    signatureHex: infoHeader.Signature,
                                    nonceHex: infoHeader.Nonce,
                                    publicKeyHex: userSessions.SignatureKey,
                                    EngineContext.Current.Resolve<IMemoryCache>(),
                                    out var errorDetails
                                )
                            )
                        )
                        {
                            throw new Exception("Signature is invalid.");
                        }
                        var roles = System.Text.Json.JsonSerializer.Deserialize<string[]>(
                            userSessions.ChannelRoles
                        );
                        if (!roles.Contains(appPost))
                        {
                            throw new Exception(
                                $"The current session does not have the required role for this channel [{appPost}]."
                            );
                        }
                        _context.InfoUser.UserSession = userSessions;
                        _workContext.UserContext.SetLoginName(userSessions.LoginName);
                        _workContext.UserContext.SetUserName(userSessions.Usrname);
                        _workContext.UserContext.SetUserCode(userSessions.UserCode);
                        BoArrays.ForEach(e => e.Input["user_session"] = userSessions.ToJToken());
                        if (!string.IsNullOrEmpty(userSessions.Info))
                        {
                            var sessionInfo = JObject.Parse(userSessions.Info);
                            portalToken = sessionInfo["token"]?.ToString();

                            if (string.IsNullOrEmpty(portalToken))
                            {
                                Console.WriteLine("Invalid RunBo.portalToken");
                            }
                        }
                        checkedSession = true;
                    }
                    else
                    {
                        checkedSession = false;
                    }
                }
                else if (IsRefreshToken)
                {
                    checkedSession = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                var error = await AddErrorSystem(ex.Message, "500");
                result.error.AddRange(error);
                return result;
            }

            if (!checkedSession)
            {
                var error = await AddErrorSystem("CMS.string.invalidsession", "401");
                result.error.AddRange(error);
                return result;
            }
            #endregion

            //===============================================IP,OS,BROWSER===============================================
            _context.InfoRequest.SetIp(Utils.Utils.GetClientIPAddress(httpContext));
            _context.InfoRequest.SetClientOs(Utils.Utils.GetClientOs(httpContext));
            _context.InfoRequest.SetClientBrowser(Utils.Utils.GetClientBrowser(httpContext));
            _context.InfoRequest.SetRequestJson(request_json);
            _context.InfoRequest.DeviceID = StringUtils.Coalesce(
                getSsidFromCookies,
                infoHeader.MyDevice.TryGetValue("device_id", out var deviceId)
                    ? deviceId?.ToString()
                    : null,
                ""
            );
            _context.InfoRequest.DeviceType = StringUtils.Coalesce(
                getSsidFromCookies,
                infoHeader.MyDevice.TryGetValue("device_type", out var deviceType)
                    ? deviceType?.ToString()
                    : null,
                ""
            );

            _context.InfoRequest.OsVersion = infoHeader.MyDevice.TryGetValue(
                "os_version",
                out var osVersion
            )
                ? osVersion?.ToString()
                : "";

            _context.InfoRequest.AppVersion = infoHeader.MyDevice.TryGetValue(
                "app_version",
                out var appVersion
            )
                ? appVersion?.ToString()
                : "";

            _context.InfoRequest.DeviceName = infoHeader.MyDevice.TryGetValue(
                "device_name",
                out var deviceName
            )
                ? deviceName?.ToString()
                : "";

            _context.InfoRequest.Brand = infoHeader.MyDevice.TryGetValue("brand", out var brand)
                ? brand?.ToString()
                : "";

            _context.InfoRequest.IsEmulator =
                infoHeader.MyDevice.TryGetValue("is_emulator", out var isEmulator)
                && bool.TryParse(isEmulator?.ToString(), out var isEmuVal)
                    ? isEmuVal
                    : false;

            _context.InfoRequest.IsRootedOrJailbroken =
                infoHeader.MyDevice.TryGetValue("is_rooted_or_jailbroken", out var isRooted)
                && bool.TryParse(isRooted?.ToString(), out var isRootedVal)
                    ? isRootedVal
                    : false;

            _context.InfoRequest.UserAgent = infoHeader.MyDevice.TryGetValue(
                "user_agent",
                out var userAgent
            )
                ? userAgent?.ToString()
                : "";

            _context.InfoRequest.IpAddress = infoHeader.MyDevice.TryGetValue(
                "ip_address",
                out var ipAddress
            )
                ? ipAddress?.ToString()
                : "";

            _context.InfoRequest.Modelname = infoHeader.MyDevice.TryGetValue(
                "model_name",
                out var modelName
            )
                ? modelName?.ToString()
                : "";

            if (infoHeader.MyDevice != null)
            {
                infoHeader.MyDevice.TryGetValue("network_type", out var networkType);
                infoHeader.MyDevice.TryGetValue("network_status", out var networkStatus);

                _context.InfoRequest.Network =
                    $"{networkType ?? ""} ({networkStatus ?? ""})".Trim();
            }
            else
            {
                _context.InfoRequest.Network = string.Empty;
            }
            _context.InfoRequest.Memory = infoHeader.MyDevice.TryGetValue(
                "total_memory",
                out var totalmemory
            )
                ? totalmemory?.ToString()
                : "";

            _context.InfoRequest.PortalToken = portalToken;
            _context.InfoRequest.Language = infoHeader.Lang;
            _context.InfoApp.App = appPost;

            foreach (var bo_ in BoArrays)
            {
                await ProcessBoRequest(result, bo_);
            }

            Console.WriteLine("result.error ==" + result.error.ToSerialize());
            if (result.error != null)
            {
                if (result.error.Count > 0)
                {
                    result.fo.Add(CreateFoError(result.error));
                }
            }
        }
        catch (Exception ex)
        {
            var error = await AddErrorSystem(ex.Message, "500");
            result?.error?.AddRange(error);
            return result;
        }
        return result;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public JObject GetConfigClient()
    {
        var text = _fileProvider.ReadAllText("./App_Data/configOfDomain.json", Encoding.UTF8);

        if (text.Contains("system_host_port"))
        {
            text = text.Replace("system_host_port", _webApiSettings.Hostport);
        }

        var configOfDomain = JObject.Parse(text);
        var configDomainResult = configOfDomain
            .ToDictionary()
            .MergeDictionary(
                new ConfigOfDomainModel()
                {
                    rsa = _webApiSettings.ClientRsaPublicKey,
                    template_host_dev = _webApiSettings.TemplateHostDev,
                    server_version = Constants.ServerVersion,
                    server_name = Constants.ServerName,
                    isDev = _webApiSettings.Isdev,
                }.ToDictionary()
            );
        return configDomainResult.ToJObject();
    }

    private async Task<List<ErrorInfoModel>> AddErrorSystem(string keyError, string key = "")
    {
        try
        {
            var errorString = await _localizationService.GetResource(
                keyError,
                _context.InfoUser?.GetUserLogin()?.Lang ?? "en"
            );

            List<ErrorInfoModel> listError =
            [
                Utils.Utils.AddActionError(
                    ErrorType.errorSystem,
                    ErrorMainForm.danger,
                    errorString,
                    keyError,
                    key
                ),
            ];

            return listError;
        }
        catch (Exception ex)
        {
            Console.WriteLine("AddErrorSystem == " + ex.ToString());
            var Error_string = "Can't connect to database. Please check your database!";
            BusinessLogHelper.Error(ex, Error_string);
            List<ErrorInfoModel> listError =
            [
                Utils.Utils.AddActionError(
                    ErrorType.errorSystem,
                    ErrorMainForm.danger,
                    Error_string,
                    keyError,
                    "#ERROR_SYSTEM: "
                ),
            ];
            return listError;
        }
    }

    public async Task ProcessBoRequest(ActionsResponseModel<object> resultFo, BoRequest boRequest)
    {
        var workflowContext = EngineContext.Current.Resolve<WorkContext>();
        var executeId = workflowContext.ExecutionId;
        _context.Bo.SetBoInput(JObject.FromObject(boRequest.Input));
        _context.Bo.SetActionInput([]);

        string appCode = _context.InfoApp.GetApp();

        JObject data = boRequest.UseMicroservice
            ? (await _postService.ExecuteAsync()).Value<JObject>("data")
            : await _postService.GetDataPostAPI(appCode);

        if (data != null)
        {
            var fo = new List<FoResponseModel<object>>
            {
                new()
                {
                    txcode = boRequest.Input.GetStringValue("learn_api"),
                    input = [],
                    executeId = executeId,
                },
            };
            ActionsResponseModel<object> rsFo = new() { fo = fo };

            List<FoResponseModel<object>> foArray = CreateFo(rsFo.fo, data);
            if (foArray != null)
            {
                resultFo.fo.AddRange(foArray);
            }

            if (
                data.TryGetValue("error_message", out var errorMessage)
                && !string.IsNullOrWhiteSpace(errorMessage?.ToString())
            )
            {
                resultFo.error.Add(
                    Utils.Utils.AddActionError(
                        ErrorType.errorSystem,
                        ErrorMainForm.danger,
                        errorMessage.ToString(),
                        data["error_code"]?.ToString() ?? "UNKNOWN_ERROR",
                        "ERROR: ",
                        nextAction: data["next_action"]?.ToString() ?? "",
                        executeId: executeId
                    )
                );
            }
        }

        if (_context.Bo.GetActionErrors() != null)
        {
            resultFo.error.AddRange(_context.Bo.GetActionErrors());
        }
    }

    private static List<FoResponseModel<object>> CreateFo(
        List<FoResponseModel<object>> arrayFo,
        JObject actionInput
    )
    {
        List<FoResponseModel<object>> rs = [];
        foreach (FoResponseModel<object> obj in arrayFo)
        {
            FoResponseModel<object> obRs = new();
            if (!string.IsNullOrWhiteSpace(obj.txcode))
            {
                obRs.txcode = obj.txcode.Trim();

                if (obj.input != null)
                {
                    Dictionary<string, object> obJoin = new();
                    foreach (var itemObj in obj.input)
                    {
                        obJoin.Add(itemObj.Key, itemObj.Value);
                    }

                    foreach (var itemActionInput in actionInput)
                    {
                        if (obJoin.ContainsKey(itemActionInput.Key))
                        {
                            obJoin[itemActionInput.Key] = itemActionInput.Value;
                        }
                        else
                        {
                            obJoin.Add(itemActionInput.Key, itemActionInput.Value);
                        }
                    }

                    obRs.executeId = obj.executeId;
                    obRs.input = obJoin;
                    rs.Add(obRs);
                }
                else if (!string.IsNullOrEmpty(obRs.txcode))
                {
                    obRs.input = actionInput.ToDictionary();

                    rs.Add(obRs);
                }
            }
            else
            {
                BusinessLogHelper.Warning(
                    "Transaction code [TXCODE] is not null. Please check learnapi or check the input data."
                );
            }
        }

        return rs;
    }

    private static FoResponseModel<object> CreateFoError(List<ErrorInfoModel> arr)
    {
        FoResponseModel<object> rs_ = new() { txcode = "#sys:fo-sys-showError" };
        Dictionary<string, object> input = new() { { "infoError", arr } };
        rs_.input = input;
        return rs_;
    }
}

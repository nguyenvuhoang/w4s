using System.Text;
using System.Text.Json.Serialization;
using LinKit.Json.Runtime;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O24OpenAPI.APIContracts.Models.CTH;
using O24OpenAPI.CMS.API.Application.Models.ContextModels;
using O24OpenAPI.CMS.API.Application.Models.Request;
using O24OpenAPI.CMS.API.Application.Models.Response;
using O24OpenAPI.CMS.API.Application.Services.Interfaces;
using O24OpenAPI.CMS.API.Application.Utils;
using O24OpenAPI.CMS.Domain.AggregateModels.LearnApiAggregate;
using O24OpenAPI.CMS.Infrastructure.Configurations;
using O24OpenAPI.Core.Configuration;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Core.Utils;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Localization;
using O24OpenAPI.Framework.Models.JwtModels;
using O24OpenAPI.Framework.Services;
using O24OpenAPI.Framework.Services.Mapping;
using O24OpenAPI.Framework.Utils;
using O24OpenAPI.GrpcContracts.GrpcClientServices.CTH;
using O24OpenAPI.GrpcContracts.GrpcClientServices.WFO;
using O24OpenAPI.Logging.Helpers;

namespace O24OpenAPI.CMS.API.Application.Features.Requests;

#region Request Models
public class BoRequestModel : BaseO24OpenAPIModel
{
    public BoRequestModel() { }

    [JsonPropertyName("bo")]
    public List<BoRequest> Bo { get; set; } = [];
}

public class BoRequest : BaseO24OpenAPIModel
{
    [JsonPropertyName("input")]
    public Dictionary<string, object> Input { get; set; } = [];
}
#endregion

// [RegisterService(Lifetime.Scoped)]
public class RequestHandler(
    ILocalizationService localizationService,
    WebApiSettings webApiSettings,
    IO24OpenAPIFileProvider fileProvider,
    JWebUIObjectContextModel context,
    CMSSetting cmsSetting,
    IJwtTokenService jwtTokenService,
    WorkContext workContext,
    ICTHGrpcClientService cthGrpcClientService,
    ILearnApiRepository learnApiRepository,
    IDataMappingService dataMappingService
) : IRequestHandler
{
    private readonly WebApiSettings _webApiSettings = webApiSettings;
    private readonly IO24OpenAPIFileProvider _fileProvider = fileProvider;
    private readonly ILocalizationService _localizationService = localizationService;
    private readonly CMSSetting _cmsSetting = cmsSetting;
    private readonly WorkContext _workContext = workContext;
    private readonly IJwtTokenService _jwtTokenService = jwtTokenService;
    private readonly ICTHGrpcClientService _cthGrpcClientService = cthGrpcClientService;

    private bool NeedCheckSignature { get; set; } = true;
    private bool IsNeedCheckSession { get; set; } = true;
    private bool IsRefreshToken { get; set; } = false;

    public async Task<ActionsResponseModel<object>> HandleAsync(
        BoRequestModel request,
        HttpContext httpContext
    )
    {
        if (request.Bo == null)
        {
            return new ActionsResponseModel<object>();
        }

        (IsNeedCheckSession, IsRefreshToken) = CheckInput(request.Bo);

        Dictionary<string, string>? infoHeaderDictionary = httpContext.GetHeaders();
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
                        ?.MapToModel<Dictionary<string, object>>() ?? []
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
                    x.EqualsOrdinalIgnoreCase(infoHeaderModel?.App)
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
        CTHUserSessionModel? currentSession = null;
        #region Validate token
        if (IsNeedCheckSession && !string.IsNullOrEmpty(infoHeader.Token))
        {
            ValidateTokenResponseModel validateTokenResponse = _jwtTokenService.ValidateToken(
                infoHeader.Token
            );
            if (!validateTokenResponse.IsValid)
            {
                List<ErrorInfoModel> error = await AddErrorSystem("CMS.string.invalidtoken", "401");
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
            string? appPost = infoHeader.App;
            _workContext.SetCurrentChannel(appPost);
            Dictionary<string, string> requestCookies = httpContext.GetCookies();
            string getSsidFromCookies = "";
            string sSIDStr = "device_id";
            if (requestCookies.TryGetValue(sSIDStr, out string? value))
            {
                getSsidFromCookies = value;
            }
            string portalToken = "";
            context.InfoUser.SetUserLogin(infoHeader);

            #region Check session
            bool checkedSession = !IsNeedCheckSession;
            try
            {
                if (!string.IsNullOrEmpty(infoHeader.Token) && !checkedSession && !IsRefreshToken)
                {
                    bool isValid = false;
                    try
                    {
                        currentSession = await _cthGrpcClientService.GetUserSessionAsync(
                            infoHeader.Token
                        );
                        if (currentSession == null)
                        {
                            List<ErrorInfoModel> error = await AddErrorSystem(
                                "Invalid Token",
                                "400"
                            );
                            result.error.AddRange(error);
                            return result;
                        }
                        isValid = true;
                    }
                    catch (Exception ex)
                    {
                        await ex.LogErrorAsync();
                        List<ErrorInfoModel> error = await AddErrorSystem(
                            "Can not connect to Control Hub",
                            "401"
                        );
                        result.error.AddRange(error);
                        return result;
                    }

                    if (isValid)
                    {
                        if (
                            NeedCheckSignature
                            && !SignatureUtils.VerifySignature(
                                requestObject: request_json.Bo[0],
                                timestamp: infoHeader.Timestamp,
                                signatureHex: infoHeader.Signature,
                                nonceHex: infoHeader.Nonce,
                                publicKeyHex: currentSession.SignatureKey,
                                EngineContext.Current.Resolve<IMemoryCache>(),
                                out string errorDetails
                            )
                        )
                        {
                            throw new Exception("Signature is invalid.");
                        }
                        string[]? roles = currentSession.ChannelRoles.FromJson<string[]>();
                        if (!roles.Contains(appPost))
                        {
                            throw new Exception(
                                $"The current session does not have the required role for this channel [{appPost}]."
                            );
                        }
                        context.InfoUser.UserSession = currentSession;
                        _workContext.UserContext.SetLoginName(currentSession.LoginName);
                        _workContext.UserContext.SetUserName(currentSession.UserName);
                        _workContext.UserContext.SetUserCode(currentSession.UserCode);
                        BoArrays.ForEach(e => e.Input["user_session"] = currentSession.ToJToken());
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
                List<ErrorInfoModel> error = await AddErrorSystem(ex.Message, "500");
                result.error.AddRange(error);
                return result;
            }

            if (!checkedSession)
            {
                List<ErrorInfoModel> error = await AddErrorSystem(
                    "CMS.string.invalidsession",
                    "401"
                );
                result.error.AddRange(error);
                return result;
            }
            #endregion

            //===============================================IP,OS,BROWSER===============================================
            context.InfoRequest.SetIp(httpContext.GetClientIPAddress());
            context.InfoRequest.SetClientOs(httpContext.GetClientOs());
            context.InfoRequest.SetClientBrowser(httpContext.GetClientBrowser());
            context.InfoRequest.SetRequestJson(request_json);
            context.InfoRequest.DeviceID = StringUtils.Coalesce(
                getSsidFromCookies,
                infoHeader.MyDevice.TryGetValue("device_id", out object? deviceId)
                    ? deviceId?.ToString()
                    : null,
                ""
            );
            context.InfoRequest.DeviceType = StringUtils.Coalesce(
                getSsidFromCookies,
                infoHeader.MyDevice.TryGetValue("device_type", out object? deviceType)
                    ? deviceType?.ToString()
                    : null,
                ""
            );

            context.InfoRequest.OsVersion = infoHeader.MyDevice.TryGetValue(
                "os_version",
                out object? osVersion
            )
                ? osVersion?.ToString()
                : "";

            context.InfoRequest.AppVersion = infoHeader.MyDevice.TryGetValue(
                "app_version",
                out object? appVersion
            )
                ? appVersion?.ToString()
                : "";

            context.InfoRequest.DeviceName = infoHeader.MyDevice.TryGetValue(
                "device_name",
                out object? deviceName
            )
                ? deviceName?.ToString()
                : "";

            context.InfoRequest.Brand = infoHeader.MyDevice.TryGetValue("brand", out object? brand)
                ? brand?.ToString()
                : "";

            context.InfoRequest.IsEmulator =
                infoHeader.MyDevice.TryGetValue("is_emulator", out object? isEmulator)
                && bool.TryParse(isEmulator?.ToString(), out bool isEmuVal)
                    ? isEmuVal
                    : false;

            context.InfoRequest.IsRootedOrJailbroken =
                infoHeader.MyDevice.TryGetValue("is_rooted_or_jailbroken", out object? isRooted)
                && bool.TryParse(isRooted?.ToString(), out bool isRootedVal)
                    ? isRootedVal
                    : false;

            context.InfoRequest.UserAgent = infoHeader.MyDevice.TryGetValue(
                "user_agent",
                out object? userAgent
            )
                ? userAgent?.ToString()
                : "";

            context.InfoRequest.IpAddress = infoHeader.MyDevice.TryGetValue(
                "ip_address",
                out object? ipAddress
            )
                ? ipAddress?.ToString()
                : "";

            context.InfoRequest.Modelname = infoHeader.MyDevice.TryGetValue(
                "model_name",
                out object? modelName
            )
                ? modelName?.ToString()
                : "";

            if (infoHeader.MyDevice != null)
            {
                infoHeader.MyDevice.TryGetValue("network_type", out object? networkType);
                infoHeader.MyDevice.TryGetValue("network_status", out object? networkStatus);

                context.InfoRequest.Network = $"{networkType ?? ""} ({networkStatus ?? ""})".Trim();
            }
            else
            {
                context.InfoRequest.Network = string.Empty;
            }
            context.InfoRequest.Memory = infoHeader.MyDevice.TryGetValue(
                "total_memory",
                out object? totalmemory
            )
                ? totalmemory?.ToString()
                : "";

            context.InfoRequest.PortalToken = portalToken;
            context.InfoRequest.Language = infoHeader.Lang;
            context.InfoApp.App = appPost;

            foreach (BoRequest bo_ in BoArrays)
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
            List<ErrorInfoModel> error = await AddErrorSystem(ex.Message, "500");
            result?.error?.AddRange(error);
            return result;
        }
        return result;
    }

    private async Task<List<ErrorInfoModel>> AddErrorSystem(string keyError, string key = "")
    {
        try
        {
            string errorString = await _localizationService.GetResource(
                keyError,
                context.InfoUser?.GetUserLogin()?.Lang ?? "en"
            );

            List<ErrorInfoModel> listError =
            [
                new ErrorInfoModel(
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
            string Error_string = "Can't connect to database. Please check your database!";
            BusinessLogHelper.Error(ex, Error_string);
            List<ErrorInfoModel> listError =
            [
                new ErrorInfoModel(
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

    private async Task ProcessBoRequest(ActionsResponseModel<object> resultFo, BoRequest boRequest)
    {
        WorkContext? workflowContext = EngineContext.Current.Resolve<WorkContext>();
        string executeId = workflowContext.ExecutionId;
        context.Bo.SetBoInput(JObject.FromObject(boRequest.Input));
        context.Bo.SetActionInput([]);

        string appCode = context.InfoApp.GetApp();

        JObject? data = (await ExecuteAsync()).Value<JObject>("data");

        if (data != null)
        {
            List<FoResponseModel<object>> fo =
            [
                new()
                {
                    txcode = boRequest.Input.GetStringValue("learn_api"),
                    input = [],
                    executeId = executeId,
                },
            ];
            ActionsResponseModel<object> rsFo = new() { fo = fo };

            List<FoResponseModel<object>> foArray = CreateFo(rsFo.fo, data);
            if (foArray != null)
            {
                resultFo.fo.AddRange(foArray);
            }

            if (
                data.TryGetValue("error_message", out JToken? errorMessage)
                && !string.IsNullOrWhiteSpace(errorMessage?.ToString())
            )
            {
                resultFo.error.Add(
                    new ErrorInfoModel(
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

        if (context.Bo.GetActionErrors() != null)
        {
            resultFo.error.AddRange(context.Bo.GetActionErrors());
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
                    Dictionary<string, object> obJoin = [];
                    foreach (KeyValuePair<string, object> itemObj in obj.input)
                    {
                        obJoin.Add(itemObj.Key, itemObj.Value);
                    }

                    foreach (KeyValuePair<string, JToken?> itemActionInput in actionInput)
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

    private (bool isNeedCheckSession, bool isRefreshToken) CheckInput(List<BoRequest> boArrays)
    {
        const string keyLogin = "LOGIN";
        bool isNeedCheckSession = true;
        bool isRefreshToken = true;

        foreach (BoRequest e in boArrays)
        {
            e.Input.TryGetValue("workflowid", out object? workflowId);
            string workflowStr = workflowId as string ?? string.Empty;

            e.Input.TryGetValue("learn_api", out object? learnApi);
            string learnApiStr = learnApi as string ?? string.Empty;

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

    private async Task<JObject> ExecuteAsync()
    {
        try
        {
            JObject boInput = context.Bo.GetBoInput();
            if (boInput == null)
            {
                throw new ArgumentNullException(
                    nameof(boInput),
                    "BoInput cannot be null or empty!"
                );
            }
            string response;
            if (
                !boInput.TryGetValue("workflowid", out JToken? workflowIdToken)
                || string.IsNullOrEmpty(workflowIdToken.ToString())
            )
            {
                string? learnApiId = boInput["learn_api"]?.ToString();
                if (string.IsNullOrEmpty(learnApiId))
                {
                    throw new ArgumentNullException(
                        nameof(learnApiId),
                        "LearnApi cannot be null or empty!"
                    );
                }
                LearnApi learnApi = await learnApiRepository.GetByChannelAndLearnApiIdAsync(
                    context.InfoApp.GetApp(),
                    learnApiId
                );
                JObject mappedRequest = BuildContentWorkflowRequest();
                if (learnApi.LearnApiMapping.HasValue())
                {
                    mappedRequest = await dataMappingService.MapDataAsync(boInput, mappedRequest);
                }
                if (learnApi.URI.StartsWith("$"))
                {
                    string uri = learnApi.URI;
                    int firstKey = uri.IndexOf('$');
                    int secondKey = uri.IndexOf('$', firstKey + 1);

                    if (firstKey != -1 && secondKey != -1 && secondKey > firstKey)
                    {
                        string settingKey = uri.Substring(firstKey + 1, secondKey - firstKey - 1);
                        string remainingPath = uri.Substring(secondKey + 1);
                        if (settingKey.Contains("WFO"))
                        {
                            string wfoURI = Singleton<O24OpenAPIConfiguration>.Instance.WFOHttpURL;
                            learnApi.URI = wfoURI + remainingPath;
                        }
                        else
                        {
                            ICMSSettingService? settingService =
                                EngineContext.Current.Resolve<ICMSSettingService>();
                            string setting = await settingService.GetStringValue(settingKey);
                            learnApi.URI = setting + remainingPath;
                        }
                    }
                }
                response = await CallAPIAsync(
                    path: learnApi.URI,
                    method: learnApi.LearnApiMethod,
                    content: mappedRequest,
                    headerToken: context.InfoUser.GetUserLogin().Token
                );
            }
            else
            {
                JObject obContent = BuildContentWorkflowRequest();
                IWFOGrpcClientService? wfoGrpcClientService =
                    EngineContext.Current.Resolve<IWFOGrpcClientService>();
                response = await wfoGrpcClientService.ExecuteWorkflowAsync(
                    JsonConvert.SerializeObject(obContent)
                );
            }

            JObject result = JObject.Parse(response);
            if (result.TryGetValue("error_message", out JToken? messageToken))
            {
                if (!string.IsNullOrEmpty(messageToken?.ToString()))
                {
                    result["data"] = new JObject
                    {
                        { "error_message", messageToken },
                        { "next_action", result["error_next_action"] },
                        { "error_code", result["error_code"] },
                    };
                }
            }
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
            throw;
        }
    }

    private JObject BuildContentWorkflowRequest()
    {
        JObject sourceInput = context.Bo.GetBoInput();
        JObject wfInput = new(sourceInput)
        {
            ["lang"] = context.InfoRequest.Language,
            ["token"] = context.InfoUser.GetUserLogin().Token,
            ["user_id"] = context.InfoUser.UserSession?.UserId.ToString() ?? "0",
            ["user_code"] = context.InfoUser.UserSession?.UserCode.ToString() ?? "0",
            ["execution_id"] = _workContext?.ExecutionId ?? Guid.NewGuid().ToString(),
            ["channel_id"] = context.InfoApp.GetApp(),
            ["transaction_date"] = DateTime.UtcNow,
            //["value_date"] = context.InfoUser.UserSession? ?? DateTime.UtcNow,
            ["device"] = JToken.FromObject(
                new DeviceModel
                {
                    IpAddress = context.InfoRequest.GetIp() ?? "::1",
                    DeviceId = string.IsNullOrWhiteSpace(context.InfoRequest.DeviceID)
                        ? Guid.NewGuid().ToString()
                        : context.InfoRequest.DeviceID,
                    OsVersion =
                        context.InfoRequest.OsVersion
                        ?? context.InfoRequest.GetClientOs()
                        ?? "unknown",
                    UserAgent =
                        context.InfoRequest.UserAgent
                        ?? context.InfoRequest.GetClientBrowser()
                        ?? string.Empty,
                    DeviceType = context.InfoRequest.DeviceType ?? "unknown",
                    AppVersion = context.InfoRequest.AppVersion ?? "1.0.0",
                    DeviceName = context.InfoRequest.DeviceName ?? "Generic Device",
                    Brand = context.InfoRequest.Brand ?? "Unknown",
                    IsEmulator = context.InfoRequest.IsEmulator,
                    IsRootedOrJailbroken = context.InfoRequest.IsRootedOrJailbroken,
                    Memory = context.InfoRequest.Memory,
                    Network = context.InfoRequest.Network,
                }
            ),
        }; // Create a new copy of the input
        return wfInput;
    }

    private static async Task<string> CallAPIAsync(
        string path,
        string method,
        object content,
        string headerToken = ""
    )
    {
        HttpClientHandler clientHandler = new()
        {
            ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
            {
                return true;
            },
        };

        HttpClient httpClient = new(clientHandler);
        httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
        if (!string.IsNullOrEmpty(headerToken))
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + headerToken);
        }

        string requestString = JsonConvert.SerializeObject(content);
        StringContent requestContent = new(requestString, Encoding.Default, "application/json");
        HttpRequestMessage requestMessage = new(new HttpMethod(method), path)
        {
            Content = requestContent,
        };
        HttpResponseMessage httpResponse = await httpClient.SendAsync(requestMessage);
        httpResponse.EnsureSuccessStatusCode();
        string result = await httpResponse.Content.ReadAsStringAsync();

        return result;
    }

    public Task<ResponseModel> ProcessAsync(RequestModel request, HttpContext httpContext)
    {
        throw new NotImplementedException();
    }
}

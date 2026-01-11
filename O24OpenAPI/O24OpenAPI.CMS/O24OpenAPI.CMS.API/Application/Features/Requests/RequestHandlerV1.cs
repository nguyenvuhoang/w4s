using LinKit.Core.Abstractions;
using LinKit.Core.Cqrs;
using LinKit.Json.Runtime;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.APIContracts.Models.CTH;
using O24OpenAPI.CMS.API.Application.Models.ContextModels;
using O24OpenAPI.CMS.API.Application.Models.Request;
using O24OpenAPI.CMS.API.Application.Models.Response;
using O24OpenAPI.CMS.API.Application.Services.Interfaces;
using O24OpenAPI.CMS.API.Application.Utils;
using O24OpenAPI.CMS.Domain.AggregateModels.LearnApiAggregate;
using O24OpenAPI.CMS.Infrastructure.Configurations;
using O24OpenAPI.Contracts.Models;
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
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace O24OpenAPI.CMS.API.Application.Features.Requests;

#region Request Models

public class RequestModel
{
    [JsonPropertyName("learn_api")]
    public string LearnApi { get; set; } = string.Empty;

    [JsonPropertyName("workflowid")]
    public string WorkflowId { get; set; } = string.Empty;

    [JsonPropertyName("fields")]
    public Dictionary<string, object> Fields { get; set; } = [];

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(WorkflowId) && string.IsNullOrWhiteSpace(LearnApi))
        {
            throw new ArgumentException("WorkflowId and LearnApi cannot both be null or empty");
        }
    }
}

public class ResponseModel
{
    /// <summary>
    /// HTTP status code hoặc business code
    /// </summary>
    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Trạng thái thành công/thất bại
    /// </summary>
    [JsonPropertyName("success")]
    public bool Success { get; set; } = true;

    /// <summary>
    /// Thông báo mô tả kết quả
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// ID để trace/track execution
    /// </summary>
    [JsonPropertyName("execution_id")]
    public string ExecutionId { get; set; } = string.Empty;

    /// <summary>
    /// Timestamp khi response được tạo
    /// </summary>
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Dữ liệu trả về
    /// </summary>
    [JsonPropertyName("data")]
    public object Data { get; set; }

    /// <summary>
    /// Thông tin lỗi chi tiết (nếu có)
    /// </summary>
    [JsonPropertyName("errors")]
    public List<ErrorInfoModel> Errors { get; set; } = [];

    /// <summary>
    /// Metadata bổ sung (optional)
    /// </summary>
    [JsonPropertyName("metadata")]
    public Dictionary<string, object> Metadata { get; set; }
}
#endregion

[RegisterService(Lifetime.Scoped)]
public class RequestHandlerV1(
    ILocalizationService localizationService,
    JWebUIObjectContextModel context,
    CMSSetting cmsSetting,
    IJwtTokenService jwtTokenService,
    WorkContext workContext,
    ICTHGrpcClientService cthGrpcClientService,
    ILearnApiRepository learnApiRepository,
    IDataMapper dataMapper
) : IRequestHandler
{
    private readonly ILocalizationService _localizationService = localizationService;
    private readonly CMSSetting _cmsSetting = cmsSetting;
    private readonly WorkContext _workContext = workContext;
    private readonly IJwtTokenService _jwtTokenService = jwtTokenService;
    private readonly ICTHGrpcClientService _cthGrpcClientService = cthGrpcClientService;

    private bool NeedCheckSignature { get; set; } = true;
    private bool IsNeedCheckSession { get; set; } = true;
    private bool IsRefreshToken { get; set; } = false;

    #region Standard
    public async Task<ResponseModel> ProcessAsync(RequestModel request, HttpContext httpContext)
    {
        (IsNeedCheckSession, IsRefreshToken) = CheckInput(request);

        Dictionary<string, string> infoHeaderDictionary = httpContext.GetHeaders();
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
    #endregion

    public async Task<ResponseModel> RunBo(
        RequestModel requestModel,
        RequestHeaderModel infoHeader,
        HttpContext httpContext
    )
    {
        #region Validate token
        if (IsNeedCheckSession && !string.IsNullOrEmpty(infoHeader.Token))
        {
            ValidateTokenResponseModel validateTokenResponse = _jwtTokenService.ValidateToken(
                infoHeader.Token
            );
            if (!validateTokenResponse.IsValid)
            {
                throw new O24OpenAPIException("invalid.token");
            }
            infoHeader.UserId = validateTokenResponse.UserId;
            _workContext.UserContext.SetUserId(infoHeader.UserId);
            _workContext.SetDeviceRequest(infoHeader.MyDevice);
        }
        #endregion
        ResponseModel result = new();
        try
        {
            string appPost = infoHeader.App;
            _workContext.SetCurrentChannel(appPost);
            Dictionary<string, string> requestCookies = httpContext.GetCookies();
            string getSsidFromCookies = "";
            string sSIDStr = "device_id";
            if (requestCookies.TryGetValue(sSIDStr, out string value))
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
                    CTHUserSessionModel currentSession;
                    try
                    {
                        currentSession = await _cthGrpcClientService.GetUserSessionAsync(
                            infoHeader.Token
                        );
                        if (currentSession == null)
                        {
                            throw new O24OpenAPIException("Invalid token.");
                        }
                        isValid = true;
                    }
                    catch (Exception ex)
                    {
                        BusinessLogHelper.Error(ex, ex.Message);
                        throw new O24OpenAPIException(ex.Message, ex.InnerException);
                    }

                    if (isValid)
                    {
                        if (
                            NeedCheckSignature
                            && !SignatureUtils.VerifySignature(
                                requestObject: requestModel,
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
                        string[] roles = currentSession.ChannelRoles.FromJson<string[]>();
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
                BusinessLogHelper.Error(ex, ex.Message);
                List<ErrorInfoModel> error = await AddErrorSystem(ex.Message, "500");
                return ResponseFactory.Error(error);
            }

            if (!checkedSession)
            {
                throw new O24OpenAPIException("Invalid token.");
            }
            #endregion

            //===============================================IP,OS,BROWSER===============================================
            context.InfoRequest.SetIp(httpContext.GetClientIPAddress());
            context.InfoRequest.SetClientOs(httpContext.GetClientOs());
            context.InfoRequest.SetClientBrowser(httpContext.GetClientBrowser());
            context.InfoRequest.SetRequestModel(requestModel);
            context.InfoRequest.DeviceID = StringUtils.Coalesce(
                getSsidFromCookies,
                infoHeader.MyDevice.TryGetValue("device_id", out object deviceId)
                    ? deviceId?.ToString()
                    : null,
                ""
            );
            context.InfoRequest.DeviceType = StringUtils.Coalesce(
                getSsidFromCookies,
                infoHeader.MyDevice.TryGetValue("device_type", out object deviceType)
                    ? deviceType?.ToString()
                    : null,
                ""
            );

            context.InfoRequest.OsVersion = infoHeader.MyDevice.TryGetValue(
                "os_version",
                out object osVersion
            )
                ? osVersion?.ToString()
                : "";

            context.InfoRequest.AppVersion = infoHeader.MyDevice.TryGetValue(
                "app_version",
                out object appVersion
            )
                ? appVersion?.ToString()
                : "";

            context.InfoRequest.DeviceName = infoHeader.MyDevice.TryGetValue(
                "device_name",
                out object deviceName
            )
                ? deviceName?.ToString()
                : "";

            context.InfoRequest.Brand = infoHeader.MyDevice.TryGetValue("brand", out object brand)
                ? brand?.ToString()
                : "";

            context.InfoRequest.IsEmulator =
                infoHeader.MyDevice.TryGetValue("is_emulator", out object isEmulator)
                && bool.TryParse(isEmulator?.ToString(), out bool isEmuVal)
                    ? isEmuVal
                    : false;

            context.InfoRequest.IsRootedOrJailbroken =
                infoHeader.MyDevice.TryGetValue("is_rooted_or_jailbroken", out object isRooted)
                && bool.TryParse(isRooted?.ToString(), out bool isRootedVal)
                    ? isRootedVal
                    : false;

            context.InfoRequest.UserAgent = infoHeader.MyDevice.TryGetValue(
                "user_agent",
                out object userAgent
            )
                ? userAgent?.ToString()
                : "";

            context.InfoRequest.IpAddress = infoHeader.MyDevice.TryGetValue(
                "ip_address",
                out object ipAddress
            )
                ? ipAddress?.ToString()
                : "";

            context.InfoRequest.Modelname = infoHeader.MyDevice.TryGetValue(
                "model_name",
                out object modelName
            )
                ? modelName?.ToString()
                : "";

            if (infoHeader.MyDevice != null)
            {
                infoHeader.MyDevice.TryGetValue("network_type", out object networkType);
                infoHeader.MyDevice.TryGetValue("network_status", out object networkStatus);

                context.InfoRequest.Network = $"{networkType ?? ""} ({networkStatus ?? ""})".Trim();
            }
            else
            {
                context.InfoRequest.Network = string.Empty;
            }
            context.InfoRequest.Memory = infoHeader.MyDevice.TryGetValue(
                "total_memory",
                out object totalmemory
            )
                ? totalmemory?.ToString()
                : "";

            context.InfoRequest.PortalToken = portalToken;
            context.InfoRequest.Language = infoHeader.Lang;
            context.InfoApp.App = appPost;

            result = await ProcessBoRequest(requestModel);

            return result;
        }
        catch (Exception ex)
        {
            BusinessLogHelper.Error(ex, ex.Message);
            List<ErrorInfoModel> error = await AddErrorSystem(ex.Message, "500");
            return ResponseFactory.Error(error);
        }
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
            BusinessLogHelper.Error(ex, ex.Message);
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

    private async Task<ResponseModel> ProcessBoRequest(RequestModel requestModel)
    {
        WorkContext workflowContext = EngineContext.Current.Resolve<WorkContext>();
        string executeId = workflowContext.ExecutionId;

        BaseResponse<object> executionResult = await ExecuteAsync(requestModel);

        if (executionResult != null)
        {
            if (executionResult.Success == false)
            {
                List<ErrorInfoModel> errors =
                [
                    new ErrorInfoModel(
                        ErrorType.errorSystem,
                        ErrorMainForm.danger,
                        executionResult.ErrorMessage,
                        executionResult.ErrorCode ?? "UNKNOWN_ERROR",
                        "ERROR: ",
                        nextAction: executionResult.ErrorNextAction ?? "",
                        executeId: executeId
                    ),
                ];
                return ResponseFactory.Error(errors, executeId);
            }
            return ResponseFactory.Success(executionResult.Data);
        }
        List<ErrorInfoModel> genericError =
        [
            new ErrorInfoModel(
                ErrorType.errorSystem,
                ErrorMainForm.danger,
                "An unknown error occurred.",
                "UNKNOWN_ERROR",
                "ERROR: ",
                executeId: executeId
            ),
        ];
        return ResponseFactory.Error(genericError, executeId);
    }

    private (bool isNeedCheckSession, bool isRefreshToken) CheckInput(RequestModel request)
    {
        const string keyLogin = "LOGIN";
        bool isNeedCheckSession = true;
        bool isRefreshToken = true;

        string workflowStr = request.WorkflowId ?? string.Empty;
        string learnApiStr = request.LearnApi ?? string.Empty;

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

        return (isNeedCheckSession, isRefreshToken);
    }

    private async Task<BaseResponse<object>> ExecuteAsync(RequestModel requestModel)
    {
        try
        {
            string response;
            if (!string.IsNullOrWhiteSpace(requestModel.LearnApi))
            {
                string learnApiId = requestModel.LearnApi;

                LearnApi learnApi = await learnApiRepository.GetByChannelAndLearnApiIdAsync(
                    context.InfoApp.GetApp(),
                    learnApiId
                );
                JsonNode mappedRequest;
                if (learnApi.LearnApiMapping.HasValue())
                {
                    JsonSerializerOptions options = new()
                    {
                        ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    };
                    mappedRequest = await dataMapper.MapDataAsync(
                        JsonSerializer.SerializeToNode(requestModel.Fields, options).AsObject(),
                        JsonNode.Parse(learnApi.LearnApiMapping).AsObject()
                    );
                }
                else
                {
                    mappedRequest = BuildContentWorkflowRequestV1(requestModel);
                }
                if (learnApiId.StartsWithOrdinalIgnoreCase("CMS"))
                {
                    object learnApiExecutionResult =
                        await LearnApiHandlers.HandleAsync(
                            learnApiId,
                            mappedRequest,
                            EngineContext.Current.Resolve<IMediator>(MediatorKey.CMS)
                        )
                        ?? throw new Exception(
                            $"A error occur when processing learn api [{learnApiId}], result is null."
                        );
                    if (learnApiExecutionResult is JToken token)
                    {
                        return new BaseResponse<object>(learnApiExecutionResult.ToJson().FromJson<object>());
                    }
                    return new BaseResponse<object>(learnApiExecutionResult);
                }
                if (learnApi.URI.StartsWith('$'))
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
                            ICMSSettingService settingService =
                                EngineContext.Current.Resolve<ICMSSettingService>();
                            string setting = await settingService.GetStringValue(settingKey);
                            learnApi.URI = setting + remainingPath;
                            // learnApi.URI = "https://localhost:5070" + remainingPath;
                        }
                    }
                }
                response = await CallAPIAsync(
                    path: learnApi.URI,
                    method: learnApi.LearnApiMethod,
                    content: mappedRequest,
                    isInternal: learnApi.IsInternal ?? true,
                    headerToken: context.InfoUser.GetUserLogin().Token
                );
            }
            else
            {
                JsonObject obContent = BuildContentWorkflowRequestV1(requestModel);
                IWFOGrpcClientService wfoGrpcClientService =
                    EngineContext.Current.Resolve<IWFOGrpcClientService>();
                response = await wfoGrpcClientService.ExecuteWorkflowAsync(obContent.ToJson());
            }

            BaseResponse<object> result = response.FromJson<BaseResponse<object>>();
            return result;
        }
        catch (Exception ex)
        {
            BusinessLogHelper.Error(ex, ex.Message);
            Console.WriteLine(ex.StackTrace);
            throw;
        }
    }

    private JsonObject BuildContentWorkflowRequestV1(RequestModel requestModel)
    {
        JsonObject wfInput = JsonSerializer.SerializeToNode(requestModel)?.AsObject() ?? [];
        wfInput["lang"] = context.InfoRequest.Language;
        wfInput["token"] = context.InfoUser.GetUserLogin().Token;
        wfInput["user_id"] = context.InfoUser.UserSession?.UserId.ToString() ?? "0";
        wfInput["user_code"] = context.InfoUser.UserSession?.UserCode.ToString() ?? "0";
        wfInput["execution_id"] = _workContext?.ExecutionId ?? Guid.NewGuid().ToString();
        wfInput["channel_id"] = context.InfoApp.GetApp();
        wfInput["transaction_date"] = DateTime.UtcNow;

        DeviceModel deviceData = new()
        {
            IpAddress = context.InfoRequest.GetIp() ?? "::1",
            DeviceId = string.IsNullOrWhiteSpace(context.InfoRequest.DeviceID)
                ? Guid.NewGuid().ToString()
                : context.InfoRequest.DeviceID,
            OsVersion =
                context.InfoRequest.OsVersion ?? context.InfoRequest.GetClientOs() ?? "unknown",
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
        };

        wfInput["device"] = JsonSerializer.SerializeToNode(
            deviceData,
            FWJsonContext.Default.DeviceModel
        );

        return wfInput;
    }

    private static async Task<string> CallAPIAsync(
        string path,
        string method,
        object content,
        bool isInternal,
        string headerToken = ""
    )
    {
        IHttpClientFactory httpClientFactory = EngineContext.Current.Resolve<IHttpClientFactory>();
        HttpClient httpClient = httpClientFactory.CreateClient("DefaultClient");
        if (!string.IsNullOrEmpty(headerToken))
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + headerToken);
        }
        if (isInternal)
        {
            WorkContext workContext = EngineContext.Current.Resolve<WorkContext>();
            string workContextJson = JsonSerializer.Serialize(
                workContext,
                CoreJsonContext.Default.WorkContext
            );
            httpClient.DefaultRequestHeaders.Add("WorkContext", workContextJson);
        }
        string requestString = content.ToJson();
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
}

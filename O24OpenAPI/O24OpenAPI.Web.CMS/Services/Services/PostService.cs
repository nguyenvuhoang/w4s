using System.Globalization;
using System.Text;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O24OpenAPI.Core.Configuration;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Core.Utils;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Services.Mapping;
using O24OpenAPI.GrpcContracts.GrpcClientServices.WFO;
using O24OpenAPI.Web.CMS.Constant;
using O24OpenAPI.Web.CMS.Models;
using O24OpenAPI.Web.CMS.Models.ContextModels;
using O24OpenAPI.Web.CMS.Services.Interfaces;

namespace O24OpenAPI.Web.CMS.Services.Services;

/// <summary>
/// The post service class
/// </summary>
public partial class PostService(
    ILearnApiService learnApiService,
    IMappingService mappingService,
    IApiService apiService,
    IUserSessionsService userSessionsService,
    IWorkflowService workflowService,
    IRepository<LearnApiExecutionLog> learnApiExecutionLog,
    JWebUIObjectContextModel context1,
    IDataMappingService dataMappingService
) : IPostService
{
    private readonly ILearnApiService _learnApiService = learnApiService;
    private readonly IMappingService _mappingService = mappingService;
    private readonly IApiService _apiService = apiService;
    private readonly IUserSessionsService _userSessionsService = userSessionsService;
    public readonly IWorkflowService _WorkflowService = workflowService;
    private readonly IRepository<LearnApiExecutionLog> _learnApiExecutionLog = learnApiExecutionLog;
    private readonly JWebUIObjectContextModel context = context1;
    private readonly IDataMappingService _dataMappingService = dataMappingService;
    private readonly WorkContext _workContext = EngineContext.Current.Resolve<WorkContext>();

    /// <summary>
    ///
    /// </summary>
    public async Task<JObject> GetDataPostAPI(string appCodeRequest)
    {
        var boInput = context.Bo.GetBoInput();

        if (boInput.TryGetValue("learn_api", out var learnApiToken))
        {
            string learnApi = learnApiToken?.ToString();
            if (string.IsNullOrEmpty(learnApi))
            {
                throw new ArgumentNullException(
                    nameof(learnApi),
                    "LearnApi cannot be null or empty!"
                );
            }
            return await RouterExecute(learnApi, appCodeRequest);
        }

        return null;
    }

    /// <summary>
    ///
    /// </summary>
    public async Task<JToken> O9CallAPIAsync(string appCodeRequest, string learn_api)
    {
        try
        {
            if (!string.IsNullOrEmpty(learn_api))
            {
                return await RouterExecute(learn_api, appCodeRequest);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
            throw new Exception(ex.Message);
        }

        return null;
    }

    private async Task<JObject> RouterExecute(string learnApi, string channel)
    {
        try
        {
            if (string.IsNullOrEmpty(learnApi))
            {
                throw new ArgumentException("learnApi cannot be null or empty");
            }

            learnApi = learnApi.Split(';')[0];

            JObject boInput = context.Bo.GetBoInput();
            if (!boInput.TryGetValue("data_select", out var dataSelect))
            {
                dataSelect = boInput;
            }
            JObject obDataForMap = dataSelect.ToObject<JObject>();

            Console.WriteLine($"learnApi__: {learnApi}");
            Console.WriteLine($"appCodeRequest: {channel}");

            LearnApiModel learnApiModel =
                await _learnApiService.GetByAppAndId(channel, learnApi)
                ?? throw new InvalidOperationException(
                    $"Invalid learn api [{learnApi}][{channel}]"
                );

            var learnApiRequestModel = CreateLearnRequestModel(learnApiModel, obDataForMap);

            if (
                boInput.TryGetValue("static_token", out var staticToken)
                && !string.IsNullOrEmpty(staticToken?.Value<string>())
            )
            {
                learnApiRequestModel.user_approve = await _userSessionsService.GetByToken(
                    staticToken.Value<string>()
                );
            }
            context.InfoRequest.Language = learnApiRequestModel.Lang;
            learnApiRequestModel.WorkflowId =
                learnApiRequestModel.LearnApiId != "cbs_workflow_execute"
                    ? learnApiRequestModel.LearnApiId
                    : learnApiRequestModel.ObjectField.Value<string>("workflowid");
            learnApiRequestModel.ChannelId = channel;

            await LearnApiExecutionLog(learnApiRequestModel);

            JToken result = await _WorkflowService.ExecuteWorkflowStep(learnApiRequestModel);
            JObject processedResult = await ProcessResponse(result, learnApiRequestModel);

            _ = Task.Run(() => LearnApiExecutionLog(learnApiRequestModel, processedResult));

            return processedResult;
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            JObject errorModel = ex.Message.BuildWorkflowResponseError();
            var workflow = new LearnApiRequestModel { ExecuteId = context.InfoRequest.ExecuteId };
            _ = TaskUtils.RunAsync(async () =>
            {
                await ex.LogErrorAsync();
            });
            return await ProcessResponse(errorModel, workflow);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected Error: {ex.Message}");
            JObject errorModel = ex.Message.BuildWorkflowResponseError();
            var workflow = new LearnApiRequestModel { ExecuteId = context.InfoRequest.ExecuteId };
            _ = TaskUtils.RunAsync(async () =>
            {
                await ex.LogErrorAsync();
            });
            return await ProcessResponse(errorModel, workflow);
        }
    }

    /// <summary>
    /// Creates the workflow request model using the specified ob
    /// </summary>
    /// <param name="ob">The ob</param>
    /// <param name="dataMap"></param>
    /// <param name="context">The context</param>
    /// <exception cref="Exception">An error occoured when create workflow request!</exception>
    /// <returns>The workflow request model</returns>
    private LearnApiRequestModel CreateLearnRequestModel(LearnApiModel ob, JObject dataMap)
    {
        try
        {
            var learnApiRequestModel = new LearnApiRequestModel
            {
                LearnApiMappingResponse = ob.LearnApiMappingResponse,
                LearnApiId = ob.LearnApiId,
                IsCache = ob.IsCache,
                LearnApiIdClear = ob.LearnApiIdClear,
                ObjectField = dataMap,
                user_sessions =
                    SessionUtils.GetUserSession(context)
                    ?? dataMap.SelectToken("user_session").JsonConvertToModel<UserSessions>()
                    ?? new UserSessions(),
            };
            learnApiRequestModel.user_sessions.Wsip = context.InfoRequest.GetIp();
            if (context.InfoUser.GetUserLogin().App != AppCode.TellerApp)
            {
                learnApiRequestModel.user_sessions.Ssesionid = O9Client.CoreBankingSession.UUID;
                learnApiRequestModel.user_sessions.Usrid = O9Client.CoreBankingSession.USRID;
                learnApiRequestModel.user_sessions.UsrBranchid = O9Client
                    .CoreBankingSession
                    .BRANCHID;
                if (
                    DateTime.TryParseExact(
                        O9Client.CoreBankingSession.BUSDATE,
                        "dd/MM/yyyy",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out var busDate
                    )
                )
                {
                    learnApiRequestModel.user_sessions.Txdt = busDate;
                }
            }

            learnApiRequestModel.ExecuteId = context.InfoRequest.ExecuteId;
            learnApiRequestModel.StringWorkingDate =
                learnApiRequestModel.user_sessions?.Txdt.ToString("dd/MM/yyyy");
            learnApiRequestModel.Lang = context.InfoUser.GetUserLogin().Lang;
            learnApiRequestModel.RequestId = context.InfoRequest.RequestId;
            learnApiRequestModel.ExecuteId = context.InfoRequest.ExecuteId;

            return learnApiRequestModel;
        }
        catch (Exception ex)
        {
            ex.Message.LogError();
            throw;
        }
    }

    private async Task<(bool, WorkflowResponseModel)> IsValidResponse(
        JToken response,
        LearnApiRequestModel model
    )
    {
        if (response == null)
        {
            var errorMessage = "Null response received from the service.";
            var err = await _apiService.BuildError(
                model.LearnApiId,
                new WorkflowResponseModel
                {
                    status = ExecutionStatus.ERROR,
                    error_message = errorMessage,
                },
                model.ExecuteId
            );
            context.Bo.AddActionErrorAll(err);
            return (false, null);
        }

        var responseModel = response.ToObject<WorkflowResponseModel>();
        if (responseModel.status != ExecutionStatus.SUCCESS)
        {
            var err = await _apiService.BuildError(
                model.LearnApiId,
                responseModel,
                model.ExecuteId
            );
            context.Bo.AddActionErrorAll(err);
            return (false, null);
        }
        return (true, responseModel);
    }

    /// <summary>
    /// Processes the response using the specified response
    /// </summary>
    private async Task<JObject> ProcessResponse(JToken response, LearnApiRequestModel model)
    {
        var (isValid, responseModel) = await IsValidResponse(response, model);
        if (!isValid)
        {
            return null;
        }
        var jsResult = responseModel.result;
        JObject mapping = [];
        if (responseModel.needs_mapping)
        {
            if (model.LearnApiMappingResponse.HasValue() && model.ConfigMappingResponse == "A")
            {
                mapping = (
                    await _mappingService.MappingResponse(model.LearnApiMappingResponse, jsResult)
                ).ToJObject();
            }
            else
            {
                foreach (var item in jsResult.SelectToken("response"))
                {
                    if (item is JObject && item["build_from"]?.ToString() == "O9WORKFLOW")
                    {
                        mapping.MergeWithReplaceArray(item["result"].ToObject<JObject>());
                    }
                    else if (item is JArray dataArray)
                    {
                        var newObj = new JObject { ["data"] = dataArray };
                        mapping.MergeWithReplaceArray(newObj);
                    }
                    else
                    {
                        mapping.MergeWithReplaceArray(item.ToObject<JObject>());
                    }
                }
            }
        }
        else
        {
            mapping = jsResult.ToObject<JObject>();
        }
        var result = mapping.ToJObject();
        var dictionary = context.Bo.GetFoInput().input;
        foreach (var kvp in dictionary)
        {
            result.Add(kvp.Key, JToken.FromObject(kvp.Value));
        }
        return result;
    }

    /// <summary>
    /// Learns the api execution log using the specified workflow
    /// </summary>
    /// <param name="workflow">The workflow</param>
    /// <param name="response">The response</param>
    private async Task LearnApiExecutionLog(LearnApiRequestModel workflow, JToken response = null)
    {
        var log =
            (
                context.Bo.GetActionErrors().Count > 0 || response != null
                    ? await _learnApiExecutionLog.GetByFields(
                        new Dictionary<string, string>()
                        {
                            { nameof(LearnApiRequestModel.ExecuteId), workflow.ExecuteId },
                        }
                    )
                    : new LearnApiExecutionLog()
            ) ?? new LearnApiExecutionLog();
        if (response != null)
        {
            log.Status = nameof(ExecutionStatus.SUCCESS);
            log.FinishOn = DateTime.UtcNow;
            log.Output = JsonConvert.SerializeObject(response);
            await _learnApiExecutionLog.Update(log);
            return;
        }
        else if (context.Bo.GetActionErrors().Count > 0)
        {
            log.Status = nameof(ExecutionStatus.ERROR);
            log.FinishOn = DateTime.UtcNow;
            log.Output = JsonConvert.SerializeObject(context.Bo.GetActionErrors());
            await _learnApiExecutionLog.Update(log);
            return;
        }

        log.ExecuteId = workflow.ExecuteId;
        log.WorkflowId = workflow.WorkflowId;
        log.Input = JsonConvert.SerializeObject(workflow);
        log.Status = nameof(ExecutionStatus.PROCESSING);
        log.CreateOn = DateTime.UtcNow;
        log.Module = workflow.Module;
        log.TableName = workflow.TableName;
        log.IdFieldName = workflow.IdFieldName;
        log.WorkflowFunc = workflow.WorkflowFunc;
        log.TxCode = workflow.TxCode;
        log.UserId =
            workflow.user_sessions?.Usrid.ToString()
            ?? SessionUtils.GetUserSession(context)?.Usrid.ToString();
        await _learnApiExecutionLog.Insert(log);
    }

    #region  ExecuteAsync
    public async Task<JObject> ExecuteAsync()
    {
        try
        {
            var boInput = context.Bo.GetBoInput();
            if (boInput == null)
            {
                throw new ArgumentNullException(
                    nameof(boInput),
                    "BoInput cannot be null or empty!"
                );
            }
            string response;
            if (
                !boInput.TryGetValue("workflowid", out var workflowIdToken)
                || string.IsNullOrEmpty(workflowIdToken.ToString())
            )
            {
                var learnApiId = boInput["learn_api"]?.ToString();
                if (string.IsNullOrEmpty(learnApiId))
                {
                    throw new ArgumentNullException(
                        nameof(learnApiId),
                        "LearnApi cannot be null or empty!"
                    );
                }
                var learnApi = await _learnApiService.GetByLearnApiIdAndChannel(
                    learnApiId,
                    context.InfoApp.GetApp()
                );
                var mappedRequest = BuildContentWorkflowRequest();
                if (learnApi.LearnApiMapping.HasValue())
                {
                    mappedRequest = await _dataMappingService.MapDataAsync(boInput, mappedRequest);
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
                            var wfoURI = Singleton<O24OpenAPIConfiguration>.Instance.WFOHttpURL;
                            learnApi.URI = wfoURI + remainingPath;
                        }
                        else
                        {
                            var settingService =
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
                var obContent = BuildContentWorkflowRequest();
                var wfoGrpcClientService = EngineContext.Current.Resolve<IWFOGrpcClientService>();
                response = await wfoGrpcClientService.ExecuteWorkflowAsync(
                    JsonConvert.SerializeObject(obContent)
                );
            }

            var result = JObject.Parse(response);
            if (result.TryGetValue("error_message", out var messageToken))
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
        var sourceInput = context.Bo.GetBoInput();
        var wfInput = new JObject(sourceInput)
        {
            ["lang"] = context.InfoRequest.Language,
            ["token"] = context.InfoUser.GetUserLogin().Token,
            ["user_id"] = context.InfoUser.UserSession?.Userid.ToString() ?? "0",
            ["user_code"] = context.InfoUser.UserSession?.UserCode.ToString() ?? "0",
            ["execution_id"] = _workContext?.ExecutionId ?? Guid.NewGuid().ToString(),
            ["channel_id"] = context.InfoApp.GetApp(),
            ["transaction_date"] = DateTime.UtcNow,
            ["value_date"] = context.InfoUser.UserSession?.Txdt ?? DateTime.UtcNow,
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

        var requestString = JsonConvert.SerializeObject(content);
        var requestContent = new StringContent(requestString, Encoding.Default, "application/json");
        var requestMessage = new HttpRequestMessage(new HttpMethod(method), path)
        {
            Content = requestContent,
        };
        var httpResponse = await httpClient.SendAsync(requestMessage);
        httpResponse.EnsureSuccessStatusCode();
        var result = await httpResponse.Content.ReadAsStringAsync();

        return result;
    }
    #endregion
}

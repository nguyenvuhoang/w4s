using System.Reflection;
using System.Text.Json;
using LinqToDB;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Configuration;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Core.Helper;
using O24OpenAPI.O24OpenAPIClient.Enums;
using O24OpenAPI.Web.CMS.Configuration;
using O24OpenAPI.Web.CMS.Constant;
using O24OpenAPI.Web.CMS.Models;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using O24OpenAPI.Web.CMS.Services.Interfaces.CBS;
using O24OpenAPI.Web.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Web.Framework.Services;
using StackExchange.Redis;

namespace O24OpenAPI.Web.CMS.Services.Services;

/// <summary>
/// The workflow service class
/// </summary>
/// <seealso cref="BaseQueueService"/>
/// <seealso cref="IWorkflowService"/>
public class WorkflowService(
    IMappingService mappingService,
    IRepository<WorkflowStep> workflowStepRepository,
    IRepository<WorkflowStepLog> workflowStepLog,
    IBaseO9WorkflowService baseWorkflowService,
    IStaticCacheManager staticCacheManager,
    IValidationService validationService,
    CMSSetting cmsSetting,
    IWorkflowDefinitionService workflowDefinitionService,
    IWorkflowStepService workflowStepService,
    ICBService cbService,
    ICrossWorkflowService crossWorkflowService
) : BaseQueueService, IWorkflowService
{
    private readonly IMappingService _mappingService = mappingService;
    private readonly IRepository<WorkflowStep> _workflowRepo = workflowStepRepository;
    private readonly IRepository<WorkflowStepLog> _workflowStepLog = workflowStepLog;
    private readonly IStaticCacheManager _staticCacheManager = staticCacheManager;
    private readonly string _commonError = "The system is busy, please try again later.";
    private readonly IBaseO9WorkflowService _baseWorkflowService = baseWorkflowService;
    private readonly WebApiSettings _setting = EngineContext.Current.Resolve<WebApiSettings>();
    private readonly IValidationService _validationService = validationService;
    private readonly CMSSetting _cmsSetting = cmsSetting;
    private readonly UserSessions _userSession = SessionUtils.GetUserSession();
    private readonly IWorkflowDefinitionService _workflowDefinitionService =
        workflowDefinitionService;
    private readonly IWorkflowStepService _workflowStepService = workflowStepService;
    private readonly ICBService _cbService = cbService;
    private readonly ICrossWorkflowService _crossWorkflowService = crossWorkflowService;

    private async Task<List<WorkflowStep>> GetWorkflowStepsAsync(string workflowId, string appCode)
    {
        _ =
            await _workflowDefinitionService.GetByWFId(workflowId)
            ?? throw await ErrorUtils.BuildException(
                errorCode: ErrorName.INVALID_WORKFLOW_DEF,
                errorRegion: ModuleCode.System,
                workflowId
            );
        var workflowStep = await _workflowStepService.GetStepsByWorkflowIdAsync(
            workflowId,
            workflowId.StartsWith("SYS") ? null : appCode
        );
        if (workflowStep.Count < 1)
        {
            throw await ErrorUtils.BuildException(
                errorCode: ErrorName.INVALID_WORKFLOW_STEP,
                errorRegion: ModuleCode.System,
                workflowId
            );
        }
        return workflowStep;
    }

    public async Task<List<WorkflowStep>> GetExecutionWorkflowAsync(
        string workflowId,
        string appCode
    )
    {
        var cacheKey = CachingKey.EntityKey<WorkflowStep>(workflowId);
        cacheKey.IsForever = true;

        return await _staticCacheManager.GetOrSetAsync(
            cacheKey,
            () => GetWorkflowStepsAsync(workflowId, appCode)
        );
    }

    public async Task<JToken> ExecuteWorkflowStep(LearnApiRequestModel learnApi)
    {
        if (TryGetCache(learnApi, out var cache))
        {
            return cache;
        }
        string workflowId = learnApi.WorkflowId;
        string channel = learnApi.ChannelId;

        var isReverse = false;
        bool isSuccess = true;
        var responseStep = new List<WorkflowScheme>();

        Console.WriteLine("workflowId::" + workflowId);

        var workflowStep = await GetExecutionWorkflowAsync(workflowId, channel);

        var stepLength = workflowStep.Count;
        if (stepLength == 0)
        {
            return $"Unable to resolve Workflow {workflowId}".BuildWorkflowResponseError(
                "SYS_00_02",
                ""
            );
        }

        try
        {
            foreach (var step in workflowStep)
            {
                var builderScheme = await BuildWorkflowScheme(
                    learnApi,
                    responseStep,
                    step,
                    channel
                );

                if (!builderScheme.Request.RequestHeader.SendingConditionPassed)
                {
                    continue;
                }

                responseStep.Add(builderScheme);
                await WorkflowStepLog(builderScheme);

                var rp = await SwitchProcess(builderScheme, step, learnApi);
                builderScheme.Response = rp.Response;
                if (
                    step.MappingResponse.HasValue()
                    && rp.Response.Data.ToJObject()["build_from"]?.ToString() != "O9WORKFLOW"
                )
                {
                    var mapping = await _mappingService.MappingResponse(
                        step.MappingResponse,
                        builderScheme.Response.ToJToken()
                    );
                    builderScheme.Response.Data = mapping.ToDictionary();
                }
                _ = WorkflowStepLog(builderScheme, true).ConfigureAwait(false);

                if (rp.Response.Status == WorkflowScheme.RESPONSE.EnumResponseStatus.ERROR)
                {
                    isReverse = true;
                    isSuccess = false;
                    break;
                }
            }

            if (isReverse)
            {
                WorkflowScheme.RESPONSE error = null;
                foreach (var step in responseStep)
                {
                    var workflowOrder = workflowStep.First(e =>
                        e.StepOrder == step.Request.RequestHeader.StepOrder
                    );
                    if (workflowOrder.IsReverse)
                    {
                        await SwitchProcess(step, workflowOrder, learnApi, isReverse);
                    }

                    if (step.Response.Status == WorkflowScheme.RESPONSE.EnumResponseStatus.ERROR)
                    {
                        error = step.Response;
                    }
                }

                if (error != null)
                {
                    return error.ErrorMessage.BuildWorkflowResponseError(
                        error.ErrorCode,
                        error.Data.ToString()
                    );
                }
                else
                {
                    return $"Unable to resolve Workflow {workflowId}".BuildWorkflowResponseError(
                        "SYS_00_02",
                        ""
                    );
                }
            }
        }
        catch (Exception e)
        {
            return ExceptionBuildResponse(e);
        }

        SetNewAndReleaseCache(learnApi, responseStep);
        return BuildResponse(responseStep);
    }

    private async Task<WorkflowScheme> BuildWorkflowScheme(
        LearnApiRequestModel workflowRequestModel,
        List<WorkflowScheme> arrayWorkflowScheme,
        WorkflowStep step,
        string channel
    )
    {
        var scheme = new WorkflowScheme();
        scheme.Request.RequestHeader.ProcessNumber = step.ProcessingNumber;
        scheme.Request.RequestHeader.Channel = channel;
        scheme.Request.RequestHeader.StepTimeout = step.StepTimeout;
        scheme.Request.RequestHeader.StepCode = step.StepCode;
        scheme.Request.RequestHeader.StepOrder = step.StepOrder;
        scheme.Request.RequestHeader.ServiceId = step.ServiceID;
        scheme.Request.RequestHeader.StepExecutionId = Guid.NewGuid().ToString();
        scheme.Request.RequestHeader.UtcSendTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var txContext = workflowRequestModel.ToModel<BaseTransactionModel>();
        txContext.TransactionNumber =
            DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString()
            + workflowRequestModel.RequestId.ToString();

        var user_session =
            _userSession
            ?? workflowRequestModel
                .ObjectField.SelectToken("user_session")
                .JsonConvertToModel<UserSessions>()
            ?? new UserSessions();

        txContext.CurrentUserCode = user_session.UserCode;
        txContext.Token = user_session.Token;
        txContext.CurrentBranchCode = user_session.UsrBranch;

        txContext.ValueDate = DateTimeOffset.Now.DateTime;
        scheme.Request.RequestHeader.TxContext = txContext.ToDictionary();
        scheme.Request.RequestHeader.TxContext["step_mapping_response"] = step.MappingResponse;
        scheme.Request.RequestHeader.TxContext["learnapi_mapping_response"] =
            workflowRequestModel.LearnApiMappingResponse;

        scheme.Request.RequestHeader.ExecutionId = workflowRequestModel.ExecuteId;
        scheme.Request.RequestHeader.SendingConditionPassed =
            await _validationService.EvaluateCondition(
                step.SendingCondition,
                workflowRequestModel.ObjectField
            );
        scheme.Request.RequestBody.WorkflowInput = workflowRequestModel;
        scheme.Request.RequestBody.Data = await _mappingService.WorkflowStepMapData(
            step.SendingTemplate,
            workflowRequestModel,
            arrayWorkflowScheme
        );

        return scheme;
    }

    private async Task<WorkflowScheme> SwitchProcess(
        WorkflowScheme workflowScheme,
        WorkflowStep workflowStep,
        LearnApiRequestModel workflowRequestModel,
        bool isReverse = false
    )
    {
        var response = new WorkflowScheme();
        if (isReverse)
        {
            workflowScheme.Request.RequestHeader.WorkflowType = WorkflowScheme
                .REQUEST
                .REQUESTHEADER
                .EnumWorkflowType
                .reverse;
        }

        async Task<WorkflowScheme> ProcessWorkflow()
        {
            try
            {
                switch (workflowStep.ProcessingNumber)
                {
                    case ProcessNumber.Version1:
                        return await InvokeAsync(
                            workflowScheme,
                            workflowStep.FullClassName,
                            workflowStep.MethodName,
                            Assembly.GetExecutingAssembly().GetName().Name
                        );

                    case ProcessNumber.StoredProcedure:
                        return await Invoke2<BaseTransactionModel>(
                            workflowScheme,
                            workflowStep.StepCode
                        );

                    case ProcessNumber.Core:
                        if (_cmsSetting.CoreMode == CoreMode.Neptune)
                        {
                            return await InvokeNeptune(
                                workflowScheme,
                                workflowRequestModel,
                                workflowStep
                            );
                        }
                        return await InvokeCore(workflowScheme);

                    case ProcessNumber.CrossService:
                        return await InvokeCrossService(
                            workflowScheme,
                            workflowStep.FullClassName,
                            workflowStep.MethodName
                        );

                    case ProcessNumber.ExecuteCommand:

                        return await ExecuteCommandProcessor(workflowScheme);

                    default:
                        throw new Exception(
                            $"Not Found Process with workflow step: {workflowStep.StepCode}"
                        );
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        try
        {
            if (!workflowStep.ShouldAwaitStep)
            {
                _ = Task.Run(async () =>
                {
                    WorkflowScheme asyncResponse = await ProcessWorkflow();
                    await UpdateWorkflowStepLogAndReverse(
                        asyncResponse,
                        workflowStep,
                        workflowRequestModel
                    );
                });
            }
            else
            {
                response = await ProcessWorkflow();
            }
        }
        catch (Exception ex)
        {
            response.Response.Status = WorkflowScheme.RESPONSE.EnumResponseStatus.ERROR;
            var o24Exception = ex as O24OpenAPIException;

            if (!_setting.DeveloperMode)
            {
                if (o24Exception != null)
                {
                    response.Response.ErrorMessage = o24Exception.Message;
                    response.Response.ErrorCode = o24Exception.ErrorCode;
                }
                else
                {
                    response.Response.RawError = ex.Message + ex.StackTrace;
                    response.Response.ErrorMessage = _commonError;
                    response.Response.ErrorCode = "SYS_00_01";
                }
            }
            else
            {
                response.Response.ErrorCode = o24Exception?.ErrorCode;
                response.Response.ErrorMessage =
                    o24Exception != null ? ex.Message : ex.Message + ex.StackTrace;
            }
        }

        return response;
    }

    public async Task<WorkflowScheme> ExecuteCommandProcessor(WorkflowScheme workflowScheme)
    {
        if (workflowScheme == null)
        {
            throw new ArgumentNullException(
                nameof(workflowScheme),
                "Workflow scheme cannot be null."
            );
        }

        if (
            workflowScheme.Request.RequestHeader.ServiceId.NullOrEmpty()
            || workflowScheme.Request.RequestHeader.ServiceId
                == Singleton<O24OpenAPIConfiguration>.Instance.YourServiceID
        )
        {
            return await InvokeCommandQuery(workflowScheme);
        }
        else
        {
            return await InvokeCrossService(workflowScheme);
        }
    }

    /// <summary>
    /// Updates the workflow step log and reverse using the specified workflow scheme
    /// </summary>
    /// <param name="workflowScheme">The workflow scheme</param>
    /// <param name="workflowStep">The workflow step</param>
    /// <param name="workflowRequestModel">The workflow request model</param>
    private async Task UpdateWorkflowStepLogAndReverse(
        WorkflowScheme workflowScheme,
        WorkflowStep workflowStep,
        LearnApiRequestModel workflowRequestModel
    )
    {
        try
        {
            await WorkflowStepLog(workflowScheme, true);
            if (workflowScheme.Response.Status == WorkflowScheme.RESPONSE.EnumResponseStatus.ERROR)
            {
                if (workflowStep.IsReverse)
                {
                    await SwitchProcess(
                        workflowScheme,
                        workflowStep,
                        workflowRequestModel,
                        isReverse: true
                    );
                }
            }
        }
        catch (Exception) { }
    }

    private static async Task<WorkflowScheme> InvokeNeptune(
        WorkflowScheme workflowScheme,
        LearnApiRequestModel workflowRequestModel,
        WorkflowStep workflowStep
    )
    {
        return await InvokeAsync(
            workflowScheme,
            workflowStep.FullClassName,
            workflowStep.MethodName,
            Assembly.GetExecutingAssembly().GetName().Name
        );
    }

    private async Task<WorkflowScheme> InvokeCore(WorkflowScheme workflowScheme)
    {
        try
        {
            var model = workflowScheme.ToO9Model();

            var timeoutSec = workflowScheme.Request.RequestHeader.StepTimeout;
            var startSec = workflowScheme.Request.RequestHeader.UtcSendTime;
            var currentTime = CommonHelper.ConvertToUnixTimestamp(DateTime.UtcNow);
            var trafficTime = currentTime - startSec;
            var apiSettings = EngineContext.Current.Resolve<WebApiSettings>();
            var bufferTime = (apiSettings.BufferTime == 0L) ? 500 : apiSettings.BufferTime;

            if (timeoutSec - 2 * trafficTime - bufferTime <= 0)
            {
                throw new Exception("Message is outdate");
            }

            var timeout =
                (timeoutSec == 0) ? TimeSpan.MaxValue : TimeSpan.FromMilliseconds(timeoutSec);

            dynamic result = null;
            JToken obRss = null;

            using CancellationTokenSource cts = new(timeout);
            try
            {
                cts.Token.ThrowIfCancellationRequested();
                var task = Task.Run(async () =>
                {
                    switch (Enum.Parse<ActionType>(model.ActionType))
                    {
                        case ActionType.RuleFunc:
                            obRss = await _baseWorkflowService.RuleFunc(model);
                            break;
                        case ActionType.ExecuteRuleFunc:
                            obRss = await _baseWorkflowService.ExecuteRuleFunc(model);
                            break;
                        case ActionType.CreateFo:
                            obRss = await _baseWorkflowService.FrontOffice(model);
                            break;
                        case ActionType.UpdateBo:
                            obRss = await _baseWorkflowService.UpdateBO(model);
                            break;
                        case ActionType.CallFunction:
                            obRss = await _baseWorkflowService.CallFunction(model);
                            break;
                        case ActionType.CreateBo:
                            obRss = await _baseWorkflowService.CreateBO(model);
                            break;
                        case ActionType.Search:
                            var search = await _baseWorkflowService.SimpleSearch(model);
                            obRss = search;
                            break;
                        case ActionType.SearchAd:
                            var searchAd = await _baseWorkflowService.AdvanceSearch(model);
                            obRss = searchAd;
                            break;
                        case ActionType.ViewBo:
                            var view = await _baseWorkflowService.View(model);
                            obRss = view;
                            break;
                        case ActionType.Delete:
                            await _baseWorkflowService.Delete(model);
                            break;
                        case ActionType.SearchList:
                            obRss = await _baseWorkflowService.SearchList(model);
                            break;
                        case ActionType.SearchInfo:
                            obRss = await _baseWorkflowService.SearchInfo(model);
                            break;
                        case ActionType.ExecuteQuery:
                            obRss = await _baseWorkflowService.ExecuteQuery(model);
                            break;
                        default:
                            throw new O24OpenAPIException(
                                $"Not Found Process with workflow step: {workflowScheme.Request.RequestHeader.StepCode}"
                            );
                    }

                    return obRss;
                });

                if (await Task.WhenAny(task, Task.Delay(timeout, cts.Token)) == task)
                {
                    result = await task;
                }
                else
                {
                    cts.Cancel();
                    throw new TimeoutException("The operation has timed out.");
                }

                if (((JObject)result).SelectToken("status")?.ToInt() == -1)
                {
                    workflowScheme.Response.Status = WorkflowScheme
                        .RESPONSE
                        .EnumResponseStatus
                        .ERROR;
                    workflowScheme.Response.ErrorMessage =
                        ((JObject)result).SelectToken("error_message")?.ToString() ?? string.Empty;
                }
                else
                {
                    workflowScheme.Response.Data = result;
                    workflowScheme.Response.Status = WorkflowScheme
                        .RESPONSE
                        .EnumResponseStatus
                        .SUCCESS;
                }
            }
            catch (OperationCanceledException)
            {
                workflowScheme.Response.Status = WorkflowScheme.RESPONSE.EnumResponseStatus.ERROR;
                workflowScheme.Response.ErrorMessage = "The operation has timed out.";
            }
            catch (Exception ex)
            {
                workflowScheme.Response.Status = WorkflowScheme.RESPONSE.EnumResponseStatus.ERROR;
                workflowScheme.Response.ErrorMessage = ex.Message;
            }
        }
        catch (Exception ex)
        {
            workflowScheme.Response.Status = WorkflowScheme.RESPONSE.EnumResponseStatus.ERROR;
            workflowScheme.Response.ErrorMessage = ex.Message;
        }

        return workflowScheme;
    }

    /// <summary>
    /// Workflows the step log using the specified workflow
    /// </summary>
    /// <param name="workflow">The workflow</param>
    /// <param name="isResponse">The is response</param>
    private async Task WorkflowStepLog(WorkflowScheme workflow, bool isResponse = false)
    {
        var log = isResponse
            ? await _workflowStepLog.Table.FirstAsync(e =>
                e.StepExecutionId == workflow.Request.RequestHeader.StepExecutionId
            )
            : JsonConvert.DeserializeObject<WorkflowStepLog>(
                JsonConvert.SerializeObject(workflow.Request.RequestHeader)
            );
        if (log == null)
        {
            return;
        }

        if (!string.IsNullOrEmpty(workflow.Request.RequestHeader.CacheExecutionId))
        {
            log.Status = workflow.Response.Status.ToString();
            log.ProcessIn =
                DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                - workflow.Request.RequestHeader.UtcSendTime;
            log.ResponseData =
                workflow.Response.Data is JsonElement
                    ? ((JsonElement)workflow.Response.Data).GetRawText()
                    : JsonConvert.SerializeObject(workflow.Response.Data);
            log.TxContextData = JsonConvert.SerializeObject(
                workflow.Request.RequestHeader.TxContext
            );
        }

        if (isResponse)
        {
            log.Status = workflow.Response.Status.ToString();
            log.ProcessIn =
                DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                - workflow.Request.RequestHeader.UtcSendTime;
            if (workflow.Response.Status == WorkflowScheme.RESPONSE.EnumResponseStatus.ERROR)
            {
                log.ResponseData = string.IsNullOrEmpty(workflow.Response.RawError)
                    ? workflow.Response.ErrorMessage
                    : workflow.Response.RawError;
            }
            else
            {
                log.ResponseData =
                    workflow.Response.Data is JsonElement
                        ? ((JsonElement)workflow.Response.Data).GetRawText()
                        : JsonConvert.SerializeObject(workflow.Response.Data);
            }

            log.TxContextData = JsonConvert.SerializeObject(
                workflow.Request.RequestHeader.TxContext
            );
            await _workflowStepLog.Update(log);
            return;
        }

        log.Status ??= nameof(ExecutionStatus.PROCESSING);
        log.RequestData =
            "{\"WorkflowInput\":"
            + JsonConvert.SerializeObject(workflow.Request.RequestBody.WorkflowInput)
            + ",\"Data\":"
            + ((JsonElement)workflow.Request.RequestBody.Data).GetRawText()
            + "}";
        log.WorkflowScheme = workflow.ToSerialize();
        await _workflowStepLog.Insert(log);
    }

    private bool TryGetCache(LearnApiRequestModel learnApi, out JObject valueCache)
    {
        if (!learnApi.IsCache)
        {
            valueCache = null;
            return false;
        }

        var cacheKey = learnApi.user_sessions.UserCode + learnApi.LearnApiId;
        var fieldKey = Utils.Utility.ComputeMd5Hash(learnApi.ObjectField);
        var cache = _staticCacheManager.GetFieldHash<List<WorkflowScheme>>(cacheKey, fieldKey);
        if (cache != null)
        {
            cache.ForEach(e =>
            {
                e.Request.RequestHeader.CacheExecutionId = e.Request.RequestHeader.ExecutionId;
                e.Request.RequestHeader.ExecutionId = learnApi.ExecuteId;
                _ = WorkflowStepLog(e).ConfigureAwait(false);
            });
            valueCache = BuildResponse(cache);
            return true;
        }

        valueCache = null;
        return false;
    }

    private void SetNewAndReleaseCache(
        LearnApiRequestModel learnApi,
        List<WorkflowScheme> arrayWorkflowScheme
    )
    {
        var cacheKey = learnApi.user_sessions.UserCode + learnApi.LearnApiId;
        if (learnApi.IsCache)
        {
            var fieldKey = Utils.Utility.ComputeMd5Hash(learnApi.ObjectField);
            _staticCacheManager.SetFieldsHash(
                cacheKey,
                new HashEntry(
                    name: fieldKey,
                    value: JsonConvert.SerializeObject(arrayWorkflowScheme)
                )
            );
        }

        var releases = string.IsNullOrEmpty(learnApi.LearnApiIdClear)
            ? []
            : JsonConvert.DeserializeObject<List<string>>(learnApi.LearnApiIdClear);
        releases.ForEach(e =>
        {
            _staticCacheManager.RemoveHash(cacheKey);
        });
    }

    private static JObject BuildResponse(List<WorkflowScheme> response)
    {
        var responseArray = new JArray();
        foreach (var e in response)
        {
            if (e.Response.Data is JsonElement jsonElement)
            {
                responseArray.Add(JToken.Parse(jsonElement.GetRawText()));
            }
            else
            {
                responseArray.Add(e.Response.Data?.ToJToken());
            }
        }

        var result = new JObject { { "response", responseArray } };

        return result.BuildWorkflowResponseSuccess();
    }

    private JObject ExceptionBuildResponse(Exception exception)
    {
        if (exception is O24OpenAPIException o24Exception)
        {
            return o24Exception.Message.BuildWorkflowResponseError(o24Exception.ErrorCode);
        }

        if (_setting.DeveloperMode)
        {
            return (exception.Message + exception.StackTrace).BuildWorkflowResponseError(
                "SYS_00_01"
            );
        }
        return _commonError.BuildWorkflowResponseError("SYS_00_01");
    }

    /// <summary>
    /// Gets the by id using the specified id
    /// </summary>
    /// <param name="id">The id</param>
    /// <returns>A task containing the workflow step</returns>
    public virtual async Task<WorkflowStep> GetById(int id)
    {
        return await _workflowRepo.GetById(id);
    }

    /// <summary>
    /// Gets the by app and id using the specified app
    /// </summary>
    /// <param name="app">The app</param>
    /// <param name="workflowId">The workflow id</param>
    /// <returns>A task containing the workflow step model</returns>
    public virtual async Task<WorkflowStepModel> GetByAppAndId(string app, string workflowId)
    {
        try
        {
            var getWorkflow = await _workflowRepo
                .Table.Where(s => s.AppCode.Equals(app.Trim()) && s.WFId.Equals(workflowId.Trim()))
                .FirstOrDefaultAsync();

            if (getWorkflow == null)
            {
                return null;
            }

            return getWorkflow.ToModel<WorkflowStepModel>();
        }
        catch (System.Exception ex)
        {
            System.Console.WriteLine("GetByApp=Exception=getLearnApi=" + ex.StackTrace);
        }

        return null;
    }

    /// <summary>
    /// Gets the all
    /// </summary>
    /// <returns>A task containing a list of workflow step model</returns>
    public virtual async Task<List<WorkflowStepModel>> GetAll()
    {
        return await _workflowRepo.Table.Select(s => s.ToModel<WorkflowStepModel>()).ToListAsync();
    }

    /// <summary>
    /// Updates the workflow step
    /// </summary>
    /// <param name="workflowStep">The workflow step</param>
    public virtual async Task<WorkflowStep> Update(WorkflowStep workflowStep)
    {
        await _workflowRepo.Update(workflowStep);
        return workflowStep;
    }

    /// <summary>
    /// Deletes the by id using the specified id
    /// </summary>
    /// <param name="id">The id</param>
    /// <returns>The workflow</returns>
    public virtual async Task<WorkflowStep> DeleteById(int id)
    {
        var workflow = await GetById(id);
        await _workflowRepo.Delete(workflow);
        return workflow;
    }

    /// <summary>
    /// Inserts the workflow
    /// </summary>
    /// <param name="workflow">The workflow</param>
    /// <returns>The workflow</returns>
    public virtual async Task<WorkflowStep> Insert(WorkflowStep workflow)
    {
        var findForm = await _workflowRepo
            .Table.Where(s => s.AppCode.Equals(workflow.AppCode) && s.WFId.Equals(workflow.WFId))
            .FirstOrDefaultAsync();
        if (findForm == null)
        {
            await _workflowRepo.Insert(workflow);
        }

        return workflow;
    }

    private async Task<WorkflowScheme> InvokeCrossService(
        WorkflowScheme workflowScheme,
        string FullClassName = "",
        string MethodName = ""
    )
    {
        try
        {
            if (workflowScheme == null)
            {
                throw new ArgumentNullException(
                    nameof(workflowScheme),
                    "Workflow scheme cannot be null."
                );
            }

            var contentModel = new CrossServiceRequest
            {
                FullClassName = FullClassName,
                MethodName = MethodName,
                WorkflowScheme = workflowScheme,
                ProcessNumber = workflowScheme.Request.RequestHeader.ProcessNumber,
            };

            var callApiModel = new CallApiModel
            {
                WorkflowId = workflowScheme.Request.RequestHeader.StepCode,
                Content = contentModel.ToSerialize(),
                Header = new Dictionary<string, string>
                {
                    { "Cache-Control", "no-cache" },
                    { "Content-Type", "application/json" },
                },
                ServiceID = workflowScheme.Request.RequestHeader.ServiceId,
            };

            var response = await _crossWorkflowService.CallApiAsync(callApiModel);

            return response;
        }
        catch (Exception ex)
        {
            workflowScheme.Response.Status = WorkflowScheme.RESPONSE.EnumResponseStatus.ERROR;
            workflowScheme.Response.ErrorMessage = ex.Message;
        }

        return workflowScheme;
    }
}

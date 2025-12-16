using System.Data;
using System.Text.Json;
using System.Text.Json.Nodes;
using O24OpenAPI.Client.Events;
using O24OpenAPI.Client.Events.EventData;
using O24OpenAPI.Client.Lib;
using O24OpenAPI.Client.Scheme.Workflow;
using O24OpenAPI.Client.Workflow;
using O24OpenAPI.Core.Domain;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Core.Helper;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Core.Utils;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Services;
using O24OpenAPI.Framework.Services.Grpc;
using O24OpenAPI.Framework.Services.Mapping;
using O24OpenAPI.WFO.Domain;
using O24OpenAPI.WFO.Lib;
using O24OpenAPI.WFO.Lib.Queue;
using O24OpenAPI.WFO.Models;
using O24OpenAPI.WFO.Services.Interfaces;
using static O24OpenAPI.Client.Scheme.Workflow.WFScheme.REQUEST;
using static O24OpenAPI.Client.Scheme.Workflow.WFScheme.REQUEST.REQUESTHEADER;
using JsonSerializer = System.Text.Json.JsonSerializer;
using WorkflowExecution = O24OpenAPI.WFO.Models.WorkflowExecution;

namespace O24OpenAPI.WFO.Services;

public class WorkflowExecutionService(
    IWorkflowService workflowService,
    IWorkflowStepService workflowStepService,
    WorkflowExecution wfExecution
) : IWorkflowExecutionService, IWorkflowExecutionGrpc
{
    private readonly IWorkflowService _workflowService = workflowService;
    private readonly IWorkflowStepService _workflowStepService = workflowStepService;
    private readonly WorkflowExecution _wfExecution = wfExecution;
    private WorkflowInput _input;
    private readonly IDataMapper _mapper = EngineContext.Current.Resolve<IDataMapper>();
    private readonly IJwtTokenService tokenService =
        EngineContext.Current.Resolve<IJwtTokenService>();
    private WorkflowInfoModel WorkflowInfoModel { get; set; } = new();

    public async Task<WorkflowResponse> StartWorkflowAsync(WorkflowInput input)
    {
        ArgumentNullException.ThrowIfNull(input);
        input.Fields ??= [];
        if (string.IsNullOrEmpty(input.Lang))
        {
            input.Lang = "en";
        }
        _input = input;
        var response = new WorkflowResponse();
        WorkflowExecutionContext context = new();
        try
        {
            _wfExecution.execution = WorkflowInfoModel;

            var (wfDef, steps) = await _workflowService.GetExecutingWorkflow(
                input.WorkflowId,
                input.ChannelId
            );

            if (steps.Count == 0)
            {
                throw new InvalidOperationException(
                    $"Workflow step {input.WorkflowId} is not defined "
                );
            }

            if (string.IsNullOrEmpty(input.ExecutionId))
            {
                input.ExecutionId = Guid.NewGuid().ToString();
            }
            List<WorkflowStep> stepExecutions = [.. steps.OrderBy(x => x.StepOrder)];
            context = new WorkflowExecutionContext(wfDef, stepExecutions)
            {
                ExecutionId = input.ExecutionId,
                CurrentStepIndex = 0,
                Input = input,
                IsReverseFlow = false,
            };

            await ExecuteWorkflow(context, response);
            var mappingResponse = await MapResponse(wfDef, steps);
            response.data = mappingResponse;
            _wfExecution.execution.response_content = mappingResponse;
        }
        catch (ExceptionWithNextAction ewa)
        {
            _wfExecution.execution.status = Enum_STATUS.Error;
            _wfExecution.execution.error = ewa.Message;
            response.error_message = ewa.Message;
            response.error_next_action = ewa.NextAction;
        }
        catch (Exception ex)
        {
            _wfExecution.execution.status = Enum_STATUS.Error;
            _wfExecution.execution.error = ex.Message;
            response.error_message = ex.Message;
        }
        finally
        {
            _wfExecution.execution.finish_on = Common.GetCurrentDateAsLongNumber();
            response.time_in_milliseconds = _wfExecution.execution.GetExecutionTime();
            response.status = _wfExecution.execution.status.ToString();
            response.execution_id = _wfExecution.execution.execution_id;
            response.transaction_date = input.TransactionDate;
            response.value_date = input.ValueDate;
            await PublishWorkflowEvent(context, response);
            _ = TaskUtils.RunAsync(async () => await ExecutionHelper.LogExecution(_wfExecution));
        }
        return response;
    }

    private async Task PublishWorkflowEvent(
        WorkflowExecutionContext context,
        WorkflowResponse response
    )
    {
        try
        {
            if (context.WorkflowEvents == null || context.WorkflowEvents.Count == 0)
            {
                return;
            }

            foreach (var workflowEvent in context.WorkflowEvents)
            {
                if (
                    (
                        _wfExecution.execution.status == Enum_STATUS.Error
                        && workflowEvent.EventType
                            != O24OpenAPIWorkflowEventTypeEnum.EventWorkflowError
                    )
                    || (
                        _wfExecution.execution.status == Enum_STATUS.Completed
                        && workflowEvent.EventType
                            != O24OpenAPIWorkflowEventTypeEnum.EventWorkflowCompleted
                    )
                )
                {
                    continue;
                }
                var eventData = new WorkflowExecutionEventData
                {
                    WorkflowId = _wfExecution.execution.workflow_id,
                    ExecutionId = context.ExecutionId,
                    Data = response.data ?? [],
                    IsSuccess = _wfExecution.execution.status == Enum_STATUS.Completed,
                    ChannelId = context.Input.ChannelId,
                    UserId = context.Input.UserId,
                };
                var o24Event = new O24OpenAPIEvent<WorkflowExecutionEventData>(
                    workflowEvent.EventType
                );
                o24Event.EventData.data = eventData;

                await QueueContext.Instance.SendMessageAsync(
                    QueueUtils.GetEventQueueName(workflowEvent.ServiceHandleEvent),
                    o24Event,
                    "event"
                );
            }
        }
        catch { }
    }

    private static void EnsureContextDataInitialized(WorkflowExecutionContext context)
    {
        if (context == null)
        {
            return;
        }

        context.ContextData ??= new(StringComparer.OrdinalIgnoreCase);

        if (context.ContextData.Count == 0 && context.Input?.Fields is { Count: > 0 })
        {
            foreach (var kv in context.Input.Fields)
            {
                context.ContextData[kv.Key] = kv.Value;
            }
        }
    }

    private async Task ExecuteWorkflow(WorkflowExecutionContext context, WorkflowResponse response)
    {
        try
        {
            EnsureContextDataInitialized(context);
            while (context.CurrentStepIndex < context.StepContexts.Count)
            {
                var stepContext = context.StepContexts[context.CurrentStepIndex];
                var currentStep = stepContext.WorkflowStep;
                var sendingCondition = currentStep.SendingCondition;
                if (!string.IsNullOrWhiteSpace(sendingCondition))
                {
                    var condition = JsonSerializer.Deserialize<Condition>(sendingCondition);
                    var isValid = ConditionEvaluator.Evaluate(context.ContextData, condition);
                    if (!isValid)
                    {
                        var stepInfo = new WorkflowStepInfoModel
                        {
                            step_execution_id =
                                $"{context.ExecutionId}_{context.CurrentStepIndex + 1}",
                            execution_id = context.ExecutionId,
                            step_code = currentStep.StepCode,
                            step_order = currentStep.StepOrder,
                            p1_status = Enum_STATUS.Skipped,
                            p2_status = Enum_STATUS.Skipped,
                            sending_condition = condition,
                            p1_content = GetSkippedContent(condition),
                        };
                        _wfExecution.execution_steps.Add(stepInfo);
                        context.CurrentStepIndex++;
                        continue;
                    }
                }
                string currentQueueName = QueueUtils.GetCommandQueueName(currentStep.ServiceId);
                try
                {
                    var scheme = await BuildWFScheme(
                        context,
                        currentStep,
                        context.CurrentStepIndex + 1
                    );
                    stepContext.WFScheme = scheme;
                    var stepInfo = new WorkflowStepInfoModel(scheme)
                    {
                        should_await = currentStep.ShouldAwaitStep,
                    };

                    if (currentStep.ShouldAwaitStep)
                    {
                        try
                        {
                            WFScheme wfSchemeResponse =
                                await QueueContext.Instance.SendAndWaitResponse(
                                    currentQueueName,
                                    scheme
                                );

                            if (!wfSchemeResponse.response.IsSuccess())
                            {
                                stepInfo.p1_status = Enum_STATUS.Error;
                                stepInfo.p1_error = wfSchemeResponse.response.error_code;
                                response.error_code = wfSchemeResponse.response.error_code;
                                response.error_message = wfSchemeResponse.response.error_message;
                                stepInfo.execution_servcie = wfSchemeResponse
                                    .request
                                    .request_header
                                    .service_instance_id;
                                stepInfo.p1_content = new Dictionary<string, object>
                                {
                                    { "error", wfSchemeResponse.response.error_message },
                                };
                                if (currentStep.IsReverse)
                                {
                                    context.IsReverseFlow = true;
                                }
                                throw new ExceptionWithNextAction(
                                    "38941",
                                    $"[{stepInfo.step_code}] [{stepInfo.p1_error}] {wfSchemeResponse.response.error_message}",
                                    wfSchemeResponse.response.error_next_action
                                );
                            }
                            else
                            {
                                stepInfo.p1_status = Enum_STATUS.Completed;
                                stepInfo.p1_content = wfSchemeResponse.response.data;
                                stepInfo.execution_servcie = wfSchemeResponse
                                    .request
                                    .request_header
                                    .service_instance_id;
                                if (
                                    wfSchemeResponse.response.data
                                        is Dictionary<string, object> data
                                    && data.Count > 0
                                )
                                {
                                    foreach (var kv in data)
                                    {
                                        context.ContextData[kv.Key] = kv.Value;
                                    }
                                }
                            }
                        }
                        catch (TimeoutException)
                        {
                            stepInfo.p1_status = Enum_STATUS.Error;
                            stepInfo.p1_error = $"Step [{stepInfo.step_code}] timeout";
                            _wfExecution.execution.is_timeout = "Y";
                            response.error_code = "TIMEOUT";
                            context.IsReverseFlow = true;
                            throw;
                        }
                        catch (Exception)
                        {
                            context.IsReverseFlow = true;
                            await ReverseWorkflow(context);
                            throw;
                        }
                        finally
                        {
                            stepInfo.p1_finish = Common.GetCurrentDateAsLongNumber();
                            _wfExecution.execution_steps.Add(stepInfo);
                        }
                    }
                    else
                    {
                        await QueueContext.Instance.SendWorkflow(currentQueueName, scheme);
                        stepInfo.p1_status = Enum_STATUS.Completed;
                        stepInfo.p1_finish = Common.GetCurrentDateAsLongNumber();
                        stepInfo.execution_servcie = scheme
                            .request
                            .request_header
                            .service_instance_id;
                        stepInfo.p1_content = new Dictionary<string, object>
                        {
                            { "status", "sent" },
                            { "info", "One-way message sent successfully" },
                            { "message_type", "NOWAIT" },
                        };
                        _wfExecution.execution_steps.Add(stepInfo);
                    }

                    context.CurrentStepIndex++;
                }
                catch (Exception ex)
                {
                    _wfExecution.execution.status = Enum_STATUS.Error;
                    _wfExecution.execution.error = ex.Message;
                    context.IsReverseFlow = true;
                    await ReverseWorkflow(context);
                    throw;
                }
            }
            _wfExecution.execution.status = Enum_STATUS.Completed;
        }
        catch (Exception ex)
        {
            _wfExecution.execution.status = Enum_STATUS.Error;
            _wfExecution.execution.error =
                $"{ex.Message} | StackTrace: {ex.StackTrace?.Substring(0, Math.Min(ex.StackTrace?.Length ?? 0, 500))}";
            throw;
        }
    }

    private void ExecuteIfStepExists(int index, Action action)
    {
        if (
            index >= 0
            && index < _wfExecution.execution_steps.Count
            && _wfExecution.execution_steps[index] != null
        )
        {
            action();
        }
    }

    private async Task ReverseWorkflow(WorkflowExecutionContext context)
    {
        _wfExecution.execution.reversed_execution_id = context.ExecutionId;
        context.CurrentStepIndex--;

        while (context.CurrentStepIndex >= 0)
        {
            ExecuteIfStepExists(
                context.CurrentStepIndex,
                () =>
                {
                    _wfExecution.execution_steps[context.CurrentStepIndex].p2_start =
                        Common.GetCurrentDateAsLongNumber();
                }
            );
            var stepContext = context.StepContexts[context.CurrentStepIndex];
            var step = stepContext.WorkflowStep;
            if (step.IsReverse)
            {
                try
                {
                    var scheme = stepContext.WFScheme;
                    scheme.request.request_header.workflow_type = EnumWorkflowType.reverse;
                    scheme.request.request_header.reversal_execution_id = context.ExecutionId;
                    scheme.request.request_header.is_compensated = "Y";
                    if (!string.IsNullOrEmpty(step.SubSendingTemplate))
                    {
                        var subMapping = await _mapper.MapDataToDictionaryAsync(
                            context.Input.Fields.DictionaryToJsonNode(),
                            JsonNode.Parse(step.SubSendingTemplate),
                            WFOMapping
                        );
                        foreach (var kvp in subMapping)
                        {
                            scheme.request.request_body.data[kvp.Key] = kvp.Value;
                        }
                    }
                    ExecuteIfStepExists(
                        context.CurrentStepIndex,
                        () =>
                        {
                            _wfExecution.execution_steps[context.CurrentStepIndex].p2_request =
                                scheme;
                        }
                    );

                    var schemeResponse = await QueueContext.Instance.SendAndWaitResponse(
                        QueueUtils.GetCommandQueueName(step.ServiceId),
                        scheme
                    );
                }
                catch (TimeoutException)
                {
                    ExecuteIfStepExists(
                        context.CurrentStepIndex,
                        () =>
                        {
                            _wfExecution.execution_steps[context.CurrentStepIndex].p2_error =
                                $"Step [{step.StepCode}] timeout";
                            _wfExecution.execution_steps[context.CurrentStepIndex].p2_status =
                                Enum_STATUS.Completed;
                        }
                    );
                    _wfExecution.execution.is_timeout = "Y";
                    context.CurrentStepIndex--;
                    continue;
                }
                finally
                {
                    ExecuteIfStepExists(
                        context.CurrentStepIndex,
                        () =>
                        {
                            _wfExecution.execution_steps[context.CurrentStepIndex].p2_finish =
                                Common.GetCurrentDateAsLongNumber();
                        }
                    );
                }
            }

            ExecuteIfStepExists(
                context.CurrentStepIndex,
                () =>
                {
                    _wfExecution.execution_steps[context.CurrentStepIndex].p2_finish =
                        Common.GetCurrentDateAsLongNumber();
                    _wfExecution.execution_steps[context.CurrentStepIndex].p2_status =
                        Enum_STATUS.Completed;
                }
            );

            context.CurrentStepIndex--;
        }
    }

    private async Task<WFScheme> BuildWFScheme(
        WorkflowExecutionContext context,
        WorkflowStep currentStep,
        int currentStepIndex
    )
    {
        var scheme = new WFScheme();
        scheme.request.request_header = new REQUESTHEADER
        {
            step_code = currentStep.StepCode,
            execution_id = context.ExecutionId,
            step_execution_id = $"{context.ExecutionId}_{currentStepIndex}",
            step_order = currentStep.StepOrder,
            step_mode = currentStep.ShouldAwaitStep ? "TWOWAY" : "ONEWAY",
            from_queue_name = QueueUtils.GetCommandQueueName("WFO"),
            to_queue_name = QueueUtils.GetCommandQueueName(currentStep.ServiceId),
            server_ip = Common.GetLocalIPAddress(),
            utc_send_time = Common.GetCurrentDateAsLongNumber(),
            step_timeout = currentStep.StepTimeout,
            user_id = context.Input.UserId,
            channel_id = context.Input.ChannelId,
            workflow_type = EnumWorkflowType.normal,
            client_device_id = context.Input.Device.DeviceId,
            processing_version = currentStep.ProcessingNumber,
            fanout_exchange = currentStep.ShouldAwaitStep ? null : "exchange_log_workflow_step",
            user_code = tokenService.GetUserCodeFromToken(context.Input.Token, true),
        };
        scheme.request.request_body = new REQUESTBODY
        {
            workflow_input = context.Input,
            data =
                !string.IsNullOrEmpty(currentStep.SendingTemplate) && context.Input.Fields.Count > 0
                    ? await _mapper.MapDataToDictionaryAsync(
                        context.Input.Fields.DictionaryToJsonNode(),
                        JsonNode.Parse(currentStep.SendingTemplate),
                        WFOMapping
                    )
                    : [],
        };
        return scheme;
    }

    private Task<object> WFOMapping(string jsonPath)
    {
        ArgumentNullException.ThrowIfNull(jsonPath);

        return jsonPath switch
        {
            string path when path.StartsWith("$.", StringComparison.Ordinal) => SelectFromSource(
                _input,
                path[2..]
            ),

            _ => SelectFromSource(_wfExecution, jsonPath),
        };
    }

    private static Task<object> SelectFromSource(object source, string path)
    {
        var json = JsonSerializer.Serialize(source);
        var node = JsonNode.Parse(json);

        var value = node.GetValueByPath(path);

        if (value == null)
        {
            return Task.FromResult<object>(null);
        }

        if (value is JsonObject obj)
        {
            var dict = obj.ToDictionary(kvp => kvp.Key, kvp => kvp.Value?.Deserialize<object>());
            return Task.FromResult((object)dict);
        }

        return Task.FromResult(value.Deserialize<object>());
    }

    private async Task<Dictionary<string, object>> MapResponse(
        WorkflowDef workflowDef,
        List<WorkflowStep> workflowSteps
    )
    {
        var result = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        if (!string.IsNullOrEmpty(workflowDef.TemplateResponse))
        {
            return await _mapper.MapDataToDictionaryAsync(
                _wfExecution.ToJsonNode(),
                workflowDef.TemplateResponse.ToJsonNode()
            );
        }

        if (workflowSteps.Any(s => !string.IsNullOrEmpty(s.MappingResponse)))
        {
            var tasks = new List<Task<Dictionary<string, object>>>();

            for (int i = 0; i < workflowSteps.Count; i++)
            {
                if (string.IsNullOrEmpty(workflowSteps[i].MappingResponse))
                {
                    continue;
                }

                tasks.Add(
                    _mapper.MapDataToDictionaryAsync(
                        _wfExecution.execution_steps[i].ToJsonNode(),
                        workflowSteps[i].MappingResponse.ToJsonNode()
                    )
                );
            }

            var mappings = await Task.WhenAll(tasks);
            foreach (var dic in mappings)
            {
                foreach (var kvp in dic)
                {
                    result[kvp.Key] = kvp.Value;
                }
            }
            return result;
        }

        foreach (var item in _wfExecution.execution_steps)
        {
            if (item.p1_content is Dictionary<string, object> dic)
            {
                foreach (var kvp in dic)
                {
                    result[kvp.Key] = kvp.Value;
                }
            }
            else if (item.p1_content is JsonElement jsonElement)
            {
                if (jsonElement.ValueKind == JsonValueKind.Object)
                {
                    var dic1 = JsonSerializer.Deserialize<Dictionary<string, object>>(
                        jsonElement.GetRawText()
                    );
                    if (dic1 != null)
                    {
                        foreach (var kvp in dic1)
                        {
                            result[kvp.Key] = kvp.Value;
                        }
                    }
                }
            }
            else if (item.p1_content is string jsonString && jsonString.StartsWith('{'))
            {
                var dic2 = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonString);
                if (dic2 != null)
                {
                    foreach (var kvp in dic2)
                    {
                        result[kvp.Key] = kvp.Value;
                    }
                }
            }
            else
            {
                var dic3 = JsonSerializer.Deserialize<Dictionary<string, object>>(
                    JsonSerializer.Serialize(item.p1_content),
                    SerializerOptions.JsonSerializerOptions
                );
                if (dic3 != null)
                {
                    foreach (var kvp in dic3)
                    {
                        result[kvp.Key] = kvp.Value;
                    }
                }
            }
        }

        return result;
    }

    public async Task<WorkflowResponse> ExecuteWorkflowByGrpc(string input)
    {
        var workflowInput = JsonSerializer.Deserialize<WorkflowInput>(
            input,
            SerializerOptions.JsonSerializerOptions
        );
        WorkflowInfoModel = new WorkflowInfoModel(workflowInput)
        {
            input_string = input,
            correlation_id = EngineContext.Current.Resolve<WorkContext>().ExecutionLogId,
        };
        var response = await StartWorkflowAsync(workflowInput);
        return response;
    }

    private static Dictionary<string, object> GetSkippedContent(object condition)
    {
        return new Dictionary<string, object>
        {
            ["status"] = "skipped",
            ["reason"] = "Condition not met",
            ["evaluatedCondition"] = condition,
        };
    }
}

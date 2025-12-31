using Linh.CodeEngine.Core;
using LinKit.Core.Abstractions;
using LinKit.Core.Cqrs;
using LinKit.Json.Runtime;
using Microsoft.Extensions.Logging;
using O24OpenAPI.Client.Enums;
using O24OpenAPI.Client.Scheme.Workflow;
using O24OpenAPI.Core;
using O24OpenAPI.Core.Domain;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Data;
using O24OpenAPI.Framework.Abstractions;
using O24OpenAPI.Framework.Domain;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Helpers;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.Framework.Models.Logging;
using O24OpenAPI.Framework.Repositories;
using O24OpenAPI.Framework.Services.Configuration;
using O24OpenAPI.Framework.Services.Logging;
using O24OpenAPI.Framework.Services.Queue;
using System.Text.Json;

namespace O24OpenAPI.Framework.Services;

public class StoredProcedureStepConfig
{
    public string StoredProcedureName { get; set; } = "";

    public bool IsReplacePostingInContext { get; set; } = false;
}

public interface IO24OpenAPIServiceManager
{
    Task<WFScheme> ConsumeWorkflow(WFScheme workflow);
}

[RegisterService(Lifetime.Scoped)]
public class O24OpenAPIServiceManager(
    IEntityAuditRepository entityAuditRepository,
    IO24OpenAPIDataProvider dataProvider
) : IO24OpenAPIServiceManager
{
    public async Task<WFScheme> ConsumeWorkflow(WFScheme workflow)
    {
        if (workflow == null)
        {
            return null;
        }

        try
        {
            WorkContext workContext = EngineContext.Current.Resolve<WorkContext>();
            workflow.SetWorkContext(workContext);

            if (workflow.request.request_header.processing_version == ProcessNumber.ExecuteCommand)
            {
                return await HandleExecuteCommand(workflow);
            }

            string stepCode = workflow.request.request_header.step_code;
            O24OpenAPIService stepConfig = await GetMappingByCode(stepCode);

            if (workflow.request.request_header.processing_version == ProcessNumber.StoredProcedure)
            {
                return await HandleStoredProcedureWorkflow(workflow, stepConfig);
            }

            return await HandleStandardWorkflow(workflow, stepConfig);
        }
        catch (Exception ex)
        {
            return await ErrorWorkflow(workflow, ex);
        }
    }

    private static bool TryParseStoredProcedureParameter(
        string input,
        out StoredProcedureStepConfig result
    )
    {
        try
        {
            result = JsonSerializer.Deserialize<StoredProcedureStepConfig>(input);
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    private static async Task<WFScheme> HandleStoredProcedureWorkflow(
        WFScheme workflow,
        O24OpenAPIService mapping
    )
    {
        try
        {
            string fullClassName = mapping.FullClassName;
            string methodName = mapping.MethodName;
            BaseTransactionModel baseTranModel = await workflow.ToModel<BaseTransactionModel>();
            var json = JsonSerializer.Serialize(baseTranModel);
            Dictionary<string, object> dictionary = JsonSerializer.Deserialize<
                Dictionary<string, object>
            >(json);
            workflow.request.request_header.tx_context = dictionary;
            Console.WriteLine($"wfScheme == {JsonSerializer.Serialize(workflow)}");
            if (
                TryParseStoredProcedureParameter(
                    fullClassName,
                    out StoredProcedureStepConfig parameterConfig
                )
            )
            {
                return await BaseQueue.Invoke2<BaseTransactionModel>(workflow, parameterConfig);
            }
            return await workflow.InvokeAsync(fullClassName, methodName);
        }
        catch (Exception ex)
        {
            return await ErrorWorkflow(workflow, ex);
        }
    }

    private async Task<WFScheme> HandleStandardWorkflow(
        WFScheme workflow,
        O24OpenAPIService mapping
    )
    {
        try
        {
            if (
                workflow.request.request_header.is_compensated != "Y"
                && mapping.IsAutoReverse != true
            )
            {
                if (mapping.IsModuleExecute == true)
                {
                    var executionResult = await DynamicCodeEngine.ExecuteAsync(
                        moduleName: mapping.FullClassName,
                        methodName: mapping.MethodName,
                        parameters: [workflow]
                    );
                    return executionResult as WFScheme;
                }
                try
                {
                    var stepInvoker = EngineContext.Current.Resolve<IWorkflowStepInvoker>(mapping.MediatorKey) ?? throw new InvalidOperationException($"IWorkflowStepInvoker resolved null. Key='{mapping.MediatorKey}'");

                    Task<WFScheme> scheme = BaseQueue.Invoke<BaseTransactionModel>(
                        workflow,
                        async () =>
                        {
                            return await stepInvoker.InvokeAsync(
                                mapping.StepCode,
                                workflow,
                                EngineContext.Current.Resolve<IMediator>(mapping.MediatorKey),
                                CancellationToken.None
                            );
                        }
                    );
                    return await scheme;
                }
                catch (KeyNotFoundException)
                {
                    return await workflow.InvokeAsync(mapping.FullClassName, mapping.MethodName);
                }
                catch (InvalidOperationException)
                {
                    return await workflow.InvokeAsync(mapping.FullClassName, mapping.MethodName);
                }
                catch (ArgumentException)
                {
                    return await workflow.InvokeAsync(mapping.FullClassName, mapping.MethodName);
                }
                throw new O24OpenAPIException(
                    $"Workflow step invoker could not be resolved for step code '{mapping.StepCode}'."
                );
            }
            else
            {
                var entityAudits = await entityAuditRepository.GetByExecutionIdAsync(
                    workflow.request.request_header.execution_id
                );
                var revertSql = await EntityAuditReverter.GenerateRevertSqlBlock(
                    entityAudits,
                    workflow.request.request_header.execution_id
                );
                await dataProvider.ExecuteNonQuery(revertSql);
                workflow.response.status = WFScheme.RESPONSE.EnumResponseStatus.SUCCESS;
                workflow.response.data = JsonSerializer.Serialize(new { });
                _ = entityAuditRepository.BulkDelete(entityAudits);
                return workflow;
            }
        }
        catch (Exception ex)
        {
            return await ErrorWorkflow(workflow, ex);
        }
    }

    private static async Task<WFScheme> HandleExecuteCommand(WFScheme workflow)
    {
        try
        {
            var invoker = new CommandInvoker();
            return await invoker.InvokeCommand(workflow);
        }
        catch (Exception ex)
        {
            return await ErrorWorkflow(workflow, ex);
        }
    }

    private static async Task<O24OpenAPIService> GetMappingByCode(string stepCode)
    {
        IO24OpenAPIMappingService service =
            EngineContext.Current.Resolve<IO24OpenAPIMappingService>()
            ?? throw new O24OpenAPIException("Mapping service could not be resolved.");

        O24OpenAPIService mapping =
            await service.GetByStepCode(stepCode)
            ?? throw new O24OpenAPIException(
                $"Step code '{stepCode}' not found in O24OpenAPIService. Please check service registration."
            );

        return mapping;
    }

    private static async Task<WFScheme> ErrorWorkflow(
        WFScheme workflow,
        Exception e,
        string transactionNumber = ""
    )
    {
        string errorCode = "ERROR";
        Exception ex = e;
        if (e.InnerException != null)
        {
            ex = e.InnerException;
        }
        string errorMessage = ex.Message;
        if (ex is O24OpenAPIException o24OpenAPIException)
        {
            string err = o24OpenAPIException.ErrorCode;
            if (!string.IsNullOrEmpty(err))
            {
                errorCode = err;
            }
        }
        if (ex is ExceptionWithNextAction exceptionWithNextAction)
        {
            string err = exceptionWithNextAction.ErrorCode;
            if (!string.IsNullOrEmpty(err))
            {
                errorCode = err;
            }

            string action = exceptionWithNextAction.NextAction;

            if (!string.IsNullOrEmpty(action))
            {
                workflow.response.error_next_action = action;
            }
        }
        else
        {
            try
            {
                StoreProcedureErrorModel errorResponse =
                    JsonSerializer.Deserialize<StoreProcedureErrorModel>(
                        ex.Message,
                        new JsonSerializerOptions
                        {
                            AllowTrailingCommas = true,
                            PropertyNameCaseInsensitive = true,
                        }
                    );

                if (
                    errorResponse != null
                    && !string.IsNullOrWhiteSpace(errorResponse.error_message)
                )
                {
                    errorCode = !string.IsNullOrEmpty(errorResponse.error_code)
                        ? errorResponse.error_code
                        : errorCode;
                    errorMessage = errorResponse.error_message;
                }
            }
            catch (JsonException) { }
            catch (Exception) { }
        }
        workflow.response.error_code = errorCode;
        workflow.response.error_message = errorMessage;
        try
        {
            var errorDict = ex.ToDictionary("ERROR_MESSAGE");
            workflow.response.data =
                errorDict?.ToJson() ?? JsonSerializer.Serialize(new { ERROR_MESSAGE = ex.Message });
        }
        catch
        {
            workflow.response.data = JsonSerializer.Serialize(new { ERROR_MESSAGE = ex.Message });
        }
        workflow.response.status = WFScheme.RESPONSE.EnumResponseStatus.ERROR;
        if (ex is NotFoundException exception)
        {
            workflow.response.data = exception.BaseModel;
        }
        if (errorCode == "ERROR")
        {
            Logging.ILogger logger = EngineContext.Current.Resolve<Logging.ILogger>();
            if (logger != null)
            {
                await logger.Error(
                    ex.Message ?? "",
                    ex,
                    null,
                    workflow.request.request_header.step_code + transactionNumber
                );
            }
        }
        var callLogModel = new CallLogRequest
        {
            LogLevelId = (int)LogLevel.Error,
            ChannelId = workflow.request.request_header.channel_id,
            Status = "E",
            Code = errorCode,
            ShortMessage = errorMessage,
            FullMessage = ex.ToString(),
            Data = workflow.response.data?.ToSerialize(),
            Reference = workflow.request.request_header.execution_id,
            IpAddress = workflow.request.request_header.server_ip,
        };
        await DefaultLogger.CallLog(callLogModel);
        return workflow;
    }
}

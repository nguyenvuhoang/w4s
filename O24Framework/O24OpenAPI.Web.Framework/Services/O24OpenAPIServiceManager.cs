using System.Text.Json;
using Linh.CodeEngine.Core;
using Linh.JsonKit.Json;
using Microsoft.Extensions.Logging;
using O24OpenAPI.Core;
using O24OpenAPI.Core.Domain;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.O24OpenAPIClient.Enums;
using O24OpenAPI.O24OpenAPIClient.Scheme.Workflow;
using O24OpenAPI.Web.Framework.Domain;
using O24OpenAPI.Web.Framework.Exceptions;
using O24OpenAPI.Web.Framework.Extensions;
using O24OpenAPI.Web.Framework.Helpers;
using O24OpenAPI.Web.Framework.Models;
using O24OpenAPI.Web.Framework.Models.Logging;
using O24OpenAPI.Web.Framework.Models.O24OpenAPI;
using O24OpenAPI.Web.Framework.Services.Configuration;
using O24OpenAPI.Web.Framework.Services.Logging;
using O24OpenAPI.Web.Framework.Services.Queue;

namespace O24OpenAPI.Web.Framework.Services;

/// <summary>
/// The stored procedure step config class
/// </summary>
public class StoredProcedureStepConfig
{
    /// <summary>
    /// Gets or sets the value of the stored procedure name
    /// </summary>
    public string StoredProcedureName { get; set; } = "";

    /// <summary>
    /// Gets or sets the value of the is replace posting in context
    /// </summary>
    public bool IsReplacePostingInContext { get; set; } = false;
}

/// <summary>
/// The 24 open api service manager class
/// </summary>
public class O24OpenAPIServiceManager
{
    /// <summary>
    /// Gets the mapping by code using the specified step code
    /// </summary>
    /// <param name="stepCode">The step code</param>
    /// <exception cref="O24OpenAPIException"></exception>
    /// <returns>The mapping</returns>
    private static async Task<O24OpenAPIService> GetMappingByCode(string stepCode)
    {
        var service =
            EngineContext.Current.Resolve<IO24OpenAPIMappingService>()
            ?? throw new O24OpenAPIException("Mapping service could not be resolved.");

        var mapping =
            await service.GetByStepCode(stepCode)
            ?? throw new O24OpenAPIException(
                $"Step code '{stepCode}' not found in O24OpenAPIService. Please check service registration."
            );

        return mapping;
    }

    /// <summary>
    /// Errors the workflow using the specified workflow
    /// </summary>
    /// <param name="workflow">The workflow</param>
    /// <param name="e">The </param>
    /// <param name="transactionNumber">The transaction number</param>
    /// <returns>The workflow</returns>
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
                var errorResponse = JsonSerializer.Deserialize<StoreProcedureErrorModel>(
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

    /// <summary>
    /// Consumes the workflow 1 using the specified workflow
    /// </summary>
    /// <param name="workflow">The workflow</param>
    /// <returns>A task containing the wf scheme</returns>
    public static async Task<WFScheme> ConsumeWorkflow1(WFScheme workflow)
    {
        if (workflow == null)
        {
            return null;
        }

        try
        {
            var workContext = EngineContext.Current.Resolve<WorkContext>();
            workflow.SetWorkContext(workContext);

            if (workflow.request.request_header.processing_version == ProcessNumber.ExecuteCommand)
            {
                return await HandleExecuteCommand(workflow);
            }

            string stepCode = workflow.request.request_header.step_code;
            var stepConfig = await GetMappingByCode(stepCode);

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

    /// <summary>
    /// Tries the parse stored procedure parameter using the specified input
    /// </summary>
    /// <param name="input">The input</param>
    /// <param name="result">The result</param>
    /// <returns>The bool</returns>
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

    /// <summary>
    /// Handles the stored procedure workflow using the specified workflow
    /// </summary>
    /// <param name="workflow">The workflow</param>
    /// <param name="mapping">The mapping</param>
    /// <returns>A task containing the wf scheme</returns>
    private static async Task<WFScheme> HandleStoredProcedureWorkflow(
        WFScheme workflow,
        O24OpenAPIService mapping
    )
    {
        try
        {
            string fullClassName = mapping.FullClassName;
            string methodName = mapping.MethodName;
            var baseTranModel = await workflow.ToModel<BaseTransactionModel>();
            var json = JsonSerializer.Serialize(baseTranModel);
            var dictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
            workflow.request.request_header.tx_context = dictionary;
            Console.WriteLine($"wfScheme == {JsonSerializer.Serialize(workflow)}");
            if (TryParseStoredProcedureParameter(fullClassName, out var parameterConfig))
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

    /// <summary>
    /// Handles the standard workflow using the specified workflow
    /// </summary>
    /// <param name="workflow">The workflow</param>
    /// <param name="mapping">The mapping</param>
    /// <returns>A task containing the wf scheme</returns>
    private static async Task<WFScheme> HandleStandardWorkflow(
        WFScheme workflow,
        O24OpenAPIService mapping
    )
    {
        try
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
            return await workflow.InvokeAsync(mapping.FullClassName, mapping.MethodName);
        }
        catch (Exception ex)
        {
            return await ErrorWorkflow(workflow, ex);
        }
    }

    /// <summary>
    /// Handles the execute command using the specified workflow
    /// </summary>
    /// <param name="workflow">The workflow</param>
    /// <returns>A task containing the wf scheme</returns>
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
}

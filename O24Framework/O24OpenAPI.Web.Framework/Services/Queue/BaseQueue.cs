using FluentValidation.Results;
using Linh.JsonKit.Json;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using O24OpenAPI.Contracts.Configuration.Client;
using O24OpenAPI.Core;
using O24OpenAPI.Core.Helper;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.O24OpenAPIClient.Scheme.Workflow;
using O24OpenAPI.Web.Framework.DBContext;
using O24OpenAPI.Web.Framework.Domain;
using O24OpenAPI.Web.Framework.Extensions;
using O24OpenAPI.Web.Framework.Models;
using O24OpenAPI.Web.Framework.Models.O24OpenAPI;
using O24OpenAPI.Web.Framework.Services.Configuration;
using O24OpenAPI.Web.Framework.Services.Logging;
using System.Text.Json.Nodes;
using System.Transactions;

namespace O24OpenAPI.Web.Framework.Services.Queue;

/// <summary>
/// The base queue class
/// </summary>
public abstract class BaseQueue
{
    /// <summary>
    /// The semaphore slim
    /// </summary>
    private static readonly SemaphoreSlim _semaphore = new(1, 1);

    /// <summary>
    /// Return workflow with Success status
    /// </summary>
    /// <param name="workflow"></param>
    /// <param name="name"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    public static WFScheme Success(WFScheme workflow, string name, object content)
    {
        workflow.response.data = content.ToDictionary(name);
        workflow.response.status = WFScheme.RESPONSE.EnumResponseStatus.SUCCESS;
        return workflow;
    }

    /// <summary>
    /// Return workflow with Success status
    /// </summary>
    /// <param name="workflow"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    public static WFScheme Success(WFScheme workflow, object content)
    {
        if (content is string || content is ValueType) // Nếu là kiểu dữ liệu cơ bản
        {
            workflow.response.data = new { data = content };
        }
        else if ((content is IEnumerable<object> || content is Array) && content is not JObject) // Nếu là List, Array, IEnumerable
        {
            workflow.response.data = new { data = content };
        }
        else
        {
            workflow.response.data = content; // Nếu là object, JObject, Dictionary thì giữ nguyên
        }

        workflow.response.status = WFScheme.RESPONSE.EnumResponseStatus.SUCCESS;
        return workflow;
    }

    /// <summary>
    /// Return workflow with Error status
    /// </summary>
    /// <param name="workflow"></param>
    /// <param name="validationResult">Validation results</param>
    /// <param name="errorMessage">Validation results</param>
    /// <returns></returns>
    public static WFScheme Error<TModel>(
        WFScheme workflow,
        ValidationResult validationResult,
        string errorMessage = ""
    )
        where TModel : BaseTransactionModel
    {
        workflow.response.data = new { };
        if (string.IsNullOrEmpty(errorMessage))
        {
            workflow.response.error_code = "ERRROR";
            workflow.response.error_message = string.Join("\n", validationResult.Errors);
        }
        else
        {
            workflow.response.error_code = "ERROR";
            workflow.response.error_message = errorMessage;
        }
        if (validationResult.Errors.Any())
        {
            workflow.response.error_code = string.Join(
                "\n",
                validationResult.Errors.Select(e => e.ErrorCode)
            );
        }
        workflow.response.data = null;
        workflow.response.status = WFScheme.RESPONSE.EnumResponseStatus.ERROR;
        try
        {
            //EngineContext
            //    .Current.Resolve<IGrpcService>()
            //    ?.SendCentralizedLog(workflow.response.error_message);
        }
        catch { }
        return workflow;
    }

    /// <summary>
    /// Invokes the workflow
    /// </summary>
    /// <typeparam name="TModel">The model</typeparam>
    /// <param name="workflow">The workflow</param>
    /// <param name="acquire">The acquire</param>
    /// <param name="keyName">The key name</param>
    /// <param name="removeSensitive">The remove sensitive</param>
    /// <param name="replacePosting">The replace posting</param>
    /// <param name="useTransactionScope">The use transaction scope</param>
    /// <param name="singleThread">The single thread</param>
    /// <exception cref="O24OpenAPIException">Mapping not found for stepCode: {stepCode}</exception>
    /// <exception cref="O24OpenAPIException">system_busy System is busy now, please try again later</exception>
    /// <returns>A task containing the wf scheme</returns>
    public static async Task<WFScheme> Invoke<TModel>(
        WFScheme workflow,
        Func<Task<object>> acquire,
        string keyName = "",
        bool removeSensitive = true,
        bool replacePosting = false,
        bool useTransactionScope = true,
        bool singleThread = true
    )
        where TModel : BaseTransactionModel
    {
        object returnObject = null;
        O24OpenAPIService stepConfig = null;

        try
        {
            if (
                workflow.request.request_header.processing_version
                != O24OpenAPIClient.Enums.ProcessNumber.ExecuteCommand
            )
            {
                string stepCode = workflow.request.request_header.step_code;
                var mappingService = EngineContext.Current.Resolve<IO24OpenAPIMappingService>();
                stepConfig =
                    await mappingService.GetByStepCode(stepCode)
                    ?? throw new O24OpenAPIException(
                        $"Mapping not found for stepCode: {stepCode}"
                    );
            }

            if (singleThread && stepConfig != null && !stepConfig.IsInquiry)
            {
                await _semaphore.WaitAsync();
                useTransactionScope = true;
            }
            else if (stepConfig == null || stepConfig.IsInquiry)
            {
                useTransactionScope = false;
            }
            try
            {
                returnObject = await DoTransaction(
                    workflow,
                    acquire,
                    workflow.request.request_header.IsReversal(),
                    null,
                    removeSensitive,
                    IsCompensated: false,
                    useTransactionScope,
                    stepConfig?.IsInquiry ?? true
                );
            }
            catch (SqlException ex) when (ex.Number == 1205)
            {
                var logger = EngineContext.Current.Resolve<ILogger>();
                if (logger != null)
                {
                    await logger.Error(
                        ex.Message ?? "Deadlock occurred",
                        ex,
                        null,
                        workflow.request.request_header.execution_id
                    );
                }

                throw new O24OpenAPIException(
                    "system_busy",
                    "System is busy now, please try again later"
                );
            }
            catch (Exception ex)
            {
                var logger = EngineContext.Current.Resolve<ILogger>();
                if (logger != null)
                {
                    await logger.Error(
                        "Unexpected error in Invoke method",
                        ex,
                        null,
                        workflow.request.request_header.execution_id
                    );
                }
                throw;
            }
            if (returnObject is not null)
            {
                if (returnObject is JsonNode jsonNode)
                {
                    returnObject = jsonNode.ToJson()?.FromJson<object>();
                }
            }
            return string.IsNullOrEmpty(keyName)
                ? Success(workflow, returnObject)
                : Success(workflow, keyName, returnObject);
        }
        finally
        {
            if (singleThread && !stepConfig?.IsInquiry == true)
            {
                _semaphore.Release();
            }
        }
    }

    /// <summary>
    /// Invokes the 2 using the specified workflow
    /// </summary>
    /// <typeparam name="TRequestModel">The request model</typeparam>
    /// <param name="workflow">The workflow</param>
    /// <param name="storedProcedureParameters">The stored procedure parameters</param>
    /// <param name="checkValidation">The check validation</param>
    /// <returns>A task containing the wf scheme</returns>
    public static async Task<WFScheme> Invoke2<TRequestModel>(
        WFScheme workflow,
        StoredProcedureStepConfig storedProcedureParameters,
        Func<Task<bool>> checkValidation = null
    )
        where TRequestModel : BaseTransactionModel
    {
        return await Invoke2<TRequestModel>(
            workflow,
            storedProcedureParameters.StoredProcedureName,
            storedProcedureParameters.IsReplacePostingInContext,
            null
        );
    }

    /// <summary>
    /// Invokes the 2 using the specified workflow
    /// </summary>
    /// <typeparam name="TRequestModel">The request model</typeparam>
    /// <param name="workflow">The workflow</param>
    /// <param name="storedProcedureName">The stored procedure name</param>
    /// <param name="replacePosting">The replace posting</param>
    /// <param name="checkValidation">The check validation</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="O24OpenAPIException"></exception>
    /// <exception cref="O24OpenAPIException">Error occurs when executing service [{GrpcClient.ClientConfig.YourServiceID}] validation : {ex.Message}</exception>
    /// <returns>The response</returns>
    public static async Task<WFScheme> Invoke2<TRequestModel>(
        WFScheme workflow,
        string storedProcedureName,
        bool replacePosting = false,
        Func<Task<bool>> checkValidation = null
    )
        where TRequestModel : BaseTransactionModel
    {
        // Early validation
        if (string.IsNullOrEmpty(storedProcedureName))
        {
            throw new ArgumentNullException(nameof(storedProcedureName));
        }

        // Calculate timeout once
        var (commandTimeout, errorMessage) = CalculateTimeout(workflow);
        if (commandTimeout <= 0)
        {
            throw new O24OpenAPIException(errorMessage);
        }

        // Validation check
        if (checkValidation != null)
        {
            try
            {
                if (!await checkValidation())
                {
                    return CreateSuccessResponse(workflow);
                }
            }
            catch (Exception ex)
            {
                throw new O24OpenAPIException(
                    $"Error occurs when executing service [{Singleton<O24OpenAPIClientConfiguration>.Instance.YourServiceID}] validation : {ex.Message}"
                );
            }
        }

        // Database operation
        using var dbContext = new ServiceDBContext(commandTimeout);
        var baseTransactionModel = await workflow.ToModel<BaseTransactionModel>();
        workflow.request.request_header.tx_context["base_transaction_model"] =
            baseTransactionModel;

        var response = dbContext.CallServiceStoredProcedure(
            storedProcedureName,
            workflow,
            workflow.request.request_header.IsReversal()
                ? ServiceDBContext.EnumIsReverse.R
                : ServiceDBContext.EnumIsReverse.N
        );

        // Format response
        response.response.status = WFScheme.RESPONSE.EnumResponseStatus.SUCCESS;
        response.response.data = JToken
            .Parse(response.response.data.ToString())
            .ConvertKeysToSnakeCase();

        return response;
    }

    /// <summary>
    /// Calculates the timeout using the specified workflow
    /// </summary>
    /// <param name="workflow">The workflow</param>
    /// <returns>The int timeout string error message</returns>
    private static (int timeout, string errorMessage) CalculateTimeout(WFScheme workflow)
    {
        var stepTimeout = workflow.request.request_header.step_timeout;
        var sendingUTC = workflow.request.request_header.utc_send_time;
        var currentTime = CommonHelper.ConvertToUnixTimestamp(DateTime.UtcNow);
        var trafficTime = currentTime - sendingUTC;

        var apiSettings = EngineContext.Current.Resolve<WebApiSettings>();
        var bufferTime = apiSettings.BufferTime == 0L ? 500 : apiSettings.BufferTime;

        var isReverse = workflow.request.request_header.IsReversal();
        var timeout = (int)(
            !isReverse
                ? ((stepTimeout - trafficTime * 2 - bufferTime) / 1000)
                : (stepTimeout / 1000)
        );

        var errorMessage =
            timeout <= 0
                ? $"The message sent to service [{Singleton<O24OpenAPIClientConfiguration>.Instance.YourServiceID}] was timeout"
                : string.Empty;

        return (timeout, errorMessage);
    }

    /// <summary>
    /// Creates the success response using the specified workflow
    /// </summary>
    /// <param name="workflow">The workflow</param>
    /// <returns>The workflow</returns>
    private static WFScheme CreateSuccessResponse(WFScheme workflow)
    {
        workflow.response.status = WFScheme.RESPONSE.EnumResponseStatus.SUCCESS;
        return workflow;
    }

    /// <summary>
    /// Does the transaction using the specified workflow
    /// </summary>
    /// <param name="workflow">The workflow</param>
    /// <param name="acquire">The acquire</param>
    /// <param name="isReverse">The is reverse</param>
    /// <param name="addInformation">The add information</param>
    /// <param name="removeSensitive">The remove sensitive</param>
    /// <param name="IsCompensated">The is compensated</param>
    /// <param name="useTransactionScope">The use transaction scope</param>
    /// <param name="isInquiry">The is inquiry</param>
    /// <exception cref="O24OpenAPIException">MessageOutdate Message is outdate</exception>
    /// <returns>A task containing the dynamic</returns>
    private static async Task<object> DoTransaction(
        WFScheme workflow,
        Func<Task<object>> acquire,
        bool isReverse,
        Func<Task<WFScheme>> addInformation = null,
        bool removeSensitive = true,
        bool IsCompensated = false,
        bool useTransactionScope = true,
        bool isInquiry = true
    )
    {
        long timeoutSec = workflow.request.request_header.step_timeout;
        long startSec = workflow.request.request_header.utc_send_time;
        long currentTime = CommonHelper.GetCurrentDateAsLongNumber();
        long trafficTime = currentTime - startSec;

        WebApiSettings apiSettings = EngineContext.Current.Resolve<WebApiSettings>();
        long bufferTime = (apiSettings.BufferTime == 0L) ? 500 : apiSettings.BufferTime;

        if (IsCompensated)
        {
            workflow.request.request_header.is_compensated = "Y";
        }

        if (timeoutSec - 2 * trafficTime - bufferTime <= 0 && !isReverse && !IsCompensated)
        {
            throw new O24OpenAPIException("MessageOutdate", "Message is outdate");
        }

        TransactionScope tx = null;
        if (useTransactionScope)
        {
            tx = new TransactionScope(
                TransactionScopeOption.RequiresNew,
                new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadCommitted,
                    Timeout =
                        (timeoutSec == 0 || IsCompensated || isReverse)
                            ? TimeSpan.MaxValue
                            : TimeSpan.FromMilliseconds(timeoutSec),
                },
                TransactionScopeAsyncFlowOption.Enabled
            );
        }

        try
        {
            object result = await acquire();

            if (addInformation != null)
            {
                workflow = await addInformation();
            }

            if (!isReverse)
            {
                workflow = await AddBusinessInformation(workflow);
            }

            tx?.Complete();
            return result;
        }
        finally
        {
            tx?.Dispose();
        }
    }

    /// <summary>
    /// Adds the business information using the specified workflow
    /// </summary>
    /// <param name="workflow">The workflow</param>
    /// <returns>The workflow</returns>
    private static async Task<WFScheme> AddBusinessInformation(WFScheme workflow)
    {
        if (!workflow.request.request_header.tx_context.ContainsKey("transaction_date"))
        {
            IWorkContext workContext = EngineContext.Current.Resolve<IWorkContext>();
            DateTime tranDt = await workContext.GetWorkingDate(
                reload: true,
                inBatch: false,
                workflow.request.request_header.channel_id
            );
            workflow.request.request_header.tx_context.Add(
                "transaction_date",
                tranDt.Date.ToString("o")
            );
        }
        return workflow;
    }

    /// <summary>
    /// Invokes the command query using the specified workflow
    /// </summary>
    /// <param name="workflow">The workflow</param>
    /// <exception cref="ArgumentNullException">Workflow scheme cannot be null.</exception>
    /// <returns>The result</returns>
    public static async Task<WFScheme> InvokeCommandQuery(WFScheme workflow)
    {
        var model = await workflow.ToModel<ModelWithQuery>();
        if (workflow == null)
        {
            throw new ArgumentNullException(
                nameof(workflow),
                "Workflow scheme cannot be null."
            );
        }

        var result = await Invoke<ModelWithQuery>(
            workflow,
            async () =>
            {
                var executeQueryService = EngineContext.Current.Resolve<IExecuteQueryService>();
                return await executeQueryService.SqlQuery(model);
            }
        );
        return result;
    }
}

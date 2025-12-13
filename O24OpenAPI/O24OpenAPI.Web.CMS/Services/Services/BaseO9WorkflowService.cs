using System.Diagnostics;
using Jits.Neptune.Web.CMS.LogicOptimal9.Common;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Web.CMS.Models;
using O24OpenAPI.Web.CMS.Models.O9;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using ILogger = O24OpenAPI.Web.Framework.Services.Logging.ILogger;

namespace O24OpenAPI.Web.CMS.Services.Services;

public class BaseO9WorkflowService : IBaseO9WorkflowService
{
    private readonly IMappingService _mappingService;
    private readonly ILogger _logger;
    private readonly IFOService _foService;
    private readonly IBaseService _baseService;
    private readonly IO9ClientService _o9clientService;

    /// <summary>
    ///
    /// </summary>
    public BaseO9WorkflowService(
        IMappingService mappingService,
        ILogger logger,
        IFOService foService,
        IBaseService baseService,
        IO9ClientService o9clientService
    )
    {
        _mappingService = mappingService;
        if (Singleton<PagingData>.Instance == null)
        {
            Singleton<PagingData>.Instance = new PagingData();
        }

        _logger = logger;
        _foService = foService;
        _baseService = baseService;
        _o9clientService = o9clientService;
    }

    /// <summary>
    /// Calls the function using the specified model
    /// </summary>
    /// <param name="model">The model</param>
    /// <returns>A task containing the token</returns>
    public async Task<JToken> CallFunction(WorkflowRequestModel workflow)
    {
        return await workflow.Invoke(
            async () =>
            {
                string result = await _o9clientService.GenJsonFunctionRequestAsync(
                    workflow.user_sessions,
                    "",
                    workflow.ObjectField,
                    workflow.WorkflowFunc
                );
                JObject jsResult = O9Utils.AnalysisFunctionResult(result);
                if (string.IsNullOrEmpty(workflow.MappingResponse))
                {
                    return jsResult.ConvertToJObject();
                }
                var response = await _mappingService.MappingResponse(
                    workflow.MappingResponse,
                    jsResult["data"] ?? jsResult
                );
                return response;
            },
            isBuildResponse: true,
            isBuildWithMapping: false
        );
    }

    /// <summary>
    /// Describes whether is pagination valid
    /// </summary>
    /// <param name="objectField">The object field</param>
    /// <returns>The bool</returns>
    private static bool IsPaginationValid(JObject objectField)
    {
        int? pageIndex = objectField["page_index"]?.ToInt();
        int? pageSize = objectField["page_size"]?.ToInt();

        return pageIndex.HasValue && pageSize.HasValue && pageSize.Value > 0;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="workflow"></param>
    /// <returns></returns>
    public async Task<JToken> SimpleSearch(WorkflowRequestModel workflow)
    {
        return await workflow.Invoke(
            async () =>
            {
                if (!IsPaginationValid(workflow.ObjectField))
                {
                    return await _baseService.Search(workflow);
                }

                return await _baseService.SearchNew(workflow);
            },
            isBuildResponse: true,
            isBuildWithMapping: false
        );
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="workflow"></param>
    /// <returns></returns>
    public async Task<JToken> AdvanceSearch(WorkflowRequestModel workflow)
    {
        return await workflow.Invoke(
            async () =>
            {
                if (!IsPaginationValid(workflow.ObjectField))
                {
                    return await _baseService.Search(workflow, true);
                }

                return await _baseService.SearchNew(workflow, true);
            },
            isBuildResponse: true,
            isBuildWithMapping: false
        );
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<JToken> View(WorkflowRequestModel model)
    {
        return await model.Invoke(async () =>
        {
            JToken response = null;
            var objectGenDataRequest = model.ObjectField.UpperKey();
            string strJsonResult = await _o9clientService.GenJsonDataRequestAsync(
                objectGenDataRequest,
                model.WorkflowFunc
            );
            if (!string.IsNullOrEmpty(strJsonResult))
            {
                JObject jsResult = JObject.Parse(strJsonResult);

                response = jsResult;
            }
            else
            {
                throw new Exception("Not found!");
            }

            return response;
        });
    }

    /// <summary>
    /// Rules the func using the specified workflow
    /// </summary>
    /// <param name="workflow">The workflow</param>
    /// <exception cref="Exception"></exception>
    /// <returns>The response</returns>
    public async Task<JToken> RuleFunc(WorkflowRequestModel workflow)
    {
        return await workflow.Invoke(
            async () =>
            {
                var condition = workflow.fields.Count(e =>
                    e.Key.ToString() != "page_index" && e.Key.ToString() != "page_size"
                ) switch
                {
                    0 => null,
                    _ => workflow.fields.Where(e =>
                        e.Key.ToString() != "page_index" && e.Key.ToString() != "page_size"
                    ),
                };

                var result = await _o9clientService.RuleFuncAsync(
                    workflow.WorkflowFunc,
                    workflow.Module,
                    condition
                );
                result = await _mappingService.MappingResponse(
                    workflow.MappingResponse,
                    result["data"] ?? result
                );
                if (
                    workflow.fields.TryGetValue("page_index", out _)
                    && workflow.fields.TryGetValue("page_size", out _)
                )
                {
                    var pagedList = result.ToPagedList<JObject>(0, int.MaxValue);
                    int totalCount = pagedList.TotalCount;
                    if (
                        Singleton<PagingData>.Instance.Paging.TryGetValue(
                            workflow.WorkflowFunc,
                            out int total
                        )
                    )
                    {
                        totalCount = total;
                    }

                    result = JToken.FromObject(pagedList.ToPageListModel(totalCount));
                }

                return result;
            },
            true,
            false
        );
    }

    /// <summary>
    /// Executes the rule func using the specified workflow
    /// </summary>
    /// <param name="workflow">The workflow</param>
    /// <returns>A task containing the token</returns>
    public async Task<JToken> ExecuteRuleFunc(WorkflowRequestModel workflow)
    {
        return await workflow.Invoke(
            async () =>
            {
                var result = await _o9clientService.ExecuteRuleFuncAsync(
                    workflow.TxCode,
                    workflow.WorkflowFunc,
                    workflow.ObjectField
                );
                if (result.IsEmptyOrNull())
                {
                    throw new O24OpenAPIException("Data not found");
                }
                result = await _mappingService.MappingResponse(
                    workflow.MappingResponse,
                    result["data"] ?? result
                );
                if (
                    workflow.fields.TryGetValue("page_index", out _)
                    && workflow.fields.TryGetValue("page_size", out _)
                )
                {
                    var pagedList = result.ToPagedList<JObject>(0, int.MaxValue);
                    int totalCount = pagedList.TotalCount;
                    if (
                        Singleton<PagingData>.Instance.Paging.TryGetValue(
                            workflow.WorkflowFunc,
                            out int total
                        )
                    )
                    {
                        totalCount = total;
                    }

                    result = JToken.FromObject(pagedList.ToPageListModel(totalCount));
                }

                return result;
            },
            isBuildResponse: true,
            isBuildWithMapping: false
        );
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="workflow"></param>
    /// <returns></returns>
    /// <exception cref="NeptuneException"></exception>
    public async Task<JToken> CreateBO(WorkflowRequestModel workflow)
    {
        return await workflow.Invoke(
            async () =>
            {
                JsonTableName clsJson = new();
                JObject jsData = workflow.ObjectField;

                JObject jsRequest = jsData;

                clsJson.TXBODY.Add(new JsonData(workflow.TableName, jsRequest));

                string result = await _o9clientService.GenJsonBackOfficeRequest(
                    workflow.user_sessions,
                    workflow.WorkflowFunc,
                    clsJson.TXBODY
                );

                JObject jsResult = O9Utils.AnalysisBOResult(result);
                return jsResult;
            },
            false
        );
    }

    /// <summary>
    /// Fronts the office using the specified workflow
    /// </summary>
    /// <param name="workflow">The workflow</param>
    /// <returns>A task containing the token</returns>
    public async Task<JToken> FrontOffice(WorkflowRequestModel workflow)
    {
        var startTime = Utils.Utils.ConvertToUnixTimestamp(DateTime.UtcNow);
        var stopwatch = Stopwatch.StartNew();

        return await workflow.Invoke(
            async () =>
            {
                var (jsRequest, status, approver) = _foService.PrepareRequest(workflow);
                var transactionFees = new JArray();
                JObject jsFees = _foService.ExtractAndRemoveIfcFees(jsRequest, ref transactionFees);

                string strResult = await _foService.GenerateFrontOfficeRequest(
                    workflow,
                    jsRequest,
                    status,
                    jsFees
                );
                JsonFrontOffice jsonFrontOffice = O9Utils.AnalysisFrontOfficeResult(strResult);
                JObject objectFo = O9Utils.ConvertFOToJObject(jsonFrontOffice);

                var (txBody, refId) = await _foService.ProcessFrontOfficeResponse(
                    workflow,
                    objectFo
                );

                // JToken vouchers = await _dataVoucherService.ProcessVoucher(objectFo);
                JObject tranResponse = _foService.PrepareTranResponse(
                    objectFo,
                    txBody,
                    null,
                    transactionFees
                );

                return await _foService.FinalizeWithoutPostings(
                    workflow,
                    txBody,
                    tranResponse,
                    (long)startTime,
                    stopwatch
                );
            },
            true
        );
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<JToken> Delete(WorkflowRequestModel model)
    {
        return await model.Invoke(
            async () =>
            {
                JObject jsRequest = [];

                JToken token = model.ObjectField.SelectToken("id");
                dynamic id = null;

                if (token != null)
                {
                    if (token.Type == JTokenType.Integer)
                    {
                        id = token.Value<int>();
                    }
                    else if (token.Type == JTokenType.String)
                    {
                        id = token.Value<string>();
                    }
                    else if (token.Type == JTokenType.Object)
                    {
                        id = token.Value<object>();
                    }
                }

                if (model.IdFieldName.Equals("object", StringComparison.CurrentCultureIgnoreCase))
                {
                    jsRequest = id;
                }
                else
                {
                    jsRequest.Add(model.IdFieldName, id);
                }

                JsonTableName clsJson = new();
                clsJson.TXBODY.Add(new JsonData(model.TableName, jsRequest.UpperKey()));

                string result = await _o9clientService.GenJsonBackOfficeRequest(
                    model.user_sessions,
                    model.WorkflowFunc,
                    clsJson.TXBODY
                );
                JObject jsResult = O9Utils.AnalysisBOResult(result, null, false);
                return jsResult;
            },
            false
        );
    }

    /// <summary>
    ///
    /// </summary>
    public async Task<JToken> UpdateBO(WorkflowRequestModel model)
    {
        return await model.Invoke(
            async () =>
            {
                JsonTableName clsJson = new();
                JObject jsData = model.ObjectField;
                string id = jsData.SelectToken("id")?.ToObject<string>();
                jsData.Remove("id");
                if (
                    !string.IsNullOrEmpty(model.IdFieldName)
                    && !jsData.ContainsKey(model.IdFieldName)
                )
                {
                    if (id.IsNumeric())
                    {
                        jsData.Add(model.IdFieldName, int.Parse(id));
                    }
                    else
                    {
                        jsData.Add(model.IdFieldName, id);
                    }
                }

                JObject jsRequest = jsData;
                object fpk = null;
                if (jsRequest.TryGetValue("FPK", out var fpkValue))
                {
                    fpk = fpkValue;
                    jsRequest.Remove("FPK");
                }

                clsJson.TXBODY.Add(new JsonData(model.TableName, jsRequest, fpk));

                string result = await _o9clientService.GenJsonBackOfficeRequest(
                    model.user_sessions,
                    model.WorkflowFunc,
                    clsJson.TXBODY
                );
                JObject jsResult = O9Utils.AnalysisBOResult(result);
                return jsResult;
            },
            false
        );
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="workflow"></param>
    /// <returns></returns>
    public async Task<JToken> SearchList(WorkflowRequestModel workflow)
    {
        return await workflow.Invoke(
            async () =>
            {
                if (workflow.ObjectField.TryGetValue("require_search", out var requireField))
                {
                    var require = requireField.ToString();
                    if (workflow.ObjectField.Value<string>(require).IsNullOrEmpty())
                    {
                        return new JArray().BuildWorkflowResponseSuccess(false);
                    }
                }

                var isSearchAdvance =
                    workflow.ObjectField.TryGetValue("is_advance", out var isAdvanceToken)
                    && isAdvanceToken.ToObject<bool>();

                workflow.fields.Remove("is_advance");

                if (workflow.ObjectField.ContainsKey("is_advance"))
                {
                    workflow.ObjectField.Remove("is_advance");
                }

                var data = await _baseService.SearchData(workflow, isSearchAdvance);

                if (data["data"] != null)
                {
                    return data["data"].BuildWorkflowResponseSuccess(true);
                }
                else
                {
                    throw new Exception("Data is invalid");
                }
            },
            false
        );
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="workflow"></param>
    /// <returns></returns>
    public async Task<JToken> SearchInfo(WorkflowRequestModel workflow)
    {
        return await workflow.Invoke(
            async () =>
            {
                var id = workflow.ObjectField.GetValue("id");
                workflow.ObjectField.Remove("id");
                workflow.fields.Remove("id");

                var data = await SearchList(workflow);
                var listData = data?["result"]?.ToObject<List<JObject>>();

                JToken response = null;
                if (id != null)
                {
                    response = listData?.FirstOrDefault(item =>
                        item.GetValue(workflow.IdFieldName.ToLower())?.ToString() == id.ToString()
                    );
                }
                else
                {
                    response = listData?.FirstOrDefault();
                }

                return response?.BuildWorkflowResponseSuccess()
                    ?? $"[{workflow.workflowid}] - Data not found!".BuildWorkflowResponseError();
            },
            false
        );
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="workflow"></param>
    /// <returns></returns>
    public async Task<JToken> ExecuteQuery(WorkflowRequestModel workflow)
    {
        return await workflow.Invoke(
            async () =>
            {
                if (!workflow.fields.TryGetValue("query_key", out object sql))
                {
                    return "Invalid Query Key!".BuildWorkflowResponseError();
                }

                var pageIndex = workflow.ObjectField["page_index"]?.ToInt() ?? 1;
                var pageSize = workflow.ObjectField["page_size"]?.ToInt() ?? 0;
                pageSize = pageSize == 0 ? int.MaxValue : pageSize;
                string resultType = "";
                if (workflow.fields.TryGetValue("result_type", out object type))
                {
                    resultType = type.ToString();
                }

                var result = await _baseService.SearchQuery(sql.ToString());
                switch (resultType.ToString())
                {
                    case "Object":
                        var resultObject = result.SelectToken("data[0]");
                        var mapping = await _mappingService.MappingResponse(
                            workflow.MappingResponse,
                            resultObject
                        );
                        return mapping.BuildWorkflowResponseSuccess(false);

                    default:
                        var pageList = result
                            .SelectToken("data")
                            .ToPagedList<JObject>(pageIndex, pageSize);
                        var totalCount = pageList.TotalCount;

                        var pageListModel = pageList.ToPageListModel(totalCount);
                        var mappingItems = await _mappingService.MappingResponse(
                            workflow.MappingResponse,
                            pageListModel.Items.ToJToken()
                        );
                        pageListModel.Items = mappingItems.ToObject<List<JObject>>();

                        return pageListModel.BuildWorkflowResponseSuccess(false);
                }
            },
            false
        );
    }
}

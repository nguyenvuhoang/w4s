using Jits.Neptune.Web.CMS.LogicOptimal9.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Web.CMS.Models;
using O24OpenAPI.Web.CMS.Services.Interfaces;

namespace O24OpenAPI.Web.CMS.Services.Services;

/// <summary>
///
/// </summary>
public class BaseService(
    IMappingService mappingService,
    IMemoryCacheService memoryCacheService,
    ICMSSettingService settingService,
    IO9ClientService o9clientService
) : IBaseService
{
    private readonly IMappingService _mappingService = mappingService;
    private readonly IMemoryCacheService _memoryCacheService = memoryCacheService;
    private readonly ICMSSettingService _settingService = settingService;
    private readonly IO9ClientService _o9clientService = o9clientService;

    /// <summary>
    ///
    /// </summary>
    /// <param name="workflow"></param>
    /// <param name="isAdvanceSearch"></param>
    /// <returns></returns>
    public async Task<JToken> Search(WorkflowRequestModel workflow, bool isAdvanceSearch = false)
    {
        var pageIndex = workflow.fields["page_index"].ToInt();
        var pageSize = workflow.fields["page_size"].ToInt();
        pageSize = pageSize == 0 ? int.MaxValue : pageSize;

        var result = await SearchData(workflow, isAdvanceSearch);
        IPagedList<JObject> pageList;
        if (workflow.fields.TryGetValue("condition_order", out var conditionOrder))
        {
            var data = result.SelectToken("data").ToObject<List<JObject>>();

            var jsonArray = JArray.Parse(conditionOrder.ToString());
            var orders = jsonArray.ToJArray().JsonConvertToModel<List<ConditionOrder>>();
            data = data.ApplyOrdering(orders).ToList();
            pageList = await data.AsQueryable().ToPagedList(pageIndex, pageSize);
        }
        else
        {
            pageList = result.SelectToken("data").ToPagedList<JObject>(pageIndex, pageSize);
        }

        int totalCount = pageList.TotalCount;

        if (Singleton<PagingData>.Instance == null)
        {
            Singleton<PagingData>.Instance = new PagingData();
        }

        if (Singleton<PagingData>.Instance.Paging.TryGetValue(workflow.WorkflowFunc, out int total))
        {
            totalCount = total;
        }

        var pageListModel = pageList.ToPageListModel(totalCount);

        var mappingItems = await _mappingService.MappingResponse(
            workflow.MappingResponse,
            pageListModel.Items.ToJToken()
        );
        pageListModel.Items = mappingItems.ToObject<List<JObject>>();

        return pageListModel.ToJToken();
    }

    /// <summary>
    ///
    /// </summary>
    public async Task<JToken> SearchData(
        WorkflowRequestModel workflow,
        bool isAdvanceSearch = false
    )
    {
        try
        {
            bool hasDistinct = false;
            if (workflow.fields.TryGetValue("has_distinct", out var distinct))
            {
                hasDistinct = bool.Parse(distinct.ToString());
            }

            var searchFunc = O9Utils.SearchFunc(workflow.fields, workflow.WorkflowFunc);

            string strSql;

            if (isAdvanceSearch)
            {
                strSql = searchFunc.GenSearchCommonSql(
                    O9Constants.O9_CONSTANT_AND,
                    EnmOrderTime.InQuery,
                    string.Empty,
                    hasDistinct
                );
            }
            else
            {
                var searchText = workflow.fields["search_text"].ToString();
                strSql = searchFunc.GenSearchCommonSql(searchText, "", EnmOrderTime.InQuery, true);
            }

            var pageIndex = workflow.ObjectField["page_index"]?.ToInt() + 1 ?? 0;
            var pageSize = workflow.ObjectField["page_size"]?.ToInt() ?? 0;
            if (pageSize < 5 || pageSize > 20)
            {
                pageIndex = 0;
            }
            else
            {
                pageIndex = (int)Math.Ceiling((double)(pageIndex * pageSize) / 20);
            }

            var result = await _o9clientService.SearchAsync(
                strSql,
                pageIndex,
                workflow.WorkflowFunc
            );
            result = searchFunc.SearchData(result);

            return result;
        }
        catch (Exception ex)
        {
            return await ex.HandleExceptionAsync(workflow.workflowid);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public async Task<JToken> SearchNew(WorkflowRequestModel workflow, bool isAdvanceSearch = false)
    {
        var pageNumber = workflow.fields["page_index"].ToInt();
        var pageSize = workflow.fields["page_size"].ToInt();

        var allObjects = await FetchPagedObjects(workflow, pageNumber, pageSize, isAdvanceSearch);

        var pageListToken = allObjects.ToJToken();
        var pageList = pageListToken.ToPagedList<JObject>(pageNumber, pageSize);
        int totalCount = GetTotalCount(pageList, workflow.WorkflowFunc);

        var pageListModel = pageList.ToPageListModel(totalCount);
        pageListModel.Items = await MapItems(
            workflow.MappingResponse,
            pageListModel.Items.ToJToken()
        );

        return pageListModel.ToJToken();
    }

    /// <summary>
    /// Fetches the paged objects using the specified workflow
    /// </summary>
    private async Task<List<JObject>> FetchPagedObjects(
        WorkflowRequestModel workflow,
        int pageNumber,
        int pageSize,
        bool isAdvanceSearch
    )
    {
        List<JObject> allObjects = [];

        int objectsPerO9Page = 20;
        int skip = pageNumber * pageSize;
        int objectsLeftToFetch = pageSize;
        int startO9Page = skip / objectsPerO9Page + 1;
        int skipRemainder = skip % objectsPerO9Page;
        //var (stringQuery, searchFunc) = GenQuery(workflow.fields, workflow.WorkflowFunc, isAdvanceSearch);
        if (skipRemainder != 0)
        {
            var partialPageObjects = //await GenSearch(workflow.fields, stringQuery, workflow.WorkflowFunc,searchFunc, startO9Page);
            await SearchDataNew(
                workflow.fields,
                workflow.WorkflowFunc,
                startO9Page,
                isAdvanceSearch
            );
            int objectsToTakeFromPartialPage = Math.Min(
                objectsLeftToFetch,
                objectsPerO9Page - skipRemainder
            );
            allObjects.AddRange(
                partialPageObjects.Skip(skipRemainder).Take(objectsToTakeFromPartialPage)
            );
            objectsLeftToFetch -= objectsToTakeFromPartialPage;
            startO9Page++;
        }

        while (objectsLeftToFetch > 0)
        {
            var pageObjects = //await GenSearch(workflow.fields, stringQuery, workflow.WorkflowFunc,searchFunc, startO9Page);
            await SearchDataNew(
                workflow.fields,
                workflow.WorkflowFunc,
                startO9Page,
                isAdvanceSearch
            );
            int objectsFetched = Math.Min(objectsLeftToFetch, pageObjects.Count);

            allObjects.AddRange(pageObjects.Take(objectsFetched));
            if (pageObjects.Count < objectsPerO9Page)
            {
                break;
            }

            objectsLeftToFetch -= objectsFetched;
            startO9Page++;
        }

        return allObjects;
    }

    /// <summary>
    /// Gets the total count using the specified page list
    /// </summary>
    /// <param name="pageList">The page list</param>
    /// <param name="workflowFunc">The workflow func</param>
    /// <returns>The total count</returns>
    private static int GetTotalCount(IPagedList<JObject> pageList, string workflowFunc)
    {
        int totalCount = pageList.TotalCount;

        if (Singleton<PagingData>.Instance == null)
        {
            Singleton<PagingData>.Instance = new PagingData();
        }

        if (Singleton<PagingData>.Instance.Paging.TryGetValue(workflowFunc, out int total))
        {
            totalCount = total;
        }

        return totalCount;
    }

    /// <summary>
    /// Maps the items using the specified mapping response
    /// </summary>
    /// <param name="mappingResponse">The mapping response</param>
    /// <param name="itemsToken">The items token</param>
    /// <returns>A task containing a list of j object</returns>
    private async Task<List<JObject>> MapItems(string mappingResponse, JToken itemsToken)
    {
        var mappingItems = await _mappingService.MappingResponse(mappingResponse, itemsToken);
        return mappingItems.ToObject<List<JObject>>();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="request"></param>
    /// <param name="function"></param>
    /// <param name="pageIndex"></param>
    /// <param name="isAdvanceSearch"></param>
    /// <param name="hasDistinct"></param>
    /// <returns></returns>
    public async Task<List<JObject>> SearchDataNew(
        Dictionary<string, object> request,
        string function,
        int pageIndex,
        bool isAdvanceSearch = false,
        bool hasDistinct = true
    )
    {
        try
        {
            var searchFunc = O9Utils.SearchFunc(request, function);
            if (request.TryGetValue("has_distinct", out var distinct))
            {
                hasDistinct = bool.Parse(distinct.ToString());
            }

            string strSql;
            if (isAdvanceSearch)
            {
                strSql = searchFunc.GenSearchCommonSql(
                    O9Constants.O9_CONSTANT_AND,
                    EnmOrderTime.InQuery,
                    string.Empty,
                    hasDistinct
                );
            }
            else
            {
                var searchText = request["search_text"].ToString();
                strSql = searchFunc.GenSearchCommonSql(searchText, "", EnmOrderTime.InQuery, true);
            }

            var result = await _o9clientService.SearchAsync(strSql, pageIndex, function);
            result = searchFunc.SearchData(result);

            var data = result.SelectToken("data").ToObject<List<JObject>>();

            if (request.TryGetValue("condition_order", out var conditionOrder))
            {
                var jsonArray = JArray.Parse(conditionOrder.ToString());
                var orders = jsonArray.ToJArray().JsonConvertToModel<List<ConditionOrder>>();
                data = data.ApplyOrdering(orders).ToList();
            }

            return data;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private async Task<Dictionary<string, string>> GetAllQueryAsync()
    {
        var allQuery = await _settingService.GetByPrimaryKey("AllQuery");
        return JsonConvert.DeserializeObject<Dictionary<string, string>>(allQuery.Value);
    }

    private static (string, SearchFunc) GenQuery(
        Dictionary<string, object> request,
        string function,
        bool isAdvanceSearch = false,
        bool hasDistinct = true
    )
    {
        var searchFunc = O9Utils.SearchFunc(request, function);
        if (request.TryGetValue("has_distinct", out var distinct))
        {
            hasDistinct = bool.Parse(distinct.ToString());
        }

        string strSql;
        if (isAdvanceSearch)
        {
            strSql = searchFunc.GenSearchCommonSql(
                O9Constants.O9_CONSTANT_AND,
                EnmOrderTime.InQuery,
                string.Empty,
                hasDistinct
            );
        }
        else
        {
            var searchText = request["search_text"].ToString();
            strSql = searchFunc.GenSearchCommonSql(searchText, "", EnmOrderTime.InQuery, true);
        }
        return (strSql, searchFunc);
    }

    private async Task<List<JObject>> GenSearch(
        Dictionary<string, object> request,
        string strSql,
        string function,
        SearchFunc searchFunc,
        int pageIndex
    )
    {
        try
        {
            var result = await _o9clientService.SearchAsync(strSql, pageIndex, function);
            result = searchFunc.SearchData(result);

            var data = result.SelectToken("data").ToObject<List<JObject>>();

            if (request.TryGetValue("condition_order", out var conditionOrder))
            {
                var jsonArray = JArray.Parse(conditionOrder.ToString());
                var orders = jsonArray.ToJArray().JsonConvertToModel<List<ConditionOrder>>();
                data = data.ApplyOrdering(orders).ToList();
            }

            return data;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="queryKey"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<JToken> SearchQuery(string queryKey, List<string> param = null)
    {
        string keyAllQuery = "AllQuery";
        var allQuery = await _memoryCacheService.GetOrCreateAsync(
            keyAllQuery,
            () => GetAllQueryAsync(),
            TimeSpan.FromMinutes(2),
            TimeSpan.FromMinutes(1)
        );

        if (!allQuery.TryGetValue(queryKey, out var strSql))
        {
            throw new KeyNotFoundException(
                $"The queryKey '{queryKey}' was not found in the query list."
            );
        }

        if (param != null && param.Count > 0)
        {
            strSql = string.Format(strSql, param.ToArray());
        }

        var result = await _o9clientService.SearchAsync(strSql);
        return result;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public async Task<TType> GetInfoByQuery<TType>(
        string queryKey,
        List<string> param = null,
        int selectIndex = 0
    )
    {
        try
        {
            var keySelect = $"data[0].0[{selectIndex}]";
            var resultQuery = await SearchQuery(queryKey, param);
            if (typeof(TType) == typeof(string))
            {
                var query = resultQuery
                    .SelectToken("data[0]")
                    .ToJObject()
                    .Properties()
                    .Select(p => p.Value.ToString())
                    .ToList();
                if (query.Count == 1)
                {
                    var response = resultQuery.SelectToken(keySelect).ToObject<TType>();
                    return response;
                }
                else
                {
                    var response = string.Join("|", query);
                    return (TType)(object)response;
                }
            }
            else
            {
                var value = resultQuery.SelectToken(keySelect);
                var response = value.ToObject<TType>();
                return response;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<TType> ExecuteSql<TType>(string sql)
    {
        try
        {
            var keySelect = $"0[0]";
            var resultQuery = await _o9clientService.SearchAsync(sql);
            var firstData = resultQuery.SelectToken("data[0]");
            if (firstData.IsEmptyOrNull())
            {
                return default;
            }
            var result = firstData.SelectToken(keySelect).JsonConvertToModel<TType>();
            return result;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="queryKey"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<TModel> GetInfoByQueryNew<TModel>(string queryKey, List<string> param = null)
    {
        var resultQuery = await SearchQuery(queryKey, param);
        JArray dataSearch = (JArray)resultQuery.SelectToken("data");

        var properties = typeof(TModel).GetProperties();
        var propertyNames = properties.Select(p => p.Name).ToArray();
        var resultSearch = dataSearch.RenameFields(propertyNames).FirstOrDefault();

        var response = resultSearch.JsonConvertToModel<TModel>();
        return response;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="stringQuery"></param>
    /// <param name="searchFunc"></param>
    /// <returns></returns>
    public async Task<JToken> SearchDataWithQuery(string stringQuery, string searchFunc)
    {
        try
        {
            var sFunc = O9Utils.SearchFunc(new JObject(), searchFunc);

            var result = await _o9clientService.SearchAsync(stringQuery, 0, searchFunc);
            result = sFunc.SearchData(result);

            return result;
        }
        catch (Exception ex)
        {
            return await ex.HandleExceptionAsync(searchFunc, stringQuery);
        }
    }
}

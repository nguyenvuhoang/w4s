using Newtonsoft.Json.Linq;
using O24OpenAPI.Web.CMS.Models;

namespace O24OpenAPI.Web.CMS.Services.Interfaces;

public interface IBaseService
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="workflow"></param>
    /// <param name="isAdvanceSearch"></param>
    /// <returns></returns>
    Task<JToken> Search(WorkflowRequestModel workflow, bool isAdvanceSearch = false);

    /// <summary>
    ///
    /// </summary>
    /// <param name="workflow"></param>
    /// <param name="isAdvanceSearch"></param>
    /// <returns></returns>
    Task<JToken> SearchNew(WorkflowRequestModel workflow, bool isAdvanceSearch = false);

    /// <summary>
    ///
    /// </summary>
    /// <param name="workflow"></param>
    /// <param name="isAdvanceSearch"></param>
    /// <returns></returns>
    Task<JToken> SearchData(WorkflowRequestModel workflow, bool isAdvanceSearch = false);

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    Task<TType> GetInfoByQuery<TType>(
        string queryKey,
        List<string> param = null,
        int selectIndex = 0
    );

    /// <summary>
    ///
    /// </summary>
    /// <param name="queryKey"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<JToken> SearchQuery(string queryKey, List<string> param = null);

    /// <summary>
    ///
    /// </summary>
    /// <param name="stringQuery"></param>
    /// <param name="searchFunc"></param>
    /// <returns></returns>
    Task<JToken> SearchDataWithQuery(string stringQuery, string searchFunc);

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <param name="queryKey"></param>
    /// <param name="param"></param>
    /// /// <returns></returns>
    Task<TModel> GetInfoByQueryNew<TModel>(string queryKey, List<string> param = null);

    Task<TType> ExecuteSql<TType>(string sql);
}

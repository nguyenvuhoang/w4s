using Apache.NMS;
using Jits.Neptune.Web.CMS.LogicOptimal9.Common;
using Newtonsoft.Json.Linq;
using O24OpenAPI.Web.CMS.Models;
using O24OpenAPI.Web.CMS.Models.O9;

namespace O24OpenAPI.Web.CMS.Services.Interfaces;

public interface IO9ClientService
{


    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    Task<string> GenJsonBodyRequestAsync(object objJsonBody,
        string functionId, string sessionId = "",
        EnmCacheAction isResultCaching = EnmCacheAction.NoCached,
        EnmSendTypeOption sendType = EnmSendTypeOption.Synchronize,
        string usrId = "-1",
        MsgPriority priority = MsgPriority.Normal);

    /// <summary>
    ///
    /// </summary>
    Task<string> GenJsonBackOfficeRequestAsync(BackOfficeModel model);

    /// <summary>
    ///
    /// </summary>
    Task<string> GenJsonFrontOfficeRequestAsync(FrontOfficeModel model, bool isnFrontOfficeMapping = true);

    /// <summary>
    ///
    /// </summary>
    /// <param name="txcode"></param>
    /// <param name="rule_name"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    Task<JToken> ExecuteRuleFuncAsync(string txcode, string rule_name, dynamic data);

    /// <summary>
    ///
    /// </summary>
    /// <param name="clsJsonData"></param>
    /// <param name="functionId"></param>
    /// <returns></returns>
    Task<string> GenJsonDataRequestAsync(object clsJsonData, string functionId);

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    Task<JToken> SearchAsync(string sql, int page = 0, string searchFunc = null, EnmCacheAction enmCacheAction = EnmCacheAction.NoCached);

    /// <summary>
    ///
    /// </summary>
    /// <param name="searchFunc"></param>
    /// <param name="currentModules"></param>
    /// <param name="condition"></param>
    /// <returns></returns>
    Task<JToken> RuleFuncAsync(string searchFunc, string currentModules,
        IEnumerable<KeyValuePair<string, object>> condition = null);
    /// <summary>
    ///
    /// </summary>
    Task<string> GenJsonFunctionRequestAsync(UserSessions user, string ptxcode, JObject ptxbody, string ptxproc = "",
    string functionId = "", EnmCacheAction isCaching = EnmCacheAction.NoCached);

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    Task<string> GenJsonBackOfficeRequest(UserSessions userSessions, string pTXCODE, List<JsonData> pTXBODY,
        string functionId = "", EnmCacheAction isCaching = EnmCacheAction.NoCached,
        string pSTATUS = "C", string pTXREFID = null,
        string pVALUEDT = null, string pUSRWS = "",
        object pAPUSER = null, string pAPUSRIP = "",
        string pAPUSRWS = "", string pAPDT = "",
        string pISREVERSE = "N", int pHBRANCHID = 0,
        int pRBRANCHID = 0, string pAPREASON = null,
        string pPRN = "", bool isMappingToArray = false);
    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    Task<string> GenJsonDataByIdRequest(JsonGetDataById clsJsonDataById, string functionId);
    /// <summary>
    ///
    /// </summary>
    /// <param name="lstTableName"></param>
    /// <returns></returns>
    Task<JObject> GetDataDefaultOfField(List<object> lstTableName);
    /// <summary>
    ///
    /// </summary>
    Task<IPagedList<TResponse>> AdvanceSearchCommon<TRequest, TResponse>(TRequest model, string sFunc) where TRequest : SearchBaseModel;
    /// <summary>
    ///
    /// </summary>
    Task<string> GenJsonFrontOfficeRequest(UserSessions user, string ptxcode, JObject ptxbody,
        string functionId = "", EnmCacheAction isResultCaching = EnmCacheAction.NoCached,
        string pstatus = "C", string ptxrefid = null, string pvaluedt = null, JObject pifcfee = null,
        string pusrws = null, object papuser = null, string papusrip = null, string papusrws = null,
        string papdt = null, string pisreverse = "N", int? phbranchid = null, int? prbranchid = null,
        string papreason = null, JsonPosting pposting = null, string pprn = null,
        string pid = null, bool isMapping = true);
    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    Task<string> GenJsonFrontOfficeRequest(UserSessions user, JsonFrontOffice clsJsonFrontOffice, string functionId = "", EnmCacheAction isResultCaching = EnmCacheAction.NoCached);
}

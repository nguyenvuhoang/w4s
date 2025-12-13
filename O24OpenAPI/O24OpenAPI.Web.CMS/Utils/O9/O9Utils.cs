using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Apache.NMS;
using Jits.Neptune.Web.CMS.LogicOptimal9.Common;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Web.CMS.Models;
using O24OpenAPI.Web.CMS.Models.O9;
using ILogger = O24OpenAPI.Web.Framework.Services.Logging.ILogger;

namespace O24OpenAPI.Web.CMS.Utils;

/// <summary>
/// The utils class
/// </summary>
public static class O9Utils
{
    //private static readonly O9Client o9Client = new O9Client();
    /// <summary>
    ///
    /// </summary>
    /// <param name="txcode"></param>
    /// <returns></returns>
    private static string TransRuleKey(string txcode)
    {
        return O9Client.memCached.GetValue(
            GlobalVariable.O9_GLOBAL_COMCODE
                + "."
                + GlobalVariable.O9_GLOBAL_MEMCACHED_KEY.RuleFunc
                + "."
                + txcode.ToUpper()
        );
    }

    /// <summary>
    ///
    /// </summary>
    public static string GenJsonBodyRequest(
        object objJsonBody,
        string functionId,
        string sessionid = "",
        EnmCacheAction isResultCaching = EnmCacheAction.NoCached,
        EnmSendTypeOption sendtype = EnmSendTypeOption.Synchronize,
        string usrId = "-1",
        MsgPriority priority = MsgPriority.Normal
    )
    {
        try
        {
            if (string.IsNullOrEmpty(functionId))
            {
                return string.Empty;
            }

            string strRequest =
                objJsonBody != null ? JsonConvert.SerializeObject(objJsonBody) : string.Empty;

            var o9Client = new O9Client();

            string strResult = o9Client
                .SendStringAsync(
                    strRequest,
                    functionId,
                    usrId,
                    sessionid,
                    isResultCaching,
                    sendtype,
                    priority
                )
                .GetAsyncResult();
            o9Client = null;
            return strResult;
        }
        catch (Exception ex)
        {
            Console.WriteLine("error_GenJsonBodyRequest == " + ex.StackTrace);
            throw;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public static string GenJsonFunctionRequest(
        UserSessions user,
        string ptxcode,
        JObject ptxbody,
        string ptxproc = "",
        string functionId = "",
        EnmCacheAction isCaching = EnmCacheAction.NoCached
    )
    {
        try
        {
            JsonFunction clsJsonFunction = new();
            if (string.IsNullOrEmpty(functionId))
            {
                functionId = GlobalVariable.UTIL_CALL_FUNC;
            }

            clsJsonFunction.TXCODE = ptxcode;
            clsJsonFunction.TXPROC = ptxproc;
            clsJsonFunction.TXBODY = ptxbody;
            return GenJsonBodyRequest(
                new JsonFunctionMapping(clsJsonFunction),
                functionId,
                user.Ssesionid,
                isCaching,
                EnmSendTypeOption.Synchronize,
                user.Usrid.ToString()
            );
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// Fronts the office using the specified user sessions
    /// </summary>
    /// <param name="userSessions">The user sessions</param>
    /// <param name="txCode">The tx code</param>
    /// <param name="requestModel">The request model</param>
    /// <exception cref="Exception"></exception>
    /// <returns>The json front office</returns>
    /// <summary>
    ///
    /// </summary>
    /// <param name="user"></param>
    /// <param name="clsJsonFrontOffice"></param>
    /// <param name="functionId"></param>
    /// <param name="isResultCaching"></param>
    /// <returns></returns>
    public static string GenJsonFrontOfficeRequest(
        UserSessions user,
        JsonFrontOffice clsJsonFrontOffice,
        string functionId = "",
        EnmCacheAction isResultCaching = EnmCacheAction.NoCached
    )
    {
        try
        {
            if (string.IsNullOrEmpty(functionId))
            {
                functionId = GlobalVariable.UTIL_CALL_PROC;
            }

            return GenJsonBodyRequest(
                new JsonFrontOfficeMapping(clsJsonFrontOffice),
                functionId,
                user.Ssesionid,
                EnmCacheAction.NoCached,
                EnmSendTypeOption.Synchronize,
                user.Usrid.ToString()
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
            return string.Empty;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public static string GenJsonFrontOfficeRequest(
        UserSessions user,
        string ptxcode,
        JObject ptxbody,
        string functionId = "",
        EnmCacheAction isResultCaching = EnmCacheAction.NoCached,
        string pstatus = "C",
        string ptxrefid = null,
        string pvaluedt = null,
        JObject pifcfee = null,
        string pusrws = null,
        object papuser = null,
        string papusrip = null,
        string papusrws = null,
        string papdt = null,
        string pisreverse = "N",
        int? phbranchid = null,
        int? prbranchid = null,
        string papreason = null,
        JsonPosting pposting = null,
        string pprn = null,
        string pid = null,
        bool isMapping = true
    )
    {
        try
        {
            JsonFrontOffice clsJsonFrontOffice = new();
            if (string.IsNullOrEmpty(functionId))
            {
                functionId = GlobalVariable.UTIL_CALL_PROC;
            }

            if (string.IsNullOrEmpty(pprn))
            {
                pprn = "";
            }

            clsJsonFrontOffice.TXCODE = ptxcode;
            clsJsonFrontOffice.TXDT = user.Txdt.ToString("dd/MM/yyyy");
            clsJsonFrontOffice.BRANCHID = user.UsrBranchid;
            clsJsonFrontOffice.USRID = user.Usrid;
            clsJsonFrontOffice.LANG = user.Lang;
            clsJsonFrontOffice.STATUS = pstatus;
            clsJsonFrontOffice.TXREFID = ptxrefid;
            clsJsonFrontOffice.VALUEDT = pvaluedt;
            clsJsonFrontOffice.USRWS = pusrws;
            clsJsonFrontOffice.APUSER = papuser;
            clsJsonFrontOffice.APUSRIP = papusrip;
            clsJsonFrontOffice.APUSRWS = papusrws;
            clsJsonFrontOffice.APDT = papdt;
            clsJsonFrontOffice.ISREVERSE = pisreverse;
            clsJsonFrontOffice.HBRANCHID = phbranchid;
            clsJsonFrontOffice.RBRANCHID = prbranchid;
            clsJsonFrontOffice.TXBODY = ptxbody;
            clsJsonFrontOffice.APREASON = papreason;
            clsJsonFrontOffice.IFCFEE = pifcfee;
            clsJsonFrontOffice.PRN = pprn;
            if (string.IsNullOrEmpty(pid))
            {
                pid = Guid.NewGuid().ToString();
            }

            clsJsonFrontOffice.ID = pid;

            if (isMapping)
            {
                var frontOfficeMapping = new JsonFrontOfficeMapping(clsJsonFrontOffice);
                var result = GenJsonBodyRequest(
                    frontOfficeMapping,
                    functionId,
                    user.Ssesionid,
                    EnmCacheAction.NoCached,
                    EnmSendTypeOption.Synchronize,
                    user.Usrid.ToString()
                );
                return result;
            }

            return GenJsonBodyRequest(
                clsJsonFrontOffice,
                functionId,
                user.Ssesionid,
                EnmCacheAction.NoCached,
                EnmSendTypeOption.Synchronize,
                user.Usrid.ToString()
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
            return string.Empty;
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="txCode"></param>
    /// <param name="jsRequest"></param>
    /// <returns></returns>
    public static JObject ProcessIfcFees(string txCode, JObject jsRequest)
    {
        if (jsRequest.ContainsKey("ifcfees") && !jsRequest["ifcfees"].IsNullOrEmpty())
        {
            JObject njsRequest = jsRequest;
            var jsFees = jsRequest.SelectToken("ifcfees").ToObject<JArray>();
            //JArray jsFees = jsRequest.GetValues<JArray>("fee_data");
            JArray njsFees = new();
            string paysrc = string.Empty;

            JsonRuleFunc rulefunc = GetListRuleFunc(txCode, "TRAN_FEE").FirstOrDefault();

            if (rulefunc != null && !string.IsNullOrEmpty(rulefunc.VALUE))
            {
                JsonRuleFunc rule = GetListRuleFunc(rulefunc.VALUE, "VSB_ITEM").FirstOrDefault();
                if (rule != null && rule.RELF.ToUpper().Equals("PAYSRC"))
                {
                    paysrc = rule
                        .ACTCDT.RemoveWhiteSpaceAndUpper()
                        .Replace("PAYSRC='", "")
                        .Replace("'", "")
                        .Trim();
                }
            }

            foreach (var jsItem in jsFees)
            {
                jsItem["paysrc"] = !string.IsNullOrEmpty(paysrc) ? paysrc : "CSH";
                njsFees.Add(jsItem);
            }

            njsRequest["ifcfees"] = njsFees;
            return njsRequest;
        }

        return jsRequest;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="query"></param>
    /// <param name="relativeFields"></param>
    /// <param name="isArray"></param>
    /// <returns></returns>
    public static async Task<JObject> GetInfoByQuery(
        string query,
        string relativeFields,
        bool isArray = false
    )
    {
        await Task.CompletedTask;
        JObject jsRequest = new()
        {
            { "Q", query },
            { "R", relativeFields },
            { "I", isArray },
        };

        string strResult = GenJsonBodyRequest(jsRequest, "UTIL_GET_INFO");

        if (!string.IsNullOrEmpty(strResult))
        {
            JsonFrontOffice clsJsonFrontOffice = AnalysisFrontOfficeResult(strResult);
            return clsJsonFrontOffice.RESULT;
        }

        return null;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="strFCCRCD"></param>
    /// <param name="strSCCRCD"></param>
    /// <param name="strFRT"></param>
    /// <param name="strSRT"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static async Task<JObject> GetCrossRate(
        string strFCCRCD,
        string strSCCRCD,
        string strFRT = "BK",
        string strSRT = "BK"
    )
    {
        await Task.CompletedTask;

        try
        {
            if (!string.IsNullOrEmpty(strFCCRCD) && strFCCRCD.Equals(strSCCRCD))
            {
                return new JObject() { { "cross_rate", 1 } };
            }

            string query = string.Empty;
            JObject jsReturn = null;
            if (!string.IsNullOrEmpty(strFCCRCD) && !string.IsNullOrEmpty(strSCCRCD))
            {
                query =
                    "SELECT ROUND(O9SYS.O9UTIL.GET_CROSS_RATE('@FCCRCD','@SCCRCD','@FRT','@SRT'),9) FROM DUAL"
                        .Replace("@FCCRCD", strFCCRCD)
                        .Replace("@SCCRCD", strSCCRCD)
                        .Replace("@FRT", strFRT)
                        .Replace("@SRT", strSRT);
                if (!query.Contains('@'))
                {
                    jsReturn = await GetInfoByQuery(query, "cross_rate");

                    if (jsReturn != null)
                    {
                        return jsReturn;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        return null;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="strCCRCD"></param>
    /// <param name="strRT"></param>
    /// <param name="strSRT"></param>
    /// <returns></returns>
    public static async Task<JObject> GetBaseRate(
        string strCCRCD,
        string strRT = "BK",
        string strSRT = "BK"
    )
    {
        await Task.CompletedTask;

        try
        {
            string query = string.Empty;
            JObject jsReturn = null;
            query = "SELECT ROUND(O9SYS.O9UTIL.GET_BCYRATE('@CCRCD', '@RT'),9) FROM DUAL"
                .Replace("@CCRCD", strCCRCD)
                .Replace("@RT", strRT);
            if (!query.Contains('@'))
            {
                jsReturn = await GetInfoByQuery(query, "bk_rate_currency");
                ConsoleWriteLine(jsReturn.ToString());
                if (jsReturn != null)
                {
                    return jsReturn;
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        return null;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="jsFo"></param>
    /// <returns></returns>
    public static JObject ConvertFOToJObject(JsonFrontOffice jsFo)
    {
        if (jsFo == null)
        {
            return null;
        }

        string txBodyKey = "TXBODY";
        JObject js = JObject.FromObject(jsFo);

        if (jsFo.IBRET != null)
        {
            js[txBodyKey] = jsFo.IBRET.ConvertFOToJObject();
        }
        if (jsFo.RESULT != null)
        {
            jsFo.TXBODY.Merge(jsFo.RESULT);
            js[txBodyKey] = jsFo.TXBODY;
        }

        if (jsFo.POSTING != null)
        {
            JArray jsPosting = jsFo.POSTING.ToJArray();
            js.Remove("POSTING");
            js.Add("postings", jsPosting);
        }

        if (js.TryGetValue("IFCFEE", out var _))
        {
            JArray jsFees = jsFo.IFCFEE.ConvertToJObjectArrayDetails();
            js.Remove("IFCFEE");
            js.Add("ifcfees", jsFees);
        }

        if (js.JsonContains("DATASET"))
        {
            JArray jsFees = jsFo.DATASET.ConvertToJObjectArrayDetails();
            js.Remove("DATASET");
            js.Add("dataset", jsFees);
        }

        return js.ConvertToJObject();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="jsFo"></param>
    /// <returns></returns>
    public static JObject ConvertFOToJObject(this JObject jsFo)
    {
        if (jsFo == null)
        {
            return null;
        }

        JObject js = JObject.FromObject(jsFo);

        if (jsFo["POSTING"] != null)
        {
            JArray jsPosting = jsFo["POSTING"].ToObject<JObject>().ConvertToJObjectArrayDetails();
            js.Remove("POSTING");
            js.Add("postings", jsPosting);
        }

        if (js.JsonContains("IFCFEE"))
        {
            JArray jsFees = jsFo["IFCFEE"].ToObject<JObject>().ConvertToJObjectArrayDetails();
            js.Remove("IFCFEE");
            js.Add("ifcfees", jsFees);
        }

        if (js.JsonContains("DATASET"))
        {
            JArray jsFees = jsFo["DATASET"].ToObject<JObject>().ConvertToJObjectArrayDetails();
            js.Remove("DATASET");
            js.Add("dataset", jsFees);
        }

        return js.ConvertToJObject();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="txcode"></param>
    /// <param name="rulefunc"></param>
    /// <returns></returns>
    public static List<JsonRuleFunc> GetListRuleFunc(string txcode, string rulefunc)
    {
        string _rules = TransRuleKey(txcode);
        List<JsonRuleFunc> clsJsonRuleFuncList = JSONDeserializeObject<List<JsonRuleFunc>>(_rules);
        List<JsonRuleFunc> clsJsonRuleFuncListFilter = null;

        if (string.IsNullOrEmpty(rulefunc))
        {
            return clsJsonRuleFuncList;
        }
        else
        {
            if (clsJsonRuleFuncList != null)
            {
                clsJsonRuleFuncListFilter = new List<JsonRuleFunc>();
                foreach (var rf in clsJsonRuleFuncList)
                {
                    if (rf.RULE.Equals(rulefunc))
                    {
                        clsJsonRuleFuncListFilter.Add(rf);
                    }
                }
            }

            return clsJsonRuleFuncListFilter;
        }
    }

    /// <summary>
    /// Searches the func using the specified data
    /// </summary>
    /// <param name="data">The data</param>
    /// <param name="sFunc">The func</param>
    /// <returns>The model search</returns>
    public static SearchFunc SearchFunc(object data, string sFunc)
    {
        var job = JToken.Parse(System.Text.Json.JsonSerializer.Serialize(data)).ToObject<JObject>();
        string listKey = O9Client.memCached.GetValue(
            GlobalVariable.O9_GLOBAL_COMCODE + ".SEARCH." + sFunc.ToUpper()
        );
        if (string.IsNullOrEmpty(listKey))
        {
            throw new Exception("Invalid SFUNC!");
        }

        var modelSearch = JsonConvert.DeserializeObject<SearchFunc>(listKey);

        string key = "oprtval_";
        foreach (var item in job.Properties().Where(x => x.Name.Contains(key)))
        {
            if (item.Name.Contains(key))
            {
                string ft = item.Name.Replace(key, "");
                foreach (var stag in modelSearch.LSTSFT.Where(x => x.FTAG == ft.ToUpper()))
                {
                    modelSearch.SetOPRTVALOfFtag(stag.FTAG, item.Value.ToString());
                }
            }
        }

        foreach (var keyValue in job)
        {
            modelSearch.SetValueOfFtag(keyValue.Key, keyValue.Value);
        }

        return modelSearch;
    }

    /// <summary>
    /// Backs the office using the specified user sessions
    /// </summary>
    /// <param name="userSessions">The user sessions</param>
    /// <param name="txCode">The tx code</param>
    /// <param name="tableName">The table name</param>
    /// <param name="model">The data</param>
    /// <exception cref="Exception"></exception>
    /// <returns>The json back office</returns>
    public static JsonBackOffice BackOffice(
        UserSessions userSessions,
        string txCode,
        string tableName,
        object model
    )
    {
        JsonTableName clsJson = new();
        clsJson.TXBODY.Add(new JsonData(tableName, model));
        string result = GenJsonBackOfficeRequest(userSessions, txCode, clsJson.TXBODY);

        var convert = JsonConvert.DeserializeObject<JsonBackOfficeBaseResponse>(result);
        if (convert == null || convert.R != 0)
        {
            throw new Exception(
                "Error BackOfficeRequest:" + (convert == null ? "Null data" : convert.M.ToString())
            );
        }

        return new JsonBackOffice(
            JsonConvert.DeserializeObject<JsonBackOfficeMapping>(convert.M.ToString()!)
        );
    }

    /// <summary>
    ///
    /// </summary>
    public static string GenJsonBackOfficeRequest(
        UserSessions userSessions,
        string pTXCODE,
        List<JsonData> pTXBODY,
        string functionId = "",
        EnmCacheAction isCaching = EnmCacheAction.NoCached,
        string pSTATUS = "C",
        string pTXREFID = null,
        string pVALUEDT = null,
        string pUSRWS = "",
        object pAPUSER = null,
        string pAPUSRIP = "",
        string pAPUSRWS = "",
        string pAPDT = "",
        string pISREVERSE = "N",
        int pHBRANCHID = 0,
        int pRBRANCHID = 0,
        string pAPREASON = null,
        string pPRN = "",
        bool isMappingToArray = false
    )
    {
        try
        {
            JsonBackOffice clsJsonBackOffice = new();
            if (string.IsNullOrEmpty(functionId))
            {
                functionId = GlobalVariable.UTIL_CALL_PROC;
            }

            clsJsonBackOffice.TXCODE = pTXCODE; // Field 1
            clsJsonBackOffice.TXDT = userSessions.Txdt.ToString("dd/MM/yyyy"); // Field 2
            clsJsonBackOffice.BRANCHID = userSessions.UsrBranchid; // Field 5
            clsJsonBackOffice.USRID = userSessions.Usrid; // Field 7
            clsJsonBackOffice.LANG = GlobalVariable.O9_GLOBAL_LANG.ToLower(); // Field 8
            clsJsonBackOffice.STATUS = pSTATUS; // Field 14
            clsJsonBackOffice.TXREFID = pTXREFID; // Field 3
            clsJsonBackOffice.VALUEDT = pVALUEDT; // Field 4
            clsJsonBackOffice.USRWS = string.IsNullOrEmpty(pUSRWS)
                ? GlobalVariable.O9_GLOBAL_COMPUTER_NAME
                : pUSRWS; // Field 6
            clsJsonBackOffice.APUSER = pAPUSER; // Field 9
            clsJsonBackOffice.APUSRIP = pAPUSRIP; // Field 10
            clsJsonBackOffice.APUSRWS = pAPUSRWS; // Field 11
            clsJsonBackOffice.APDT = pAPDT; // Field 12
            clsJsonBackOffice.ISREVERSE = pISREVERSE; // Field 14
            clsJsonBackOffice.HBRANCHID = pHBRANCHID; // Field 15
            clsJsonBackOffice.RBRANCHID = pRBRANCHID; // Field 16
            clsJsonBackOffice.TXBODY = pTXBODY; // Field 17
            clsJsonBackOffice.APREASON = pAPREASON; // Field 18
            clsJsonBackOffice.PRN = pPRN; // Field 19
            clsJsonBackOffice.ID = Guid.NewGuid().ToString(); // Field 20

            // Sent to hosting
            var result = GenJsonBodyRequest(
                new JsonBackOfficeMapping(clsJsonBackOffice, isMappingToArray),
                functionId,
                userSessions.Ssesionid,
                EnmCacheAction.NoCached,
                EnmSendTypeOption.Synchronize,
                userSessions.Usrid.ToString()
            );
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public static string ExecuteBackOffice(BackOfficeModel model)
    {
        try
        {
            JsonBackOffice clsJsonBackOffice = model.ToJsonBackOffice();

            string result = GenJsonBodyRequest(
                new JsonBackOfficeMapping(clsJsonBackOffice, model.HasArrayValue),
                model.FunctionId,
                model.SessionId,
                EnmCacheAction.NoCached,
                EnmSendTypeOption.Synchronize,
                model.UserId.ToString()
            );
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return string.Empty;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public static JToken Search(
        string sql,
        int page = 0,
        string searchFunc = null,
        EnmCacheAction enmCacheAction = EnmCacheAction.NoCached
    )
    {
        string proc = "UTIL_SEARCH_DATA";
        string hsql = "";

        try
        {
            JObject jsCondition = new()
            {
                { "S", sql },
                { "H", hsql },
                { "L", "en" },
                { "P", page },
            };

            string strResult = GenJsonBodyRequest(jsCondition, proc, "", enmCacheAction);
            if (!string.IsNullOrEmpty(strResult))
            {
                // JToken data = new JArray();
                // JToken parsedJson = JToken.Parse(strResult);
                // if (parsedJson["0"] != null)
                // {
                //     data = JToken.Parse(strResult).SelectToken("0");
                // }
                JToken data = JToken.Parse(strResult).SelectToken("0");

                JsonSerializerSettings js = new() { DateFormatString = "dd/MM/yyyy" };
                object o = JsonConvert.DeserializeObject<object>(
                    strResult,
                    new JsonConverterCore()
                );

                JObject result = new() { { "data", data } };

                if (page == 1)
                {
                    JToken pg = JToken.Parse(strResult).SelectToken("T");

                    JObject jPaging = CreatePaging(pg, page);
                    result.Add("paging", jPaging);
                    if (searchFunc != null)
                    {
                        if (Singleton<PagingData>.Instance == null)
                        {
                            Singleton<PagingData>.Instance = new PagingData();
                        }

                        Singleton<PagingData>.Instance.Paging[searchFunc] = jPaging["total"]
                            .ToInt();
                    }
                }

                return result;
            }

            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
            throw new Exception("Search");
        }
    }

    /// <summary>
    /// Creates the paging using the specified j page
    /// </summary>
    /// <param name="jPage">The page</param>
    /// <param name="page">The page</param>
    /// <exception cref="Exception">CreatePaging</exception>
    /// <returns>The object</returns>
    public static JObject CreatePaging(JToken jPage, int page)
    {
        try
        {
            JObject jPaging = (JObject)((JArray)jPage)[0];
            double total = double.Parse(jPaging.SelectToken("0").ToString());
            jPaging.Add("last_page", (int)Math.Ceiling(total / GlobalVariable.O9_PAGE_SIZE));
            jPaging.Add("per_page", GlobalVariable.O9_PAGE_SIZE);
            jPaging.Add("current_page", page);
            jPaging.Add("total", (int)total);
            jPaging.Remove("0");

            return jPaging;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.StackTrace);
            throw new Exception("CreatePaging");
        }
    }

    /// <summary>
    ///
    /// </summary>
    public static TransactionResponse AnalysisLoginResult(
        ILogger logger,
        string error,
        string wsName = ""
    )
    {
        TransactionResponse reponse = new();
        if (error.Equals("SYS_INVALID_SESSION"))
        {
            reponse.SetCode(Codetypes.SYS_INVALID_SESSION);
            reponse.ERRORDESC = reponse.ERRORDESC + "[" + wsName + "]";
        }
        else if (error.Equals("UMG_INVALID_LOGIN_TIME"))
        {
            reponse.SetCode(Codetypes.UMG_INVALID_LOGIN_TIME);
        }
        else if (error.Equals("UMG_INVALID_EXP_POLICY"))
        {
            reponse.SetCode(Codetypes.UMG_INVALID_EXP_POLICY);
        }
        else if (error.Equals("SYS_LOGIN_FALSE"))
        {
            reponse.SetCode(Codetypes.SYS_LOGIN_FALSE);
        }
        else if (error.Equals("SYS_LOGIN_BLOCK"))
        {
            reponse.SetCode(Codetypes.SYS_LOGIN_BLOCK);
        }
        else if (error.Equals("UMG_INVALID_STATUS"))
        {
            reponse.SetCode(Codetypes.UMG_INVALID_STATUS);
        }
        else if (error.Equals("UMG_INVALID_EXPDT"))
        {
            reponse.SetCode(Codetypes.UMG_INVALID_EXPDT);
        }
        else
        {
            reponse.SetCode(Codetypes.Err_Unknown);
        }

        return reponse;
    }

    /// <summary>
    ///
    /// </summary>
    public static bool isErrorResponse(string strResponse)
    {
        if (
            strResponse.Equals("TIMEOUTAQ")
            || strResponse.Contains("COREERRORSYSTEM")
            || string.IsNullOrEmpty(strResponse)
        )
        {
            return true;
        }

        return false;
    }

    /// <summary>
    ///
    /// </summary>
    public static TransactionResponse AnalysisFrontOfficeResult(
        ILogger logger,
        string strResponse,
        bool ispare = true
    )
    {
        TransactionResponse TransactionResponse = new();
        JsonFrontOffice clsJsonFrontOffice;
        JsonFrontOfficeMapping clsJsonFrontOfficeMP;
        try
        {
            JsonResponse jsonResponse = JsonConvert.DeserializeObject<JsonResponse>(strResponse);
            if (jsonResponse.IsOK())
            {
                if (ispare)
                {
                    clsJsonFrontOfficeMP = JsonConvert.DeserializeObject<JsonFrontOfficeMapping>(
                        jsonResponse.GetMessage()
                    );
                    clsJsonFrontOffice = new JsonFrontOffice(clsJsonFrontOfficeMP);
                }
                else
                {
                    clsJsonFrontOffice = JsonConvert.DeserializeObject<JsonFrontOffice>(
                        jsonResponse.GetMessage()
                    );
                }

                TransactionResponse.SetCode(Codetypes.Code_Success_Trans);
                if (clsJsonFrontOffice.RESULT != null)
                {
                    TransactionResponse.SetResult(clsJsonFrontOffice.RESULT);
                }

                if (clsJsonFrontOffice.IBRET != null)
                {
                    TransactionResponse.SetResult(clsJsonFrontOffice.IBRET);
                }
            }
            else
            {
                if (jsonResponse.GetMessage().Contains("SYS_AUTHORIZE_VIOLATE"))
                {
                    TransactionResponse.SetCode(Codetypes.Err_Unauthorized);
                }
                else
                {
                    string messageresult = jsonResponse.GetMessage();
                    int errorCode = int.Parse(GetErrorCode(logger, messageresult));
                    if (errorCode == 9998)
                    {
                        TransactionResponse.SetCode(Codetypes.ERR_CORE_BANKING);
                    }
                    else
                    {
                        TransactionResponse.SetCode(
                            new CodeDescription(errorCode, GetReasonString(logger, messageresult))
                        );
                    }
                }
            }

            return TransactionResponse;
        }
        catch (Exception)
        {
            TransactionResponse.SetCode(Codetypes.Err_Unknown);
            return TransactionResponse;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public static JObject AnalysisFunctionResult(string strResponse, bool isNeedMapping = true)
    {
        var jsResult = new JObject();
        JsonFunction clsFunction;
        JsonFunctionMapping jsonFunctionMapping;
        try
        {
            var jsonResponse = GetJsonResponse(strResponse);

            if (jsonResponse.IsOK())
            {
                jsonFunctionMapping = JsonConvert.DeserializeObject<JsonFunctionMapping>(
                    jsonResponse.GetMessage(),
                    JsonSerializerSetting
                );
                clsFunction = new JsonFunction(jsonFunctionMapping);
                var rsOk = clsFunction.RESULT ?? new JObject();
                jsResult = rsOk;
            }
            else
            {
                throw new Exception(jsonResponse.GetMessage());
            }

            return jsResult;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    ///
    /// </summary>
    public static JObject GetMapFieldFrontOffice(string txcode)
    {
        if (string.IsNullOrEmpty(txcode))
        {
            return null;
        }

        JObject jsObject = null;
        string key = "O9." + O9Client.OP_MCKEY_FMAP + "." + txcode;
        string strMapField = O9Client.memCached.GetValue(key);
        if (strMapField != null && !strMapField.Equals(""))
        {
            jsObject = JsonConvert.DeserializeObject<JObject>(strMapField);
        }

        return jsObject;
    }

    /// <summary>
    ///
    /// </summary>
    public static bool JsonContains(JObject jsObject, string key)
    {
        object jsItem = null;
        if (!string.IsNullOrEmpty(key))
        {
            jsItem = jsObject.SelectToken(key);
        }

        if (jsItem != null)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    ///
    /// </summary>
    public static void UpdateWorkingDate(string date)
    {
        if (!GlobalVariable.O9_GLOBAL_TXDT.Equals(date))
        {
            GlobalVariable.O9_GLOBAL_TXDT = date;
        }
    }

    public static CodeDescription GetReasonString1(string strResult)
    {
        CodeDescription Result = new CodeDescription();

        string strReason = string.Empty;
        int start = strResult.IndexOf("OTM#") + 4;
        int end = strResult.IndexOf(":") - 5;

        Result.ERRORCODE = int.Parse(strReason = strResult.Substring(start, end));
        int start2 = strResult.IndexOf("]") + 1;
        string strEnd = " - " + "en";
        Result.ERRORDESC = strReason = strResult
            .Substring(start2, strResult.Length - start2)
            .Trim();

        if (Result.ERRORDESC.EndsWith(strEnd))
        {
            Result.ERRORDESC = strReason.Substring(0, strReason.Length - strEnd.Length);
        }

        if (strReason.Contains("ORA-"))
        {
            strReason = "Error when processing. Please contact EMI to get more information";
        }

        return Result;
    }

    /// <summary>
    ///
    /// </summary>
    public static string GetReasonString(ILogger logger, string strResult)
    {
        logger.Information("Core Return information: " + strResult);
        string strReason = string.Empty;
        int start = strResult.IndexOf("]") + 1;
        string strEnd = " - " + "en";
        strReason = strResult.Substring(start, strResult.Length - start);
        if (strReason.EndsWith(strEnd))
        {
            strReason = strReason.Substring(0, strReason.Length - strEnd.Length);
        }

        if (strReason.Contains("ORA-"))
        {
            strReason = "Error when processing. Please contact Bank to get more information";
        }

        return strReason.Trim();
    }

    /// <summary>
    ///
    /// </summary>
    public static string GetErrorCode(ILogger logger, string strResult)
    {
        logger.Information("Core Return information: " + strResult);
        string strReason = string.Empty;
        int start = strResult.IndexOf("#") + 1;
        string[] substr = strResult.Split(':');
        strReason = strResult.Substring(start, substr[0].Length - start);
        if (strResult.Contains("ORA-"))
        {
            strReason = "9998";
        }

        return strReason.Trim();
    }

    /// <summary>
    ///
    /// </summary>
    public static string GetErrorName(string strResult)
    {
        if (strResult.Contains("ORA-"))
        {
            return "";
        }

        string strReason = string.Empty;
        int start = strResult.IndexOf("[") + 1;
        string[] substr = strResult.Split(']');
        strReason = strResult.Substring(start, substr[0].Length - start);
        return strReason.Trim();
    }

    /// <summary>
    ///
    /// </summary>
    public static string ConvertTimeStampToShortString(long time)
    {
        DateTime dt = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dt = dt.AddMilliseconds(time);
        return dt.ToString(GlobalVariable.FORMAT_SHORT_DATE);
    }

    /// <summary>
    ///
    /// </summary>
    public static string ConvertTimeStampToLongString(long time)
    {
        DateTime dt = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dt = dt.AddMilliseconds(time);
        return dt.ToString(GlobalVariable.FORMAT_LONG_DATE);
    }

    /// <summary>
    ///
    /// </summary>
    public static JObject RemoveNullForSearchData(JToken strSearch)
    {
        JObject jsResult = JObject.FromObject(strSearch);
        if (jsResult.ContainsKey("data"))
        {
            if (string.IsNullOrEmpty(jsResult.GetValue("data").ToString()))
            {
                jsResult["data"] = new JArray();
            }
        }

        return jsResult;
    }

    /// <summary>
    ///
    /// </summary>
    public static string GetErrorByLang(string errName)
    {
        if (string.IsNullOrEmpty(errName))
        {
            return "";
        }

        return GetErrorByLang(JObject.Parse(errName));
    }

    /// <summary>
    ///
    /// </summary>
    public static string GetErrorByLang(JObject jsErrDescr)
    {
        string strErrDescr = "";

        if (jsErrDescr != null && jsErrDescr.Count > 0)
        {
            if (jsErrDescr != null)
            {
                strErrDescr = GetStringFromJsonObjectByLang(jsErrDescr);
            }

            if (string.IsNullOrEmpty(strErrDescr))
            {
                strErrDescr = GetStringFromJsonObjectByLang(jsErrDescr, "en");
            }
        }

        return "OTM#" + strErrDescr;
    }

    /// <summary>
    ///
    /// </summary>
    public static string GetStringFromJsonObjectByLang(JObject jsObject, string lang = "")
    {
        try
        {
            if (jsObject != null)
            {
                if (string.IsNullOrEmpty(lang))
                {
                    lang = GlobalVariable.O9_GLOBAL_LANG;
                }

                foreach (var jsToken in jsObject)
                {
                    if (jsToken.Key.ToLower().Equals(lang))
                    {
                        return jsToken.Value.ToString().Replace("\"", "");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
            throw new Exception("GetStringFromJsonObjectByLang");
        }

        return null;
    }

    /// <summary>
    ///
    /// </summary>
    public static JsonSerializerSettings JsonSerializerSetting = new()
    {
        FloatParseHandling = FloatParseHandling.Decimal,
    };

    /// <summary>
    ///
    /// </summary>
    public static JsonFrontOffice AnalysisFrontOfficeResult(string response, string channel = "")
    {
        try
        {
            JsonResponse clsJsonResponse = GetJsonResponse(response);

            if (clsJsonResponse != null)
            {
                if (clsJsonResponse.IsOK())
                {
                    var message = clsJsonResponse.GetMessage();
                    var jsonFrontOfficeMapping =
                        JsonConvert.DeserializeObject<JsonFrontOfficeMapping>(
                            message,
                            JsonSerializerSetting
                        );

                    JsonFrontOffice result = new(jsonFrontOfficeMapping);
                    return result;
                }
                else
                {
                    if (clsJsonResponse.GetMessage().Contains("SYS_AUTHORIZE_VIOLATE"))
                    {
                        CoreUtils.LoginO9ForDigital().GetAsyncResult();
                    }
                    throw new O24OpenAPIException(clsJsonResponse.GetMessage());
                }
            }

            throw new O24OpenAPIException("No response");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
            throw new O24OpenAPIException(ex.Message);
        }
    }

    /// <summary>
    ///
    /// </summary>
    public static EnmResultResponse AnalysisWarningResult(string strResult, string strReason = "")
    {
        if (!string.IsNullOrEmpty(strResult))
        {
            strReason = GetReasonString(strResult);
            if (strReason.Contains("SYS_OVER_LIMIT"))
            {
                return EnmResultResponse.SYS_OVER_LIMIT;
            }
            else if (strReason.Contains("SYS_APR_STATUS"))
            {
                return EnmResultResponse.SYS_APR_STATUS;
            }
            else if (strReason.Contains("SYS_APR_AMT"))
            {
                return EnmResultResponse.SYS_APR_AMT;
            }
            else if (strReason.Contains("STK_CHANGE_BRANCH"))
            {
                return EnmResultResponse.STK_CHANGE_BRANCH;
            }
            else if (strReason.Contains("SYS_APR_WD"))
            {
                return EnmResultResponse.SYS_APR_WD;
            }
            else if (strReason.Contains("SYS_APPROVAL_REQUIRED"))
            {
                return EnmResultResponse.SYS_APPROVAL_REQUIRED;
            }
            else if (strReason.Contains("SYS_APR_CWR"))
            {
                return EnmResultResponse.SYS_APR_CWR;
            }
            else if (strReason.Contains("PMT_APR_MULTI"))
            {
                return EnmResultResponse.PMT_APR_MULTI;
            }
            else if (strReason.Contains("SYS_APR_DPT_OPN"))
            {
                return EnmResultResponse.SYS_APR_DPT_OPN;
            }
            else if (strReason.Contains("SYS_WARNING"))
            {
                return EnmResultResponse.SYS_WARNING;
            }
        }

        return EnmResultResponse.NOT_SUCCESS;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="strResponse"></param>
    /// <param name="jsResult"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static JObject AnalysisBackOfficeResult(string strResponse, JObject jsResult = null)
    {
        try
        {
            JsonResponse clsJsonResponse = O9Utils.GetJsonResponse(strResponse);

            if (clsJsonResponse != null)
            {
                if (clsJsonResponse.IsOK())
                {
                    JsonBackOffice clsJsonBackOffice = new(
                        JsonConvert.DeserializeObject<JsonBackOfficeMapping>(
                            clsJsonResponse.GetMessage(),
                            JsonSerializerSetting
                        )
                    );
                    if (clsJsonBackOffice != null && clsJsonBackOffice.TXBODY.Count > 0)
                    {
                        return (JObject)clsJsonBackOffice.TXBODY[0].DATA;
                    }
                }
                else
                {
                    throw new Exception(clsJsonResponse.GetMessage());
                }
            }

            return jsResult;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.StackTrace);
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="strResponse"></param>
    /// <param name="jsResult"></param>
    /// <param name="clsJsonBackOffice"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static bool AnalysisBackOfficeResultClient(
        string strResponse,
        ref JObject jsResult,
        ref JsonBackOffice clsJsonBackOffice
    )
    {
        try
        {
            JsonResponse clsJsonResponse = O9Utils.GetJsonResponse(strResponse);

            if (clsJsonResponse != null)
            {
                if (clsJsonResponse.IsOK())
                {
                    clsJsonBackOffice = new(
                        JsonConvert.DeserializeObject<JsonBackOfficeMapping>(
                            clsJsonResponse.GetMessage(),
                            JsonSerializerSetting
                        )
                    );
                    if (clsJsonBackOffice != null && clsJsonBackOffice.TXBODY.Count > 0)
                    {
                        jsResult = (JObject)clsJsonBackOffice.TXBODY[0].DATA;
                    }

                    return true;
                }
                else
                {
                    throw new Exception(clsJsonResponse.GetMessage());
                }
            }

            return false;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.StackTrace);
            throw new Exception(ex.Message);
        }
    }

    public static async Task<bool> LoginO9ForDigital()
    {
        await Task.CompletedTask;
        var setting = GlobalVariable.Optimal9Settings;
        JsonLogin loginRequest = new()
        {
            L = setting.O9UserName,
            P = !setting.O9Encrypt
                ? setting.O9Password
                : O9Encrypt.SHA256Encrypt(setting.O9Password),
            A = false,
        };

        string strResult = GenJsonBodyRequest(
            loginRequest,
            GlobalVariable.UMG_LOGIN,
            "",
            EnmCacheAction.NoCached,
            EnmSendTypeOption.Synchronize,
            "-1",
            MsgPriority.Normal
        );

        if (isErrorResponse(strResult))
        {
            return false;
        }

        var clsJsonLoginResponse = JsonConvert.DeserializeObject<JsonLoginResponse>(strResult);
        if (clsJsonLoginResponse != null)
        {
            if (!string.IsNullOrEmpty(clsJsonLoginResponse.UUID))
            {
                O9Client.CoreBankingSession = clsJsonLoginResponse;
                return true;
            }
        }
        return false;
    }

    /// <summary>
    ///
    /// </summary>
    public static JObject AnalysisBOResult(
        string strResponse,
        JObject jsResult = null,
        bool isNeedMapping = true
    )
    {
        try
        {
            JsonResponse clsJsonResponse = GetJsonResponse(strResponse);

            if (clsJsonResponse != null)
            {
                if (clsJsonResponse.IsOK())
                {
                    var backOfficeMapping = JsonConvert.DeserializeObject<JsonBackOfficeMapping>(
                        clsJsonResponse.GetMessage(),
                        JsonSerializerSetting
                    );
                    JsonBackOffice clsJsonBackOffice = new(backOfficeMapping);

                    if (clsJsonBackOffice != null && clsJsonBackOffice.TXBODY.Count > 0)
                    {
                        var rsOk = (JObject)clsJsonBackOffice.TXBODY[0].DATA; //.ConvertToJObject();
                        jsResult = rsOk.BuildWorkflowResponseSuccess(isNeedMapping);
                    }
                }
                else
                {
                    throw new Exception(clsJsonResponse.GetMessage());
                    //jsResult = clsJsonResponse.GetMessage().BuildWorkflowResponseError();
                }
            }

            return jsResult;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="strResponse"></param>
    /// <param name="jsResult"></param>
    /// <param name="isNeedMapping"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static JObject AnalysisBOResultSingle(
        string strResponse,
        JObject jsResult = null,
        bool isNeedMapping = true
    )
    {
        try
        {
            JsonResponse clsJsonResponse = GetJsonResponse(strResponse);

            if (clsJsonResponse != null)
            {
                if (clsJsonResponse.IsOK())
                {
                    var dataMapping = JsonConvert.DeserializeObject<JsonDataMapping>(
                        clsJsonResponse.GetMessage(),
                        JsonSerializerSetting
                    );
                    JsonBackOfficeMapping backOfficeMapping = new();
                    backOfficeMapping.S.Add(dataMapping);
                    JsonBackOffice clsJsonBackOffice = new(backOfficeMapping);

                    if (clsJsonBackOffice != null && clsJsonBackOffice.TXBODY.Count > 0)
                    {
                        //var rsOk = ((JObject)clsJsonBackOffice.TXBODY[0].DATA).ConvertToJObject();
                        jsResult = clsJsonBackOffice
                            .TXBODY[0]
                            .BuildWorkflowResponseSuccess(isNeedMapping);
                    }
                }
                else
                {
                    //throw new Exception(clsJsonResponse.GetMessage());
                    jsResult = clsJsonResponse.GetMessage().BuildWorkflowResponseError();
                }
            }

            return jsResult;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    ///
    /// </summary>
    public static string GetReasonString(string strResult)
    {
        string strReason = "";
        int start = strResult.IndexOf("]") + 1;
        string strEnd = " - en";
        strReason = strResult.Substring(start, strResult.Length - start);
        if (strReason.EndsWith(strEnd))
        {
            strReason = strReason.Substring(0, strReason.Length - strEnd.Length);
        }

        return strReason.Trim();
    }

    /// <summary>
    ///
    /// </summary>
    public static JsonResponse GetJsonResponse(string response)
    {
        if (string.IsNullOrEmpty(response))
        {
            JsonResponse r = new();
            r.M =
                "{\"en\":\"Please check connection to Server!\",\"vi\":\"Vui lòng kiểm tra kết nối với máy chủ!\",\"lo\":\"\",\"km\":\"\",\"mn\":\"\"}";
            r.R = (int)EnmJsonResponse.E;
        }

        return JsonConvert.DeserializeObject<JsonResponse>(response, JsonSerializerSetting);
    }

    /// <summary>
    ///
    /// </summary>
    public static DateTime? ConvertStringToDateTime(string strDateTime)
    {
        if (!string.IsNullOrEmpty(strDateTime))
        {
            CultureInfo culInfo = new(O9Constants.O9_GENERAL_CULTURE);
            // if (O9Config.O9_GLOBAL_USER_PARAM != null) culInfo = new CultureInfo(O9Config.O9_GLOBAL_USER_PARAM.GENERAL_LANGUAGE_CULTURE ?? "en-GB");
            culInfo.DateTimeFormat.ShortDatePattern = GlobalVariable
                .O9_GLOBAL_HEADOFFICE_PARAM
                .GENERAL_SHORT_DATE_FMT;
            culInfo.DateTimeFormat.LongDatePattern = GlobalVariable
                .O9_GLOBAL_HEADOFFICE_PARAM
                .GENERAL_LONG_DATE_FMT;
            culInfo.DateTimeFormat.ShortTimePattern = GlobalVariable
                .O9_GLOBAL_HEADOFFICE_PARAM
                .GENERAL_SHORT_TIME_FMT;
            culInfo.DateTimeFormat.LongTimePattern = GlobalVariable
                .O9_GLOBAL_HEADOFFICE_PARAM
                .GENERAL_LONG_TIME_FMT;
            return Convert.ToDateTime(strDateTime, culInfo);
        }

        return null;
    }

    /// <summary>
    ///
    /// </summary>
    public static string ConvertDateToStringFormat(DateTime? dt, string fmt = "")
    {
        if (string.IsNullOrEmpty(fmt))
        {
            fmt = O9Constants.O9_CONSTANT_SHORT_DATE_FORMAT;
        }

        if (dt != null)
        {
            CultureInfo cultInfo = new("en-GB");
            return dt?.ToString(fmt, cultInfo);
        }

        return "";
    }

    /// <summary>
    ///
    /// </summary>
    public static long ConvertDateToLong(DateTime? dt)
    {
        if (dt == null)
        {
            return 0;
        }

        //Converted to local timezone
        DateTime utcDateTime = (DateTime)dt;
        TimeZoneInfo timeZone = TimeZoneInfo.Local;
        var dateTimeUnspec = DateTime.SpecifyKind(utcDateTime, DateTimeKind.Unspecified);
        var convert = TimeZoneInfo.ConvertTimeToUtc(dateTimeUnspec, timeZone);

        long epoch = (convert.Ticks - 621355968000000000) / TimeSpan.TicksPerMillisecond;
        //return dt.Value.Ticks / TimeSpan.TicksPerMillisecond;
        return epoch;
    }

    /// <summary>
    ///
    /// </summary>
    public static long ConvertDateStringToLong(string stringDate)
    {
        if (string.IsNullOrEmpty(stringDate))
        {
            return 0;
        }

        DateTime? dt = ConvertStringToDateTime(stringDate);
        return ConvertDateToLong(dt);
    }

    /// <summary>
    ///
    /// </summary>
    public static string ConvertDateToLongStringFormat(DateTime dt, string fmt = "")
    {
        if (string.IsNullOrEmpty(fmt))
        {
            fmt = GlobalVariable.O9_GLOBAL_HEADOFFICE_PARAM.GENERAL_DATE_TIME_FMT;
        }

        CultureInfo culture = new("en-US");
        return dt.ToString(fmt, culture);
    }

    /// <summary>
    ///
    /// </summary>
    public static DateTime ConvertLongToDateTime(long dt)
    {
        //return new DateTime(dt * 10000 + 621355968000000000);
        DateTime start = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        DateTime date = start.AddMilliseconds(dt).ToLocalTime();
        return date;
    }

    /// <summary>
    ///
    /// </summary>
    public static string ConvertLongToDateString(long longDate)
    {
        DateTime dt = ConvertLongToDateTime(longDate);
        return ConvertDateToLongStringFormat(dt);
    }

    /// <summary>
    ///
    /// </summary>
    public static DateTime ConvertToDateTimeFormat(object dt)
    {
        return Convert.ToDateTime(
            dt,
            new CultureInfo(O9Constants.O9_GENERAL_CULTURE).DateTimeFormat
        );
    }

    /// <summary>
    ///
    /// </summary>
    public static string ConvertStringDatimeToDefaultFormat(string strDatime)
    {
        foreach (var item in CultureInfo.CurrentCulture.DateTimeFormat.GetAllDateTimePatterns())
        {
            DateTime dt;
            if (
                DateTime.TryParseExact(
                    strDatime,
                    item,
                    new CultureInfo(O9Constants.O9_GENERAL_CULTURE),
                    DateTimeStyles.None,
                    out dt
                )
            )
            {
                return dt.ToString(O9Constants.O9_CONSTANT_SHORT_DATE_FORMAT);
            }
        }

        return strDatime;
    }

    /// <summary>
    ///
    /// </summary>
    public static bool IsDatetime(string strDatetime, out string format)
    {
        DateTime dt;
        foreach (var item in GlobalVariable.DatetimeFormat)
        {
            if (
                DateTime.TryParseExact(
                    strDatetime,
                    item,
                    new CultureInfo(O9Constants.O9_GENERAL_CULTURE),
                    DateTimeStyles.None,
                    out dt
                )
            )
            {
                format = item;
                return true;
            }
        }

        foreach (var item in CultureInfo.CurrentCulture.DateTimeFormat.GetAllDateTimePatterns())
        {
            if (
                DateTime.TryParseExact(
                    strDatetime,
                    item,
                    new CultureInfo(O9Constants.O9_GENERAL_CULTURE),
                    DateTimeStyles.None,
                    out dt
                )
            )
            {
                format = item;
                return true;
            }
        }

        format = string.Empty;
        return false;
    }

    /// <summary>
    ///
    /// </summary>
    public static bool CheckValueIsDate(object value, ref DateTime dt)
    {
        if (
            !DateTime.TryParseExact(
                value.ToString(),
                O9Constants.O9_CONSTANT_SHORT_DATE_FORMAT,
                new CultureInfo(O9Constants.O9_GENERAL_CULTURE),
                DateTimeStyles.None,
                out dt
            )
        )
        {
            dt = ConvertToDateTimeFormat(O9Constants.O9_CONSTANT_DATE_COMPARE);
            return false;
        }

        return true;
    }

    /// <summary>
    ///
    /// </summary>
    public static bool CheckValueIsDateExtension(object value, ref DateTime dt)
    {
        foreach (var item in GlobalVariable.DatetimeFormat)
        {
            if (
                DateTime.TryParseExact(
                    value.ToString(),
                    item,
                    new CultureInfo(O9Constants.O9_GENERAL_CULTURE),
                    DateTimeStyles.None,
                    out dt
                )
            )
            {
                return true;
            }
        }

        foreach (var item in CultureInfo.CurrentCulture.DateTimeFormat.GetAllDateTimePatterns())
        {
            if (
                DateTime.TryParseExact(
                    value.ToString(),
                    item,
                    new CultureInfo(O9Constants.O9_GENERAL_CULTURE),
                    DateTimeStyles.None,
                    out dt
                )
            )
            {
                return true;
            }
        }

        if (
            !DateTime.TryParseExact(
                value.ToString(),
                O9Constants.O9_CONSTANT_SHORT_DATE_FORMAT,
                new CultureInfo(O9Constants.O9_GENERAL_CULTURE),
                DateTimeStyles.None,
                out dt
            )
        )
        {
            dt = ConvertToDateTimeFormat(O9Constants.O9_CONSTANT_DATE_COMPARE);
            return false;
        }

        return true;
    }

    /// <summary>
    ///
    /// </summary>
    public static string ConvertTimeToStringFormat(DateTime dt, string fmt = "")
    {
        if (string.IsNullOrEmpty(fmt))
        {
            fmt = GlobalVariable.O9_GLOBAL_HEADOFFICE_PARAM.GENERAL_SHORT_TIME_FMT;
        }

        try
        {
            return dt.ToString(fmt);
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static DateTime ConvertDateTimeToUTC(string dt)
    {
        DateTime utcDateTime = DateTime.Parse(dt);

        TimeZoneInfo timeZone = TimeZoneInfo.Local;

        var dateTimeUnspec = DateTime.SpecifyKind(utcDateTime, DateTimeKind.Unspecified);

        var convert = TimeZoneInfo.ConvertTimeToUtc(dateTimeUnspec, timeZone);

        return convert;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static DateTime ConvertDateTimeFromUTC(string dt)
    {
        DateTime utcDateTime = DateTime.Parse(dt);

        TimeZoneInfo timeZone = TimeZoneInfo.Local;

        var dateTimeUnspec = DateTime.SpecifyKind(utcDateTime, DateTimeKind.Unspecified);

        var convert = TimeZoneInfo.ConvertTimeFromUtc(dateTimeUnspec, timeZone);

        return convert;
    }

    /// <summary>
    ///
    /// </summary>
    public static bool CheckValueIsTime(object value, ref DateTime dt)
    {
        if (
            !DateTime.TryParseExact(
                value.ToString(),
                O9Constants.O9_CONSTANT_TIME_FORMAT,
                new CultureInfo(O9Constants.O9_GENERAL_CULTURE),
                DateTimeStyles.None,
                out dt
            )
        )
        {
            dt = ConvertToDateTimeFormat(O9Constants.O9_CONSTANT_DATE_COMPARE);
            return false;
        }

        return true;
    }

    /// <summary>
    ///
    /// </summary>
    public static JObject MappingFtag(JObject js, SearchFunc sf)
    {
        string key;
        string keyNew;
        JObject jsResult = new();
        foreach (var item in js)
        {
            key = item.Key;
            foreach (var item2 in sf.LSTSFT)
            {
                if (key.ToUpper() == item2.FTAG.Replace("_", "").ToUpper())
                {
                    keyNew = item2.FTAG;
                    jsResult.Add(keyNew, js.GetValue(key));
                    break;
                }
            }
        }

        return jsResult.ToLowerKey();
    }

    /// <summary>
    ///
    /// </summary>
    public static string GenQuery(string query, JObject jsObj)
    {
        try
        {
            string strReturn = "";
            string key = "";
            string tableKey = "";
            string value = "";
            string[] strQuery;

            if (query.Contains(O9Constants.O9_CONSTANT_PARAM_KEY + O9Constants.O9_CONSTANT_O9MSG))
            {
                query = query.Replace(
                    O9Constants.O9_CONSTANT_PARAM_KEY + O9Constants.O9_CONSTANT_O9MSG,
                    jsObj.ToString()
                );
            }

            if (
                query.Contains("'" + O9Constants.O9_CONSTANT_PARAM_KEY)
                || query.Contains("'" + O9Constants.O9_CONSTANT_TABLE_KEY)
            )
            {
                foreach (var jsToken in jsObj)
                {
                    key =
                        "'"
                        + O9Constants.O9_CONSTANT_PARAM_KEY
                        + jsToken.Key.Trim().ToUpper()
                        + "'";
                    tableKey =
                        "'"
                        + O9Constants.O9_CONSTANT_TABLE_KEY
                        + jsToken.Key.Trim().ToUpper()
                        + "'";

                    if (jsToken.Value.Type.Equals(JTokenType.String))
                    {
                        value = jsToken.Value.ToString();
                        value = "'" + value.Replace("'", "''") + "'";
                    }
                    else
                    {
                        value = jsToken.Value.ToString();
                    }

                    if (query.Contains(key) || query.Contains(tableKey))
                    {
                        if (!string.IsNullOrEmpty(value))
                        {
                            query = query.Replace(key, value);
                            if (query.Contains(tableKey))
                            {
                                query = query.Replace(tableKey, value.Replace("'", ""));
                            }

                            if (
                                !query.Contains("'" + O9Constants.O9_CONSTANT_PARAM_KEY)
                                && !query.Contains("'" + O9Constants.O9_CONSTANT_TABLE_KEY)
                            )
                            {
                                break;
                            }
                        }
                    }
                }

                if (!query.Contains("'" + O9Constants.O9_CONSTANT_PARAM_KEY))
                {
                    strReturn = query;
                }
                else
                {
                    strQuery = query.Split("'");
                    foreach (var str in strQuery)
                    {
                        if (str.StartsWith(O9Constants.O9_CONSTANT_PARAM_KEY))
                        {
                            query = query.Replace(str, "");
                        }
                    }

                    strReturn = query;
                }
            }
            else
            {
                strReturn = query;
            }

            return strReturn;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.StackTrace);
            throw new Exception("GenQuery");
        }
    }

    /// <summary>
    ///
    /// </summary>
    public static void SortedFieldsData(SearchFunc sf, ref JArray jarray)
    {
        JsonSearchFtag sftag = new();
        DataTable dtb = new();
        foreach (var item in jarray)
        {
            if (item.Type == JTokenType.Object)
            {
                DataRow row = dtb.NewRow();
                foreach (var jobj in JObject.FromObject(item).Properties())
                {
                    if (!dtb.Columns.Contains(jobj.Name))
                    {
                        dtb.Columns.Add(jobj.Name, jobj.Name.GetType());
                    }

                    row[jobj.Name] = jobj.Value;
                }

                dtb.Rows.Add(row);
            }
        }

        dtb.TableName = "dataset";
        string[] lstcolumnNames = dtb
            .Columns.Cast<DataColumn>()
            .Select(x => x.ColumnName)
            .ToArray();
        DataTable dt = dtb;
        //Set column name for table
        string ColumnName;
        int i = 0;

        foreach (JsonSearchFtag sfag in sf.LSTSFT.Where(x => x.VISIBLE == true))
        {
            if (dtb.Columns.Count > i)
            {
                ColumnName = string.Empty;
                ColumnName = O9Utils.GetStringFromJsonObjectByLang(sfag.CAPTION);

                if (ColumnName != string.Empty)
                {
                    dtb.Columns[i].ColumnName = sfag.FTAG.ToLower();
                }

                i++;
            }
        }

        //sort
        string sort = string.Empty;
        for (i = 0; i < sf.LSTSFT.Count; i++)
        {
            sftag = sf.LSTSFT[i];
            if (sftag != null && sftag.VISIBLE == true)
            {
                if (sftag.ORD == 1 || sftag.ORD == 2)
                {
                    sort += sftag.ORD == 1 ? sftag.FTAG + " ASC," : sftag.FTAG + " DESC,";
                }
            }
        }

        if (sort.EndsWith(","))
        {
            sort = sort.TrimEnd(',');
        }

        dtb.DefaultView.Sort = sort;
        dt = dtb.DefaultView.ToTable();

        for (i = 0; i < dt.Columns.Count; i++)
        {
            dt.Columns[i].ColumnName = lstcolumnNames[i];
        }

        jarray = JArray.Parse(JsonConvert.SerializeObject(dt));
    }

    /// <summary>
    ///
    /// </summary>
    public static JObject ConvertJArrayToJObject(JArray array)
    {
        JObject jsObj = new();
        foreach (var item in array)
        {
            JObject jsob = JObject.FromObject(item);
            foreach (var item2 in jsob.Properties())
            {
                JArray jtmp = new();
                if (jsObj.Count == 0 || !jsObj.ContainsKey(item2.Name.ToString()))
                {
                    jtmp.Add(item2.Value);
                    jsObj.Add(item2.Name, jtmp);
                }
                else
                {
                    jtmp = jsObj.Value<JArray>(item2.Name);
                    jtmp.Add(item2.Value);
                    jsObj[item2.Name] = jtmp;
                }
            }
        }

        return jsObj;
    }

    /// <summary>
    ///
    /// </summary>
    public static void MergeJsonObject(
        ref JObject jsObjSrc,
        JObject jsObjChild,
        bool isReplace = false,
        bool isReplaceIfNullValue = false
    )
    {
        foreach (var jsKeyVal in jsObjChild)
        {
            if (isReplace)
            {
                if (jsObjSrc.ContainsKey(jsKeyVal.Key))
                {
                    jsObjSrc.Remove(jsKeyVal.Key);
                }

                jsObjSrc.Add(jsKeyVal.Key, jsKeyVal.Value);
            }
            else
            {
                if (!jsObjSrc.ContainsKey(jsKeyVal.Key))
                {
                    jsObjSrc.Add(jsKeyVal.Key, jsKeyVal.Value);
                }
                else
                {
                    if (isReplaceIfNullValue && jsKeyVal.Value != null)
                    {
                        jsObjSrc.Remove(jsKeyVal.Key);
                        jsObjSrc.Add(jsKeyVal.Key, jsKeyVal.Value);
                    }
                }
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public static JObject CheckMessageRequest(JObject js, string lstField)
    {
        JObject jsReturn = new();
        if (!string.IsNullOrEmpty(lstField))
        {
            string[] lst = lstField.ToLower().Split(O9Constants.O9_CONSTANT_VERTICAL_CROSS);
            if (lst != null && lst.Count() > 0)
            {
                foreach (var item in js.Properties())
                {
                    if (lst.Contains(item.Name.ToLower()))
                    {
                        jsReturn.Add(item.Name, item.Value);
                    }
                }

                return jsReturn;
            }
        }

        return js;
    }

    /// <summary>
    ///
    /// </summary>
    public static JObject CheckMessageRequestValue(JObject js, string lstField)
    {
        JObject jsReturn = new();
        if (!string.IsNullOrEmpty(lstField))
        {
            string[] lst = lstField.ToLower().Split(O9Constants.O9_CONSTANT_VERTICAL_CROSS);
            if (lst != null && lst.Count() > 0)
            {
                foreach (var item in js.Properties())
                {
                    if (lst.Contains(item.Name.ToLower()) && item.Value != null)
                    {
                        jsReturn.Add(item.Name, item.Value);
                    }
                }

                return jsReturn;
            }
        }

        return js;
    }

    /// <summary>
    ///
    /// </summary>
    public static EnmResultResponse AnalysisFunctionResult(
        string strResponse,
        out JsonFunction clsJsonFunction
    )
    {
        try
        {
            clsJsonFunction = null;
            JsonResponse clsJsonResponse = GetJsonResponse(strResponse);
            EnmResultResponse enmReturn = EnmResultResponse.NOT_SUCCESS;
            if (clsJsonResponse != null)
            {
                if (clsJsonResponse.IsOK())
                {
                    JsonFunctionMapping jsonFunctionMapping =
                        JsonConvert.DeserializeObject<JsonFunctionMapping>(
                            clsJsonResponse.GetMessage(),
                            JsonSerializerSetting
                        );
                    clsJsonFunction = new JsonFunction(jsonFunctionMapping);
                    enmReturn = EnmResultResponse.SUCCESS;
                }
                else
                {
                    JsonFunctionMapping jsonFunctionMapping =
                        JsonConvert.DeserializeObject<JsonFunctionMapping>(
                            clsJsonResponse.GetMessage(),
                            JsonSerializerSetting
                        );
                    clsJsonFunction = new JsonFunction(jsonFunctionMapping);
                }
            }

            return enmReturn;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.StackTrace);
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    ///
    /// </summary>
    public static JArray GetFieldOfTable()
    {
        JArray jsArray = null;
        string key =
            GlobalVariable.O9_GLOBAL_COMCODE + "." + GlobalVariable.O9_GLOBAL_MEMCACHED_KEY.FoFTab;
        // jsArray = (JArray) O9Caching.GetData(key);
        string strValue = O9Client.memCached.GetValue(key);
        if (!string.IsNullOrEmpty(strValue))
        {
            jsArray = JsonConvert.DeserializeObject<JArray>(strValue);
        }

        return jsArray;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="clsJsonDataById"></param>
    /// <param name="functionId"></param>
    /// <returns></returns>
    public static string GenJsonDataByIdRequest(JsonGetDataById clsJsonDataById, string functionId)
    {
        try
        {
            if (clsJsonDataById == null)
            {
                return "";
            }

            string strResult = "";
            strResult = GenJsonBodyRequest(clsJsonDataById, functionId);
            if (!string.IsNullOrEmpty(strResult))
            {
                if (clsJsonDataById.M)
                {
                    return JsonData.ConvertMappingToOriginal(strResult).ToString();
                }
            }

            return strResult;
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="clsJsonData"></param>
    /// <param name="functionId"></param>
    /// <returns></returns>
    public static string GenJsonDataRequest(object clsJsonData, string functionId)
    {
        try
        {
            if (clsJsonData == null)
            {
                return "";
            }

            string strResult = GenJsonBodyRequest(clsJsonData, functionId);

            if (!string.IsNullOrEmpty(strResult) && strResult.EndsWith(","))
            {
                strResult = strResult.TrimEnd(',');
            }

            return strResult;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="ex"></param>
    public static void ConsoleWriteLine(Exception ex)
    {
        ConsoleWriteLine(ex.ToString());
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="text"></param>
    public static void ConsoleWriteLine(string text)
    {
        if (text == null)
        {
            return;
        }

        Console.OutputEncoding = Encoding.UTF8;
        Debug.WriteLine(text);
        // Console.ReadKey();
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="str"></param>
    /// <param name="isClientCaching"></param>
    /// <returns></returns>
    public static T JSONDeserializeObject<T>(
        string str,
        EnmCacheAction isClientCaching = EnmCacheAction.NoCached
    )
    {
        try
        {
            if (!string.IsNullOrEmpty(str))
            {
                return JsonConvert.DeserializeObject<T>(str, JsonSerializerSetting);
            }
        }
        catch (Exception ex)
        {
            O9Utils.ConsoleWriteLine(ex);
        }

        return default(T);
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="model"></param>
    /// <param name="sFunc"></param>
    /// <returns></returns>
    public static IPagedList<TResponse> SimpleSearchCommon<TResponse>(
        SimpleSearchModel model,
        string sFunc
    )
    {
        model.PageSize = model.PageSize == 0 ? int.MaxValue : model.PageSize;

        var searchFunc = O9Utils.SearchFunc(model, sFunc);
        var strSql = searchFunc.GenSearchCommonSql(
            model.SearchText,
            "",
            EnmOrderTime.InQuery,
            true
        );
        var result = O9Utils.Search(strSql, model.PageIndex);

        result = searchFunc.SearchData(result);
        var response = result.DataListToPagedList<TResponse>(model.PageIndex, model.PageSize);
        return response;
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="model"></param>
    /// <param name="sFunc"></param>
    /// <returns></returns>
    public static IPagedList<TResponse> AdvanceSearchCommon<TRequest, TResponse>(
        TRequest model,
        string sFunc
    )
        where TRequest : SearchBaseModel
    {
        try
        {
            model.PageSize = model.PageSize == 0 ? int.MaxValue : model.PageSize;

            var modelSearch = O9Utils.SearchFunc(model, sFunc);
            var strSql = modelSearch.GenSearchCommonSql(
                O9Constants.O9_CONSTANT_AND,
                EnmOrderTime.InQuery,
                string.Empty,
                true
            );

            var result = O9Utils.Search(strSql, model.PageIndex);
            result = modelSearch.SearchData(result);

            var response = result.DataListToPagedList<TResponse>(model.PageIndex, model.PageSize);
            return response;
        }
        catch (Exception ex)
        {
            Console.WriteLine("error_AdvanceSearchCommon == " + ex.Message);
            return null;
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="searchFunc"></param>
    /// <param name="currentModules"></param>
    /// <param name="condition"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static JToken RuleFunc(
        string searchFunc,
        string currentModules,
        IEnumerable<KeyValuePair<string, object>> condition = null
    )
    {
        var clsJsonSearch = new JsonSearch
        {
            F = searchFunc.ToUpper(),
            M = currentModules.ToUpper(),
            C = "O9",
        };

        var strResult = GenJsonBodyRequest(clsJsonSearch, "UTIL_GEN_SEARCH_ADVANCED", "");
        if (!string.IsNullOrEmpty(strResult))
        {
            var m_searchFunc = JsonConvert.DeserializeObject<SearchFunc>(strResult);
            m_searchFunc.ToJObject();
            var sqlQuery = "";
            if (condition == null)
            {
                sqlQuery = m_searchFunc.GenSearchCommonSql(
                    O9Constants.O9_CONSTANT_AND,
                    EnmOrderTime.InQuery,
                    string.Empty,
                    true
                );
            }
            else
            {
                sqlQuery = m_searchFunc.GenSearchCommonSql(condition, O9Constants.O9_CONSTANT_AND);
            }

            var result = Search(sqlQuery, 0);
            result = m_searchFunc.SearchData(result);
            return result;
        }

        throw new Exception("Error RuleFunc Null Data");
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="query"></param>
    /// <param name="fields"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    private static JObject GetInfoExecute(string query, string fields, dynamic data)
    {
        if (!string.IsNullOrEmpty(query))
        {
            query = GenQuery(query, data);
        }

        ConsoleWriteLine(query);
        JObject jsRequest = new();
        bool isArray = false;
        string proc = "UTIL_GET_INFO";
        try
        {
            jsRequest.Add("Q", query);
            jsRequest.Add("R", fields);
            jsRequest.Add("I", isArray);

            string strResult = GenJsonBodyRequest(jsRequest, proc);

            if (!string.IsNullOrEmpty(strResult))
            {
                JsonFrontOffice fo = AnalysisFrontOfficeResult(strResult);
                return fo.RESULT.ConvertListDateStringToLong();
            }

            return new JObject();
        }
        catch (Exception ex)
        {
            ConsoleWriteLine(ex);
            return null;
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="query"></param>
    /// <param name="fields"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    private static JToken LookupDataExecute(string query, string fields, dynamic data)
    {
        if (!string.IsNullOrEmpty(query))
        {
            query = GenQuery(query, data);
        }

        ConsoleWriteLine(query);

        try
        {
            SearchFunc sf = new();
            sf = GenAdvanceSearch("LKP_DATA", "SYS");
            JObject js = data;
            int page = 0;
            string term = string.Empty;
            string strSql = query;
            if (js != null)
            {
                if (js.HasValues)
                {
                    page = js.GetValues<int>("page");
                    js.Remove("page");
                    if (js.ContainsKey("term"))
                    {
                        term = js.GetValues<string>("term");
                        js.Remove("term");
                    }

                    if (!string.IsNullOrEmpty(term))
                    {
                        string[] columns = GetAttributeFieldNames(query);
                        strSql = MergeQuery(query, columns, term);
                    }
                }
            }

            JToken result = Search(strSql, page);
            if (result.SelectToken("data") == null || !result.SelectToken("data").HasValues)
            {
                result["data"] = new JArray();
            }

            JToken dataSearch = result.SelectToken("data");
            //dataSearch = RenameFields(dataSearch);

            return result;
        }
        catch (Exception ex)
        {
            O9Utils.ConsoleWriteLine(ex);
            return null;
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="query"></param>
    /// <param name="columns"></param>
    /// <param name="condition"></param>
    /// <returns></returns>
    public static string MergeQuery(string query, string[] columns, string condition)
    {
        string currentQuery = string.Empty;
        if (condition.Equals("*"))
        {
            condition = "%";
        }

        string conAdd = string.Empty;
        for (int i = 0; i < columns.Length; i++)
        {
            conAdd +=
                " UPPER("
                + columns[i]
                + ") LIKE '%"
                + condition.ToUpper()
                + "%' "
                + O9Constants.O9_CONSTANT_OR;
            if (i == 1)
            {
                break;
            }
        }

        if (!string.IsNullOrEmpty(conAdd))
        {
            if (conAdd.EndsWith(O9Constants.O9_CONSTANT_OR))
            {
                conAdd = conAdd.Substring(0, conAdd.Length - O9Constants.O9_CONSTANT_OR.Length);
            }

            conAdd = "(" + conAdd.Trim() + ")";
            int wherePosition = query.ToUpper().IndexOf("WHERE");
            if (wherePosition > 0)
            {
                currentQuery =
                    query.Substring(0, wherePosition + 5)
                    + " "
                    + conAdd
                    + " "
                    + O9Constants.O9_CONSTANT_AND
                    + query.Substring(wherePosition + 5, query.Length - wherePosition - 5);
            }
            else
            {
                int orderByPosition = query.ToUpper().IndexOf("ORDER BY");
                if (orderByPosition > 0)
                {
                    currentQuery =
                        query.Substring(0, orderByPosition)
                        + "WHERE "
                        + conAdd
                        + " "
                        + query.Substring(orderByPosition, query.Length - orderByPosition);
                }
                else
                {
                    currentQuery = query + " WHERE " + conAdd;
                }
            }
        }

        return currentQuery;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public static string[] GetAttributeFieldNames(string query)
    {
        string[] columns = new string[] { };
        List<string> list = new();
        query = query.Trim();
        query = Regex.Replace(query.Replace("vbLf", " ").Trim(), " {2,}", " ").ToLower();
        int selectPosition = query.ToLower().IndexOf("select ");
        int fromPosition = query.ToLower().IndexOf("from ");
        string nt;
        string tmp = string.Empty;
        string value = string.Empty;
        if (selectPosition >= 0)
        {
            string column = query.Substring(selectPosition + 7, fromPosition - 7);
            string[] st = column.Split(",");
            foreach (var item in st)
            {
                nt = item.Trim();
                if (!string.IsNullOrEmpty(tmp))
                {
                    tmp = tmp + ",";
                    nt = tmp + nt;
                }

                nt = tmp + nt;
                if (!nt.Contains('(') && !nt.Contains(')'))
                {
                    value = nt;
                }
                else
                {
                    if (nt.Contains(')') && nt.ToLower().Contains(" as "))
                    {
                        value = nt;
                        tmp = string.Empty;
                    }
                    else
                    {
                        tmp = nt;
                    }
                }

                int dot = value.LastIndexOf(".");
                string trimmedFieldName = string.Empty;
                string normalisedFieldName = string.Empty;
                if (dot >= 0)
                {
                    trimmedFieldName = value.Substring(dot + 1);
                }
                else
                {
                    trimmedFieldName = value;
                }

                string strAs = " as ";
                int asClause = trimmedFieldName.ToLower().IndexOf(strAs);
                if (asClause > 0)
                {
                    normalisedFieldName = trimmedFieldName.Substring(asClause + strAs.Length);
                }
                else
                {
                    strAs = " ";
                    asClause = trimmedFieldName.ToLower().IndexOf(strAs);
                    if (asClause > 0)
                    {
                        normalisedFieldName = trimmedFieldName.Substring(asClause + strAs.Length);
                    }
                    else
                    {
                        normalisedFieldName = trimmedFieldName;
                    }
                }

                value = normalisedFieldName;
                list.Add(value);
            }
        }

        columns = list.ToArray();
        return columns;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="searchFunc"></param>
    /// <param name="module"></param>
    /// <returns></returns>
    public static SearchFunc GenAdvanceSearch(string searchFunc, string module)
    {
        try
        {
            JsonSearch clsJsonSearch = new()
            {
                F = searchFunc,
                M = module,
                C = O9Constants.O9_CONSTANT_COMCODE,
            };
            string strResult = GenJsonBodyRequest(clsJsonSearch, "UTIL_GEN_SEARCH_ADVANCED");
            if (string.IsNullOrEmpty(strResult))
            {
                return null;
            }

            JObject js = JObject.Parse(strResult);
            SearchFunc sf = new(); //O9Utils.JSONDeserializeObject<SearchFunc>(js.ToUpperKey().ToString());
            sf.STORV = js.Value<string>("storv");
            sf.DTNAME = js.Value<string>("dtname");
            List<JsonSearchFtag> lstftag = new();
            foreach (var item in js.GetValue("lstsft").Children())
            {
                JsonSearchFtag sftag = new()
                {
                    FTAG = item.Value<string>("ftag"),
                    FTYPE = item.Value<string>("ftype"),
                    INPTYPE = item.Value<string>("inptype"),
                    CAPTION = item.Value<JObject>("caption"),
                    INCDT = item.Value<bool>("incdt"),
                    INSELECT = item.Value<bool>("inselect"),
                    VISIBLE = item.Value<bool>("visible"),
                    ORD = item.Value<int>("ord"),
                };
                lstftag.Add(sftag);
            }

            sf.LSTSFT = lstftag;
            return sf;
        }
        catch (Exception ex)
        {
            O9Utils.ConsoleWriteLine(ex);
            return null;
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="txcode"></param>
    /// <param name="rule_name"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static JToken ExecuteRuleFunc(string txcode, string rule_name, dynamic data)
    {
        // get query info first
        string _rules = TransRuleKey(txcode);
        List<JsonRuleFunc> clsJsonRuleFuncList = O9Utils.JSONDeserializeObject<List<JsonRuleFunc>>(
            _rules
        );
        if (clsJsonRuleFuncList != null)
        {
            foreach (var rf in clsJsonRuleFuncList)
            {
                if (rf.FUNC.Equals(rule_name.ToUpper()))
                {
                    if (rf.RULE.Equals("GET_INFO"))
                    {
                        var result = GetInfoExecute(rf.QUERY, rf.RELF, data);
                        return JToken.FromObject(result);
                    }

                    if (rf.RULE.Equals("LKP_DATA"))
                    {
                        var result = LookupDataExecute(rf.QUERY, rf.RELF, data);
                        return result;
                    }
                }
            }
        }

        return null;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public static string GenPassword()
    {
        var strTemp = string.Empty;
        var random = new Random();
        var pasLength = GlobalVariable.O9_GLOBAL_USER_PARAM?.GENERAL_PASS_LENGTH ?? 6;
        for (var i = 0; i < pasLength; i++)
        {
            string myValue = (9 * random.Next(i)).ToString();
            strTemp += myValue;
        }

        return strTemp;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="userSessions"></param>
    /// <param name="txcode"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static JsonFrontOffice PostFrontOffice(
        UserSessions userSessions,
        string txcode,
        dynamic data
    )
    {
        try
        {
            JObject jsRequest = data;
            string strResult = GenJsonFrontOfficeRequest(userSessions, txcode.ToUpper(), jsRequest);

            return AnalysisFrontOfficeResult(strResult);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="lstTableName"></param>
    /// <returns></returns>
    public static async Task<JObject> GetDataDefaultOfField(List<object> lstTableName)
    {
        await Task.CompletedTask;
        try
        {
            JObject jsReturn = null;
            string strResult = string.Empty;
            JObject jsRequest = new() { { "T", JToken.FromObject(lstTableName) } };

            strResult = GenJsonBodyRequest(jsRequest, "ADM_GET_DATA_DEFAULT");

            if (!string.IsNullOrEmpty(strResult))
            {
                jsReturn = JObject.Parse(strResult);
            }

            return jsReturn;
        }
        catch (Exception ex)
        {
            ConsoleWriteLine(ex);
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="jsRequest"></param>
    /// <param name="jsDefault"></param>
    /// <returns></returns>
    public static JObject MergeRequestAndDefault(JObject jsRequest, JObject jsDefault)
    {
        if (jsDefault != null && jsRequest != null)
        {
            jsRequest = jsRequest.ConvertToJObject();
            jsDefault = jsDefault.ConvertToJObject();

            foreach (var item in jsDefault)
            {
                if (!jsRequest.ContainsKey(item.Key))
                {
                    jsRequest.Add(item.Key, item.Value);
                }
            }
        }

        return jsRequest;
    }

    /// <summary>
    /// Aeses the encrypt using the specified text to encrypt
    /// </summary>
    /// <param name="textToEncrypt">The text to encrypt</param>
    /// <returns>The string</returns>
}

/// <summary>
///
/// </summary>
public static class JObjectExtensions
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="jObject"></param>
    /// <param name="list"></param>
    /// <returns></returns>
    public static JObject ConvertListDateStringToLong(
        this JObject jObject,
        List<string> list = null
    )
    {
        try
        {
            DateTime dt = new();
            if (list != null)
            {
                list = list.ConvertAll(d => d.ToUpper().Trim());
                foreach (var item in jObject)
                {
                    if (item.Value.Type == JTokenType.Date)
                    {
                        jObject[item.Key] = O9Utils.ConvertDateStringToLong(
                            jObject
                                .Value<DateTime>(item.Key)
                                .ToString(
                                    O9Constants.O9_CONSTANT_SHORT_DATE_FORMAT
                                        + " "
                                        + O9Constants.O9_CONSTANT_TIME_FORMAT
                                )
                        );
                    }

                    if (item.Value.Type == JTokenType.Array)
                    {
                        JArray vl = new();
                        if (item.Value.Count() > 0)
                        {
                            foreach (var item2 in item.Value.Children())
                            {
                                if (item2.Type == JTokenType.Object)
                                {
                                    JObject js = JObject.FromObject(item2);
                                    js.ConvertListDateStringToLong(list);
                                    vl.Add(js);
                                }
                                else
                                {
                                    vl.Add(item2);
                                }
                            }

                            jObject[item.Key] = vl;
                        }
                    }

                    if (item.Value.Type == JTokenType.Object)
                    {
                        JObject js = JObject.FromObject(item.Value);
                        js.ConvertListDateStringToLong();
                        jObject[item.Key] = js;
                    }

                    if (item.Value.Type == JTokenType.String)
                    {
                        if (list.Contains(item.Key.ToUpper().Trim()))
                        {
                            if (O9Utils.CheckValueIsDateExtension(item.Value, ref dt))
                            {
                                jObject[item.Key] = O9Utils.ConvertDateStringToLong(
                                    dt.ToString(
                                        O9Constants.O9_CONSTANT_SHORT_DATE_FORMAT
                                            + " "
                                            + O9Constants.O9_CONSTANT_TIME_FORMAT
                                    )
                                );
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (var item in jObject)
                {
                    if (item.Value.Type == JTokenType.Date)
                    {
                        jObject[item.Key] = O9Utils.ConvertDateStringToLong(
                            jObject
                                .Value<DateTime>(item.Key)
                                .ToString(
                                    O9Constants.O9_CONSTANT_SHORT_DATE_FORMAT
                                        + " "
                                        + O9Constants.O9_CONSTANT_TIME_FORMAT
                                )
                        );
                    }

                    if (item.Value.Type == JTokenType.Array)
                    {
                        JArray vl = new();
                        if (item.Value.Count() > 0)
                        {
                            foreach (var item2 in item.Value.Children())
                            {
                                if (item2.Type == JTokenType.Object)
                                {
                                    JObject js = JObject.FromObject(item2);
                                    js.ConvertListDateStringToLong();
                                    vl.Add(js);
                                }
                                else
                                {
                                    vl.Add(item2);
                                }
                            }

                            jObject[item.Key] = vl;
                        }
                    }

                    if (item.Value.Type == JTokenType.Object)
                    {
                        JObject js = JObject.FromObject(item.Value);
                        js.ConvertListDateStringToLong();
                        jObject[item.Key] = js;
                    }

                    if (item.Value.Type == JTokenType.String)
                    {
                        if (O9Utils.CheckValueIsDateExtension(item.Value, ref dt))
                        {
                            jObject[item.Key] = O9Utils.ConvertDateStringToLong(
                                dt.ToString(
                                    O9Constants.O9_CONSTANT_SHORT_DATE_FORMAT
                                        + " "
                                        + O9Constants.O9_CONSTANT_TIME_FORMAT
                                )
                            );
                        }
                    }
                }
            }

            return jObject;
        }
        catch (Exception)
        {
            return jObject;
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="jsObject"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static T GetValues<T>(this JObject jsObject, string key)
    {
        jsObject = jsObject.ToLowerKey();
        if (key != null)
        {
            if (jsObject.ContainsKey(key.ToLower()))
            {
                return jsObject.Value<T>(key.ToLower());
            }
        }

        return default(T);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="jsObject"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public static void AddOrReplace(this JObject jsObject, string key, JObject value)
    {
        if (jsObject.JsonContains(key))
        {
            jsObject.Remove(key);
        }

        jsObject.Add(key, value);
    }

    /// <summary>
    /// Check value has Json object or not
    /// </summary>
    public static bool JsonContains(this JObject jsObject, string key)
    {
        object jsItem = null;

        if (!string.IsNullOrEmpty(key))
        {
            jsItem = jsObject.SelectToken(key);
        }

        if (jsItem != null)
        {
            return true;
        }

        return false;
    }
}

/// <summary>
///
/// </summary>
public static class UrlUtils
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static (string url, string ip, string port) ExtractUrl(string url)
    {
        if (!string.IsNullOrEmpty(url))
        {
            string adjustedUrl = url.Replace("*", "localhost");
            string pattern = @"https?://([\d.]+):(\d+)";
            var match = Regex.Match(adjustedUrl, pattern);

            if (match.Success)
            {
                string ip = match.Groups[1].Value;
                string port = match.Groups[2].Value;
                return (adjustedUrl, ip, port);
            }
        }

        return ("https://localhost:6066", "localhost", "6066");
    }
}

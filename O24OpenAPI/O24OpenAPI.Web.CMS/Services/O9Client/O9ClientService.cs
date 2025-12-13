using Apache.NMS;
using Jits.Neptune.Web.CMS.LogicOptimal9.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Web.CMS.Models;
using O24OpenAPI.Web.CMS.Models.O9;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using O9Constants = Jits.Neptune.Web.CMS.LogicOptimal9.Common.O9Constants;

namespace O24OpenAPI.Web.CMS.Services.Services;

public class O9ClientService : IO9ClientService
{
    public async Task<bool> LoginO9()
    {
        var setting = GlobalVariable.Optimal9Settings;
        JsonLogin loginRequest = new()
        {
            L = setting.O9UserName,
            P = !setting.O9Encrypt ? setting.O9Password : O9Encrypt.MD5Encrypt(setting.O9Password),
            A = false,
        };

        var strResult = await GenJsonBodyRequestAsync(
            objJsonBody: loginRequest,
            functionId: GlobalVariable.UMG_LOGIN,
            "",
            isResultCaching: EnmCacheAction.NoCached,
            EnmSendTypeOption.Synchronize,
            usrId: "-1",
            priority: MsgPriority.Normal
        );
        if (O9Utils.isErrorResponse(strResult))
        {
            return false;
        }

        var clsJsonLoginResponse = JsonConvert.DeserializeObject<JsonLoginResponse>(strResult);
        if (clsJsonLoginResponse != null)
        {
            if (string.IsNullOrEmpty(clsJsonLoginResponse.E))
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
    /// <returns></returns>
    public async Task<string> GenJsonBodyRequestAsync(
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

            string strRequest = string.Empty;
            string strResult = string.Empty;
            if (objJsonBody != null)
            {
                strRequest = JsonConvert.SerializeObject(objJsonBody);
            }

            var o9Client = new O9Client();
            strResult = await o9Client.SendStringAsync(
                strRequest,
                functionId,
                usrId,
                sessionid,
                EnmCacheAction.NoCached,
                EnmSendTypeOption.Synchronize,
                MsgPriority.Normal
            );
            o9Client = null;
            return strResult;
        }
        catch (Exception ex)
        {
            Console.WriteLine("error_GenJsonBodyRequest == " + ex.StackTrace);
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    ///
    /// </summary>
    public async Task<string> GenJsonFrontOfficeRequestAsync(
        FrontOfficeModel model,
        bool isFrontOfficeMapping = true
    )
    {
        try
        {
            var clsJsonFrontOffice = model.ToJsonFrontOffice();

            if (model.HasJsonFrontOfficeMapping)
            {
                return await GenJsonBodyRequestAsync(
                    isFrontOfficeMapping == true
                        ? new JsonFrontOfficeMapping(clsJsonFrontOffice)
                        : clsJsonFrontOffice,
                    model.FunctionId,
                    model.SessionId,
                    EnmCacheAction.NoCached,
                    EnmSendTypeOption.Synchronize,
                    model.UserId.ToString()
                );
            }

            return await GenJsonBodyRequestAsync(
                clsJsonFrontOffice,
                model.FunctionId,
                model.SessionId,
                EnmCacheAction.NoCached,
                EnmSendTypeOption.Synchronize,
                model.UserId.ToString()
            );
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    ///
    /// </summary>
    public async Task<string> GenJsonBackOfficeRequestAsync(BackOfficeModel model)
    {
        try
        {
            JsonBackOffice clsJsonBackOffice = model.ToJsonBackOffice();

            string result = await GenJsonBodyRequestAsync(
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
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public async Task<string> GenJsonBackOfficeRequest(
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
            var result = await GenJsonBodyRequestAsync(
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
            throw new Exception(ex.Message);
        }
    }

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
    public async Task<JToken> ExecuteRuleFuncAsync(string txcode, string rule_name, dynamic data)
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
                        var result = await GetInfoExecuteAsync(rf.QUERY, rf.RELF, data);
                        return JToken.FromObject(result);
                    }

                    if (rf.RULE.Equals("LKP_DATA"))
                    {
                        var result = await LookupDataExecuteAsync(rf.QUERY, rf.RELF, data);
                        return result;
                    }
                }
            }
        }

        return null;
    }

    private async Task<JObject> GetInfoExecuteAsync(string query, string fields, dynamic data)
    {
        if (!string.IsNullOrEmpty(query))
        {
            var jsData = JObject.FromObject(data);
            query = O9Utils.GenQuery(query, jsData);
        }
        O9Utils.ConsoleWriteLine(query);
        JObject jsRequest = new();
        bool isArray = false;
        string proc = "UTIL_GET_INFO";
        try
        {
            jsRequest.Add("Q", query);
            jsRequest.Add("R", fields);
            jsRequest.Add("I", isArray);

            string strResult = await GenJsonBodyRequestAsync(jsRequest, proc);

            if (!string.IsNullOrEmpty(strResult))
            {
                JsonFrontOffice fo = O9Utils.AnalysisFrontOfficeResult(strResult);
                return fo.RESULT.ConvertListDateStringToLong();
            }

            return new JObject();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    private async Task<JToken> LookupDataExecuteAsync(string query, string fields, dynamic data)
    {
        try
        {
            if (!string.IsNullOrEmpty(query))
            {
                query = O9Utils.GenQuery(query, data);
            }

            SearchFunc sf = new();
            sf = await GenAdvanceSearchAsync("LKP_DATA", "SYS");
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
                        string[] columns = O9Utils.GetAttributeFieldNames(query);
                        strSql = O9Utils.MergeQuery(query, columns, term);
                    }
                }
            }

            JToken result = await SearchAsync(strSql, page);
            if (result.SelectToken("data") == null || !result.SelectToken("data").HasValues)
            {
                result["data"] = new JArray();
            }

            JToken dataSearch = result.SelectToken("data");

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
    /// <returns></returns>
    public async Task<SearchFunc> GenAdvanceSearchAsync(string searchFunc, string module)
    {
        try
        {
            JsonSearch clsJsonSearch = new()
            {
                F = searchFunc,
                M = module,
                C = O9Constants.O9_CONSTANT_COMCODE,
            };
            string strResult = await GenJsonBodyRequestAsync(
                clsJsonSearch,
                "UTIL_GEN_SEARCH_ADVANCED"
            );
            if (string.IsNullOrEmpty(strResult))
            {
                return null;
            }

            JObject js = JObject.Parse(strResult);
            SearchFunc sf = new()
            {
                STORV = js.Value<string>("storv"),
                DTNAME = js.Value<string>("dtname"),
            };
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
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    ///
    /// </summary>
    public async Task<JToken> SearchAsync(
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

            string strResult = await GenJsonBodyRequestAsync(jsCondition, proc, "", enmCacheAction);
            if (!string.IsNullOrEmpty(strResult))
            {
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

                    JObject jPaging = O9Utils.CreatePaging(pg, page);
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
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="clsJsonData"></param>
    /// <param name="functionId"></param>
    /// <returns></returns>
    public async Task<string> GenJsonDataRequestAsync(object clsJsonData, string functionId)
    {
        try
        {
            if (clsJsonData == null)
            {
                return "";
            }

            string strResult = await GenJsonBodyRequestAsync(clsJsonData, functionId);

            if (!string.IsNullOrEmpty(strResult) && strResult.EndsWith(","))
            {
                strResult = strResult.TrimEnd(',');
            }

            return strResult;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    ///
    /// </summary>
    public async Task<string> GenJsonFunctionRequestAsync(
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
            if (string.IsNullOrEmpty(functionId))
            {
                functionId = GlobalVariable.UTIL_CALL_FUNC;
            }

            JsonFunction clsJsonFunction = new()
            {
                TXCODE = ptxcode,
                TXPROC = ptxproc,
                TXBODY = ptxbody,
            };
            return await GenJsonBodyRequestAsync(
                new JsonFunctionMapping(clsJsonFunction),
                functionId,
                user.Ssesionid,
                isCaching,
                EnmSendTypeOption.Synchronize,
                user.Usrid.ToString()
            );
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    ///
    /// </summary>
    public async Task<JToken> RuleFuncAsync(
        string searchFunc,
        string currentModules,
        IEnumerable<KeyValuePair<string, object>> condition = null
    )
    {
        try
        {
            var clsJsonSearch = new JsonSearch
            {
                F = searchFunc.ToUpper(),
                M = currentModules.ToUpper(),
                C = "O9",
            };

            var strResult = await GenJsonBodyRequestAsync(
                clsJsonSearch,
                "UTIL_GEN_SEARCH_ADVANCED",
                ""
            );
            if (string.IsNullOrEmpty(strResult))
            {
                throw new Exception("Error RuleFunc Null Data");
            }

            var m_searchFunc = JsonConvert.DeserializeObject<SearchFunc>(strResult);
            m_searchFunc.ToJObject();
            string sqlQuery;
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

            var result = await SearchAsync(sqlQuery, 0);
            result = m_searchFunc.SearchData(result);
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
    public async Task<string> GenJsonDataByIdRequest(
        JsonGetDataById clsJsonDataById,
        string functionId
    )
    {
        try
        {
            if (clsJsonDataById == null)
            {
                return "";
            }

            string strResult = "";
            strResult = await GenJsonBodyRequestAsync(clsJsonDataById, functionId);
            if (!string.IsNullOrEmpty(strResult))
            {
                if (clsJsonDataById.M)
                {
                    return JsonData.ConvertMappingToOriginal(strResult).ToString();
                }
            }

            return strResult;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    ///
    /// </summary>
    public async Task<JObject> GetDataDefaultOfField(List<object> lstTableName)
    {
        try
        {
            JObject jsReturn = null;
            string strResult = string.Empty;
            JObject jsRequest = new() { { "T", JToken.FromObject(lstTableName) } };

            strResult = await GenJsonBodyRequestAsync(jsRequest, "ADM_GET_DATA_DEFAULT");

            if (!string.IsNullOrEmpty(strResult))
            {
                jsReturn = JObject.Parse(strResult);
            }

            return jsReturn;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    ///
    /// </summary>
    public async Task<IPagedList<TResponse>> AdvanceSearchCommon<TRequest, TResponse>(
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

            var result = await SearchAsync(strSql, model.PageIndex);
            result = modelSearch.SearchData(result);

            var response = result.DataListToPagedList<TResponse>(model.PageIndex, model.PageSize);
            return response;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    ///
    /// </summary>
    public async Task<string> GenJsonFrontOfficeRequest(
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
            if (string.IsNullOrEmpty(functionId))
            {
                functionId = GlobalVariable.UTIL_CALL_PROC;
            }

            if (string.IsNullOrEmpty(pprn))
            {
                pprn = "";
            }

            JsonFrontOffice clsJsonFrontOffice = new()
            {
                TXCODE = ptxcode,
                TXDT = user.Txdt.ToString("dd/MM/yyyy"),
                BRANCHID = user.UsrBranchid,
                USRID = user.Usrid,
                LANG = user.Lang,
                STATUS = pstatus,
                TXREFID = ptxrefid,
                VALUEDT = pvaluedt,
                USRWS = pusrws,
                APUSER = papuser,
                APUSRIP = papusrip,
                APUSRWS = papusrws,
                APDT = papdt,
                ISREVERSE = pisreverse,
                HBRANCHID = phbranchid,
                RBRANCHID = prbranchid,
                TXBODY = ptxbody,
                APREASON = papreason,
                IFCFEE = pifcfee,
                PRN = pprn,
            };
            if (string.IsNullOrEmpty(pid))
            {
                pid = Guid.NewGuid().ToString();
            }

            clsJsonFrontOffice.ID = pid;

            if (isMapping)
            {
                var frontOfficeMapping = new JsonFrontOfficeMapping(clsJsonFrontOffice);
                var result = await GenJsonBodyRequestAsync(
                    frontOfficeMapping,
                    functionId,
                    user.Ssesionid,
                    EnmCacheAction.NoCached,
                    EnmSendTypeOption.Synchronize,
                    user.Usrid.ToString()
                );
                return result;
            }

            return await GenJsonBodyRequestAsync(
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
            throw new Exception(ex.Message);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public async Task<string> GenJsonFrontOfficeRequest(
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

            return await GenJsonBodyRequestAsync(
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
            throw new Exception(ex.Message);
        }
    }
}

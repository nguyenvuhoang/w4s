using Jits.Neptune.Web.CMS.LogicOptimal9.Common;
using Newtonsoft.Json.Linq;
using O24OpenAPI.Web.CMS.Models.O9;

namespace O24OpenAPI.Web.CMS.Utils;

public class SearchFunc
{
    /// <summary>
    ///
    /// </summary>
    public string STORV { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string HSQL { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string DTNAME { get; set; }

    /// <summary>
    ///
    /// </summary>
    public List<JsonSearchFtag> LSTSFT { get; set; }

    /// <summary>
    ///
    /// </summary>
    public SearchFunc() { }

    /// <summary>
    ///
    /// </summary>
    public SearchFunc(BackOfficeSetting mc)
    {
        this.STORV = mc.ViewName;
        this.DTNAME = mc.TableName;
        this.LSTSFT = mc.Fields;
    }

    /// <summary>
    ///
    /// </summary>
    public string SelectFields
    {
        get
        {
            if (this.LSTSFT == null)
            {
                return "";
            }

            string fields = "";
            foreach (var sf in this.LSTSFT)
            {
                if (sf.INSELECT)
                {
                    fields += sf.FSELECT;
                    fields += O9Constants.O9_CONSTANT_COMMA;
                }
            }

            return fields;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public List<JsonSearchFtag> SearchFtagAnd
    {
        get
        {
            var response = new List<JsonSearchFtag>();

            try
            {
                var query = (
                    from d in this.LSTSFT
                    where
                        (
                            string.IsNullOrEmpty(d.ORAND)
                            || d.ORAND.Equals(O9Constants.O9_CONSTANT_AND)
                        ) && d.INCDT
                    select d
                ).ToList();
                return query;
            }
            catch (Exception e)
            {
                Console.WriteLine("SearchFtagAnd_error == " + e.Message);
                return new List<JsonSearchFtag> { };
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public List<JsonSearchFtag> SearchFtagOr
    {
        get
        {
            try
            {
                var query = (
                    from d in this.LSTSFT
                    where
                        (
                            string.IsNullOrEmpty(d.ORAND)
                            || d.ORAND.Equals(O9Constants.O9_CONSTANT_OR)
                        ) && d.INCDT
                    select d
                ).ToList();
                return query;
            }
            catch (Exception e)
            {
                Console.WriteLine("SearchFtagOr_error == " + e.Message);
                return new List<JsonSearchFtag> { };
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public List<JsonSearchFtag> ConditionFields
    {
        get
        {
            //sfc.LSTSFT.Where(x => x.INCDT && ((isSetConditionVisibleField ? true : x.VISIBLE) || !String.IsNullOrEmpty(x.FDEFAULT))).ToList()
            return this.LSTSFT.Where(x => x.INCDT).ToList();
        }
    }

    /// <summary>
    ///
    /// </summary>
    // Get value of ftag
    public object GetValueByFtag(string strTag)
    {
        int i = GetIndexByFtag(strTag);
        if (i > -1)
        {
            return LSTSFT[i].FVAL;
        }

        return null;
    }

    /// <summary>
    ///
    /// </summary>
    // Set value for ftag
    public void SetValueOfFtag(string ftagName, object value)
    {
        var a = LSTSFT.Where(x => x.FTAG.ToUpper().Equals(ftagName.ToUpper())).ToList();
        foreach (var sft in LSTSFT.Where(x => x.FTAG.ToUpper().Equals(ftagName.ToUpper())).ToList())
        {
            sft.FVAL = value;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public void SetOPRTVALOfFtag(string ftagName, string value)
    {
        foreach (var sft in LSTSFT.Where(x => x.FTAG.ToUpper().Equals(ftagName.ToUpper())).ToList())
        {
            sft.OPRTVAL = value;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public void SetOPRTOfFtag(string ftagName, string value)
    {
        foreach (var sft in LSTSFT.Where(x => x.FTAG.ToUpper().Equals(ftagName.ToUpper())).ToList())
        {
            sft.OPRT = value;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public void SetINCDTFtag(string ftagName)
    {
        foreach (var sft in LSTSFT.Where(x => x.FTAG.ToUpper().Equals(ftagName.ToUpper())).ToList())
        {
            sft.INCDT = true;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public void SetValueOfFtag(string ftagName, object value, string OrAnd)
    {
        foreach (var sft in LSTSFT.Where(x => x.FTAG.ToUpper().Equals(ftagName.ToUpper())).ToList())
        {
            sft.FVAL = value;
            sft.ORAND = OrAnd;
        }
    }

    /// <summary>
    ///
    /// </summary>
    // Get index by ftag
    public int GetIndexByFtag(string strTag, bool inSelect = false)
    {
        int j = 0;
        int i = 0;

        foreach (var f in LSTSFT)
        {
            if (f.FTAG.ToUpper().Equals(strTag.ToUpper()))
            {
                if (inSelect)
                {
                    if (f.INSELECT)
                    {
                        return j;
                    }
                }
                else
                {
                    return i;
                }
            }

            i++;
            j++;
        }

        return -1;
    }

    /// <summary>
    /// Description: Clear array fval
    /// </summary>
    public void ClearArrayFval()
    {
        foreach (var sft in this.LSTSFT)
        {
            if (sft.ARRFVAL != null)
            {
                sft.ARRFVAL.Clear();
            }
        }
    }

    /// <summary>
    /// Description: Clear array fval
    /// </summary>
    public void ClearAllFval()
    {
        foreach (var sft in LSTSFT.Where(x => x.FVAL != null).ToList())
        {
            sft.FVAL = null;
        }
    }

    /// <summary>
    /// Description: SearchData
    /// </summary>
    public JToken SearchData(JToken jtResult)
    {
        string dataKey = "data";
        if (jtResult == null)
        {
            var js = new JObject { { dataKey, new JArray() } };
            return js;
        }

        JToken data = jtResult.SelectToken(dataKey);
        if (data == null || data.Type == JTokenType.Null)
        {
            jtResult[dataKey] = new JArray();
        }
        else
        {
            RenameFields(data);
        }

        return jtResult;
    }

    /// <summary>
    /// Description: BindataSearchInselectVisible
    /// </summary>
    public JToken BindataSearchInselectVisible(JToken jtResult)
    {
        if (jtResult == null)
        {
            var js = new JObject { { "data", new JArray() } };
            return js;
        }

        JToken data = jtResult.SelectToken("data");
        data = RenameFieldsInselectVisible(data);
        return jtResult;
    }

    /// <summary>
    /// Description: BindataSearchVisible
    /// </summary>
    public JToken BindataSearchVisible(JToken jtResult)
    {
        if (jtResult == null)
        {
            var js = new JObject { { "data", new JArray() } };
            return js;
        }

        JToken data = jtResult.SelectToken("data");
        data = RenameFieldsVisible(data);
        return jtResult;
    }

    /// <summary>
    /// Description: GenSearchCommonSql
    /// </summary>
    public string GenSearchCommonSql(
        object condition,
        string strOrAnd = "",
        EnmOrderTime orderTime = EnmOrderTime.InQuery,
        bool isSetConditionVisibleField = true,
        bool hasDistinct = false
    )
    {
        try
        {
            var cdt = "";
            IEnumerable<KeyValuePair<string, object>> conditionJObject = null;
            if (condition != null)
            {
                cdt = condition.ToString().TrimEnd();
                conditionJObject =
                    condition.GetType().Name == "WhereEnumerableIterator`1"
                        ? (IEnumerable<KeyValuePair<string, object>>)condition
                        : null;
            }

            string cdt1 = "";
            if (condition != null && !string.IsNullOrEmpty(cdt))
            {
                cdt = cdt.Replace("'", "''");
                cdt = Utility.Injection(cdt);
                cdt1 = cdt;
                if (this.LSTSFT != null)
                {
                    foreach (var sf in this.ConditionFields)
                    {
                        cdt = cdt1;
                        if (
                            !string.IsNullOrEmpty(sf.FORMAT)
                            || sf.FTAG.Equals("ACNO")
                            || sf.FTAG.Equals("MBACNO")
                            || sf.FTAG.Equals("LBACNO")
                            || "CD".Equals(sf.FTAG.Substring(sf.FTAG.Length - 2))
                        )
                        {
                            if (conditionJObject != null)
                            {
                                var value = conditionJObject.FirstOrDefault(e =>
                                    e.Key.ToUpper() == sf.FTAG
                                );
                                if (value.Value != null)
                                {
                                    sf.FVAL = value.Value.ToString();
                                }
                            }
                            else
                            {
                                cdt = cdt.Replace("-", "").Trim();
                            }
                        }

                        if (!string.IsNullOrEmpty(sf.FDEFAULT))
                        {
                            sf.FVAL = GetSearchFDefault(sf.FDEFAULT);
                            if (sf.FVAL == null)
                            {
                                sf.FVAL = cdt;
                            }
                        }
                        else
                        {
                            if (sf.FVAL == null && conditionJObject == null)
                            {
                                sf.FVAL = cdt;
                            }

                            if (sf.FVAL == null && conditionJObject != null)
                            {
                                var value = conditionJObject.FirstOrDefault(e =>
                                    e.Key.ToUpper() == sf.FTAG
                                );
                                if (value.Value != null)
                                {
                                    sf.FVAL = value.Value.ToString();
                                }
                            }
                        }
                    }
                }
            }

            return GenSearchCommonSql(strOrAnd, orderTime, "", hasDistinct);
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.StackTrace);
            throw new O24OpenAPIException("Error when generate search common sql");
        }
    }

    // /// <summary>
    // /// Description: GenSearchCommonSql
    // /// </summary>
    // public string GenSearchCommonSql(JObject condition, string strOrAnd = "", EnmOrderTime orderTime = EnmOrderTime.InQuery, bool isSetConditionVisibleField = true, bool hasDistinct = false) {
    //     try {
    //         foreach (var jt in condition)
    //         {
    //             foreach (var sf in this.LSTSFT.Where(x => x.INCDT && (x.FTAG.Equals(jt.Key.ToUpper()) || !string.IsNullOrEmpty(x.FDEFAULT))).ToList())
    //             {
    //                 sf.FVAL = jt.Value.ToString();
    //                 if (!String.IsNullOrEmpty(sf.FDEFAULT))
    //                 {
    //                     sf.FVAL = GetSearchFDefault(sf.FDEFAULT);
    //                 }
    //             }
    //         }
    //         return GenSearchCommonSql(strOrAnd, orderTime, "", hasDistinct);
    //     } catch (Exception ex)
    //     {
    //         System.Console.WriteLine(ex.StackTrace);
    //         throw new O24OpenAPIException("Error when generate search common sql");
    //     }
    // }

    /// <summary>
    /// Description: GetSearchFDefault
    /// </summary>
    private static object GetSearchFDefault(string fdefault)
    {
        if (fdefault == O9Constants.O9_CONSTANT_PARAM_KEY + O9Constants.O9_CONSTANT_CRRBCY)
        {
            return GlobalVariable.O9_GLOBAL_HEADOFFICE_PARAM.GENERAL_CRRBCY;
        }
        else if (fdefault == O9Constants.O9_CONSTANT_PARAM_KEY + O9Constants.O9_CONSTANT_CRRBRCD)
        {
            //return O9Config.O9_GLOBAL_BRANCHCD;
            return "";
        }
        else if (fdefault == O9Constants.O9_CONSTANT_PARAM_KEY + O9Constants.O9_CONSTANT_LANG)
        {
            return O9Constants.O9_CONSTANT_LANG;
        }
        else if (fdefault == O9Constants.O9_CONSTANT_PARAM_KEY + O9Constants.O9_CONSTANT_COMCODE)
        {
            return O9Constants.O9_CONSTANT_COMCODE;
        }
        else if (fdefault == O9Constants.O9_CONSTANT_PARAM_KEY + O9Constants.O9_CONSTANT_O9BUSDATE)
        {
            //return O9Config.O9_GLOBAL_TXDT;
            return "";
        }
        else
        {
            return fdefault;
        }
    }

    /// <summary>
    /// Description: MergeSqlString
    /// </summary>
    public string MergeSqlString(
        string strSelect,
        string strWhere,
        string strOrder,
        bool hasDistinct = false
    )
    {
        try
        {
            strSelect = strSelect.Trim();
            strWhere = strWhere.Trim();
            strOrder = strOrder.Trim();

            if (strSelect.EndsWith(O9Constants.O9_CONSTANT_COMMA))
            {
                strSelect = strSelect.Substring(
                    0,
                    strSelect.Length - O9Constants.O9_CONSTANT_COMMA.Length
                );
            }

            if (strWhere.EndsWith(O9Constants.O9_CONSTANT_OR))
            {
                strWhere = strWhere.Substring(0, strWhere.Length - 2);
            }

            if (strWhere.EndsWith(O9Constants.O9_CONSTANT_AND))
            {
                strWhere = strWhere.Substring(0, strWhere.Length - 3);
            }

            if (strOrder.EndsWith(O9Constants.O9_CONSTANT_COMMA))
            {
                strOrder = strOrder.Substring(
                    0,
                    strOrder.Length - O9Constants.O9_CONSTANT_COMMA.Length
                );
            }

            strSelect =
                "SELECT "
                + (hasDistinct ? "DISTINCT " : "")
                + strSelect
                + " FROM "
                + this.STORV
                + " A";
            strWhere = string.IsNullOrEmpty(strWhere) ? string.Empty : " WHERE " + strWhere;
            strOrder = string.IsNullOrEmpty(strOrder) ? string.Empty : " ORDER BY " + strOrder;

            return strSelect.TrimEnd() + strWhere.TrimEnd() + strOrder.TrimEnd();
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.StackTrace);
            throw new Exception("MergeSqlString");
        }
    }

    /// <summary>
    /// Description: GenSearchCommonSql
    /// </summary>
    public string GenSearchCommonSql(
        string strOrAnd = "",
        EnmOrderTime orderTime = EnmOrderTime.InQuery,
        string strWhere = "",
        bool hasDistinct = false
    )
    {
        try
        {
            string strSelect = this.SelectFields;
            string strWhereAnd = "";
            string strWhereOtherOr = "";
            string strWhereOtherAnd = "";
            string strWhereOr = "";
            string strOrder = "";

            List<JsonSearchFtag> lstSearchFtagAnd = this.SearchFtagAnd;
            List<JsonSearchFtag> lstSearchFtagOr = this.SearchFtagOr;

            foreach (var sf in this.LSTSFT)
            {
                sf.AddOrAndToSearchFtag(strOrAnd);
                // Gen order by
                if (orderTime == EnmOrderTime.InQuery)
                {
                    strOrder = sf.GenOrderString(strOrder);
                }
            }

            if (string.IsNullOrEmpty(strWhere))
            {
                lstSearchFtagAnd = this
                    .LSTSFT.Where(x => x.INCDT && x.ORAND.Equals(O9Constants.O9_CONSTANT_AND))
                    .ToList();
                lstSearchFtagOr = this
                    .LSTSFT.Where(x => x.INCDT && !x.ORAND.Equals(O9Constants.O9_CONSTANT_AND))
                    .ToList();
                // Gen where string with AND condition
                strWhereAnd = GenWhereString(lstSearchFtagAnd, strWhereAnd, strOrAnd);
                strWhereAnd = ProcessSqlWhereString(strWhereAnd, O9Constants.O9_CONSTANT_AND);

                // Gen where string and with has fdefault
                strWhereOtherAnd = GenWhereString(
                    lstSearchFtagAnd,
                    strWhereOtherAnd,
                    strOrAnd,
                    true
                );
                strWhereOtherAnd = ProcessSqlWhereString(
                    strWhereOtherAnd,
                    O9Constants.O9_CONSTANT_AND
                );

                // Gen where string or with has fdefault
                strWhereOtherOr = GenWhereString(lstSearchFtagOr, strWhereOtherOr, strOrAnd, true);
                strWhereOtherOr = ProcessSqlWhereString(
                    strWhereOtherOr,
                    O9Constants.O9_CONSTANT_AND
                );

                // Gen where string with OR condition
                strWhereOr = GenWhereString(lstSearchFtagOr, strWhereOr, strOrAnd);
                strWhereOr = ProcessSqlWhereString(strWhereOr, O9Constants.O9_CONSTANT_OR);

                // Merge where string
                strWhere = strWhereAnd + strWhereOtherAnd + strWhereOtherOr + strWhereOr;
            }

            //             CustomizeSearchCommand(sfc, strSelect, strWhere, strOrder, strOrAnd)    'Added by ChiNM
            return MergeSqlString(strSelect, strWhere, strOrder, hasDistinct);
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.StackTrace);
            throw new Exception("GenSearchCommonSql");
        }
    }

    /// <summary>
    /// Description: ProcessSqlWhereString
    /// </summary>
    private static string ProcessSqlWhereString(string strWhere, string strOrAnd)
    {
        strWhere = strWhere.Trim();
        if (!string.IsNullOrEmpty(strWhere))
        {
            if (
                strWhere.EndsWith(O9Constants.O9_CONSTANT_AND)
                || strWhere.EndsWith(O9Constants.O9_CONSTANT_OR)
            )
            {
                strWhere = strWhere.Substring(0, strWhere.Length - strOrAnd.Length);
            }

            return "(" + strWhere.Trim() + ") " + strOrAnd + " ";
        }

        return string.Empty;
    }

    /// <summary>
    /// Description: GenWhereString
    /// </summary>
    private static string GenWhereString(
        List<JsonSearchFtag> lstSearchFtag,
        string strWhere,
        string strOrAnd,
        bool hasFDefault = false
    )
    {
        if (lstSearchFtag == null)
        {
            return strWhere;
        }

        foreach (
            var sf in lstSearchFtag
                .Where(x =>
                    hasFDefault
                        ? !string.IsNullOrEmpty(x.FDEFAULT)
                        : string.IsNullOrEmpty(x.FDEFAULT)
                )
                .ToList()
        )
        {
            strWhere = GenWhereString(sf, strWhere, strOrAnd);
            if (string.IsNullOrEmpty(sf.FDEFAULT))
            {
                sf.ORAND = "";
            }
        }

        return strWhere;
    }

    /// <summary>
    /// Description: GenWhereString
    /// </summary>
    private static string GenWhereString(JsonSearchFtag sf, string strWhere, string strOrAnd = "")
    {
        try
        {
            string strIn = "";
            DateTime dt = new DateTime();
            double dbl;
            string str = "";
            string ftag = "";
            string orAnd = "";

            if (sf.FVAL != null && sf.INCDT)
            {
                orAnd = "";
                if (!string.IsNullOrEmpty(sf.ORAND))
                {
                    orAnd = sf.ORAND;
                }
                else if (!string.IsNullOrEmpty(strOrAnd))
                {
                    orAnd = sf.ORAND;
                }
                else
                {
                    orAnd = O9Constants.O9_CONSTANT_OR;
                }

                orAnd = " " + orAnd.Trim().ToUpper() + " ";

                switch (sf.FTYPE.ToUpper())
                {
                    case "C":
                    case "J":
                    case "V":
                        if (sf.FVAL is not DBNull && sf.FVAL is not List<string>)
                        {
                            str = sf.FVAL.ToString();
                        }

                        ftag = sf.FWHERE;

                        if (
                            !str.ToUpper().StartsWith("IS NULL")
                            && !str.ToUpper().StartsWith("IS NOT NULL")
                        )
                        {
                            if (sf.FVAL is not List<string>)
                            {
                                if (
                                    !string.IsNullOrEmpty(sf.OPRT)
                                    && sf.OPRT.ToUpper().Equals("CONTAINS")
                                )
                                {
                                    strWhere = string.IsNullOrEmpty(str)
                                        ? strWhere
                                        : strWhere
                                            + sf.OPRT
                                            + "("
                                            + ftag
                                            + ",'"
                                            + str
                                            + "')>0"
                                            + orAnd;
                                }
                                else if (
                                    !string.IsNullOrEmpty(sf.OPRT)
                                    || !string.IsNullOrEmpty(sf.OPRTVAL)
                                )
                                {
                                    strWhere = string.IsNullOrEmpty(str)
                                        ? strWhere
                                        : strWhere
                                            + "UPPER("
                                            + ftag
                                            + ") ="
                                            + (string.IsNullOrEmpty(sf.OPRT) ? sf.OPRT : sf.OPRTVAL)
                                            + " '"
                                            + str.ToUpper()
                                            + "'"
                                            + orAnd;
                                }
                                else
                                {
                                    strWhere = string.IsNullOrEmpty(str)
                                        ? strWhere
                                        : strWhere
                                            + "UPPER("
                                            + ftag
                                            + ") LIKE '%"
                                            + str.ToUpper()
                                            + "%'"
                                            + orAnd;
                                }
                            }
                            else
                            {
                                if (((List<string>)sf.FVAL).Count > 0)
                                {
                                    foreach (var s in (List<string>)sf.FVAL)
                                    {
                                        strIn += $"'{s.ToUpper()}'{O9Constants.O9_CONSTANT_COMMA}";
                                    }

                                    if (strIn.EndsWith(O9Constants.O9_CONSTANT_COMMA))
                                    {
                                        strIn = strIn.Substring(
                                            0,
                                            strIn.Length - O9Constants.O9_CONSTANT_COMMA.Length
                                        );
                                    }

                                    strWhere += $"UPPER({ftag}) IN ({strIn}){orAnd}";
                                }
                            }
                        }
                        else
                        {
                            strWhere += ftag + " " + str + orAnd;
                        }

                        break;
                    case "N":
                    case "B":
                        if (sf.FVAL is not DBNull && sf.FVAL is not List<string>)
                        {
                            if (!NumberParser.TryParseDouble(sf.FVAL.ToString(), out dbl))
                            {
                                dbl = double.MinValue;
                            }

                            strWhere =
                                dbl == double.MinValue
                                    ? strWhere
                                    : strWhere
                                        + sf.FTAG
                                        + (string.IsNullOrEmpty(sf.OPRTVAL) ? "=" : sf.OPRTVAL)
                                        + NumberParser.ToString(dbl)
                                        + orAnd;
                        }
                        else
                        {
                            if (sf.FVAL is List<string> list)
                            {
                                foreach (var s in list)
                                {
                                    strIn += s.ToUpper() + O9Constants.O9_CONSTANT_COMMA;
                                }

                                if (strIn.EndsWith(O9Constants.O9_CONSTANT_COMMA))
                                {
                                    strIn = strIn.Substring(
                                        0,
                                        strIn.Length - O9Constants.O9_CONSTANT_COMMA.Length
                                    );
                                }

                                strWhere += sf.FTAG + " IN (" + strIn + ")" + orAnd;
                            }
                        }

                        break;
                    case "D":
                        if (sf.FVAL is not DBNull)
                        {
                            if (O9Utils.CheckValueIsDate(sf.FVAL, ref dt))
                            {
                                strWhere =
                                    dt
                                    != O9Utils.ConvertToDateTimeFormat(
                                        O9Constants.O9_CONSTANT_DATE_COMPARE
                                    )
                                        ? strWhere
                                            + "TO_DATE("
                                            + sf.FTAG
                                            + ",'"
                                            + O9Constants.O9_CONSTANT_DATE_FORMAT
                                            + "') "
                                            + (
                                                sf.OPRTVAL != null
                                                && !string.IsNullOrEmpty(sf.OPRTVAL)
                                                    ? sf.OPRTVAL
                                                    : "="
                                            )
                                            + " TO_DATE('"
                                            + O9Utils.ConvertDateToStringFormat(dt)
                                            + "','"
                                            + O9Constants.O9_CONSTANT_DATE_FORMAT
                                            + "')"
                                            + orAnd
                                        : strWhere;
                            }
                            else if (O9Utils.CheckValueIsTime(sf.FVAL, ref dt))
                            {
                                strWhere =
                                    dt
                                    != O9Utils.ConvertToDateTimeFormat(
                                        O9Constants.O9_CONSTANT_DATE_COMPARE
                                    )
                                        ? strWhere
                                            + sf.FTAG
                                            + (
                                                sf.OPRTVAL != null
                                                && !string.IsNullOrEmpty(sf.OPRTVAL)
                                                    ? sf.OPRTVAL
                                                    : "="
                                            )
                                            + "'"
                                            + O9Utils.ConvertTimeToStringFormat(
                                                dt,
                                                O9Constants.O9_CONSTANT_TIME_FORMAT
                                            )
                                            + "'"
                                            + orAnd
                                        : strWhere;
                            }
                        }

                        break;
                    default:
                        break;
                }
            }

            return strWhere;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.StackTrace);
            throw new Exception("GenWhereString");
        }
    }

    /// <summary>
    /// Description: RenameFields
    /// </summary>
    public JToken RenameFields(JToken data)
    {
        try
        {
            if (data is JArray)
            {
                foreach (var row in data.Children<JObject>())
                {
                    if (this.LSTSFT.Count > 0)
                    {
                        int i = 0;
                        // 13/12/2020: huandv cập nhật xử lí phần rename field trả về từ search,
                        // bởi vì lúc gen query chỉ lấy fields inselect nên lúc map field trả về cũng phải inselect
                        foreach (var f in this.LSTSFT.Where(ftag => ftag.INSELECT))
                        {
                            string index = i.ToString();
                            if (row.ContainsKey(index))
                            {
                                row[index].Rename(f.FTAG.ToLower());
                            }

                            //if (f.INPTYPE.Equals("MKD"))
                            //{
                            //    if (row[f.FTAG.ToLower()].Type != JTokenType.Null)
                            //    {
                            //        DateTime dt = row[f.FTAG.ToLower()].Value<DateTime>();
                            //        try
                            //        {
                            //            row[f.FTAG.ToLower()] = O9Utils.ConvertDateToLong(dt);
                            //        }
                            //        catch (Exception)
                            //        {
                            //            row[f.FTAG.ToLower()] = null;
                            //        }
                            //    }
                            //}
                            i++;
                        }

                        //KienVT Gan gia tri cua Ftag CDID -> Ftag
                        i = 0;
                        foreach (var item2 in this.LSTSFT.Where(x => x.FTAG.Contains("_CDID")))
                        {
                            if (
                                row.ContainsKey(item2.FTAG.Replace("_CDID", "").ToLower())
                                && row.ContainsKey(item2.FTAG.ToLower())
                            )
                            {
                                row[item2.FTAG.Replace("_CDID", "").ToLower()] = row[
                                    item2.FTAG.ToLower()
                                ];
                                row.Remove(item2.FTAG.ToLower());
                            }
                        }
                    }
                }
            }

            return data;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.StackTrace);
            throw new Exception("RenameFields");
        }
    }

    /// <summary>
    /// Description: RenameFieldsInselectVisible
    /// </summary>
    private JToken RenameFieldsInselectVisible(JToken data)
    {
        try
        {
            if (data is JArray)
            {
                foreach (var row in data.Children<JObject>())
                {
                    if (this.LSTSFT.Count > 0)
                    {
                        int i = 0;
                        foreach (var f in this.LSTSFT.Where(x => x.INSELECT == true))
                        {
                            string index = i.ToString();

                            if (row.ContainsKey(index))
                            {
                                row[index].Rename(f.FTAG.ToLower());
                            }

                            if (f.INPTYPE.Equals("MKD"))
                            {
                                if (row[f.FTAG.ToLower()].Type != JTokenType.Null)
                                {
                                    DateTime dt = row[f.FTAG.ToLower()].Value<DateTime>();
                                    try
                                    {
                                        row[f.FTAG.ToLower()] = O9Utils.ConvertDateToLong(dt);
                                    }
                                    catch (Exception)
                                    {
                                        row[f.FTAG.ToLower()] = null;
                                    }
                                }
                            }

                            i++;
                        }

                        //KienVT Gan gia tri cua Ftag CDID -> Ftag
                        i = 0;
                        foreach (var item2 in this.LSTSFT.Where(x => x.FTAG.Contains("_CDID")))
                        {
                            if (
                                row.ContainsKey(item2.FTAG.Replace("_CDID", "").ToLower())
                                && row.ContainsKey(item2.FTAG.ToLower())
                            )
                            {
                                row[item2.FTAG.Replace("_CDID", "").ToLower()] = row[
                                    item2.FTAG.ToLower()
                                ];
                                row.Remove(item2.FTAG.ToLower());
                            }
                        }

                        i = 0;
                        foreach (var f in this.LSTSFT.Where(x => x.VISIBLE == false))
                        {
                            string index = i.ToString();

                            if (row.ContainsKey(f.FTAG.ToLower()))
                            {
                                row.Remove(f.FTAG.ToLower());
                            }

                            i++;
                        }
                    }
                }
            }

            return data;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.StackTrace);
            throw new Exception("RenameFieldsInselectVisible");
        }
    }

    /// <summary>
    /// Description: RenameFieldsVisible
    /// </summary>
    private JToken RenameFieldsVisible(JToken data)
    {
        try
        {
            if (data is JArray)
            {
                foreach (var row in data.Children<JObject>())
                {
                    if (this.LSTSFT.Count > 0)
                    {
                        int i = 0;
                        foreach (var f in this.LSTSFT.Where(x => x.VISIBLE == true))
                        {
                            string index = i.ToString();
                            if (f.VISIBLE == true)
                            {
                                if (row.ContainsKey(index))
                                {
                                    row[index].Rename(f.FTAG.ToLower());
                                }

                                if (f.INPTYPE.Equals("MKD"))
                                {
                                    if (row[f.FTAG.ToLower()].Type != JTokenType.Null)
                                    {
                                        DateTime dt = row[f.FTAG.ToLower()].Value<DateTime>();
                                        try
                                        {
                                            row[f.FTAG.ToLower()] = O9Utils.ConvertDateToLong(dt);
                                        }
                                        catch (Exception)
                                        {
                                            row[f.FTAG.ToLower()] = null;
                                        }
                                    }
                                }
                            }

                            i++;
                        }

                        //KienVT Gan gia tri cua Ftag CDID -> Ftag
                        i = 0;
                        foreach (var item2 in this.LSTSFT.Where(x => x.FTAG.Contains("_CDID")))
                        {
                            if (
                                row.ContainsKey(item2.FTAG.Replace("_CDID", "").ToLower())
                                && row.ContainsKey(item2.FTAG.ToLower())
                            )
                            {
                                row[item2.FTAG.Replace("_CDID", "").ToLower()] = row[
                                    item2.FTAG.ToLower()
                                ];
                                row.Remove(item2.FTAG.ToLower());
                            }
                        }
                    }
                }
            }

            return data;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.StackTrace);
            throw new Exception("RenameFieldsVisible");
        }
    }
}

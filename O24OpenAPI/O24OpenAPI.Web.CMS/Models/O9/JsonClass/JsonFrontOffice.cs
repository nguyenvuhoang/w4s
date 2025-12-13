using Newtonsoft.Json.Linq;

namespace O24OpenAPI.Web.CMS.Models.O9;

public class JsonFrontOffice : JsonHeader
{
    /// <summary>
    /// Transaction body
    /// </summary>
    public JObject TXBODY { get; set; }

    /// <summary>
    /// Transaction posting
    /// </summary>
    public JsonPosting POSTING { get; set; }

    /// <summary>
    /// fee
    /// </summary>
    public JObject IFCFEE { get; set; }

    /// <summary>
    /// Transaction result
    /// </summary>
    public JObject RESULT { get; set; }

    /// <summary>
    ///
    /// </summary>
    public JObject IBRET { get; set; }

    /// <summary>
    ///
    /// </summary>
    public JObject DATASET { get; set; }

    /// <summary>
    ///
    /// </summary>
    public JsonFrontOffice() { }

    /// <summary>
    ///
    /// </summary>
    public JsonFrontOffice(JsonFrontOfficeMapping jsonFrontOfficeMapping)
    {
        if (jsonFrontOfficeMapping != null)
        {
            TXCODE = jsonFrontOfficeMapping.A;
            TXDT = jsonFrontOfficeMapping.B;
            TXREFID = jsonFrontOfficeMapping.C;
            VALUEDT = jsonFrontOfficeMapping.D;
            BRANCHID = jsonFrontOfficeMapping.E;
            USRID = jsonFrontOfficeMapping.F;
            LANG = jsonFrontOfficeMapping.G;
            USRWS = jsonFrontOfficeMapping.H;
            APUSER = jsonFrontOfficeMapping.I;
            APUSRIP = jsonFrontOfficeMapping.J;
            APUSRWS = jsonFrontOfficeMapping.K;
            APDT = jsonFrontOfficeMapping.L;
            STATUS = jsonFrontOfficeMapping.M;
            ISREVERSE = jsonFrontOfficeMapping.N;
            HBRANCHID = jsonFrontOfficeMapping.O;
            RBRANCHID = jsonFrontOfficeMapping.P;
            APREASON = jsonFrontOfficeMapping.Q;
            PRN = jsonFrontOfficeMapping.R;
            TXBODY = ConvertMappingToOriginal(jsonFrontOfficeMapping.A, jsonFrontOfficeMapping.S);
            if (jsonFrontOfficeMapping.T != null)
            {
                POSTING = new JsonPosting(jsonFrontOfficeMapping.T);
            }

            RESULT = jsonFrontOfficeMapping.V;
            DATASET = jsonFrontOfficeMapping.W;
            ID = jsonFrontOfficeMapping.ID;
            IBRET = jsonFrontOfficeMapping.IBRET;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public JObject ConvertMappingToOriginal(string txcode, JObject txbody)
    {
        if (string.IsNullOrEmpty(txcode))
        {
            return null;
        }

        JObject jsMapField = O9Utils.GetMapFieldFrontOffice(txcode);
        if (jsMapField != null && jsMapField.Count > 0)
        {
            JObject jsOriginal = new();
            foreach (var jsValue in txbody)
            {
                bool isHasItem = false;
                foreach (var jsMap in jsMapField)
                {
                    if (
                        jsMap.Value != null
                        && jsMap.Value.GetType() == typeof(JValue)
                        && ((JValue)jsMap.Value).Value.Equals(jsValue.Key)
                    )
                    {
                        jsOriginal.Add(jsMap.Key.ToLower(), jsValue.Value);
                        isHasItem = true;
                        break;
                    }
                }
                if (!isHasItem)
                {
                    jsOriginal.Add(jsValue.Key.ToLower(), jsValue.Value);
                }
            }
            return jsOriginal;
        }
        return txbody;
    }
}

/// <summary>
///
/// </summary>
public class JsonFrontOfficeMapping : JsonHeaderMapping
{
    /// <summary>
    ///
    /// </summary>
    public JObject S { get; set; }

    /// <summary>
    ///
    /// </summary>
    public JsonPostingMapping T { get; set; }

    /// <summary>
    ///
    /// </summary>
    public JObject U { get; set; }

    /// <summary>
    ///
    /// </summary>
    public JObject V { get; set; }

    /// <summary>
    ///
    /// </summary>
    public JObject W { get; set; }

    /// <summary>
    ///
    /// </summary>
    public JObject IBRET { get; set; }

    /// <summary>
    ///
    /// </summary>
    public JsonFrontOfficeMapping() { }

    /// <summary>
    ///
    /// </summary>
    public JsonFrontOfficeMapping(JsonFrontOffice jsonFrontOffice)
    {
        if (jsonFrontOffice != null)
        {
            A = jsonFrontOffice.TXCODE;
            B = jsonFrontOffice.TXDT;
            C = jsonFrontOffice.TXREFID;
            D = jsonFrontOffice.VALUEDT;
            E = jsonFrontOffice.BRANCHID;
            F = jsonFrontOffice.USRID;
            G = jsonFrontOffice.LANG;
            H = jsonFrontOffice.USRWS;
            I = jsonFrontOffice.APUSER;
            J = jsonFrontOffice.APUSRIP;
            K = jsonFrontOffice.APUSRWS;
            L = jsonFrontOffice.APDT;
            M = jsonFrontOffice.STATUS;
            N = jsonFrontOffice.ISREVERSE;
            O = jsonFrontOffice.HBRANCHID;
            P = jsonFrontOffice.RBRANCHID;
            Q = jsonFrontOffice.APREASON;
            R = jsonFrontOffice.PRN;
            S = ConvertOriginalToMapping(jsonFrontOffice.TXCODE, jsonFrontOffice.TXBODY);
            if (jsonFrontOffice.POSTING != null)
            {
                T = new JsonPostingMapping(jsonFrontOffice.POSTING);
            }

            if (jsonFrontOffice.IFCFEE != null && jsonFrontOffice.IFCFEE.Count > 0)
            {
                U = JObject.FromObject(
                    new JsonIfcFeeMapping(
                        O9Utils.JSONDeserializeObject<JsonIfcFee>(jsonFrontOffice.IFCFEE.ToString())
                    )
                );
            }
            V = jsonFrontOffice.RESULT;
            W = jsonFrontOffice.DATASET;
            ID = jsonFrontOffice.ID;
            IBRET = jsonFrontOffice.IBRET;
        }
    }

    private static JObject ConvertOriginalToMapping(string txcode, JObject txbody)
    {
        if (string.IsNullOrEmpty(txcode))
        {
            return null;
        }

        JObject jsMapField = O9Utils.GetMapFieldFrontOffice(txcode);
        if (jsMapField != null && jsMapField.Count > 0)
        {
            JObject jsMapping = new JObject();
            foreach (var jsValue in txbody)
            {
                if (O9Utils.JsonContains(jsMapField, jsValue.Key))
                {
                    jsMapping.Add(
                        ((JValue)jsMapField.SelectToken(jsValue.Key)).Value.ToString(),
                        jsValue.Value
                    );
                }
                else
                {
                    jsMapping.Add(jsValue.Key, jsValue.Value);
                }
            }
            return jsMapping;
        }
        return txbody;
    }
}

/// <summary>
///
/// </summary>
public class JsonFrontOfficeResponse
{
    /// <summary>
    ///
    /// </summary>
    public object M { get; set; }

    /// <summary>
    ///
    /// </summary>
    public int R { get; set; }
}

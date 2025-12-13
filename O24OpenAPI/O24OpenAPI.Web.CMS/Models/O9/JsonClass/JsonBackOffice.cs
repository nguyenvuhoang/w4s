namespace O24OpenAPI.Web.CMS.Models.O9;


/// <summary>
///
/// </summary>
public class JsonBackOffice : JsonHeader
{
    /// <summary>
    ///
    /// </summary>
    public List<JsonData> TXBODY { get; set; }
    /// <summary>
    ///
    /// </summary>
    public JsonBackOffice(JsonBackOfficeMapping clsJsonBackOfficeMapping = null)
    {
        try
        {
            if (clsJsonBackOfficeMapping != null)
            {
                TXCODE = clsJsonBackOfficeMapping.A;
                TXDT = clsJsonBackOfficeMapping.B;
                TXREFID = clsJsonBackOfficeMapping.C;
                VALUEDT = clsJsonBackOfficeMapping.D;
                BRANCHID = clsJsonBackOfficeMapping.E;
                USRID = clsJsonBackOfficeMapping.F;
                LANG = clsJsonBackOfficeMapping.G;
                USRWS = clsJsonBackOfficeMapping.H;
                APUSER = clsJsonBackOfficeMapping.I;
                APUSRIP = clsJsonBackOfficeMapping.J;
                APUSRWS = clsJsonBackOfficeMapping.K;
                APDT = clsJsonBackOfficeMapping.L;
                STATUS = clsJsonBackOfficeMapping.M;
                ISREVERSE = clsJsonBackOfficeMapping.N;
                HBRANCHID = clsJsonBackOfficeMapping.O;
                RBRANCHID = clsJsonBackOfficeMapping.P;
                APREASON = clsJsonBackOfficeMapping.Q;
                PRN = clsJsonBackOfficeMapping.R;
                TXBODY = new List<JsonData>();
                //return original from mapping of txbody
                foreach (var clsJsonDataMapping in clsJsonBackOfficeMapping.S)
                {
                    TXBODY.Add(new JsonData(clsJsonDataMapping));
                }
                ID = clsJsonBackOfficeMapping.ID;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
            throw new O24OpenAPIException("Error when mapping backOffice");
        }
    }
}
/// <summary>
///
/// </summary>
public class JsonBackOfficeMapping : JsonHeaderMapping
{
    /// <summary>
    ///
    /// </summary>
    public List<JsonDataMapping> S { get; set; } = new();
    /// <summary>
    ///
    /// </summary>
    public JsonBackOfficeMapping(JsonBackOffice clsJsonBackOffice = null, bool isMappingToArray = false)
    {
        try
        {
            if (clsJsonBackOffice != null)
            {
                A = clsJsonBackOffice.TXCODE;
                B = clsJsonBackOffice.TXDT;
                C = clsJsonBackOffice.TXREFID;
                D = clsJsonBackOffice.VALUEDT;
                E = clsJsonBackOffice.BRANCHID;
                F = clsJsonBackOffice.USRID;
                G = clsJsonBackOffice.LANG;
                H = clsJsonBackOffice.USRWS;
                I = clsJsonBackOffice.APUSER;
                J = clsJsonBackOffice.APUSRIP;
                K = clsJsonBackOffice.APUSRWS;
                L = clsJsonBackOffice.APDT;
                M = clsJsonBackOffice.STATUS;
                N = clsJsonBackOffice.ISREVERSE;
                O = clsJsonBackOffice.HBRANCHID;
                P = clsJsonBackOffice.RBRANCHID;
                Q = clsJsonBackOffice.APREASON;
                R = clsJsonBackOffice.PRN;
                S = new List<JsonDataMapping>();
                // mapping txbody
                foreach (var clsJsonData in clsJsonBackOffice.TXBODY)
                {
                    S.Add(new JsonDataMapping(clsJsonData, isMappingToArray));
                }

                ID = clsJsonBackOffice.ID;
            }
        }
        catch (Exception ex)
        {
            ex.HandleException();
            throw new O24OpenAPIException("Error when mapping backOffice");
        }
    }
}


/// <summary>
///
/// </summary>
public class JsonBackOfficeBaseResponse
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

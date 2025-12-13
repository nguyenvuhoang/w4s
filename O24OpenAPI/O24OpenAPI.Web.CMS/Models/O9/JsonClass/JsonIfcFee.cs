using Newtonsoft.Json.Linq;

namespace O24OpenAPI.Web.CMS.Models.O9;

/// <summary>
/// 
/// </summary>
public class JsonIfcFee
{
    /// <summary>
    /// 
    /// </summary>
    public JArray SFAPPL { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public JArray CCRID { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public JArray IFCCD { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public JArray IFCAMT { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public JArray PAYRATE { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public JArray PAYSRC { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public JArray SRATE { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public JArray SAMT { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public JArray RRATE { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public JArray RAMT { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public JArray IFCVAL { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public JArray FLRVAL { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public JArray CEIVAL { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public JArray CCRCD { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public JsonIfcFee(JsonIfcFeeMapping clsJsonIfcFeeMapping = null) {
        if (clsJsonIfcFeeMapping != null) {
            IFCAMT = clsJsonIfcFeeMapping.A; 
            IFCCD = clsJsonIfcFeeMapping.C; 
            IFCVAL = clsJsonIfcFeeMapping.E; 
            FLRVAL = clsJsonIfcFeeMapping.G; 
            CEIVAL = clsJsonIfcFeeMapping.H; 
            CCRID = clsJsonIfcFeeMapping.I; 
            CCRCD = clsJsonIfcFeeMapping.L; 
            SFAPPL = clsJsonIfcFeeMapping.P; 
            PAYRATE = clsJsonIfcFeeMapping.R; 
            PAYSRC = clsJsonIfcFeeMapping.S;
            SRATE = clsJsonIfcFeeMapping.T; 
            SAMT = clsJsonIfcFeeMapping.U; 
            RRATE = clsJsonIfcFeeMapping.V; 
            RAMT = clsJsonIfcFeeMapping.W; 
        }
    }
}

/// <summary>
/// 
/// </summary>
public class JsonIfcFeeMapping
{
    /// <summary>
    /// 
    /// </summary>
    public JArray A { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public JArray B { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public JArray C { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public JArray E { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public JArray G { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public JArray H { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public JArray I { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public JArray L { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public JArray P { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public JArray R { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public JArray S { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public JArray T { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public JArray U { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public JArray V { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public JArray W { get; set; }
    //     Private m_ As JArray

    /// <summary>
    /// 
    /// </summary>
    public JsonIfcFeeMapping(JsonIfcFee clsJsonIfcFee = null)
    {
        if (clsJsonIfcFee != null)
        {
            A = clsJsonIfcFee.IFCAMT;
            C = clsJsonIfcFee.IFCCD;
            E = clsJsonIfcFee.IFCVAL;
            G = clsJsonIfcFee.FLRVAL;
            H = clsJsonIfcFee.CEIVAL;
            I = clsJsonIfcFee.CCRID;
            L = clsJsonIfcFee.CCRCD;
            P = clsJsonIfcFee.SFAPPL;
            R = clsJsonIfcFee.PAYRATE;
            S = clsJsonIfcFee.PAYSRC;
            T = clsJsonIfcFee.SRATE;
            U = clsJsonIfcFee.SAMT;
            V = clsJsonIfcFee.RRATE;
            W = clsJsonIfcFee.RAMT;
        }
    }
}
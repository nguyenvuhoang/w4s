using Newtonsoft.Json.Linq;

namespace O24OpenAPI.Web.CMS.Models.O9;

public class JsonTxDef
{
    /// <summary>
    ///
    /// </summary>
    public string TXCODE { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string TXNAME { get; set; }

    /// <summary>
    ///
    /// </summary>
    public JObject DESCR { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string FVRSTS { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string TXIFC { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string TXPIG { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string MDLCODE { get; set; }

    /// <summary>
    ///
    /// </summary>
    public int INVNUM { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string PRN { get; set; }

    /// <summary>
    ///
    /// </summary>
    public int IDX { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string PAYSRC { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string LIMITTYPE { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool ISTMPL { get; set; }
}

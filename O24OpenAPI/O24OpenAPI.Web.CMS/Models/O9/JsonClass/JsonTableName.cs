namespace O24OpenAPI.Web.CMS.Models.O9;

/// <summary>
/// 
/// </summary>
public class JsonTableName
{
    /// <summary>
    /// 
    /// </summary>
    public List<JsonData> TXBODY { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public JsonTableName() { 
        TXBODY = new List<JsonData>();
    }
}
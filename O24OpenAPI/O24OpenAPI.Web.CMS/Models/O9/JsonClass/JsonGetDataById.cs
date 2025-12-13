namespace O24OpenAPI.Web.CMS.Models.O9;

/// <summary>
///
/// </summary>
public class JsonGetDataById
{
    /// <summary>
    ///
    /// </summary>
    public object I { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool M { get; set; }

    /// <summary>
    ///
    /// </summary>
    public JsonGetDataById(object i = null, bool m = true)
    {
        I = i;
        M = m;
    }
}

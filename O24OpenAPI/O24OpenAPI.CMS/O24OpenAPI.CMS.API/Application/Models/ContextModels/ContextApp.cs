namespace O24OpenAPI.CMS.API.Application.Models.ContextModels;

/// <summary>
///
/// </summary>
public class ContextAppModel
{
    /// <summary>
    ///
    /// </summary>
    public ContextAppModel() { }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string App { get; set; } = "";

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public string GetApp() => App;
}

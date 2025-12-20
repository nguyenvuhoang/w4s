namespace O24OpenAPI.CMS.API.Application.Models.ContextModels;

public class JWebUIObjectContextModel
{
    /// <summary>
    ///
    /// </summary>
    public JWebUIObjectContextModel() { }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public ContextInfoRequestModel InfoRequest = new();

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public ContextBoInputModel Bo { get; set; } = new ContextBoInputModel();

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public ContextAppModel InfoApp = new();

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public ContextInfoUserModel InfoUser = new();
}

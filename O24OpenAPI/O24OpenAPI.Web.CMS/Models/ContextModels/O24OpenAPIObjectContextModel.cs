

using O24OpenUI.Web.CMS.Models;

namespace O24OpenAPI.Web.CMS.Models;

public class O24OpenAPIObjectContextModel
{
    /// <summary>
    ///
    /// </summary>
    public O24OpenAPIObjectContextModel() { }
    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public ContextInfoRequestModel InfoRequest = new ContextInfoRequestModel();
    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public ContextBoInputModel Bo = new ContextBoInputModel();
    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public ContextAppModel InfoApp = new ContextAppModel();
    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public ContextInfoUserModel InfoUser = new ContextInfoUserModel();
}

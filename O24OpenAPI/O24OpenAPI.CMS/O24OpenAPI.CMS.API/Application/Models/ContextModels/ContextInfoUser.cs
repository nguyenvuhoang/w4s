using O24OpenAPI.APIContracts.Models.CTH;
using O24OpenAPI.CMS.API.Application.Models.Request;

namespace O24OpenAPI.CMS.API.Application.Models.ContextModels;

/// <summary>
///
/// </summary>
public class ContextInfoUserModel
{
    /// <summary>
    ///
    /// </summary>
    readonly string token = string.Empty;

    /// <summary>
    ///
    /// </summary>
    public ContextInfoUserModel() { }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    private RequestHeaderModel _userLogin;
    public CTHUserSessionModel UserSession { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public RequestHeaderModel GetUserLogin()
    {
        return _userLogin;
    }

    public void SetUserLogin(RequestHeaderModel userLogin)
    {
        _userLogin = userLogin;
    }
}

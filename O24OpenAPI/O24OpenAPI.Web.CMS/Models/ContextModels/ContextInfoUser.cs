namespace O24OpenAPI.Web.CMS.Models;

/// <summary>
///
/// </summary>
public class ContextInfoUserModel
{
    /// <summary>
    ///
    /// </summary>
    string token = string.Empty;

    /// <summary>
    ///
    /// </summary>
    public ContextInfoUserModel() { }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    private RequestHeaderModel _userLogin;
    public UserSessions UserSession { get; set; }

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

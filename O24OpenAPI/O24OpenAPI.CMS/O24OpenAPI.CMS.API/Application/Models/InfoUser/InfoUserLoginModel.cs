namespace O24OpenAPI.CMS.API.Application.Models.InfoUser;

/// <summary>
///
/// </summary>
public class InfoUserLoginModel
{
    /// <summary>
    ///
    /// </summary>
    public InfoUserLoginModel() { }

    /// <summary>
    /// User code
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Token
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// PortalToken
    /// </summary>
    public string PortalToken { get; set; } = string.Empty;

    /// <summary>
    /// User code
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Username
    /// </summary>
    public string App { get; set; } = string.Empty;

    /// <summary>
    /// User code
    /// </summary>
    public string KeyFeedback { get; set; } = string.Empty;

    /// <summary>
    /// Username
    /// </summary>
    public string FormCode { get; set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string FormName { get; set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string FromActionCode { get; set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string FromActionName { get; set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public bool isDebug { get; set; } = false;
}

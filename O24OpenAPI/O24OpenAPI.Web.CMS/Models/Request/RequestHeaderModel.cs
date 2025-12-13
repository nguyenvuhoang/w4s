namespace O24OpenAPI.Web.CMS.Models;

/// <summary>
///
/// </summary>
public class RequestHeaderModel : BaseO24OpenAPIModel
{
    /// <summary>
    ///
    /// </summary>
    public RequestHeaderModel() { }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string Domain { get; set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string Url { get; set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string Lang { get; set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string App { get; set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, object> MyDevice { get; set; } = new Dictionary<string, object>();

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public long RequestTime { get; set; } = 0;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string UserId { get; set; } = "";

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string PortalToken { get; set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public bool isDebug { get; set; } = false;

    public string Signature { get; set; } = string.Empty;
    public string Timestamp { get; set; } = string.Empty;
    public string Nonce { get; set; } = string.Empty;

    public string GetDeviceName()
    {
        return MyDevice.TryGetValue("device_name", out object value)
            ? value.ToString()
            : (
                MyDevice.TryGetValue("browser", out object value1)
                    ? value1.ToString()
                    : string.Empty
            );
    }

    public string GetDeviceId()
    {
        return MyDevice.TryGetValue("device_id", out object value)
            ? value.ToString()
            : string.Empty;
    }
}

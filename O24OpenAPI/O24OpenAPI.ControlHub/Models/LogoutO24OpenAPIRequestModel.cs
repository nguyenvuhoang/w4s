using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.ControlHub.Models;

/// <summary>
/// The login to 24 open api request model class
/// </summary>
/// <seealso cref="BaseTransactionModel"/>
public class LogoutO24OpenAPIRequestModel : BaseTransactionModel
{
    /// <summary>
    /// Gets or sets the value of the login name
    /// </summary>
    public string LoginName { get; set; }
    /// <summary>
    /// /// Gets or sets the value of the device
    /// </summary>
    public string DeviceId { get; set; }
    /// <summary>
    /// /// Gets or sets the value of the device
    /// </summary>
    public string DeviceType { get; set; }

    /// <summary>
    /// Gets or sets the value of the ip address
    /// </summary>
    public string IpAddress { get; set; }
    /// <summary>
    /// User Agent
    /// </summary>
    public string UserAgent { get; set; }
    /// <summary>
    /// OsVersion
    /// </summary>
    public string OsVersion { get; set; }
    /// <summary>
    /// App Version
    /// </summary>
    public string AppVersion { get; set; }
    /// <summary>
    /// Device Name
    /// </summary>
    public string DeviceName { get; set; }
    /// <summary>
    /// Brand
    /// </summary>
    public string Brand { get; set; }
    /// <summary>
    /// IsEmulator
    /// </summary>
    public bool IsEmulator { get; set; }
    /// <summary>
    /// IsRootedOrJailbroken
    /// </summary>
    public bool IsRootedOrJailbroken { get; set; }
    /// <summary>
    /// Modelname
    /// </summary>
    public string Modelname { get; set; } = string.Empty;
}

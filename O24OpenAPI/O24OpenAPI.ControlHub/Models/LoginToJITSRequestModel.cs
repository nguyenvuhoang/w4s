using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.ControlHub.Models;

/// <summary>
/// The login to 24 open api request model class
/// </summary>
/// <seealso cref="BaseTransactionModel"/>
public class LoginToJITSRequestModel : BaseTransactionModel
{
    /// <summary>
    /// Gets or sets the value of the reference
    /// </summary>
    public string Reference { get; set; }

    /// <summary>
    /// Gets or sets the value of the user id
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// Gets or sets the value of the login name
    /// </summary>
    public string LoginName { get; set; }

    public string UserCode { get; set; }
    public string UserName { get; set; }

    /// <summary>
    /// Gets or sets the value of the password
    /// </summary>
    public string Password { get; set; }
    public string BranchCode { get; set; }

    /// <summary>
    /// Gets or sets the value of the is supper admin
    /// </summary>
    public bool IsSupperAdmin { get; set; }

    /// <summary>
    /// /// Gets or sets the value of the device
    /// </summary>
    public string DeviceId { get; set; } = string.Empty;
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
    /// Gets or sets the value of the roles
    /// </summary>
    public string RoleChannel { get; set; }
    public bool IsJITSUser { get; set; } = true;
    /// <summary>
    /// Gets or sets the value of the PushId
    /// </summary>
    public string PushId { get; set; }
    /// <summary>
    /// OSVERSION
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
    /// <summary>
    /// IsResetDevice
    /// </summary>
    public bool IsResetDevice { get; set; } = false;
    /// <summary>
    /// Core Token
    /// </summary>
    public string CoreToken { get; set; } = string.Empty;
    /// <summary>
    /// Refresh Token
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;
    /// <summary>
    /// Network
    /// </summary>
    public string Network { get; set; } = string.Empty;
    /// <summary>
    /// Memory
    /// </summary>
    public string Memory { get; set; } = string.Empty;

}

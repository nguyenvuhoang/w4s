using O24OpenAPI.Core.Domain.Users;

namespace O24OpenAPI.Web.CMS.Models.Digital;

/// <summary>
/// The active otp model class
/// </summary>
/// <seealso cref="BaseTransactionModel"/>
/// <seealso cref="IUser"/>

public class ActiveOTPModel : BaseTransactionModel
{
    /// <summary>
    /// Gets or sets the value of the active status
    /// </summary>
    public User UserInfo { get; set; }

    /// <summary>
    /// Gets or sets the value of the active status
    /// </summary>
    public string ActiveStatus { get; set; }

    /// <summary>
    /// Gets or sets the value of the in active status
    /// </summary>
    public string DeActiveStatus { get; set; }

    /// <summary>
    /// Gets or sets the value of the device id
    /// </summary>
    public string DeviceId { get; set; }

    /// <summary>
    /// /// Gets or sets the value of the pincode
    /// </summary>
    public string Pincode { get; set; }

    /// <summary>
    /// Gets or sets the value of the active code
    /// </summary>
    public string ActiveCode { get; set; }

    /// <summary>
    /// Gets or sets the value of the push id
    /// </summary>
    public string PushID { get; set; }

    public string DeviceType { get; set; }
}

/// <summary>
/// The register otp model class
/// </summary>
/// <seealso cref="BaseTransactionModel"/>
/// <seealso cref="IUser"/>

public class RegisterOTPModel : BaseTransactionModel
{
    /// <summary>
    /// Gets or sets the value of the active status
    /// </summary>
    public User UserInfo { get; set; }

    /// <summary>
    /// Gets or sets the value of the device id
    /// </summary>
    public string DeviceId { get; set; }
}

/// <summary>
/// The register otp encrypt model class
/// </summary>
/// <seealso cref="IUser"/>
public class RegisterOTPEncryptModel : IUser
{
    /// <summary>
    /// Gets or sets the value of the time
    /// </summary>
    public DateTime Time { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the value of the user code
    /// </summary>
    public string UserCode { get; set; }

    /// <summary>
    /// Gets or sets the value of the login name
    /// </summary>
    public string LoginName { get; set; }

    /// <summary>
    /// Gets or sets the value of the username
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Gets or sets the value of the branch code
    /// </summary>
    public string BranchCode { get; set; }

    /// <summary>
    /// Gets or sets the value of the device id
    /// </summary>
    public string DeviceId { get; set; }
}

/// <summary>
/// The verify otp model class
/// </summary>
/// <seealso cref="BaseTransactionModel"/>
public class VerifyOtpModel : BaseTransactionModel
{
    /// <summary>
    /// Gets or sets the value of UserInfo
    /// </summary>
    public User UserInfo { get; set; }
    public string DeviceId { get; set; }

    /// <summary>
    /// Gets or sets the value of the active status
    /// </summary>
    public string ActiveStatus { get; set; }

    /// <summary>
    /// Gets or sets the value of the otp
    /// </summary>
    public string Otp { get; set; }

    /// <summary>
    /// Gets or sets the value of the trans id
    /// </summary>
    public new string TransId { get; set; }
    public string ActionType { get; set; }
    public Dictionary<string, string> Data { get; set; }
    public int NotificationId { get; set; }
}

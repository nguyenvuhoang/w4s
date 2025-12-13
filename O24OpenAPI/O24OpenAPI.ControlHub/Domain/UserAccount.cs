using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.ControlHub.Domain;

/// <summary>
/// The user account class
/// </summary>
/// <seealso cref="BaseEntity"/>
public class UserAccount : BaseEntity
{
    /// <summary>
    /// Gets or sets the value of the channel id
    /// </summary>
    public string ChannelId { get; set; }

    /// <summary>
    /// Gets or sets the value of the user id
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// Gets or sets the value of the user name
    /// </summary>
    public string UserName { get; set; }
    public string UserCode { get; set; }

    /// <summary>
    /// Gets or sets the value of the login name
    /// </summary>
    public string LoginName { get; set; }
    public string ContractNumber { get; set; }

    public string FirstName { get; set; }

    public string MiddleName { get; set; }

    public string LastName { get; set; }

    /// <summary>
    /// Gets or sets the value of the roles
    /// </summary>
    public string RoleChannel { get; set; }

    public int Gender { get; set; } = 0;

    public string Address { get; set; }

    public string Email { get; set; }

    public DateTime? Birthday { get; set; }

    public string Phone { get; set; }

    public string Status { get; set; }

    public string UserCreated { get; set; }
    public DateTime? LastLoginTime { get; set; }

    public string UserModified { get; set; }

    public bool? IsLogin { get; set; }

    public DateTime? ExpireTime { get; set; }

    public string BranchID { get; set; }
    public string BranchCode { get; set; } = string.Empty;

    public string DepartmentCode { get; set; }

    public int UserLevel { get; set; } = 0;

    public string UserType { get; set; }

    public string IsShow { get; set; }

    public int PolicyID { get; set; } = 0;

    public string UUID { get; set; }

    public int Failnumber { get; set; } = 0;

    public bool IsSuperAdmin { get; set; } = false;

    /// <summary>
    /// Gets or sets the value of the updated on utc
    /// </summary>
    public DateTime? UpdatedOnUtc { get; set; }

    /// <summary>
    /// Gets or sets the value of the created on utc
    /// </summary>
    public DateTime? CreatedOnUtc { get; set; }

    /// <summary>
    /// Gets or sets the value of the is Biometric Supported
    /// </summary>
    public bool IsBiometricSupported { get; set; }
    /// <summary>
    /// LockedUntil
    /// </summary>
    public DateTime? LockedUntil { get; set; }
    /// <summary>
    /// NotificationType
    /// </summary>
    public string NotificationType { get; set; } = string.Empty;
    public bool IsFirstLogin { get; set; } = true;

    public UserAccount Clone()
    {
        return (UserAccount)this.MemberwiseClone();
    }
}

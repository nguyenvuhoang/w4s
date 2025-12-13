using O24OpenAPI.APIContracts.Models.CTH;
using O24OpenAPI.Core.Extensions;

namespace O24OpenAPI.Web.CMS.Domain;

/// <summary>
/// The user sessions class
/// </summary>
/// <seealso cref="BaseEntity"/>
public class UserSessions : BaseEntity
{
    /// <summary>
    ///
    /// </summary>
    public int Usrid { get; set; }

    /// <summary>
    /// user id
    /// </summary>
    public string Userid { get; set; } = string.Empty;

    /// <summary>
    /// Token
    /// </summary>
    public string Token { get; set; }

    /// <summary>
    /// /// Gets or sets the value of the hashed refresh token
    /// /// </summary>
    public string HashedRefreshToken { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string UsrBranch { get; set; }

    /// <summary>
    ///
    /// </summary>
    public int UsrBranchid { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string Usrname { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string LoginName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string Lang { get; set; }

    /// <summary>
    ///
    /// </summary>
    public DateTime Txdt { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string Ssesionid { get; set; }

    /// <summary>
    /// Usrid
    /// </summary>
    public string UserCode { get; set; } = string.Empty;

    /// <summary>
    /// Ssntime
    /// </summary>
    public DateTime Ssntime { get; set; }

    /// <summary>
    /// Exptime
    /// </summary>
    public DateTime Exptime { get; set; }

    /// <summary>
    /// Wsip
    /// </summary>
    public string Wsip { get; set; }

    /// <summary>
    /// Mac
    /// </summary>
    public string Mac { get; set; }

    /// <summary>
    /// Wsname
    /// </summary>
    public string Wsname { get; set; }

    /// <summary>
    /// Acttype
    /// </summary>
    public string Acttype { get; set; }

    /// <summary>
    /// Application code
    /// </summary>
    public string ApplicationCode { get; set; }

    /// <summary>
    /// Meta Info
    /// </summary>
    public string Info { get; set; }

    /// <summary>
    /// CommandList
    /// </summary>
    public string CommandList { get; set; } = string.Empty;

    /// <summary>
    /// Last update date
    /// </summary>
    public DateTime? UpdatedOnUtc { get; set; }

    /// <summary>
    /// ResetPassword
    /// </summary>
    public bool ResetPassword { get; set; }

    /// <summary>
    /// Last update date
    /// </summary>
    public DateTime? CreatedOnUtc { get; set; }

    /// <summary>
    /// Session device
    /// </summary>
    public string SessionDevice { get; set; } = string.Empty;

    public string ChannelRoles { get; set; }

    public string Roles { get; set; }
    public string SignatureKey { get; set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    public UserSessions()
    {
        Ssntime = System.DateTime.Now;
        Exptime = System.DateTime.Now;
        Ssesionid = string.Empty;
        Wsip = string.Empty;
        Mac = string.Empty;
        Wsname = string.Empty;
        Token = string.Empty;
        Acttype = "I";
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserSessions"/> class
    /// </summary>
    /// <param name="userSessions">The user sessions</param>
    public UserSessions(UserSessions userSessions)
    {
        Usrid = userSessions.Usrid;
        Userid = userSessions.Userid;
        UsrBranch = userSessions.UsrBranch;
        UsrBranchid = userSessions.UsrBranchid;
        Usrname = userSessions.Usrname;
        LoginName = userSessions.LoginName;
        Lang = userSessions.Lang;
        Txdt = userSessions.Txdt;
        Ssesionid = userSessions.Ssesionid;
        UserCode = userSessions.UserCode;
        Ssntime = userSessions.Ssntime;
        Exptime = userSessions.Exptime;
        Wsip = userSessions.Wsip;
        Mac = userSessions.Mac;
        Wsname = userSessions.Wsname;
        Token = userSessions.Token;
        Acttype = userSessions.Acttype;
        ApplicationCode = userSessions.ApplicationCode;
        Info = userSessions.Info;
        CommandList = userSessions.CommandList;
        UpdatedOnUtc = userSessions.UpdatedOnUtc;
        ResetPassword = userSessions.ResetPassword;
        CreatedOnUtc = DateTime.UtcNow;
        SessionDevice = userSessions.SessionDevice;
        ChannelRoles = userSessions.ChannelRoles;
        Roles = userSessions.Roles;
        SignatureKey = userSessions.SignatureKey;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserSessions"/> class
    /// </summary>
    /// <param name="userSession">The user session</param>
    public UserSessions(CTHUserSessionModel userSession)
    {
        Usrid = userSession.UserId.TryParse<int>(out var id) ? id : 0;
        Userid = userSession.UserId;
        LoginName = userSession.LoginName;
        Lang = "en";
        Txdt = DateTime.UtcNow;
        Ssesionid = userSession.Reference;
        UserCode = userSession.UserId;
        Wsip = userSession.IpAddress;
        Token = userSession.Token;
        ApplicationCode = userSession.ChannelId;
        ChannelRoles = userSession.ChannelRoles;
        UserCode = userSession.UserCode;
        UsrBranch = userSession.BranchCode;
        Usrname = userSession.UserName;
    }

    /// <summary>
    /// Clones this instance
    /// </summary>
    /// <returns>The user sessions</returns>
    public UserSessions Clone()
    {
        return (UserSessions)this.MemberwiseClone();
    }
}

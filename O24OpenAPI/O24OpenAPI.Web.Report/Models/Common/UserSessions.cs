namespace O24OpenAPI.Web.Report.Models.Common;

public class UserSessions
{
    /// <summary>
    ///
    /// </summary>
    public int Usrid { get; set; }

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
    /// Token
    /// </summary>
    public string Token { get; set; }

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

    public Dictionary<string, string> Parameter { get; set; } = null;

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

    public UserSessions(UserSessions userSessions)
    {
        Usrid = userSessions.Usrid;
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
    }
}

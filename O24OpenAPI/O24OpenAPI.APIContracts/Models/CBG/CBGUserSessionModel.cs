namespace O24OpenAPI.APIContracts.Models.CBG;

public class CBGUserSessionModel
{
    /// <summary>
    ///
    /// </summary>
    public int Usrid { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? UsrBranch { get; set; }

    /// <summary>
    ///
    /// </summary>
    public int UsrBranchid { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? Usrname { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? LoginName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? Lang { get; set; }

    /// <summary>
    ///
    /// </summary>
    public DateTime Txdt { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? Ssesionid { get; set; }

    /// <summary>
    /// Usrid
    /// </summary>
    public string? UserCode { get; set; }

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
    public string? Wsip { get; set; }

    /// <summary>
    /// Mac
    /// </summary>
    public string? Mac { get; set; }

    /// <summary>
    /// Wsname
    /// </summary>
    public string? Wsname { get; set; }

    /// <summary>
    /// Token
    /// </summary>
    public string? Token { get; set; }

    /// <summary>
    /// Acttype
    /// </summary>
    public string? Acttype { get; set; }

    /// <summary>
    /// Application code
    /// </summary>
    public string? ApplicationCode { get; set; }

    /// <summary>
    /// Meta Info
    /// </summary>
    public string? Info { get; set; }

    /// <summary>
    /// CommandList
    /// </summary>
    public string? CommandList { get; set; }

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
    public string? SessionDevice { get; set; }
}

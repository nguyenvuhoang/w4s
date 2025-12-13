namespace Jits.Neptune.Web.CMS.LogicOptimal9.Common;

/// <summary>
/// CodeDescription
/// </summary>
public class CodeDescription
{
    /// <summary>
    /// Error code number
    /// </summary>
    public int ERRORCODE { get; set; }

    /// <summary>
    /// Error description
    /// </summary>
    public string ERRORDESC { get; set; }
    
    /// <summary>
    /// get set
    /// </summary>
    public CodeDescription()
    {
    }
    
    /// <summary>
    /// get set
    /// </summary>
    public CodeDescription(int errorCode, string errorDesc)
    {
        ERRORCODE = errorCode;
        ERRORDESC = errorDesc;
    }

}

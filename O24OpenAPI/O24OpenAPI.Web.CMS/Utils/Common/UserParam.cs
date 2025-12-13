
namespace Jits.Neptune.Web.CMS.LogicOptimal9.Common;
/// <summary>
/// 
/// </summary>
public class UserParam
{
    /// <summary>
    /// 
    /// </summary>
    public string GENERAL_SHORT_DATE_FMT { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string GENERAL_LONG_DATE_FMT { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string GENERAL_SHORT_TIME_FMT { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string GENERAL_LONG_TIME_FMT { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string GENERAL_TIME_ZONE { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string GENERAL_DATE_TIME_FMT { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string GENERAL_LANGUAGE { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string GENERAL_SUBLANGUAGE { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string GENERAL_LANGUAGE_CULTURE { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string GENERAL_PHONE { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string GENERAL_DECIMAL_CHAR { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string GENERAL_THOUSAND_CHAR { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string GENERAL_NUMBER_FMT { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int GENERAL_PASS_LENGTH { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public UserParam(BranchParam param) {
        GENERAL_SHORT_DATE_FMT = param.GENERAL_SHORT_DATE_FMT;
        GENERAL_LONG_DATE_FMT = param.GENERAL_LONG_DATE_FMT;
        GENERAL_SHORT_TIME_FMT = param.GENERAL_SHORT_TIME_FMT;
        GENERAL_LONG_TIME_FMT = param.GENERAL_LONG_TIME_FMT; 
        GENERAL_TIME_ZONE = param.GENERAL_TIME_ZONE; 
        GENERAL_DATE_TIME_FMT = param.GENERAL_DATE_TIME_FMT;
        GENERAL_LANGUAGE = param.GENERAL_LANGUAGE; 
        GENERAL_SUBLANGUAGE = param.GENERAL_SUBLANGUAGE;
        GENERAL_LANGUAGE_CULTURE = param.GENERAL_LANGUAGE_CULTURE;
        GENERAL_PHONE = param.GENERAL_PHONE; 
        GENERAL_DECIMAL_CHAR = param.GENERAL_DECIMAL_CHAR; 
        GENERAL_THOUSAND_CHAR = param.GENERAL_THOUSAND_CHAR; 
        GENERAL_NUMBER_FMT = param.GENERAL_NUMBER_FMT;
        GENERAL_PASS_LENGTH = param.GENERAL_PASS_LENGTH;
        //m_GENERAL_FXRATE_LIST
    }
    /// <summary>
    /// 
    /// </summary>
    public UserParam()
    {
    }
}
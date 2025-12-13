namespace Jits.Neptune.Web.CMS.LogicOptimal9.Common;
/// <summary>
/// 
/// </summary>
public class BranchParam : SystemParam {

    /// <summary>
    /// 
    /// </summary>
    public BranchParam(HeadOfficeParam param)
    {
        GENERAL_SHORT_DATE_FMT = param.GENERAL_SHORT_DATE_FMT;
        GENERAL_LONG_DATE_FMT = param.GENERAL_LONG_DATE_FMT;
        GENERAL_SHORT_TIME_FMT = param.GENERAL_SHORT_TIME_FMT;
        GENERAL_LONG_TIME_FMT = param.GENERAL_LONG_TIME_FMT;
        GENERAL_TIME_ZONE = param.GENERAL_TIME_ZONE;
        GENERAL_DATE_TIME_FMT = param.GENERAL_DATE_TIME_FMT;
        GENERAL_LANGUAGE = param.GENERAL_LANGUAGE;
        GENERAL_SUBLANGUAGE = param.GENERAL_SUBLANGUAGE;
        GENERAL_LANGUAGE_CULTURE = param.GENERAL_LANGUAGE_CULTURE;
        GENERAL_COUNTRY = param.GENERAL_COUNTRY;
        GENERAL_PHONE = param.GENERAL_PHONE;
        GENERAL_FAX = param.GENERAL_FAX;
        GENERAL_EMAIL = param.GENERAL_EMAIL;
        GENERAL_PASS_LENGTH = param.GENERAL_PASS_LENGTH;
    }
}
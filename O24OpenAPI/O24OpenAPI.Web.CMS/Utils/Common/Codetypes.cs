namespace Jits.Neptune.Web.CMS.LogicOptimal9.Common;

/// <summary>
/// Codetypes
/// </summary>
public static class Codetypes
{
    /// <summary>
    /// 
    /// </summary>
    public static CodeDescription Err_Not_Started = new CodeDescription(-1, "System not yet started. Please wait abit");

    /// <summary>
    /// 
    /// </summary>
    public static CodeDescription Code_Success_Login = new CodeDescription(0, "Transaction successfully!");

    /// <summary>
    /// 
    /// </summary>
    public static CodeDescription Code_Success_Trans = new CodeDescription(0, "Transaction successfully!");

    /// <summary>
    /// 
    /// </summary>
    public static CodeDescription Err_Unauthorized = new CodeDescription(1, "Unauthorized");

    /// <summary>
    /// 
    /// </summary>
    public static CodeDescription Err_ExpireDate = new CodeDescription(2, "Token expired date");

    /// <summary>
    /// 
    /// </summary>
    public static CodeDescription Err_InvalidToken = new CodeDescription(3, "Invalid token");

    /// <summary>
    /// 
    /// </summary>
    public static CodeDescription Err_Duplicate = new CodeDescription(5, "Message code is duplicated");

    /// <summary>
    /// 
    /// </summary>
    public static CodeDescription Err_AccessFolder = new CodeDescription(6, "Can''t submit this file. Please contact SBILH to checking this case");

    /// <summary>
    /// 
    /// </summary>
    public static CodeDescription SYS_INVALID_SESSION = new CodeDescription(7, "Invalid session by ");

    /// <summary>
    /// 
    /// </summary>
    public static CodeDescription UMG_INVALID_LOGIN_TIME = new CodeDescription(8, "Do not allow login in this time");

    /// <summary>
    /// 
    /// </summary>
    public static CodeDescription UMG_INVALID_EXP_POLICY = new CodeDescription(9, "The policy for user is expired!");

    /// <summary>
    /// 
    /// </summary>
    public static CodeDescription SYS_LOGIN_FALSE = new CodeDescription(10, "The username or password you entered is incorrect!");

    /// <summary>
    /// 
    /// </summary>
    public static CodeDescription SYS_LOGIN_BLOCK = new CodeDescription(11, "System auto block after atemp fail login");

    /// <summary>
    /// 
    /// </summary>
    public static CodeDescription UMG_INVALID_STATUS = new CodeDescription(12, "User invalid status");

    /// <summary>
    /// 
    /// </summary>
    public static CodeDescription UMG_INVALID_EXPDT = new CodeDescription(13, "User's expiry date already");

    /// <summary>
    /// 
    /// </summary>
    public static CodeDescription Err_Image_Extensions = new CodeDescription(14, "Extensions is not support. Extensions must be in (PNG|JPG)");

    /// <summary>
    /// 
    /// </summary>
    public static CodeDescription Err_Folder_Config = new CodeDescription(15, "Config not correct. Please contact SBILH to checking this case");

    /// <summary>
    /// 
    /// </summary>
    public static CodeDescription Err_Image_Duplicate = new CodeDescription(16, "Duplicate file name image");

    /// <summary>
    /// 
    /// </summary>
    public static CodeDescription Err_MT103_Duplicate = new CodeDescription(17, "Duplicate file MT103");

    /// <summary>
    /// 
    /// </summary>
    public static CodeDescription Err_MT103_Extensions = new CodeDescription(18, "Extensions is not support. Extensions must be in (mt103|MT103)");

    /// <summary>
    /// 
    /// </summary>
    public static CodeDescription Err_AML_Exception = new CodeDescription(19, "Exception when sending message to AML");

    /// <summary>
    /// 
    /// </summary>
    public static CodeDescription Err_InvalidLicense = new CodeDescription(20, "Invalid License");

    /// <summary>
    /// 
    /// </summary>
    public static CodeDescription Err_Get_AC_Timeout = new CodeDescription(9991, "Timeout when waiting from CoreAPI");

    /// <summary>
    /// 
    /// </summary>
    public static CodeDescription ERR_CORE_BANKING = new CodeDescription(9998, "Error message from server. Please contact Bank to more information");

    /// <summary>
    /// 
    /// </summary>
    public static CodeDescription Err_Unknown = new CodeDescription(9999, "Error message from server. Please contact Bank to more information");

}

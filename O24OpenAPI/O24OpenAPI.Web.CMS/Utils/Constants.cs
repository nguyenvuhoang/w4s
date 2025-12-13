namespace O24OpenAPI.Web.CMS.Utils;

/// <summary>
/// The constants class
/// </summary>
public static class Constants
{
    /// <summary>
    ///
    /// </summary>
    public const string ServerName = "CMS";

    /// <summary>
    ///
    /// </summary>
    public const string ServerVersion = "1.0";

    /// <summary>
    /// The name space default
    /// </summary>
    public const string NameSpaceDefault = "O24OpenAPI.Web.CMS..";
}

public static class CoreMode
{
    public const string Neptune = "Neptune";
    public const string O9 = "O9";
}

public static class ExecutionStatus
{
    /// <summary>
    ///
    /// </summary>
    public const int SUCCESS = 0;

    /// <summary>
    ///
    /// </summary>
    public const int ERROR = -1;

    /// <summary>
    ///
    /// </summary>
    public const int PROCESSING = 1;
}

public static class DeviceStatus
{
    public const string ACTIVE = "A";
    public const string INACTIVE = "I";
}

public static class DeviceType
{
    public const string ANDROID = "AND";
    public const string IOS = "IOS";
}

public static class NotificationType
{
    public const string PUSH = "PUSH";
    public const string EMAIL = "EMAIL";
}

public static class SmartOTPActionType
{
    public const string ConfirmLogin = "CONFIRM_LOGIN";
    public const string ConfirmTransaction = "CONFIRM_TRANSACTION";
}

public static class DateTimeFormat
{
    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public static readonly string[] DateFormats = new string[]
    {
        "dd/MM/yyyy",
        "MM/dd/yyyy",
        "yyyy-MM-dd",
        "dd-MM-yyyy",
        "MM-dd-yyyy",
        "yyyy/MM/dd",
        "dd MMM yyyy",
        "MMM dd, yyyy",
        "MMMM dd, yyyy",
        "dd MMMM yyyy",
        "yyyyMMdd",
        "yyyy-MM-ddTHH:mm:ss",
        "yyyy-MM-ddTHH:mm:ssZ",
        "yyyy-MM-ddTHH:mm:ss.fff",
        "yyyy-MM-ddTHH:mm:ss.fffZ",
        "HH:mm:ss",
        "HH:mm",
        "h:mm tt",
        "hh:mm tt",
        "h:mm:ss tt",
        "hh:mm:ss tt",
        "yyyy-MM-dd HH:mm:ss",
        "dd/MM/yyyy HH:mm:ss",
        "MM/dd/yyyy HH:mm:ss",
        "yyyy/MM/dd HH:mm:ss",
        "dd-MM-yyyy HH:mm:ss",
        "MM-dd-yyyy HH:mm:ss",
        "yyyyMMddHHmmss",
        "yyyyMMddTHHmmss",
        "dd MMM yyyy HH:mm:ss",
        "MMM dd, yyyy HH:mm:ss",
        "MMMM dd, yyyy HH:mm:ss",
        "dd MMMM yyyy HH:mm:ss",
        "dd/MM/yy H:mm:ss",
        "MM/dd/yy H:mm:ss",
        "yy-MM-dd H:mm:ss",
        "dd-MM-yy H:mm:ss",
        "MM-dd-yy H:mm:ss",
        "yy/MM/dd H:mm:ss",
        "dd MMM yy H:mm:ss",
        "MMM dd, yy H:mm:ss",
        "MMMM dd, yy H:mm:ss",
        "dd MMMM yy H:mm:ss",
        "yyMMdd H:mm:ss",
        "yy-MM-ddTHH:mm:ss",
        "yy-MM-ddTHH:mm:ssZ",
        "yy-MM-ddTHH:mm:ss.fff",
        "yy-MM-ddTHH:mm:ss.fffZ",
        "yy/MM/dd HH:mm:ss",
        "dd-MM-yy HH:mm:ss",
        "MM-dd-yy HH:mm:ss",
        "yyMMddHHmmss",
        "yyMMddTHHmmss",
        "dd MMM yy HH:mm:ss",
        "MMM dd, yy HH:mm:ss",
        "MMMM dd, yy HH:mm:ss",
        "dd MMMM yy HH:mm:ss",
    };
}

using Jits.Neptune.Web.CMS.LogicOptimal9.Common;
using O24OpenAPI.Web.CMS.Models.O9;

namespace O24OpenAPI.Web.CMS.Utils.Common;

public class GlobalVariable
{
    /// <summary>
    ///
    /// </summary>
    public static string O9_GLOBAL_TABLE_DATA = "O9DATA";

    /// <summary>
    ///
    /// </summary>
    public static char O9_GLOBAL_BANK_ISONLINE;

    /// <summary>
    ///
    /// </summary>
    public static string O9_GLOBAL_IP_ADDRESS = "";

    /// <summary>
    ///
    /// </summary>
    public static string O9_GLOBAL_COMPUTER_NAME = "";

    /// <summary>
    ///
    /// </summary>
    public static string O9_GLOBAL_MAC_ADDRESS = "";

    /// <summary>
    ///
    /// </summary>
    public static string O9_GLOBAL_SERIAL_NUMBERID = "";

    /// <summary>
    ///
    /// </summary>
    public static string SecretToken = "";

    /// <summary>
    ///
    /// </summary>
    public static bool CheckSecretToken = false;

    /// <summary>
    ///
    /// </summary>
    public static List<JsonTxDef> O9_GLOBAL_ALL_TXDEF_LIST = null;

    /// <summary>
    ///
    /// </summary>
    public static HeadOfficeParam O9_GLOBAL_HEADOFFICE_PARAM = null;

    /// <summary>
    ///
    /// </summary>
    public static BranchParam O9_GLOBAL_BRANCH_PARAM = null;

    /// <summary>
    ///
    /// </summary>
    public static UserParam O9_GLOBAL_USER_PARAM = null;

    /// <summary>
    ///
    /// </summary>
    public static ErrorInstance O9_GLOBAL_ERROR = null;

    /// <summary>
    ///
    /// </summary>
    public static MemcachedKey O9_GLOBAL_MEMCACHED_KEY = null;

    /// <summary>
    ///
    /// </summary>
    public static string O9_GLOBAL_SEPARATE_ADDRESS = "###";

    /// <summary>
    ///
    /// </summary>
    public static string O9_GLOBAL_REPORT_URL = "";

    /// <summary>
    ///
    /// </summary>
    public static string OP_CLIENT_REPORT_USER = "";

    /// <summary>
    ///
    /// </summary>
    public static string OP_CLIENT_USE_KEYBOARD = "";

    /// <summary>
    ///
    /// </summary>
    public static string OP_CLIENT_USE_LANGUAGE = "";

    /// <summary>
    ///
    /// </summary>
    public static bool OP_CLIENT_TEST_PERFORMANCE = false;

    /// <summary>
    ///
    /// </summary>
    public static string OP_CLIENT_GET_ROLE_RIGHT_MEMCACHED = "";

    /// <summary>
    ///
    /// </summary>
    public static string OP_CLIENT_ONLY_GET_TXIFC_FROM_MEMCACHED = "";

    /// <summary>
    ///
    /// </summary>
    public static string OP_CLIENT_REPORT_PASSWORD = "";

    /// <summary>
    ///
    /// </summary>
    public static string O9_GLOBAL_COMCODE = "O9";

    /// <summary>
    ///
    /// </summary>
    public static string O9_GLOBAL_COMTYPE = "";

    /// <summary>
    ///
    /// </summary>
    public static bool O9_GLOBAL_IS_GET_PARAM = false;

    /// <summary>
    ///
    /// </summary>
    public static int O9_PAGE_SIZE = 20;

    /// <summary>
    ///
    /// </summary>
    public static Dictionary<int, int> O9_GLOBAL_BRANCHID_USRID = new Dictionary<int, int>();

    /// <summary>
    ///
    /// </summary>
    public static int LOGEXPIRES = 30;

    /// <summary>
    ///
    /// </summary>
    public static string FOFormatDatime = "yyyy-MM-dd";

    /// <summary>
    ///
    /// </summary>
    public static bool IsSetDefaultValue = true;

    /// <summary>
    ///
    /// </summary>
    public static bool IsCheckKey = true;

    /// <summary>
    /// global variable for all setting
    /// </summary>
    public static string O9_GLOBAL_COREAPILB = "";

    /// <summary>
    ///
    /// </summary>
    public static string CoreMode { get; set; } = "";

    /// <summary>
    ///
    /// </summary>
    public static string Optimal9 = "Optimal9";

    /// <summary>
    ///
    /// </summary>
    public static string Neptune = "Neptune";

    /// <summary>
    ///
    /// </summary>
    public static Optimal9Settings Optimal9Settings = null;

    /// <summary>
    ///
    /// </summary>
    public static int O9_PERIOD_LOGIN = 600;

    /// <summary>
    ///
    /// </summary>
    public static string O9_GLOBAL_TXDT = "";

    /// <summary>
    ///
    /// </summary>
    public static string O9_GLOBAL_LANG = "en";

    /// <summary>
    ///
    /// </summary>
    public static bool IsUsingJWT = false;

    /// <summary>
    ///
    /// </summary>
    public static bool IsUsingLicense = false;

    /// <summary>
    /// App config
    /// </summary>
    public static int TIME_UPDATE_TXDT = 30; // 30S

    /// <summary>
    /// Date format
    /// </summary>
    public static string FORMAT_SHORT_DATE = "dd/MM/yyyy";

    /// <summary>
    /// Date format
    /// </summary>
    public static string FORMAT_LONG_DATE = "dd/MM/yyyy hh:mm:ss";

    /// <summary>
    /// call procedure
    /// </summary>
    public static string UTIL_CALL_PROC = "UTIL_CALL_PROC";

    /// <summary>
    /// call function
    /// </summary>
    public static string UTIL_CALL_FUNC = "UTIL_CALL_FUNC";

    /// <summary>
    /// Get working date
    /// </summary>
    public static string UTIL_GET_BUSDATE = "UTIL_GET_BUSDATE";

    /// <summary>
    /// call login
    /// </summary>
    public static string UMG_LOGIN = "UMG_LOGIN";

    /// <summary>
    /// Database using Oracle
    /// </summary>
    public const string Oracle = "Oracle";

    /// <summary>
    /// Database using MySql
    /// </summary>
    public const string MySql = "MySql";

    /// <summary>
    /// Database using SqlLite
    /// </summary>
    public const string SqlLite = "SqlLite";

    /// <summary>
    /// Database using MSSQL
    /// </summary>
    public const string MSSQL = "MSSQL";

    /// <summary>
    /// Database using PostPressSql
    /// </summary>
    public const string PostPressSql = "PostPressSql";

    /// <summary>
    /// Database using MemoryDB
    /// </summary>
    public const string MemoryDB = "MemoryDB";

    /// <summary>
    ///
    /// </summary>
    public static string WorkingDate = "WORKINGDATE";

    /// <summary>
    ///
    /// </summary>
    public static string LoginName = "USERNAME";

    /// <summary>
    ///
    /// </summary>
    public static string Session = "SESSIONID";

    /// <summary>
    ///
    /// </summary>
    public static string BranchCode = "BRANCHCODE";

    /// <summary>
    ///
    /// </summary>
    public static string Token = "TOKEN";

    /// <summary>
    ///
    /// </summary>
    public static List<string> DatetimeFormat = new List<string>
    {
        "dd/MM/yyyy",
        "dd/M/yyyy",
        "d/MM/yyyy",
        "d/M/yyyy",
        "dd/MM/yyyy HH:mm:ss",
        "dd/MM/yyyy HH:mm",
        "MM/dd/yyyy",
        "yyyy/MM/dd",
        "yyyy-MM-dd'T'",
        "dd-MM-yyyy",
        "MM-dd-yyyy",
        "yyyy-MM-dd",
        "yyyy-MM-dd'Z'",
        "dd/MM/yyyy hh:mm:ss tt",
        "yyyy-MM-dd'T'HH:mm:ss.ff",
        "yyyy-MM-dd'T'HH:mm:ss",
        "yyyy-MM-dd'T'HH:mm:ss.fff'Z'",
        "yyyy-MM-dd'T'HH:mm:ss",
        "d/M/YYYY H:m:s tt",
        "ddd MMM dd yyyy HH:mm:ss 'GMT'K '(Indochina Time)'",
    };

    /// <summary>
    ///
    /// </summary>
    public static DateTimeOffset ExpireTime()
    {
        return DateTimeOffset.Now.AddDays(32400 / 24 / 60);
    }
}

namespace O24OpenAPI.Web.CMS.Models.O9;

public class CoreConfig
{
    /// <summary>
    ///
    /// </summary>
    public int WKDTimes { get; set; } = 30;

    /// <summary>
    ///
    /// </summary>
    public string CoreName { get; set; } = "OracleLinux";

    /// <summary>
    ///
    /// </summary>
    public string CoreIP { get; set; } = "127.0.0.1";

    /// <summary>
    ///
    /// </summary>
    public string CoreMac { get; set; } = "JITS";

    /// <summary>
    ///
    /// </summary>
    public string Serial { get; set; } = "Serial1";

    /// <summary>
    ///
    /// </summary>
    public string GetReplyString(string replyname, string seperator, int queues)
    {
        return replyname
            + Serial
            + seperator
            + CoreName
            + seperator
            + CoreMac
            + seperator
            + CoreIP
            + seperator
            + GlobalVariable.O9_GLOBAL_COREAPILB
            + queues;
    }
}

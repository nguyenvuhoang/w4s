namespace O24OpenAPI.Web.CMS.Models.Request;

public partial class MailConfigSearchModel : BaseTransactionModel
{
    /// <summary>
    ///
    /// </summary>
    public MailConfigSearchModel() { }
    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string ConfigId { get; set; }
    /// <summary>
    /// Host
    /// </summary>
    public string Host { get; set; }
    /// <summary>
    /// Port
    /// </summary>
    public int Port { get; set; }
    /// <summary>
    /// Sender
    /// </summary>
    public string Sender { get; set; }
    /// <summary>
    /// Password
    /// </summary>
    public string Password { get; set; }
    /// <summary>
    /// EnableTLS
    /// </summary>
    public bool EnableTLS { get; set; }
    /// <summary>
    /// EmailTest
    /// </summary>
    public string EmailTest { get; set; }
    /// <summary>
    /// PageIndex
    /// </summary>
    public int PageIndex { get; set; } = 0;

    /// <summary>
    /// PageSize
    /// </summary>
    public int PageSize { get; set; } = int.MaxValue;
}

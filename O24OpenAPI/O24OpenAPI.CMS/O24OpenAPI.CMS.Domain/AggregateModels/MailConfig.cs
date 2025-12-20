namespace O24OpenAPI.CMS.Domain.AggregateModels;

public partial class MailConfig : BaseEntity
{
    /// <summary>
    ///
    /// </summary>
    public MailConfig() { }

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
}

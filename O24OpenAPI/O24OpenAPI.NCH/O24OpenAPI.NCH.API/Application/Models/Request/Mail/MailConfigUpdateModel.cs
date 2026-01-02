using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.NCH.API.Application.Models.Request.Mail;

public partial class MailConfigUpdateModel : BaseTransactionModel
{
    /// <summary>
    /// Constructor
    /// </summary>
    public MailConfigUpdateModel() { }

    public int Id { get; set; }

    /// <summary>
    /// ConfigId
    /// </summary>
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

using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.O24NCH.Models.Response;

public partial class MailConfigResponse : BaseO24OpenAPIModel
{
    public MailConfigResponse() { }

    public int Id { get; set; }
    public string ConfigId { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
    public string Sender { get; set; }
    public string Password { get; set; }
    public bool EnableTLS { get; set; }
    public string EmailTest { get; set; }
}

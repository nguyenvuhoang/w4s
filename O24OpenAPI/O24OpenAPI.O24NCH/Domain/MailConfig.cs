using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.O24NCH.Domain;

public class MailConfig : BaseEntity
{
    public string ConfigId { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
    public string Sender { get; set; }
    public string Password { get; set; }
    public bool EnableTLS { get; set; }
    public string EmailTest { get; set; }


    public MailConfig() { }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="configId"></param>
    /// <param name="host"></param>
    /// <param name="port"></param>
    /// <param name="sender"></param>
    /// <param name="password"></param>
    /// <param name="enableTLS"></param>
    /// <param name="emailTest"></param>
    public MailConfig(string configId, string host, int port, string sender, string password, bool enableTLS, string emailTest)
    {
        this.ConfigId = configId;
        this.Host = host;
        this.Port = port;
        this.Sender = sender;
        this.Password = password;
        this.EnableTLS = enableTLS;
        this.EmailTest = emailTest;
    }
}

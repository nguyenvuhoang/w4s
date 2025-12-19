using O24OpenAPI.Core.Configuration;

namespace O24OpenAPI.CMS.Infrastructure.Configurations;

public class CoreBankingSetting : ISettings
{
    public string StaticTokenPortal { get; set; }
    public string UserNameLoginNeptune { get; set; }
    public string PasswordLoginNeptune { get; set; }
    public string NeptuneURL { get; set; }
}

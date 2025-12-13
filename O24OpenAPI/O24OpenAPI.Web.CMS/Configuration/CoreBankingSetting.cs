using O24OpenAPI.Core.Configuration;

namespace O24OpenAPI.Web.CMS.Configuration;

public class CoreBankingSetting : ISettings
{
    public string StaticTokenPortal { get; set; }
    public string UserNameLoginNeptune { get; set; }
    public string PasswordLoginNeptune { get; set; }
    public string NeptuneURL { get; set; }
}

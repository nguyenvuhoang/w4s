using O24OpenAPI.APIContracts.Models.DTS;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.ControlHub.Models.Roles;

public class ResetPasswordResponseModel : BaseO24OpenAPIModel
{
    public string UserCode { get; set; }
    public string PhoneNumber { get; set; }
    public string TemplateCode { get; set; }
    public string NotificationType { get; set; }
    public string Email { get; set; }
    public Dictionary<string, object> SmsData { get; set; }
    public Dictionary<string, object> EmailDataTemplate { get; set; }
    public List<DTSMimeEntityModel> MimeEntities { get; set; }
}

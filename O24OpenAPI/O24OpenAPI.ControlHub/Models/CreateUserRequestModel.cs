using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.ControlHub.Models;

public class CreateUserRequestModel : BaseTransactionModel
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string LoginName { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Phone { get; set; }
    public int Gender { get; set; }
    public string Birthday { get; set; } = string.Empty;
    public string Address { get; set; }
    public string Email { get; set; }
    public string UserCreated { get; set; }
    public string UserLevel { get; set; }
    public int PolicyId { get; set; }
    public List<string> UserGroup { get; set; }
    public string ContractNumber { get; set; } = string.Empty;
    public string UserChannelId { get; set; } = string.Empty;
    public string RoleChannel { get; set; } = string.Empty;
    public string NotificationType { get; set; } = "MAIL";
    public string ContractType { get; set; } = string.Empty;
    public string UserType { get; set; } = "0502";
}

using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

namespace O24OpenAPI.CTH.API.Application.Models.Roles;

public class UserCommandResponseModel
{
    public string ParentId { get; set; }
    public string CommandId { get; set; }
    public string Label { get; set; }
    public string CommandType { get; set; }
    public string Href { get; set; }
    public int RoleId { get; set; }
    public string RoleName { get; set; }
    public string Invoke { get; set; }
    public string Approve { get; set; }
    public string Icon { get; set; }
    public string GroupMenuVisible { get; set; }
    public string Prefix { get; set; }
    public string GroupMenuListAuthorizeForm { get; set; }
    public string CommandNameLanguage { get; set; } = string.Empty;
    public string GroupMenuId { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool IsAgreement { get; set; } = false;
    public List<UserCommandResponseModel> Children { get; set; }
    /// <summary>
    /// Constructor for UserCommandResponseModel
    /// </summary>
    public UserCommandResponseModel()
    {

    }

    public UserCommandResponseModel(
        UserCommand userCommand
    )
    {
        ParentId = userCommand.ParentId;
        CommandId = userCommand.CommandId;
        Label = userCommand.CommandName;
        CommandType = userCommand.CommandType;
        Href = userCommand.CommandURI;
        GroupMenuVisible = userCommand.GroupMenuVisible;
        CommandNameLanguage = userCommand.CommandNameLanguage;
        GroupMenuId = userCommand.GroupMenuId;
        Icon = userCommand.GroupMenuIcon;
        GroupMenuListAuthorizeForm = userCommand.GroupMenuListAuthorizeForm;
    }

}

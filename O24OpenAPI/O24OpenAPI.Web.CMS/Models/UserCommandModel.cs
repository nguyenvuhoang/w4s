using Newtonsoft.Json;

namespace O24OpenAPI.Web.CMS.Models;

public class UserCommandModel : BaseO24OpenAPIModel
{
    [JsonProperty("roleid")]
    public int? RoleId { get; set; }

    [JsonProperty("commandid")]
    public string CommandId { get; set; }

    [JsonProperty("parentid")]
    public string ParentId { get; set; }

    [JsonProperty("commandname")]
    public string CommandName { get; set; }

    [JsonProperty("commandnamelanguage")]
    public string CommandNameLanguage { get; set; }

    [JsonProperty("enabled")]
    public bool Enabled { get; set; }

    [JsonProperty("displayorder")]
    public int DisplayOrder { get; set; }

    [JsonProperty("groupmenuicon")]
    public string GroupMenuIcon { get; set; }

    [JsonProperty("groupmenuvisible")]
    public string GroupMenuVisible { get; set; }

    [JsonProperty("groupmenuid")]
    public string GroupMenuId { get; set; }

    [JsonProperty("invoke")]
    public int? Invoke { get; set; }
}

public class UserCommandResponse : BaseO24OpenAPIModel
{
    public UserCommandResponse() { }

    public UserCommandResponse(UserCommand userCommand)
    {
        ApplicationCode = userCommand.ApplicationCode;
        CommandId = userCommand.CommandId;
        ParentId = userCommand.ParentId;
        CommandName = userCommand.CommandName;
        CommandNameLanguage = userCommand.CommandNameLanguage;
        CommandType = userCommand.CommandType;
        CommandURI = userCommand.CommandURI;
        Enabled = userCommand.Enabled;
        IsVisible = userCommand.IsVisible;
        DisplayOrder = userCommand.DisplayOrder;
        GroupMenuIcon = userCommand.GroupMenuIcon;
        GroupMenuVisible = userCommand.GroupMenuVisible;
        GroupMenuId = userCommand.GroupMenuId;
        GroupMenuListAuthorizeForm = userCommand.GroupMenuListAuthorizeForm;
    }

    [JsonProperty("application_code")]
    public string ApplicationCode { get; set; }

    [JsonProperty("command_id")]
    public string CommandId { get; set; }

    [JsonProperty("parent_id")]
    public string ParentId { get; set; }

    [JsonProperty("command_name")]
    public string CommandName { get; set; }

    [JsonProperty("command_name_language")]
    public string CommandNameLanguage { get; set; }

    [JsonProperty("command_type")]
    public string CommandType { get; set; }

    [JsonProperty("command_uri")]
    public string CommandURI { get; set; }

    [JsonProperty("enabled")]
    public bool Enabled { get; set; }

    [JsonProperty("is_visible")]
    public bool IsVisible { get; set; }

    [JsonProperty("display_order")]
    public int DisplayOrder { get; set; }

    [JsonProperty("group_menu_icon")]
    public string GroupMenuIcon { get; set; }

    [JsonProperty("group_menu_visible")]
    public string GroupMenuVisible { get; set; }

    [JsonProperty("group_menu_id")]
    public string GroupMenuId { get; set; }

    [JsonProperty("group_menu_list_authorize_form")]
    public string GroupMenuListAuthorizeForm { get; set; }
}

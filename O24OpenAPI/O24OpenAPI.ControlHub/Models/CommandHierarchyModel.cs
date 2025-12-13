namespace O24OpenAPI.ControlHub.Models;

public class CommandHierarchyModel
{
    public string ParentId { get; set; }
    public string CommandId { get; set; }
    public string Label { get; set; }
    public string CommandType { get; set; }
    public string CommandUri { get; set; }
    public int RoleId { get; set; }
    public string RoleName { get; set; }
    public bool Invoke { get; set; }
    public bool Approve { get; set; }
    public string Icon { get; set; }
    public string GroupMenuVisible { get; set; }
    public string GroupMenuId { get; set; }
    public string GroupMenuListAuthorizeForm { get; set; }
    public bool IsAgreement { get; set; } = false;

    public List<CommandHierarchyModel> Children { get; set; }
}

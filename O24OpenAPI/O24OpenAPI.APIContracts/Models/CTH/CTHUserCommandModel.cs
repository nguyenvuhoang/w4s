namespace O24OpenAPI.APIContracts.Models.CTH;

public class CTHUserCommandModel
{
    public string ApplicationCode { get; set; }
    public string CommandId { get; set; }
    public string ParentId { get; set; }
    public string CommandName { get; set; }
    public string CommandNameLanguage { get; set; }
    public string CommandType { get; set; }
    public string CommandURI { get; set; } = string.Empty;
    public bool? Enabled { get; set; } = false;
    public bool? IsVisible { get; set; } = false;
    public int DisplayOrder { get; set; }
    public string GroupMenuIcon { get; set; } = string.Empty;
    public string GroupMenuVisible { get; set; } = string.Empty;
    public string GroupMenuId { get; set; } = string.Empty;
    public string GroupMenuListAuthorizeForm { get; set; } = string.Empty;
}

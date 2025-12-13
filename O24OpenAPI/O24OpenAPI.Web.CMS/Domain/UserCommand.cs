namespace O24OpenAPI.Web.CMS.Domain;

public class UserCommand : BaseEntity
{
    public string ApplicationCode { get; set; }
    public string CommandId { get; set; }
    public string ParentId { get; set; }
    public string CommandName { get; set; }
    public string CommandNameLanguage { get; set; }
    public string CommandType { get; set; }
    public string CommandURI { get; set; }
    public bool Enabled { get; set; } = false;
    public bool IsVisible { get; set; } = false;
    public int DisplayOrder { get; set; }
    public string GroupMenuIcon { get; set; } = string.Empty;
    public string GroupMenuVisible { get; set; }
    public string GroupMenuId { get; set; } = string.Empty;
    public string GroupMenuListAuthorizeForm { get; set; } = string.Empty;
    public DateTime? CreatedOnUtc { get; set; }
    public DateTime? UpdatedOnUtc { get; set; }
}

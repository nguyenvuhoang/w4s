using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

public partial class UserCommand : BaseEntity
{
    public string ApplicationCode { get; set; }
    public string CommandId { get; set; }
    public string ParentId { get; set; }
    public string CommandName { get; set; }
    public string CommandNameLanguage { get; set; }
    public string CommandType { get; set; }
    public string CommandURI { get; set; }
    public bool Enabled { get; set; }
    public bool IsVisible { get; set; }
    public int DisplayOrder { get; set; }
    public string GroupMenuIcon { get; set; }
    public string GroupMenuVisible { get; set; }
    public string GroupMenuId { get; set; }
    public string GroupMenuListAuthorizeForm { get; set; }
    public DateTime? CreatedOnUtc { get; set; }
    public DateTime? UpdatedOnUtc { get; set; }
}

using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

public partial class RoleType : BaseEntity
{
    public string RoleTypeCode { get; set; } = string.Empty;
    public string RoleTypeName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public string ServiceID { get; set; }
    public bool IsActive { get; set; } = true;
}

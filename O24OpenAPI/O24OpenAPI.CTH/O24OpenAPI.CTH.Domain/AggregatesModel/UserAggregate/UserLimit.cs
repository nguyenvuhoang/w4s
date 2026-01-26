using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

[Auditable]
public partial class UserLimit : BaseEntity
{
    public int RoleId { get; set; }
    public string CommandId { get; set; } = null!;
    public string CurrencyCode { get; set; } = null!;
    public decimal? ULimit { get; set; }
    public string? CvTable { get; set; }
    public string LimitType { get; set; } = null!;
    public int? Margin { get; set; }
}

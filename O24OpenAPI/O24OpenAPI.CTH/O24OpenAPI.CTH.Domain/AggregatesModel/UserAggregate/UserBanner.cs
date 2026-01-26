using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

[Auditable]
public partial class UserBanner : BaseEntity
{
    public string? UserCode { get; set; }
    public string? BannerSource { get; set; }
    public DateTime? CreatedOnUTC { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedOnUTC { get; set; }
}

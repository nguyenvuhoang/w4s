using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

public partial class UserBanner : BaseEntity
{
    public string UserCode { get; set; }
    public string BannerSource { get; set; }
    public DateTime? CreatedOnUTC { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedOnUTC { get; set; }
}

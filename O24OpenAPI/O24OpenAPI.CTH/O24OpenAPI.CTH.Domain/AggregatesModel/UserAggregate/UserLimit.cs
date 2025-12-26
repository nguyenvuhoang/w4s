using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

public partial class UserLimit : BaseEntity
{
    public int RoleId { get; set; }
    public string CommandId { get; set; } = null!;
    public string CurrencyCode { get; set; } = null!;
    public decimal? ULimit { get; set; }
    public string CvTable { get; set; }
    public string LimitType { get; set; } = null!;
    public int? Margin { get; set; }
    public DateTime? UpdatedOnUtc { get; set; }
    public DateTime? CreatedOnUtc { get; set; }
}

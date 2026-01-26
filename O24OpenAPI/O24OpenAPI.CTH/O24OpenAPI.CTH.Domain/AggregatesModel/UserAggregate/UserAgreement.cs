using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

[Auditable]
public partial class UserAgreement : BaseEntity
{
    public string? AgreementNumber { get; set; }
    public string? AgreementType { get; set; }
    public string? TransactionCode { get; set; }
    public string? Content { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }
    public DateTime? CreatedUTC { get; set; }
    public DateTime? ModifiedUTC { get; set; }
}

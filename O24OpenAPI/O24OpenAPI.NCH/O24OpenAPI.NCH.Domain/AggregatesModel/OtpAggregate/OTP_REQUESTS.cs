using O24OpenAPI.Core.Domain;
using O24OpenAPI.NCH.Domain.Constants;

namespace O24OpenAPI.NCH.Domain.AggregatesModel.OtpAggregate;

public partial class OTP_REQUESTS : BaseEntity
{
    public string? UserCode { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string OtpCode { get; set; } = string.Empty;
    public string Purpose { get; set; } = string.Empty;
    public bool IsUsed { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ExpiresAt { get; set; }
    public string? TransactionId { get; set; }
    public string Status { get; set; } = OTP_REQUESTS_STATUS.PENDING;
}

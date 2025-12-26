using O24OpenAPI.Core.Domain;
using O24OpenAPI.O24NCH.Constant;

namespace O24OpenAPI.O24NCH.Domain;

public partial class StoreOtp : BaseEntity
{
    public string PhoneNumber { get; set; }
    public string OtpHash { get; set; }
    public string OtpSalt { get; set; } = string.Empty;
    public ReviewPlatform Platform { get; set; } = ReviewPlatform.Any;

    // Hiệu lực/giới hạn
    public DateTime? StartAt { get; set; }
    public DateTime? EndAt { get; set; }
    public bool IsActive { get; set; } = true;
    public int? MaxUses { get; set; }
    public int UsedCount { get; set; } = 0;
    public string Note { get; set; } = string.Empty;
    public DateTime CreatedOnUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedOnUtc { get; set; }
}

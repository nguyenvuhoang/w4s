using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.O24NCH.Domain;

public class SMSMappingResponse : BaseEntity
{
    /// <summary>
    /// Tên provider, ví dụ: UNITEL, ETL, LTC
    /// </summary>
    public string ProviderName { get; set; } = string.Empty;

    /// <summary>
    /// Mã phản hồi từ provider
    /// </summary>
    public string ResponseCode { get; set; } = string.Empty;

    /// <summary>
    /// Mô tả chi tiết của mã phản hồi
    /// </summary>
    public string ResponseDescription { get; set; } = string.Empty;

    /// <summary>
    /// Có phải mã phản hồi thành công không?
    /// </summary>
    public bool IsSuccess { get; set; } = false;

    /// <summary>
    /// Mẫu thông báo có thể dùng hiển thị/log tùy provider
    /// </summary>
    public string MsgTemplate { get; set; } = string.Empty;

    /// <summary>
    /// Loại dịch vụ (SendSMS, CheckStatus, v.v.)
    /// </summary>
    public string ServiceType { get; set; } = string.Empty;

    /// <summary>
    /// Is this a fallback mapping?
    /// </summary>
    public bool IsFallback { get; set; } = false;
    /// <summary>
    /// Is this a retry mapping?
    /// </summary>
    public bool IsRetry { get; set; } = false;


    /// <summary>
    /// Ngày tạo
    /// </summary>
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Ngày cập nhật cuối
    /// </summary>
    public DateTime DateUpdated { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Người tạo
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Người cập nhật cuối
    /// </summary>
    public string UpdatedBy { get; set; } = string.Empty;
}

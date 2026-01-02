using O24OpenAPI.Core.Domain;
using O24OpenAPI.NCH.Domain.Constants;

namespace O24OpenAPI.NCH.Domain.AggregatesModel.SmsAggregate;

public partial class SMSSendOut : BaseEntity
{
    public string EndToEnd { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string MessageContent { get; set; } = string.Empty;
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = SMSSendOutStatus.PENDING;
    public string FullRequest { get; set; } = string.Empty;
    public string RequestHeader { get; set; } = string.Empty;
    public string RequestMessage { get; set; } = string.Empty;
    public string ResponseMessage { get; set; } = string.Empty;
    public string? SMSProviderId { get; set; }
    public string? ProviderMsgId { get; set; }
    public string? OtpRequestId { get; set; }
    public int? ElapsedMilliseconds { get; set; }
    public int RetryCount { get; set; } = 0;
    public bool IsResend { get; set; } = false;
    public bool IsFallback { get; set; }
    public string FallbackFromProvider { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;
}

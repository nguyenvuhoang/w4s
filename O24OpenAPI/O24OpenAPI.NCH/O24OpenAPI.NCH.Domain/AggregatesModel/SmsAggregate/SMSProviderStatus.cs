using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.NCH.Domain.AggregatesModel.SmsAggregate;

public partial class SMSProviderStatus : BaseEntity
{
    /// <summary>
    /// Foreign key to SMSProvider
    /// </summary>
    public string ProviderId { get; set; }

    /// <summary>
    /// Time the health check was performed
    /// </summary>
    public DateTime CheckTime { get; set; }

    /// <summary>
    /// Indicates whether the provider is online
    /// </summary>
    public bool IsOnline { get; set; }

    /// <summary>
    /// Response time in milliseconds
    /// </summary>
    public int ResponseTimeMs { get; set; } = 0;

    /// <summary>
    /// Response message
    /// </summary>
    public string ResponseMessage { get; set; } = string.Empty;

    /// <summary>
    /// Error message if the provider check failed
    /// </summary>
    public string ErrorDetail { get; set; }

    /// <summary>
    /// CreatedOnUtc Date Time
    /// </summary>
    public DateTime CreatedOnUtc { get; set; }

    /// <summary>
    /// UpdateOnUtc Date Time
    /// </summary>
    public DateTime UpdatedOnUtc { get; set; }

    /// <summary>
    /// Navigation property to SMSProvider (optional)
    /// </summary>
    public virtual SMSProvider SMSProvider { get; set; }
}

using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.Logger.Domain;

public class ApplicationLog : BaseEntity
{
    public DateTimeOffset LogTimestamp { get; set; }
    public string? LogLevel { get; set; }
    public string? ServiceName { get; set; }
    public string CorrelationId { get; set; } = string.Empty;
    public string? LogType { get; set; }
    public string? Direction { get; set; }
    public string? ActionName { get; set; }
    public string? Flow { get; set; }
    public long DurationMs { get; set; }
    public string? Message { get; set; }
    public string? Headers { get; set; }
    public string? RequestPayload { get; set; }
    public string? ResponsePayload { get; set; }
    public string? ExceptionDetails { get; set; }
    public string? Properties { get; set; }
    public DateTime? CreatedOnUtc { get; set; }
}

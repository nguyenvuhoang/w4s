namespace O24OpenAPI.CMS.Domain.AggregateModels;

public partial class QRData : BaseEntity
{
    public string? ChannelId { get; set; }
    public string? HashedId { get; set; }
    public string? Data { get; set; }

    public DateTime? ExpirationUtc { get; set; } = DateTime.UtcNow.AddMinutes(60);
}

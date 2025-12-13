namespace O24OpenAPI.Web.CMS.Domain;

public class QRData : BaseEntity
{
    public string ChannelId { get; set; }
    public string HashedId { get; set; }
    public string Data { get; set; }

    public DateTime? ExpirationUtc { get; set; } = DateTime.UtcNow.AddMinutes(60);

    /// <summary>
    /// Gets or sets the value of the updated on utc
    /// </summary>
    public DateTime? UpdatedOnUtc { get; set; }

    /// <summary>
    /// Gets or sets the value of the created on utc
    /// </summary>
    public DateTime? CreatedOnUtc { get; set; }
}

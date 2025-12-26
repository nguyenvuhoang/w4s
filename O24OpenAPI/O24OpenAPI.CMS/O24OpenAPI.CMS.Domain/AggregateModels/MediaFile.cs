namespace O24OpenAPI.CMS.Domain.AggregateModels;

public partial class MediaFile : BaseEntity
{
    public string? TrackerCode { get; set; }
    public string? FileUrl { get; set; }
    public string? FileHash { get; set; }
    public string? Base64String { get; set; }
    public string? FolderName { get; set; }
    public string? FileName { get; set; }
    public string? FileExtension { get; set; }
    public long? FileSize { get; set; }
    public string Status { get; set; } = "ACTIVE";
    public string? CreatedBy { get; set; }
    public DateTime? ExpiredOnUtc { get; set; }
}

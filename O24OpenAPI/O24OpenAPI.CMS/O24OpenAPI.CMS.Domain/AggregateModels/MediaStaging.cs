namespace O24OpenAPI.CMS.Domain;

public class MediaStaging : BaseEntity
{
    public string TrackerCode { get; set; }
    public string FileUrl { get; set; }
    public string Base64String { get; set; }
    public string FolderName { get; set; }
    public string FileName { get; set; }
    public string FileExtension { get; set; }
    public long? FileSize { get; set; }
    public string FileHash { get; set; }
    public string Status { get; set; } = "PENDING"; // PENDING, LINKED, EXPIRED
    public DateTime CreatedOnUtc { get; set; } = DateTime.UtcNow;
    public DateTime? ExpiredOnUtc { get; set; }
    public string CreatedBy { get; set; }
}

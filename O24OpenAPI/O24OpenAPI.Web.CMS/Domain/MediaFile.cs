namespace O24OpenAPI.Web.CMS.Domain;

public class MediaFile : BaseEntity
{
    public string TrackerCode { get; set; }
    public string FileUrl { get; set; }
    public string FileHash { get; set; }
    public string Base64String { get; set; }
    public string FolderName { get; set; }
    public string FileName { get; set; }
    public string FileExtension { get; set; }
    public long? FileSize { get; set; }
    public string Status { get; set; } = "ACTIVE";
    public DateTime CreatedOnUtc { get; set; } = DateTime.UtcNow;
    public DateTime? ExpiredOnUtc { get; set; }
    public string CreatedBy { get; set; }

}

namespace O24OpenAPI.Web.CMS.Models.Media
{
    public class MediaModel
    {
        public string TrackerCode { get; set; }
        public string FileUrl { get; set; }
        public string FileHash { get; set; }
        public string FolderName { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public long? FileSize { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime? ExpiredOnUtc { get; set; }
        public string CreatedBy { get; set; }
        public bool IsTemp { get; set; }
    }
}

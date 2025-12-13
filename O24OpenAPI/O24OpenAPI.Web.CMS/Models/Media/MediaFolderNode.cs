namespace O24OpenAPI.Web.CMS.Models.Media
{
    /// <summary>
    /// Defines the <see cref="MediaFolderNode" />
    /// </summary>
    public class MediaFolderNode
    {
        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Path
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the Folders
        /// </summary>
        public List<MediaFolderNode> Folders { get; set; } = [];

        /// <summary>
        /// Gets or sets the Files
        /// </summary>
        public List<MediaFileItem> Files { get; set; } = [];
    }

}

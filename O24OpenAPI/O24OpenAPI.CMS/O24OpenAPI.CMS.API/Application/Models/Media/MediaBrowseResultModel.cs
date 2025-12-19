namespace O24OpenAPI.CMS.API.Application.Models.Media;

/// <summary>
/// Defines the <see cref="MediaBrowseResultModel" />
/// </summary>
public class MediaBrowseResultModel
{
    /// <summary>
    /// Gets or sets the Category
    /// </summary>
    public string Category { get; set; }

    /// <summary>
    /// Gets or sets the Path
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// Gets or sets the Folders
    /// </summary>
    public List<MediaFolderItem> Folders { get; set; } = new();

    /// <summary>
    /// Gets or sets the Files
    /// </summary>
    public List<MediaFileItem> Files { get; set; } = new();
}

/// <summary>
/// Defines the <see cref="MediaFolderItem" />
/// </summary>
public class MediaFolderItem
{
    /// <summary>
    /// Gets or sets the Name
    /// </summary>
    public string Name { get; set; }
}

/// <summary>
/// Defines the <see cref="MediaFileItem" />
/// </summary>
public class MediaFileItem
{
    /// <summary>
    /// Gets or sets the Name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the S3Key
    /// </summary>
    public string S3Key { get; set; }

    /// <summary>
    /// Gets or sets the Size
    /// </summary>
    public long? Size { get; set; }

    /// <summary>
    /// Gets or sets the LastModified
    /// </summary>
    public DateTime? LastModified { get; set; }

    // Optional: gắn luôn trackerCode để FE dùng /m/{trackerCode}

    /// <summary>
    /// Gets or sets the TrackerCode
    /// </summary>
    public string TrackerCode { get; set; }
}

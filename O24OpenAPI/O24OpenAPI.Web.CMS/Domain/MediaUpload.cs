namespace O24OpenAPI.Web.CMS.Domain;

/// <summary>
/// </summary>
public partial class MediaUpload : BaseEntity
{
    /// <summary>
    ///
    /// </summary>
    public MediaUpload() { }

    /// <summary>
    /// </summary>
    /// <summary>
    /// </summary>
    public int UserId { get; set; } = 0;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string MediaName { get; set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string MediaType { get; set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string MediaData { get; set; } = string.Empty;

    /// <summary>
    ///
    /// </summary> <summary>
    ///
    /// </summary>
    /// <value></value>
    public bool AutoDeleteBySchedule { get; set; } = false;

    /// <summary>
    /// Last update date
    /// </summary>
    public DateTime? UpdatedOnUtc { get; set; }

    /// <summary>
    /// Last update date
    /// </summary>
    public DateTime? CreatedOnUtc { get; set; }
}

using O24OpenAPI.Contracts.Events;

namespace O24OpenAPI.APIContracts.Events;

public class BannerModifyEvent : IntegrationEvent
{
    public string Group { get; set; } = string.Empty;
    public string ImgId { get; set; } = string.Empty;

    /// <summary>
    /// Image source path or URL
    /// </summary>
    public string ImgSource { get; set; } = string.Empty;

    /// <summary>
    /// Banner level (priority/order)
    /// </summary>
    public int Level { get; set; }

    /// <summary>
    /// Banner status (e.g. ACTIVE/INACTIVE)
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Date the banner was created
    /// </summary>
    public DateTime? DateCreated { get; set; }

    /// <summary>
    /// User who created the banner
    /// </summary>
    public string UserCreated { get; set; } = string.Empty;

    /// <summary>
    /// Date the banner was approved
    /// </summary>
    public DateTime? DateApproved { get; set; }

    /// <summary>
    /// User who approved the banner
    /// </summary>
    public string UserApproved { get; set; } = string.Empty;

    /// <summary>
    /// Date the banner was last modified
    /// </summary>
    public DateTime? DateModified { get; set; }

    /// <summary>
    /// User who last modified the banner
    /// </summary>
    public string UserModified { get; set; } = string.Empty;

    /// <summary>
    /// Service code related to the banner
    /// </summary>
    public string Service { get; set; } = string.Empty;

    /// <summary>
    /// Banner type (e.g. promo, news, etc.)
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Banner channel (e.g. Mobile, etc.)
    /// </summary>
    public string Channel { get; set; } = string.Empty;

    /// <summary>
    /// Banner position (e.g. HomeScreen, etc.)
    /// </summary>
    public string Position { get; set; } = string.Empty;

    /// <summary>
    /// Banner type using (e.g. Theme, Popup, etc.)
    /// </summary>
    public string TypeUsing { get; set; } = string.Empty;

    /// <summary>
    /// Banner order
    /// </summary>
    public int Order { get; set; }

}

using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.Report.Domain.AggregateModels.ViewerSettingAggregate;

public partial class ViewerSetting : BaseEntity
{
    /// <summary>
    ///
    /// </summary>
    public ViewerSetting() { }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string CodeTemplate { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public bool ShowPrintButton { get; set; } = true;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public bool ShowOpenButton { get; set; } = false;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public bool ShowSaveButton { get; set; } = true;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public bool ShowSendEmailButton { get; set; } = false;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public bool ShowDesignButton { get; set; } = false;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public bool ShowResourcesButton { get; set; } = true;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public bool UseCompression { get; set; } = false;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public int CacheTimeout { get; set; } = 60;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public int RequestTimeout { get; set; } = 60;

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string CacheMode { get; set; } = "ObjectCache";

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string OrganizationId { get; set; } = string.Empty;
}

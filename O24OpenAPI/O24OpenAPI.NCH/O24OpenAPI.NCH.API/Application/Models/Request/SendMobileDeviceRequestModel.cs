namespace O24OpenAPI.NCH.API.Application.Models.Request;

using O24OpenAPI.Framework.Models;

/// <summary>
/// Defines the <see cref="SendMobileDeviceRequestModel" />
/// </summary>
public class SendMobileDeviceRequestModel : BaseTransactionModel
{
    public string UserCode { get; set; }

    /// <summary>
    /// Gets or sets the PushId
    /// </summary>
    public string PushId { get; set; }

    /// <summary>
    /// Gets or sets the Message
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Gets or sets the Title
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Image Url
    /// </summary>
    public string ImageUrl { get; set; } = string.Empty;

    /// <summary>
    /// Template ID
    /// </summary>
    public string TemplateId { get; set; } = string.Empty;
}

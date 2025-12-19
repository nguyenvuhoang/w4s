namespace O24OpenAPI.CMS.API.Application.Models.Request;

/// <summary>
/// Defines the <see cref="SignalRSendModel" />
/// </summary>
public class SignalRSendModel : BaseTransactionModel
{
    /// <summary>
    /// Gets or sets the TransactionCode
    /// </summary>
    public new string TransactionCode { get; set; }

    /// <summary>
    /// Gets or sets the Userid
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the DeviceId
    /// </summary>
    public string DeviceId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the Data
    /// </summary>
    public Dictionary<string, object> Data { get; set; } = [];
}

using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.Logger.Models.ChannelLog;

/// <summary>
/// The channel log create model class
/// </summary>
/// <seealso cref="BaseO24OpenAPIModel"/>
public class ChannelLogCreateModel : BaseO24OpenAPIModel
{
    /// <summary>
    /// Gets or sets the value of the log level id
    /// </summary>
    /// public int LogLevelId { get; set; }

    /// <summary>
    /// Gets or sets the value of the channel id
    /// </summary>
    public string ChannelId { get; set; }

    /// <summary>
    /// Gets or sets the value of the short message
    /// </summary>
    public string ShortMessage { get; set; }

    /// <summary>
    /// Gets or sets the value of the full message
    /// </summary>
    public string FullMessage { get; set; }

    /// <summary>
    /// Gets or sets the value of the user id
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// Gets or sets the value of the reference
    /// </summary>
    public string Reference { get; set; }
}

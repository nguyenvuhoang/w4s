using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.O24ACT.Models.ChannelLog;

public class ChannelLogCreateModel : BaseO24OpenAPIModel
{
    public int LogLevelId { get; set; }
    public string ChannelId { get; set; }
    public string ShortMessage { get; set; }
    public string FullMessage { get; set; }
    public string UserId { get; set; }
    public string Reference { get; set; }
}

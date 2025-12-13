using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.ControlHub.Models.Channel;

public class UpdateChannelModel : BaseTransactionModel
{
    public bool IsOpen { get; set; }
    public string ChannelAction { get; set; }
}

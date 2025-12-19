using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.ControlHub.Models.Channel;

public class CanLoginChannelModel : BaseTransactionModel
{
    public string UserId { get; set; }
}

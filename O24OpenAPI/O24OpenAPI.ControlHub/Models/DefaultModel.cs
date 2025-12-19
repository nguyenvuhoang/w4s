using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.ControlHub.Models;

public class DefaultModel : BaseTransactionModel
{
    public string UserCode { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string DeviceId { get; set; }
}

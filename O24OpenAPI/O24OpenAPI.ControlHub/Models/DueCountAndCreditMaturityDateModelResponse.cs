using O24OpenAPI.Core.Abstractions;

namespace O24OpenAPI.ControlHub.Models;

public class DueCountAndCreditMaturityDateModelResponse : BaseO24OpenAPIModel
{
    /// <summary>
    /// date
    /// </summary>
    public DateTime CreditMaturityDate { get; set; }

    /// <summary>
    /// DueCount
    /// </summary>
    public int DueCount { get; set; }
}

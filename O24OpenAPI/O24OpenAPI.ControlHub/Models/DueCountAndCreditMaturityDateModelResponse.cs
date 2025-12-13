using O24OpenAPI.Web.Framework.Models;

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

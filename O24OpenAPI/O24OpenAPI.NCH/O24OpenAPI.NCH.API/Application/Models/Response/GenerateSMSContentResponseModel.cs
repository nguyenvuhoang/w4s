using O24OpenAPI.Core.Abstractions;

namespace O24OpenAPI.NCH.Models.Response;

public class GenerateSMSContentResponseModel : BaseO24OpenAPIModel
{
    public string TransactionId { get; set; }
}

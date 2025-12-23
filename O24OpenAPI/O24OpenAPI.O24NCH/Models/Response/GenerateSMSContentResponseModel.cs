using O24OpenAPI.Core.Abstractions;

namespace O24OpenAPI.O24NCH.Models.Response;

public class GenerateSMSContentResponseModel : BaseO24OpenAPIModel
{
    public string TransactionId { get; set; }
}

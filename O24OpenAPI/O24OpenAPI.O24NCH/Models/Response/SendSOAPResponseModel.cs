using O24OpenAPI.Core.Abstractions;

namespace O24OpenAPI.O24NCH.Models.Response;

public class SendSOAPResponseModel : BaseO24OpenAPIModel
{
    public string TransactionId { get; set; }
    public string MessageId { get; set; }
    public int ErrorCode { get; set; }
    public string ErrorMessage { get; set; }
    public string RawCode { get; set; }
    public string ProviderKey { get; set; }
    public string RawResponseCode { get; set; }
    public bool IsSuccess { get; set; }
}

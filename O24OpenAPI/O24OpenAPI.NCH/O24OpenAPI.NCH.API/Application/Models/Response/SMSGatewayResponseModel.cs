using O24OpenAPI.Core.Abstractions;

namespace O24OpenAPI.NCH.API.Application.Models.Response;

public class SMSGatewayResponseModel : BaseO24OpenAPIModel
{
    public string PhoneNumber { get; set; }
    public string Message { get; set; }
    public string TransactionId { get; set; }
    public string Provider { get; set; }
    public bool IsSuccess { get; set; }
    public int ErrorCode { get; set; }
    public string ErrorMessage { get; set; }
    public string SMSGWTransactionId { get; set; }
}

using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.O24NCH.Models.Response;

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

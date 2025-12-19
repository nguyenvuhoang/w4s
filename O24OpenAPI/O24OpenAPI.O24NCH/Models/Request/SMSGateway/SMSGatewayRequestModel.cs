using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.O24NCH.Models.Request.SMS;

public class SMSGatewayRequestModel : BaseTransactionModel
{
    public string PhoneNumber { get; set; }
    public string Purpose { get; set; }
    public Dictionary<string, object> SenderData { get; set; }
    public string Message { get; set; }
    public string ProviderName { get; set; }
    public string TransactionId { get; set; }
    public string MessageType { get; set; } = "OTP";
}

using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.O24NCH.Models.Request;

public class GenerateOTPRequestModel : BaseTransactionModel
{
    public string UserCode { get; set; } = string.Empty;
    public string PhoneNumber { get; set; }
    public string Purpose { get; set; }
    public string Account { get; set; } = string.Empty;
    public decimal? Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
}

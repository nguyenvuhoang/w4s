using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.O24NCH.Models.Request;

public class VeriveryOTPRequestModel : BaseTransactionModel
{
    public string UserCode { get; set; }
    public string PhoneNumber { get; set; }
    public string Purpose { get; set; }
    public string OTP { get; set; }
    public string VerifyOTPCode { get; set; }
}

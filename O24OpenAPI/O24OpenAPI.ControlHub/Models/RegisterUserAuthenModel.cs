using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.ControlHub.Models;

public class RegisterUserAuthenModel : BaseTransactionModel
{
    public string AuthenType { get; set; }
    public string PhoneNumber { get; set; }
    public string UserCode { get; set; }
    public string SmartOTPCode { get; set; }
    public bool IsValidOTP { get; set; }
}

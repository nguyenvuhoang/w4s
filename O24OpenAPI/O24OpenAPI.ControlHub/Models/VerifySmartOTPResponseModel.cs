namespace O24OpenAPI.ControlHub.Models;

public class VerifySmartOTPResponseModel
{
    public bool IsValid { get; set; }
    public string StoredSecretKey { get; set; }
}

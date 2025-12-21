namespace O24OpenAPI.CTH.API.Application.Models;

public class VerifySmartOTPResponseModel
{
    public bool IsValid { get; set; }
    public string StoredSecretKey { get; set; }
}

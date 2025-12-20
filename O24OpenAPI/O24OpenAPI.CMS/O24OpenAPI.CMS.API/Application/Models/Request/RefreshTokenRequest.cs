namespace O24OpenAPI.CMS.API.Application.Models.Request;

public class RefreshTokenRequest : BaseTransactionModel
{
    public string RefreshToken { get; set; }
}

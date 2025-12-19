namespace O24OpenAPI.CMS.API.Application.Models.OpenAPI;

public class RotateSecretRequestModel : BaseTransactionModel
{
    public int Id { get; set; }
    public string? ClientId { get; set; } = string.Empty;
    public string? ClientSecretDescription { get; set; }
    public DateTime? ClientSecretExpiresOnUtc { get; set; }
}

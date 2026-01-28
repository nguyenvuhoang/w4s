namespace O24OpenAPI.CMS.API.Application.Models.Zalo
{
    public class CreateZaloOATokenModel : BaseO24OpenAPIModel
    {
        public string Code { get; set; } = null!; public string? OaId { get; set; } = default!;
        public string? AppId { get; set; } = default!;
        public string? AccessToken { get; set; } = default!;
        public string? RefreshToken { get; set; } = default!;
        public string? ExpiresIn { get; set; } = default!;
    }
}

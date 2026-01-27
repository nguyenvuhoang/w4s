namespace O24OpenAPI.CMS.API.Application.Features.Zalo
{
    using LinKit.Core.Cqrs;

    public class ZaloExchangeTokenCommand : BaseO24OpenAPIModel, ICommand<bool>
    {
        public string OaId { get; set; } = default!;
        public string Code { get; set; } = default!;
        public string? CodeVerifier { get; set; }
    }

}

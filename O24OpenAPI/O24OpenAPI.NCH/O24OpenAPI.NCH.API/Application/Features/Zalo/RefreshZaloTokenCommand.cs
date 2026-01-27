using LinKit.Core.Cqrs;
using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.NCH.Domain.AggregatesModel.ZaloAggregate;

namespace O24OpenAPI.NCH.API.Application.Features.Zalo
{
    public class RefreshZaloTokenCommand : BaseO24OpenAPIModel, ICommand<ZaloOAToken>
    {
        public string OaId { get; set; } = default!;
    }

    [CqrsHandler]
    public class RefreshZaloTokenHandler(
        IZaloOATokenRepository tokenRepo,
        IHttpClientFactory httpClientFactory
    ) : ICommandHandler<RefreshZaloTokenCommand, ZaloOAToken>
    {
        public async Task<ZaloOAToken> HandleAsync(RefreshZaloTokenCommand request, CancellationToken ct = default)
        {
            var token = await tokenRepo.GetActiveByOaIdAsync(request.OaId, ct)
                ?? throw new InvalidOperationException($"No active Zalo token for OA {request.OaId}");

            if (string.IsNullOrWhiteSpace(token.RefreshToken))
                throw new InvalidOperationException("Missing refresh_token");

            // TODO: Zalo refresh endpoint + form fields theo spec thực tế của bạn
            // var resp = await client.PostAsync("...refresh...", ...)

            // giả lập parse kết quả
            var newAccessToken = "NEW_ACCESS_TOKEN";
            var newRefreshToken = token.RefreshToken; // hoặc refresh mới
            var expiresIn = 90000;
            var expiresAt = DateTime.UtcNow.AddSeconds(expiresIn);

            // rotate token
            await tokenRepo.DeactivateActiveAsync(request.OaId, ct);

            var newToken = new ZaloOAToken
            {
                OaId = token.OaId,
                AppId = token.AppId,
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                ExpiresIn = expiresIn,
                ExpiresAtUtc = expiresAt,
                IsActive = true,
                CreatedOnUtc = DateTime.UtcNow
            };

            await tokenRepo.InsertAsync(newToken);
            return newToken;
        }
    }

}

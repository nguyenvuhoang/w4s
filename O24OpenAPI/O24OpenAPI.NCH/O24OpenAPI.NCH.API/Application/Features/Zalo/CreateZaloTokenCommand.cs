using LinKit.Core.Cqrs;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.NCH.Domain.AggregatesModel.ZaloAggregate;

namespace O24OpenAPI.NCH.API.Application.Features.Zalo;

public class CreateZaloTokenCommand : BaseTransactionModel, ICommand<ZaloOAToken>
{
    public string OaId { get; set; } = default!;
    public string AppId { get; set; } = default!;

    public string AccessToken { get; set; } = default!;
    public string? RefreshToken { get; set; }

    public int ExpiresIn { get; set; }
    public DateTime? ExpiresAtUtc { get; set; }
}

[CqrsHandler]
public class CreateZaloTokenCommandHandler(
    IZaloOATokenRepository zaloOATokenRepository
) : ICommandHandler<CreateZaloTokenCommand, ZaloOAToken>
{
    public async Task<ZaloOAToken> HandleAsync(CreateZaloTokenCommand request, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.OaId))
            throw new InvalidOperationException("OaId is required");
        if (string.IsNullOrWhiteSpace(request.AppId))
            throw new InvalidOperationException("AppId is required");
        if (string.IsNullOrWhiteSpace(request.AccessToken))
            throw new InvalidOperationException("AccessToken is required");
        if (request.ExpiresIn <= 0 && request.ExpiresAtUtc == null)
            throw new InvalidOperationException("ExpiresIn or ExpiresAtUtc is required");

        var now = DateTime.UtcNow;

        var expiresAtUtc = request.ExpiresAtUtc ?? now.AddSeconds(request.ExpiresIn);

        await zaloOATokenRepository.DeactivateActiveAsync(request.OaId, ct);

        var newToken = new ZaloOAToken
        {
            OaId = request.OaId.Trim(),
            AppId = request.AppId.Trim(),

            AccessToken = request.AccessToken,
            RefreshToken = request.RefreshToken,

            ExpiresIn = request.ExpiresIn > 0
                ? request.ExpiresIn
                : (int)Math.Max(0, (expiresAtUtc - now).TotalSeconds),

            ExpiresAtUtc = expiresAtUtc,

            IsActive = true,
            CreatedOnUtc = now
        };

        await zaloOATokenRepository.InsertAsync(newToken);
        return newToken;
    }
}

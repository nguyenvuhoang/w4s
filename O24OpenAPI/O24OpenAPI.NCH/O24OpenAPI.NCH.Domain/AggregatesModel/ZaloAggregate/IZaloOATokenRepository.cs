using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.NCH.Domain.AggregatesModel.ZaloAggregate;

public interface IZaloOATokenRepository : IRepository<ZaloOAToken>
{
    Task<ZaloOAToken?> GetActiveByOaIdAsync(string oaId, CancellationToken ct);
    Task DeactivateActiveAsync(string oaId, CancellationToken ct);
    Task UpdateLastUsedAsync(string oaId, CancellationToken ct);
}

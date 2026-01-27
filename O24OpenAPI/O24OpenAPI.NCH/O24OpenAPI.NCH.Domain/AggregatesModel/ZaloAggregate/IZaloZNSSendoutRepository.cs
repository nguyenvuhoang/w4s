using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.NCH.Domain.AggregatesModel.ZaloAggregate;

public interface IZaloZNSSendoutRepository : IRepository<ZaloZNSSendout>
{
    Task<bool> ExistsByRefIdAsync(string refId, CancellationToken ct = default);
}

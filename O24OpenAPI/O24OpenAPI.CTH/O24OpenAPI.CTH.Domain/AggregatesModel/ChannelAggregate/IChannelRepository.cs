using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.CTH.Domain.AggregatesModel.ChannelAggregate;

public interface IChannelRepository : IRepository<Channel>
{
    Task<List<Channel>> GetAllAsync(CancellationToken ct = default);
    Task<Channel?> GetByChannelIdAsync(
        string channelId,
        CancellationToken ct = default
    );
    Task<List<Channel>> GetByChannelIdsAsync(
        IEnumerable<string> channelIds,
        CancellationToken cancellationToken = default
    );

}

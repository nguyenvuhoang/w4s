using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.CTH.API.Application.Models.Channel;
using O24OpenAPI.CTH.Domain.AggregatesModel.ChannelAggregate;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Features.Channel;

public class UpdateChannelModel : BaseTransactionModel, ICommand<ChannelVm>
{
    public bool IsOpen { get; set; }
    public string ChannelAction { get; set; }
}

public class UpdateChannelStatusHandler(IChannelRepository channelRepository, IStaticCacheManager staticCacheManager)
    : ICommandHandler<UpdateChannelModel, ChannelVm>
{
    public async Task<ChannelVm> HandleAsync(
        UpdateChannelModel request,
        CancellationToken cancellationToken = default
    )
    {
        string channelId = request.ChannelId;
        bool isOpen = request.IsOpen;
        if (string.IsNullOrWhiteSpace(channelId))
        {
            throw new ArgumentException("channelId is required", nameof(channelId));
        }

        var channel =
            await channelRepository.Table.FirstOrDefaultAsync(
                c => c.ChannelId == channelId,
                cancellationToken
            ) ?? throw new InvalidOperationException($"Channel '{channelId}' not found.");

        if (channel.Status == isOpen)
        {
            await InvalidateChannelActiveCacheAsync(channelId);

            var existed = await GetChannelByCodeAsync(channelId, ct);
            if (existed is not null)
            {
                return existed;
            }

            return new ChannelVm
            {
                Id = channel.Id,
                ChannelId = channel.ChannelId,
                ChannelName = channel.ChannelName,
                Description = channel.Description,
                Status = channel.Status,
                IsAlwaysOpen = channel.IsAlwaysOpen,
                TimeZoneId = channel.TimeZoneId,
                Weekly = [],
            };
        }

        channel.Status = isOpen;
        await channelRepository.Update(channel);

        await InvalidateChannelActiveCacheAsync(channelId);

        var updated = await GetChannelByCodeAsync(channelId, ct);
        if (updated is null)
        {
            return new ChannelVm
            {
                Id = channel.Id,
                ChannelId = channel.ChannelId,
                ChannelName = channel.ChannelName,
                Description = channel.Description,
                Status = channel.Status,
                IsAlwaysOpen = channel.IsAlwaysOpen,
                TimeZoneId = channel.TimeZoneId,
                Weekly = [],
            };
        }

        return updated;
    }

    private async Task InvalidateChannelActiveCacheAsync(string channelId)
    {
        var key = BuildActiveCacheKey(channelId);

        // Tuỳ IStaticCacheManager bạn đang dùng, 1 trong 2 (hoặc cả hai) method dưới sẽ tồn tại:
        try
        {
            await staticCacheManager.Remove(key);
        }
        catch
        { /* ignore if not supported */
        }
        try
        {
            await staticCacheManager.RemoveByPrefix("channel:active:");
        }
        catch
        { /* ignore */
        }
    }

    private static CacheKey BuildActiveCacheKey(string channelId) =>
        new($"channel:active:{channelId}");
}

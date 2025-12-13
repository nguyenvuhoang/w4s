using O24OpenAPI.ControlHub.Models.Channel;

namespace O24OpenAPI.ControlHub.Services.Interfaces;

public interface IChannelService
{
    Task<List<ChannelVm>> GetChannelsWithWeeklyAsync(CancellationToken ct = default);
    Task<ChannelVm> GetChannelByCodeAsync(string channelId, CancellationToken ct = default);
    Task<ChannelVm> UpdateChannelStatusAsync(string channelId, bool isOpen, CancellationToken ct = default);
    Task<bool> IsChannelActiveAsync(string channelId, CancellationToken ct = default);
    Task<bool> CanLoginAsync(string channelId, string userId, CancellationToken ct = default);
}

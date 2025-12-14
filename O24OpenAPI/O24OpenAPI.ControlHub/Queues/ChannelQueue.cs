using O24OpenAPI.ControlHub.Models.Channel;
using O24OpenAPI.ControlHub.Services.Interfaces;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.O24OpenAPIClient.Scheme.Workflow;
using O24OpenAPI.Web.Framework.Extensions;
using O24OpenAPI.Web.Framework.Services.Queue;

namespace O24OpenAPI.ControlHub.Queues;

public class ChannelQueue : BaseQueue
{
    private readonly IChannelService _channelService =
        EngineContext.Current.Resolve<IChannelService>();

    public async Task<WFScheme> GetAll(WFScheme wfScheme)
    {
        var model = await wfScheme.ToModel<ChannelGetModel>();
        return await Invoke<ChannelGetModel>(
            wfScheme,
            async () =>
            {
                var response = await _channelService.GetChannelsWithWeeklyAsync();
                return response;
            }
        );
    }

    public async Task<WFScheme> GetOne(WFScheme wfScheme)
    {
        var model = await wfScheme.ToModel<ChannelGetModel>();
        return await Invoke<ChannelGetModel>(
            wfScheme,
            async () =>
            {
                var response = await _channelService.GetChannelByCodeAsync(model.ChannelId);
                return response;
            }
        );
    }

    public async Task<WFScheme> UpdateStatus(WFScheme wfScheme)
    {
        var model = await wfScheme.ToModel<UpdateChannelModel>();
        return await Invoke<UpdateChannelModel>(
            wfScheme,
            async () =>
            {
                var response = await _channelService.UpdateChannelStatusAsync(
                    model.ChannelAction,
                    model.IsOpen
                );
                return response;
            }
        );
    }

    public async Task<WFScheme> VerifyStatus(WFScheme wfScheme)
    {
        var model = await wfScheme.ToModel<VerifyChannelModel>();
        return await Invoke<VerifyChannelModel>(
            wfScheme,
            async () =>
            {
                var response = await _channelService.IsChannelActiveAsync(model.ChannelId);
                return response;
            }
        );
    }

    public async Task<WFScheme> CanLogin(WFScheme wfScheme)
    {
        var model = await wfScheme.ToModel<CanLoginChannelModel>();
        return await Invoke<CanLoginChannelModel>(
            wfScheme,
            async () =>
            {
                var response = await _channelService.CanLoginAsync(model.ChannelId, model.UserId);
                return response;
            }
        );
    }
}

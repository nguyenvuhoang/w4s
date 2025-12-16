using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using O24OpenAPI.APIContracts.Events;
using O24OpenAPI.Client.EventBus.Abstractions;
using O24OpenAPI.Web.CMS.Services.Services;

namespace O24OpenAPI.Web.CMS.Services.Events;

public class EventBannerHandler : IIntegrationEventHandler<BannerModifyEvent>
{
    private readonly IHubContext<SignalHubService> _signal = EngineContext.Current.Resolve<
        IHubContext<SignalHubService>
    >();

    public Task Handle(BannerModifyEvent @event)
    {
        try
        {
            var json = JsonSerializer.Serialize(
                @event,
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
            );

            var bannerObj = JsonSerializer.Deserialize<object>(json)!;

            return SignalHubService.SendUpdateBannerAsync(_signal, bannerObj);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Task.CompletedTask;
        }
    }
}

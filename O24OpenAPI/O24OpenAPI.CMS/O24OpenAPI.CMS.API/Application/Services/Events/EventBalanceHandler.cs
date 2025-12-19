using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using O24OpenAPI.APIContracts.Events;
using O24OpenAPI.Client.EventBus.Abstractions;
using O24OpenAPI.CMS.API.Application.Services.Services;

namespace O24OpenAPI.CMS.API.Application.Services.Events;

public class EventBalanceHandler : IIntegrationEventHandler<BalanceModifyEvent>
{
    private readonly IHubContext<SignalHubService> _signal = EngineContext.Current.Resolve<
        IHubContext<SignalHubService>
    >();

    public Task Handle(BalanceModifyEvent @event)
    {
        try
        {
            var json = JsonSerializer.Serialize(
                @event,
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
            );

            var balanceObj = JsonSerializer.Deserialize<object>(json)!;

            return SignalHubService.SendUpdateBalanceAsync(
                _signal,
                @event.UserId,
                @event.DeviceId,
                balanceObj
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Task.CompletedTask;
        }
    }
}

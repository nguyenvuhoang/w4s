using Microsoft.AspNetCore.SignalR;
using O24OpenAPI.APIContracts.Events;
using O24OpenAPI.Client.EventBus.Abstractions;
using O24OpenAPI.CMS.API.Application.Services.Services;
using O24OpenAPI.Logging.Helpers;

namespace O24OpenAPI.CMS.API.Application.Services.Events;

public class EventLogOutHandler(IHubContext<SignalHubService> signal)
    : IIntegrationEventHandler<UserLogoutEvent>
{
    private readonly IHubContext<SignalHubService> _signal = signal;

    public Task Handle(UserLogoutEvent @event)
    {
        try
        {
            BusinessLogHelper.Info(
                "Handling UserLogoutEvent for UserCode: {UserCode}, DeviceId: {DeviceId}",
                @event.UserCode,
                @event.DeviceId
            );

            var user = new { IsLogout = true };

            return SignalHubService.SendUserLogOut(_signal, @event.UserCode, @event.DeviceId, user);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Task.CompletedTask;
        }
    }
}

using O24OpenAPI.Contracts.Events;

namespace O24OpenAPI.APIContracts.Events;

public class UserLogoutEvent : IntegrationEvent
{
    public string UserCode { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string DeviceId { get; set; }
}

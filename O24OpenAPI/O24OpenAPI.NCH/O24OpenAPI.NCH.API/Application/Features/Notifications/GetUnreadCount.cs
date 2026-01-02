using LinKit.Core.Cqrs;
using Newtonsoft.Json;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.NCH.Domain.AggregatesModel.NotificationAggregate;
using System.Text.Json.Serialization;

namespace O24OpenAPI.NCH.API.Application.Features.Notifications;

public class GetUnreadCountCommand
    : BaseTransactionModel,
        LinKit.Core.Cqrs.ICommand<GetUnreadCountResponse>
{
    public string UserCode { get; set; }
    public string AppType { get; set; }
}

public class GetUnreadCountResponse
{
    [JsonProperty("count")]
    [JsonPropertyName("count")]
    public int Count { get; set; }
}

[CqrsHandler]
public class GetUnreadCountHandler(INotificationRepository notificationRepository)
    : ICommandHandler<GetUnreadCountCommand, GetUnreadCountResponse>
{
    [WorkflowStep(WorkflowStep.NCH.WF_STEP_NCH_GET_UNREAD_COUNT)]
    public async Task<GetUnreadCountResponse> HandleAsync(
        GetUnreadCountCommand request,
        CancellationToken cancellationToken = default
    )
    {
        int unreadCount = await notificationRepository.GetUnreadCount(
            request.UserCode ?? request.CurrentUserCode,
            request.ChannelId
        );
        return new GetUnreadCountResponse { Count = unreadCount };
    }
}

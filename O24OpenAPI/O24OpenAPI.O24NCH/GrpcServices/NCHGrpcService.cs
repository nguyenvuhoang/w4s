using Grpc.Core;
using O24OpenAPI.Core.Domain;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Grpc.Common;
using O24OpenAPI.Grpc.NCH;
using O24OpenAPI.GrpcContracts.GrpcServerServices;
using O24OpenAPI.O24NCH.Models.Request;
using O24OpenAPI.O24NCH.Services.Interfaces;
using System.Text.Json;
using static O24OpenAPI.Grpc.NCH.NCHGrpcService;

namespace O24OpenAPI.O24NCH.GrpcServices;

public class NCHGrpcService : NCHGrpcServiceBase
{
    private readonly INotificationService _notificationService =
        EngineContext.Current.Resolve<INotificationService>();

    public override async Task<GrpcResponse> SendNotification(
        SendNotificationRequest request,
        ServerCallContext context
    )
    {
        return await GrpcExecutor.ExecuteAsync(
            context,
            async () =>
            {
                var deserializeData = JsonSerializer.Deserialize<Dictionary<string, object>>(
                    request.Data
                );
                var deserializeDataTemplate = JsonSerializer.Deserialize<
                    Dictionary<string, object>
                >(request.DataTemplate);
                var deserializeMimeEntities = JsonSerializer.Deserialize<List<MimeEntity>>(
                    request.MimeEntities
                );

                var model = new Models.Request.NotificationRequestModel
                {
                    UserCode = request.UserCode,
                    PhoneNumber = request.LoginName,
                    Purpose = request.Purpose,
                    SenderData = deserializeData,
                    NotificationType = request.NotificationType,
                    DataTemplate = deserializeDataTemplate,
                    MimeEntities = deserializeMimeEntities,
                    ChannelId = EngineContext.Current.Resolve<WorkContext>().CurrentChannel,
                    Email = request.ReceiverMail,
                    Message = request.Message
                };

                var result = await _notificationService.SendNotification(model);
                return result;
            }
        );
    }
}

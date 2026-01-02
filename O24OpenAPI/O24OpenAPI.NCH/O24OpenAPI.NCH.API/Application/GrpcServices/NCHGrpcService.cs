using Grpc.Core;
using O24OpenAPI.Grpc.Common;
using O24OpenAPI.Grpc.NCH;
using O24OpenAPI.GrpcContracts.GrpcServerServices;
using O24OpenAPI.NCH.API.Application.Models.Request;
using System.Text.Json;
using static O24OpenAPI.Grpc.NCH.NCHGrpcService;

namespace O24OpenAPI.NCH.API.Application.GrpcServices;

public class NCHGrpcService : NCHGrpcServiceBase
{
    public override async Task<GrpcResponse> SendNotification(
        SendNotificationRequest request,
        ServerCallContext context
    )
    {
        // Bản rút gọn theo CTH:
        // - Chưa port INotificationService từ O24NCH
        // - Tạm thời ACK để không block build / integration
        return await GrpcExecutor.ExecuteAsync(
            context,
            async () =>
            {
                // Parse để validate payload (tránh warning unused)
                _ = JsonSerializer.Deserialize<Dictionary<string, object>>(request.Data);
                _ = JsonSerializer.Deserialize<Dictionary<string, object>>(request.DataTemplate);
                _ = JsonSerializer.Deserialize<List<O24MimeEntity>>(request.MimeEntities);

                await Task.CompletedTask;
                return true;
            }
        );
    }
}

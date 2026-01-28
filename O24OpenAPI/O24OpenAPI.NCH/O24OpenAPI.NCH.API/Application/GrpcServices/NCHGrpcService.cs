using Grpc.Core;
using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Grpc.Common;
using O24OpenAPI.Grpc.NCH;
using O24OpenAPI.GrpcContracts.Extensions;
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
        return await GrpcExecutor.ExecuteAsync(
            context,
            async () =>
            {
                _ = JsonSerializer.Deserialize<Dictionary<string, object>>(request.Data);
                _ = JsonSerializer.Deserialize<Dictionary<string, object>>(request.DataTemplate);
                _ = JsonSerializer.Deserialize<List<O24MimeEntity>>(request.MimeEntities);

                await Task.CompletedTask;
                return true;
            }
        );
    }

    public override async Task<GrpcResponse> CreateZaloOAToken(
        CreateZaloTokenRequest request,
        ServerCallContext context
    )
    {
        var mediator = EngineContext.Current.Resolve<IMediator>(MediatorKey.NCH);
        var result = mediator.SendAsync(new GrpcContracts.Models.NCHModels.CreateZaloOATokenCommand
        {
            OaId = request.OaId,
            AppId = request.AppId,
            AccessToken = request.AccessToken,
            RefreshToken = request.RefreshToken,
            ExpiresIn = request.ExpiresIn
        });
        return await result.GetGrpcResponseAsync();
    }
}

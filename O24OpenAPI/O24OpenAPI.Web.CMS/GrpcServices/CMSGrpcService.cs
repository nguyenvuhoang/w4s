using Grpc.Core;
using O24OpenAPI.Grpc.CMS;
using O24OpenAPI.Grpc.Common;
using O24OpenAPI.GrpcContracts.GrpcServerServices;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using static O24OpenAPI.Grpc.CMS.CMSGrpcService;

namespace O24OpenAPI.Web.CMS.GrpcServices;

public class CMSGrpcService : CMSGrpcServiceBase
{
    private readonly IUserSessionsService _userSessionService =
        EngineContext.Current.Resolve<IUserSessionsService>();

    public override async Task<GrpcResponse> GetUserSession(
        GetUserSessionRequest request,
        ServerCallContext context
    )
    {
        return await GrpcExecutor.ExecuteAsync(
            context,
            async () =>
            {
                var session = await _userSessionService.GetByToken(request.Token);
                return session;
            }
        );
    }
}

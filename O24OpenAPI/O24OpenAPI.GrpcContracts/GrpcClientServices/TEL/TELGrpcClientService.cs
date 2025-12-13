using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Grpc.TEL;
using O24OpenAPI.GrpcContracts.GrpcClient;

namespace O24OpenAPI.GrpcContracts.GrpcClientServices.TEL;

public class TELGrpcClientService : BaseGrpcClientService, ITELGrpcClientService
{
    public TELGrpcClientService()
    {
        ServerId = "TEL";
    }

    private readonly IGrpcClient<TELGrpcService.TELGrpcServiceClient> _telGrpcClient =
        EngineContext.Current.Resolve<IGrpcClient<TELGrpcService.TELGrpcServiceClient>>();
}

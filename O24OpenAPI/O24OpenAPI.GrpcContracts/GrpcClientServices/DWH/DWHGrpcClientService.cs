using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Grpc.DWH;
using O24OpenAPI.GrpcContracts.GrpcClient;

namespace O24OpenAPI.GrpcContracts.GrpcClientServices.DWH;

public class DWHGrpcClientService : BaseGrpcClientService, IDWHGrpcClientService
{
    public DWHGrpcClientService()
    {
        ServerId = "DWH";
    }

    private readonly IGrpcClient<DWHGrpcService.DWHGrpcServiceClient> _dwhGrpcClient =
        EngineContext.Current.Resolve<IGrpcClient<DWHGrpcService.DWHGrpcServiceClient>>();
}

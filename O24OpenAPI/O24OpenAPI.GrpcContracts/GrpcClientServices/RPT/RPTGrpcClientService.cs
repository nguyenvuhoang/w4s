using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Grpc.RPT;
using O24OpenAPI.GrpcContracts.GrpcClient;

namespace O24OpenAPI.GrpcContracts.GrpcClientServices.RPT;

public class RPTGrpcClientService : BaseGrpcClientService, IRPTGrpcClientService
{
    public RPTGrpcClientService()
    {
        ServerId = "RPT";
    }

    private readonly IGrpcClient<RPTGrpcService.RPTGrpcServiceClient> _rptGrpcClient =
        EngineContext.Current.Resolve<IGrpcClient<RPTGrpcService.RPTGrpcServiceClient>>();
}

using O24OpenAPI.GrpcContracts.Factory;

namespace O24OpenAPI.GrpcContracts.GrpcClientServices.CMS;

public class CMSGrpcClientService : BaseGrpcClientService, ICMSGrpcClientService
{
    private readonly IGrpcClientFactory _grpcClientFactory;

    public CMSGrpcClientService(IGrpcClientFactory grpcClientFactory)
    {
        ServerId = "CMS";
        _grpcClientFactory = grpcClientFactory;
    }
}

using Grpc.Core;
using O24OpenAPI.Core.Configuration;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.GrpcContracts.Factory;

namespace O24OpenAPI.GrpcContracts.GrpcClientServices.CMS;

public class CMSGrpcClientService : BaseGrpcClientService, ICMSGrpcClientService
{
    private readonly IGrpcClientFactory _grpcClientFactory;
    private readonly Metadata _defaultHeader;

    public CMSGrpcClientService(IGrpcClientFactory grpcClientFactory)
    {
        ServerId = "CMS";
        _grpcClientFactory = grpcClientFactory;
        _defaultHeader = new Metadata()
        {
            {
                "flow",
                $"{Singleton<O24OpenAPIConfiguration>.Instance.YourServiceID} -> {ServerId}"
            },
        };
    }
}

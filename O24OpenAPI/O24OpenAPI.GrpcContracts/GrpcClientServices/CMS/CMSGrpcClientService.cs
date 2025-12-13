using O24OpenAPI.APIContracts.Models.CBG;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Grpc.CMS;
using O24OpenAPI.GrpcContracts.GrpcClient;

namespace O24OpenAPI.GrpcContracts.GrpcClientServices.CMS;

public class CMSGrpcClientService : BaseGrpcClientService, ICMSGrpcClientService
{
    public CMSGrpcClientService()
    {
        ServerId = "CMS";
    }

    private readonly IGrpcClient<CMSGrpcService.CMSGrpcServiceClient> _CMSGrpcClient =
        EngineContext.Current.Resolve<IGrpcClient<CMSGrpcService.CMSGrpcServiceClient>>();

    public async Task<CBGUserSessionModel> GetUserSessionAsync(string token)
    {
        var getUserSessionRequest = new GetUserSessionRequest { Token = token };
        var result = await InvokeAsync<CBGUserSessionModel>(
            async (header) =>
            {
                return await _CMSGrpcClient.Client.GetUserSessionAsync(
                    getUserSessionRequest,
                    header
                );
            }
        );
        return result;
    }
}

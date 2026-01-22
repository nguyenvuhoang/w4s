using LinKit.Json.Runtime;
using O24OpenAPI.Contracts.Models;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Grpc.Common;
using O24OpenAPI.Grpc.LOG;
using O24OpenAPI.GrpcContracts.GrpcClient;

namespace O24OpenAPI.GrpcContracts.GrpcClientServices.LOG;

public class LOGGrpcClientService : BaseGrpcClientService, ILOGGrpcClientService
{
    public LOGGrpcClientService()
    {
        ServerId = "LOG";
    }

    private readonly IGrpcClient<LOGGrpcService.LOGGrpcServiceClient> _logGrpcClient =
        EngineContext.Current.Resolve<IGrpcClient<LOGGrpcService.LOGGrpcServiceClient>>();

    public async Task<string> TestAsync(string name)
    {
        TestRequest executeWorkflowRequest = new() { Message = name };
        var result = await InvokeAsync<string>(
            async (header) =>
            {
                return await _logGrpcClient.Client.TestAsync(executeWorkflowRequest, header);
            }
        );
        return result;
    }

    public async Task<bool> SubmitLogAsync(LogEntryModel logEntry)
    {
        GrpcRequest request = new() { Data = logEntry.ToJson() };
        await InvokeAsync<string>(
            async (header) =>
            {
                return await _logGrpcClient.Client.SubmitLogAsync(request, header);
            }
        );
        return true;
    }
}

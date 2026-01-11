using Grpc.Core;
using O24OpenAPI.Grpc.CMS;

namespace O24OpenAPI.CMS.API.Application.GrpcServices
{
    public class CMSGrpcServer : CMSGrpcService.CMSGrpcServiceBase
    {
        public override async Task<HelloReply> SayHello(
            HelloRequest request,
            ServerCallContext context
        )
        {
            await Task.CompletedTask;
            return new HelloReply { Hello = $"hello {request.Name}" };
        }
    }
}

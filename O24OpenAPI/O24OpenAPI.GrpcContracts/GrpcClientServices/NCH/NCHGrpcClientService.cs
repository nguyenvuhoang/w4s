using LinKit.Json.Runtime;
using O24OpenAPI.APIContracts.Models.DTS;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Grpc.NCH;
using O24OpenAPI.GrpcContracts.GrpcClient;

namespace O24OpenAPI.GrpcContracts.GrpcClientServices.NCH;

public class NCHGrpcClientService : BaseGrpcClientService, INCHGrpcClientService
{
    public NCHGrpcClientService()
    {
        ServerId = "NCH";
    }

    private readonly IGrpcClient<NCHGrpcService.NCHGrpcServiceClient> _nchGrpcClient =
        EngineContext.Current.Resolve<IGrpcClient<NCHGrpcService.NCHGrpcServiceClient>>();

    public async Task<string> SendNotificationAsync(
        string userCode,
        string loginName,
        string purpose,
        string notificationType,
        string receiverMail,
        Dictionary<string, object> data,
        Dictionary<string, object> dataTemplate,
        List<DTSMimeEntityModel> mimeEntities,
        string messsage = ""
    )
    {
        var request = new SendNotificationRequest
        {
            UserCode = userCode,
            LoginName = loginName,
            Purpose = purpose,
            NotificationType = notificationType,
            ReceiverMail = receiverMail,
            Data = data.ToJson(),
            DataTemplate = dataTemplate.ToJson(),
            MimeEntities = mimeEntities.ToJson(),
            Message = messsage,
        };

        return await InvokeAsync<string>(
            async (header) => await _nchGrpcClient.Client.SendNotificationAsync(request, header)
        );
    }
}

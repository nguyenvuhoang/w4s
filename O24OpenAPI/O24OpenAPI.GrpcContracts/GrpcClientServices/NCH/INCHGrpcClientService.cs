using O24OpenAPI.APIContracts.Models.DTS;

namespace O24OpenAPI.GrpcContracts.GrpcClientServices.NCH;

public interface INCHGrpcClientService
{
    Task<string> SendNotificationAsync(
        string userCode,
        string loginName,
        string purpose,
        string notificationType,
        string receiverMail,
        Dictionary<string, object> data,
        Dictionary<string, object> dataTemplate,
        List<DTSMimeEntityModel> mimeEntities,
        string messsage = ""
    );
}

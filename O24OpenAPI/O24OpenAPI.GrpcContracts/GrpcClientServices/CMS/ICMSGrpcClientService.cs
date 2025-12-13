using O24OpenAPI.APIContracts.Models.CBG;

namespace O24OpenAPI.GrpcContracts.GrpcClientServices.CMS;

public interface ICMSGrpcClientService
{
    Task<CBGUserSessionModel> GetUserSessionAsync(string token);
}

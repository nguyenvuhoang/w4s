using O24OpenAPI.Contracts.Abstractions;

namespace O24OpenAPI.GrpcContracts.GrpcClientServices.WFO;

public interface IWFOGrpcClientService : IWFOGrpcClientBaseService
{
    Task<string> PingAsync(string name);
}

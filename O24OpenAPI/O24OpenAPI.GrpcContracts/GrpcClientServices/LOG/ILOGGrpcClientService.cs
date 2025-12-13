using O24OpenAPI.Contracts.Models.Log;

namespace O24OpenAPI.GrpcContracts.GrpcClientServices.LOG;

public interface ILOGGrpcClientService
{
    Task<string> TestAsync(string name);
    Task SubmitLogAsync(LogEntryModel logEntry);
}

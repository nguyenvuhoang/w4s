using O24OpenAPI.Contracts.Models;

namespace O24OpenAPI.GrpcContracts.GrpcClientServices.LOG;

public interface ILOGGrpcClientService
{
    Task<string> TestAsync(string name);
    Task<bool> SubmitLogAsync(LogEntryModel logEntry);
}

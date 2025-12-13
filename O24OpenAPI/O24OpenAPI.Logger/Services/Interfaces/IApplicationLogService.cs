using O24OpenAPI.Logger.Domain;

namespace O24OpenAPI.Logger.Services.Interfaces;

public interface IApplicationLogService
{
    Task AddAsync(ApplicationLog log);
    Task ClearApplicationLogAsync();
}

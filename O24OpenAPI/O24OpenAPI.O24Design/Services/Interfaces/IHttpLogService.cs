using O24OpenAPI.Framework.Domain.Logging;

namespace O24OpenAPI.O24Design.Services.Interfaces;

public interface IHttpLogService
{
    Task AddAsync(HttpLog log);
}

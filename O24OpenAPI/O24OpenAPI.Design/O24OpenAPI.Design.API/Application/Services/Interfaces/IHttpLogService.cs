using O24OpenAPI.Framework.Domain.Logging;

namespace O24OpenAPI.Design.API.Application.Services.Interfaces;

public interface IHttpLogService
{
    Task AddAsync(HttpLog log);
}

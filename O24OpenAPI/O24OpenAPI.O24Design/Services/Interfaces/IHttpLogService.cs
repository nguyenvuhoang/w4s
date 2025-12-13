using O24OpenAPI.Web.Framework.Domain.Logging;

namespace O24OpenAPI.Sample.Services.Interfaces;

public interface IHttpLogService
{
    Task AddAsync(HttpLog log);
}

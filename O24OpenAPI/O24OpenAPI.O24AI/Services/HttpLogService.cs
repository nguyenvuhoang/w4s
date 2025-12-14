using O24OpenAPI.Sample.Services.Interfaces;
using O24OpenAPI.Web.Framework.Domain.Logging;

namespace O24OpenAPI.Sample.Services;

public class HttpLogService(IRepository<HttpLog> repository) : ILogService<HttpLog>
{
    private readonly IRepository<HttpLog> _repository = repository;

    public async Task AddAsync(HttpLog log)
    {
        await _repository.Insert(log);
    }
}

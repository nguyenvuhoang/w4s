using O24OpenAPI.Core.SeedWork;
using O24OpenAPI.Sample.Services.Interfaces;
using O24OpenAPI.Framework.Domain.Logging;

namespace O24OpenAPI.Sample.Services;

public class HttpLogService(IRepository<HttpLog> repository) : ILogService<HttpLog>
{
    private readonly IRepository<HttpLog> _repository = repository;

    public async Task AddAsync(HttpLog log)
    {
        await _repository.Insert(log);
    }
}

using O24OpenAPI.Framework.Domain.Logging;
using O24OpenAPI.O24Design.Services.Interfaces;

namespace O24OpenAPI.O24Design.Services;

public class HttpLogService(IRepository<HttpLog> repository) : ILogService<HttpLog>
{
    private readonly IRepository<HttpLog> _repository = repository;

    public async Task AddAsync(HttpLog log)
    {
        await _repository.Insert(log);
    }
}

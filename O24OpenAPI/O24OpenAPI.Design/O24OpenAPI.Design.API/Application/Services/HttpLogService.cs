using O24OpenAPI.Framework.Domain.Logging;
using O24OpenAPI.Design.API.Application.Services.Interfaces;
using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.Design.API.Application.Services;

public class HttpLogService(IRepository<HttpLog> repository) : ILogService<HttpLog>
{
    private readonly IRepository<HttpLog> _repository = repository;

    public async Task AddAsync(HttpLog log)
    {
        await _repository.Insert(log);
    }
}

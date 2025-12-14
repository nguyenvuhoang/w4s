using O24OpenAPI.Logger.Domain;
using O24OpenAPI.Logger.Services.Interfaces;
using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.Logger.Services;

public class ApplicationLogService(
    IRepository<ApplicationLog> repository,
    LoggerSetting loggerSetting
) : IApplicationLogService
{
    private readonly IRepository<ApplicationLog> _repository = repository;
    private readonly LoggerSetting _loggerSetting = loggerSetting;

    public async Task AddAsync(ApplicationLog log)
    {
        await _repository.InsertAsync(log);
    }

    public Task ClearApplicationLogAsync()
    {
        return _repository.DeleteWhere(log =>
            log.LogTimestamp < DateTime.UtcNow.AddDays(-_loggerSetting.LogRetentionDays)
        );
    }
}

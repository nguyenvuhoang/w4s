using O24OpenAPI.Design.API.Application.Services.Interfaces;

namespace O24OpenAPI.Design.API.Application.Services.CommandHandler;

public class LogCommandHandler<T>
    where T : class
{
    private readonly ILogService<T> _logService;

    public LogCommandHandler(ILogService<T> logService)
    {
        _logService = logService;
    }

    public async Task Handle(LogCommand<T> command)
    {
        await _logService.AddAsync(command.LogData);
    }
}

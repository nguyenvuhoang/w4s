using O24OpenAPI.O24Design.Services.Interfaces;

namespace O24OpenAPI.O24Design.Services.CommandHandler;

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

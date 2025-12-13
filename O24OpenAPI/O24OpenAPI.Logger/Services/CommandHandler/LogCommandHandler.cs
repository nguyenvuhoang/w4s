using MediatR;
using O24OpenAPI.Logger.Services.Interfaces;

namespace O24OpenAPI.Logger.Services.CommandHandler;

/// <summary>
/// The log command handler class
/// </summary>
public class LogCommandHandler<T> : IRequestHandler<LogCommand<T>>
    where T : class
{
    /// <summary>
    /// The log service
    /// </summary>
    private readonly ILogService<T> _logService;

    /// <summary>
    /// Initializes a new instance of the <see cref="LogCommandHandler{T}"/> class
    /// </summary>
    /// <param name="logService">The log service</param>
    public LogCommandHandler(ILogService<T> logService)
    {
        _logService = logService;
    }

    /// <summary>
    /// Handles the request
    /// </summary>
    /// <param name="request">The request</param>
    /// <param name="cancellationToken">The cancellation token</param>
    public async Task Handle(LogCommand<T> request, CancellationToken cancellationToken)
    {
        await _logService.AddAsync(request.LogData);
    }
}

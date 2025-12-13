using MediatR;
using O24OpenAPI.Logger.Services.Interfaces;

namespace O24OpenAPI.Logger.Services.QueryHandler;

/// <summary>
/// The view detail query handler class
/// </summary>
public class ViewDetailQueryHandler<T> : IRequestHandler<ViewDetailQuery<T>, T>
    where T : class
{
    private readonly ILogService<T> _logService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ViewDetailQueryHandler{T}"/> class
    /// </summary>
    /// <param name="logService">The log service</param>
    public ViewDetailQueryHandler(ILogService<T> logService)
    {
        _logService = logService;
    }

    /// <summary>
    /// Handles the view detail request
    /// </summary>
    /// <param name="request">The request</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The detail item</returns>
    public async Task<T> Handle(
        ViewDetailQuery<T> request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var result = await _logService.GetByExecutionIdAsync(request.ExecutionId);
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
}

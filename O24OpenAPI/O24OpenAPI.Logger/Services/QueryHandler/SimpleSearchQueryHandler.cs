using MediatR;
using O24OpenAPI.Logger.Services.Interfaces;
using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.Logger.Services.QueryHandler;

/// <summary>
/// The simple search query handler class
/// </summary>
public class SimpleSearchQueryHandler<T> : IRequestHandler<SimpleSearchQuery<T>, PagedModel>
    where T : class
{
    /// <summary>
    /// The log service
    /// </summary>
    private readonly ILogService<T> _logService;

    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleSearchQueryHandler{T}"/> class
    /// </summary>
    /// <param name="logService">The log service</param>
    public SimpleSearchQueryHandler(ILogService<T> logService)
    {
        _logService = logService;
    }

    /// <summary>
    /// Handles the request
    /// </summary>
    /// <param name="request">The request</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A task containing the object</returns>
    public async Task<PagedModel> Handle(
        SimpleSearchQuery<T> request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var result = await _logService.SimpleSearch(request.SearchModel);
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
}

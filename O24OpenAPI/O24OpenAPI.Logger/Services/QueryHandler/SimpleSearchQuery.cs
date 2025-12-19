using MediatR;
using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.Logger.Models.Log;

namespace O24OpenAPI.Logger.Services.QueryHandler;

/// <summary>
/// The simple search query class
/// </summary>
/// <seealso cref="SimpleSearchModel"/>
/// <seealso cref="IRequest"/>
public class SimpleSearchQuery<T> : IRequest<PagedModel>
    where T : class
{
    /// <summary>
    /// Gets the value of the search model
    /// </summary>
    public SearchModel SearchModel { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleSearchQuery{T}"/> class
    /// </summary>
    /// <param name="searchModel">The search model</param>
    public SimpleSearchQuery(SearchModel searchModel)
    {
        SearchModel = searchModel;
    }
}

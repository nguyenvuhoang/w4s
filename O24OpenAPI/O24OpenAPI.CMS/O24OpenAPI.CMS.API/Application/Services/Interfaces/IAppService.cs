using O24OpenAPI.CMS.Domain.AggregateModels.AppAggregate;

namespace O24OpenAPI.CMS.API.Application.Services.Interfaces;

/// <summary>
/// User Account service
/// </summary>
public partial interface IAppService
{
    /// <summary>
    /// Gets the by identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>Task&lt;Bo&gt;.</returns>
    Task<App> GetById(int id);
}

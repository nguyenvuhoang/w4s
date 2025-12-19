using O24OpenAPI.CMS.API.Application.Services.Interfaces;

namespace O24OpenAPI.CMS.API.Application.Services.Services;

/// <summary>
/// App service
/// </summary>
/// <remarks>
/// Ctor
/// </remarks>
/// <param name="localizationService"></param>
/// <param name="AppRepository"></param>
/// <param name="mediaUploadRepository"></param>
public partial class AppService(IRepository<App> AppRepository) : IAppService
{
    #region Fields

    private readonly IRepository<App> _appRepository = AppRepository;

    #endregion
    #region Ctor

    #endregion
    /// <summary>
    /// Gets GetById
    /// </summary>
    /// <returns>Task&lt;App&gt;.</returns>
    public virtual async Task<App> GetById(int id)
    {
        return await _appRepository.GetById(id);
    }
}

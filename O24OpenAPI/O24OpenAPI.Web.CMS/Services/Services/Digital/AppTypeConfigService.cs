using LinqToDB;
using O24OpenAPI.Web.CMS.Models.Digital;
using O24OpenAPI.Web.CMS.Services.Interfaces;

namespace O24OpenAPI.Web.CMS.Services.Services.Digital;

public class AppTypeConfigService(
    IRepository<AppTypeConfig> appTypeConfigRepository
) : IAppTypeConfigService
{
    private readonly IRepository<AppTypeConfig> _appTypeConfigRepository = appTypeConfigRepository;

    /// <summary>
    /// Loads the application type configuration based on the provided model.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<List<AppTypeConfigResponseModel>> LoadAppTypeConfig(AppTypeConfigRequestModel model)
    {
        var configAppType = await _appTypeConfigRepository.Table
             .Where(s => s.IsActive)
             .Select(s => new AppTypeConfigResponseModel
             {
                 AppCode = s.AppCode,
                 AppName = s.AppName,
                 AppTypeDescription = s.AppTypeDescription,
                 AppTypeIcon = s.AppTypeIcon,
                 RedirectPage = s.RedirectPage,
                 OrderIndex = s.OrderIndex,
                 IsActive = s.IsActive,
                 CreatedOnUtc = s.CreatedOnUtc,
                 UpdatedOnUtc = s.UpdatedOnUtc
             })
             .OrderBy(s => s.OrderIndex).ToListAsync();

        return configAppType;
    }
}

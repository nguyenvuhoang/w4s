using O24OpenAPI.Web.CMS.Models.Digital;

namespace O24OpenAPI.Web.CMS.Services.Interfaces;

public interface IAppTypeConfigService
{
    /// <summary>
    /// Load application type configuration based on the provided request model.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<List<AppTypeConfigResponseModel>> LoadAppTypeConfig(AppTypeConfigRequestModel model);
}

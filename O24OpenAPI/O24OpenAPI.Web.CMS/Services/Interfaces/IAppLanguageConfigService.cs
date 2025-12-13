using O24OpenAPI.Web.CMS.Models.Digital;

namespace O24OpenAPI.Web.CMS.Services.Interfaces;

public interface IAppLanguageConfigService
{
    Task<List<AppLanguageConfigResponseModel>> LoadAppLanguageAsync(AppLanguageConfigRequestModel model);
    Task<PagedResult<AppLanguageConfigResponseModel>> LoadAppLanguagePageAsync(
            AppLanguageConfigRequestModel model,
            CancellationToken ct = default);
    Task<PagedResult<AppLanguageVersionResponseModel>> LoadAppLanguageVersionAsync(
         AppLanguageConfigRequestModel model,
         CancellationToken ct = default);
}

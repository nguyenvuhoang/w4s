using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.Framework.Localization;
using O24OpenAPI.Report.Domain.AggregateModels.ViewerSettingAggregate;

namespace O24OpenAPI.Report.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
internal class ViewerSettingRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager,
    ILocalizationService localizationService
) : EntityRepository<ViewerSetting>(dataProvider, staticCacheManager), IViewerSettingRepository
{
    public virtual async Task<ViewerSetting?> GetByCodeTemplate(string code_template)
    {
        return await Table.Where(s => s.CodeTemplate.Equals(code_template)).FirstOrDefaultAsync();
    }

    public virtual async Task Delete(int viewerSettingId)
    {
        ViewerSetting viewerSetting = await GetById(viewerSettingId);
        if (viewerSetting == null)
        {
            throw new O24OpenAPIException(
                await localizationService.GetResource("Report.ViewerSetting.Value.NotFound")
            );
        }

        await Delete(viewerSetting);
    }
}

using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.Report.Domain.AggregateModels.ViewerSettingAggregate;

public interface IViewerSettingRepository : IRepository<ViewerSetting>
{
    Task<ViewerSetting?> GetByCodeTemplate(string code_template);
    Task Delete(int viewerSettingId);
}

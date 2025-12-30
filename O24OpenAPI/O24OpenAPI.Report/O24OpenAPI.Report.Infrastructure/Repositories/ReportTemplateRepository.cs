using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.Report.Domain.AggregateModels.ReportAggregate;

namespace O24OpenAPI.Report.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
internal class ReportTemplateRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<ReportTemplate>(dataProvider, staticCacheManager), IReportTemplateRepository
{
    public virtual async Task<ReportTemplate?> GetByCodeTemplate(string code)
    {
        return await Table.Where(s => s.Code.Equals(code)).FirstOrDefaultAsync();
    }
}

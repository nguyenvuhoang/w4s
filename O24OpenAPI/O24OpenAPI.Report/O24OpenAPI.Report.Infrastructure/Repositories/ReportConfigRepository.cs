using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.Report.Domain.AggregateModels.ReportAggregate;

namespace O24OpenAPI.Report.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
internal class ReportConfigRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<ReportConfig>(dataProvider, staticCacheManager), IReportConfigRepository
{
    public virtual async Task<ReportConfig?> GetByCodeTemplate(string code_template)
    {
        return await Table.Where(s => s.CodeTemplate.Equals(code_template)).FirstOrDefaultAsync();
    }

    public virtual async Task<ReportConfig?> GetByCode(string reportCode)
    {
        return await Table.Where(s => s.Code.Equals(reportCode)).FirstOrDefaultAsync();
    }

    public virtual async Task<IPagedList<ReportConfig>> Search(SimpleSearchModel model)
    {
        System.Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(model));
        model.PageSize = model.PageSize == 0 ? int.MaxValue : model.PageSize;
        IPagedList<ReportConfig> companies = await GetAllPaged(
            query =>
            {
                query = query.OrderByDescending(a => a.Id);
                return query;
            },
            model.PageIndex,
            model.PageSize
        );
        return companies;
    }
}

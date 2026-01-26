using O24OpenAPI.Core;
using O24OpenAPI.Core.SeedWork;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.Report.Domain.AggregateModels.ReportAggregate;

public interface IReportConfigRepository : IRepository<ReportConfig>
{
    Task<ReportConfig?> GetByCodeTemplate(string code_template);
    Task<ReportConfig?> GetByCode(string reportCode);
    Task<IPagedList<ReportConfig>> Search(SimpleSearchModel model);
}

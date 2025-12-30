using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.Report.Domain.AggregateModels.ReportAggregate;

public interface IReportTemplateRepository : IRepository<ReportTemplate>
{
    Task<ReportTemplate?> GetByCodeTemplate(string code);
}

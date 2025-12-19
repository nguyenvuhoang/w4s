using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Web.Report.Domain;
using O24OpenAPI.Web.Report.Services.Interfaces;

namespace O24OpenAPI.Web.Report.Services;

/// <summary>
///
/// </summary>
/// <param name="templateReportRepository"></param>
public class TemplateReportService(IRepository<ReportTemplate> templateReportRepository)
    : ITemplateReportService
{
    private readonly IRepository<ReportTemplate> _templateReportRepository =
        templateReportRepository;

    /// <summary>
    ///
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public virtual async Task<ReportTemplate> GetByCodeTemplate(string code)
    {
        return await _templateReportRepository
            .Table.Where(s => s.Code.Equals(code))
            .FirstOrDefaultAsync();
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual async Task<ReportTemplate> GetById(int id)
    {
        var result = await _templateReportRepository.GetById(id);
        return result;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public async Task<ReportTemplate> Update(ReportTemplate entity)
    {
        await _templateReportRepository.Update(entity);
        return entity;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public async Task<ReportTemplate> Insert(ReportTemplate entity)
    {
        await _templateReportRepository.Insert(entity);
        return entity;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public async Task Delete(ReportTemplate entity)
    {
        await _templateReportRepository.Delete(entity);
    }
}

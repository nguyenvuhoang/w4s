using O24OpenAPI.Core;
using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.Web.Report.Domain;
using O24OpenAPI.Web.Report.Services.Interfaces;

namespace O24OpenAPI.Web.Report.Services;

public partial class ReportConfigService(IRepository<ReportConfig> reportConfigRepository)
    : IReportConfigService
{
    private readonly IRepository<ReportConfig> _reportConfigRepository = reportConfigRepository;

    /// <summary>
    /// /// Get a reportConfig by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual async Task<ReportConfig> GetById(int id)
    {
        return await _reportConfigRepository.GetById(id);
    }

    /// <summary>
    /// Get all reportConfig
    /// </summary>
    /// <returns></returns>
    public virtual async Task<List<ReportConfig>> GetAll()
    {
        return await _reportConfigRepository.Table.ToListAsync();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="code_template"></param>
    /// <returns></returns>
    public virtual async Task<ReportConfig> GetByCodeTemplate(string code_template)
    {
        return await _reportConfigRepository
            .Table.Where(s => s.CodeTemplate.Equals(code_template))
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// GetByCode
    /// </summary>
    /// <param name="code_template"></param>
    /// <returns>ReportConfig</returns>
    public virtual async Task<ReportConfig> GetByCode(string reportCode)
    {
        return await _reportConfigRepository
            .Table.Where(s => s.Code.Equals(reportCode))
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Simple search model
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public virtual async Task<IPagedList<ReportConfig>> Search(SimpleSearchModel model)
    {
        System.Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(model));
        model.PageSize = model.PageSize == 0 ? int.MaxValue : model.PageSize;
        IPagedList<ReportConfig> companies = await _reportConfigRepository.GetAllPaged(
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

    /// <summary>
    /// Create a reportConfig
    /// </summary>
    /// <param name="reportConfig"></param>
    /// <returns></returns>
    public virtual async Task<ReportConfig> Create(ReportConfig reportConfig)
    {
        await _reportConfigRepository.Insert(reportConfig);
        return reportConfig;
    }

    /// <summary>
    /// Update a reportConfig
    /// </summary>
    /// <param name="reportConfig"></param>
    /// <param name="referenceId"></param>
    /// <returns></returns>
    public virtual async Task Update(ReportConfig reportConfig, string referenceId = "")
    {
        await _reportConfigRepository.Update(reportConfig);
    }

    /// <summary>
    /// Delete a reportConfig
    /// </summary>
    /// <param name="reportConfigId"></param>
    /// <returns></returns>
    public virtual async Task Delete(int reportConfigId)
    {
        ReportConfig reportConfig = await GetById(reportConfigId);
        await _reportConfigRepository.Delete(reportConfig);
    }
}

using O24OpenAPI.Core;
using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.Web.Report.Domain;

namespace O24OpenAPI.Web.Report.Services.Interfaces;

public partial interface IReportConfigService
{
    /// <summary>
    /// get a codelist by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ReportConfig> GetById(int id);

    /// <summary>
    /// Get all reportConfig
    /// </summary>
    /// <returns></returns>
    Task<List<ReportConfig>> GetAll();

    /// <summary>
    /// Simple search ReportConfig
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<IPagedList<ReportConfig>> Search(SimpleSearchModel model);

    /// <summary>
    /// create a ReportConfig
    /// </summary>
    /// <param name="reportConfig"></param>
    /// <returns></returns>
    Task<ReportConfig> Create(ReportConfig reportConfig);

    /// <summary>
    /// update a codelist
    /// </summary>
    /// <param name="reportConfig"></param>
    /// <param name="referenceId"></param>
    /// <returns></returns>
    Task Update(ReportConfig reportConfig, string referenceId = "");

    /// <summary>
    /// Delete a ReportConfig
    /// </summary>
    /// <param name="reportConfigId"></param>
    /// <returns></returns>
    Task Delete(int reportConfigId);

    /// <summary>
    ///
    /// </summary>
    /// <param name="code_template"></param>
    /// <returns></returns>
    Task<ReportConfig> GetByCodeTemplate(string code_template);

    /// <summary>
    /// GetByCode
    /// </summary>
    /// <param name="code_template"></param>
    /// <returns>ReportConfig</returns>
    Task<ReportConfig> GetByCode(string reportCode);
}

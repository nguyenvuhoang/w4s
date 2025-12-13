using O24OpenAPI.Web.Report.Domain;

namespace O24OpenAPI.Web.Report.Services.Interfaces;

public interface ITemplateReportService
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    Task<ReportTemplate> GetByCodeTemplate(string code);

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    // Task<IPagedList<TemplateReportSearchResponseModel>> Search(SimpleSearchModel model);

    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ReportTemplate> GetById(int id);

    /// <summary>
    ///
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<ReportTemplate> Update(ReportTemplate entity);

    /// <summary>
    ///
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<ReportTemplate> Insert(ReportTemplate entity);

    /// <summary>
    ///
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task Delete(ReportTemplate entity);
}

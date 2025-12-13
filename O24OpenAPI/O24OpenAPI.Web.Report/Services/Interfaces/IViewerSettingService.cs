using O24OpenAPI.Web.Report.Domain;

namespace O24OpenAPI.Web.Report.Services.Interfaces;

public partial interface IViewerSettingService
{
    /// <summary>
    /// get a codelist by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ViewerSetting> GetById(int id);

    /// <summary>
    /// create a ViewerSetting
    /// </summary>
    /// <param name="reportConfig"></param>
    /// <returns></returns>
    Task<ViewerSetting> Create(ViewerSetting reportConfig);

    /// <summary>
    /// update a codelist
    /// </summary>
    /// <param name="reportConfig"></param>
    /// <param name="referenceId"></param>
    /// <returns></returns>
    Task Update(ViewerSetting reportConfig, string referenceId = "");

    /// <summary>
    /// Delete a ViewerSetting
    /// </summary>
    /// <param name="reportConfigId"></param>
    /// <returns></returns>
    Task Delete(int reportConfigId);

    /// <summary>
    ///
    /// </summary>
    /// <param name="codeTemplate"></param>
    /// <returns></returns>
    Task<ViewerSetting> GetByCodeTemplate(string codeTemplate);
}

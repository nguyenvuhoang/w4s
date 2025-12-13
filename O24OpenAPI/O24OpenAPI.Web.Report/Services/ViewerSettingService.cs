using O24OpenAPI.Core;
using O24OpenAPI.Data;
using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Web.Framework.Localization;
using O24OpenAPI.Web.Report.Domain;
using O24OpenAPI.Web.Report.Services.Interfaces;

namespace O24OpenAPI.Web.Report.Services.Services;

/// <summary>
/// ViewerSetting service constructor
/// </summary>
/// <param name="viewerSettingRepository"></param>
/// <param name="localizationService"></param>
public partial class ViewerSettingService(
    IRepository<ViewerSetting> viewerSettingRepository,
    ILocalizationService localizationService
) : IViewerSettingService
{
    private readonly IRepository<ViewerSetting> _viewerSettingRepository =
        viewerSettingRepository;

    private readonly ILocalizationService _localizationService = localizationService;

    /// <summary>
    /// Get a viewerSetting by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual async Task<ViewerSetting> GetById(int id)
    {
        return await _viewerSettingRepository.GetById(id);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="code_template"></param>
    /// <returns></returns>
    public virtual async Task<ViewerSetting> GetByCodeTemplate(string code_template)
    {
        return await _viewerSettingRepository
            .Table.Where(s => s.CodeTemplate.Equals(code_template))
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Create a viewerSetting
    /// </summary>
    /// <param name="viewerSetting"></param>
    /// <returns></returns>
    public virtual async Task<ViewerSetting> Create(ViewerSetting viewerSetting)
    {
        await _viewerSettingRepository.Insert(viewerSetting);
        return viewerSetting;
    }

    /// <summary>
    /// Update a viewerSetting
    /// </summary>
    /// <param name="viewerSetting"></param>
    /// <param name="referenceId"></param>
    /// <returns></returns>
    public virtual async Task Update(ViewerSetting viewerSetting, string referenceId = "")
    {
        await _viewerSettingRepository.Update(viewerSetting, referenceId);
    }

    /// <summary>
    /// Delete a viewerSetting
    /// </summary>
    /// <param name="viewerSettingId"></param>
    /// <returns></returns>
    public virtual async Task Delete(int viewerSettingId)
    {
        var viewerSetting = await GetById(viewerSettingId);
        if (viewerSetting == null)
        {
            throw new O24OpenAPIException(
                await _localizationService.GetResource("Report.ViewerSetting.Value.NotFound")
            );
        }

        await _viewerSettingRepository.Delete(viewerSetting);
    }
}

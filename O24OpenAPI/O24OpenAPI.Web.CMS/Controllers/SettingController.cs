using Microsoft.AspNetCore.Mvc;
using O24OpenAPI.Web.Framework.Controllers;
using O24OpenAPI.Web.Framework.Services.Configuration;

namespace O24OpenAPI.Web.CMS.Controllers;

public class SettingController(ISettingService settingService) : BaseController
{
    private readonly ISettingService _settingService = settingService;

    [HttpPost]
    public virtual async Task<IActionResult> Update(string key, string value)
    {
        var setting = await _settingService.GetSetting(key);
        setting.Value = value;
        await _settingService.UpdateSetting(setting);
        return Ok(setting);
    }

    [HttpGet]
    public virtual async Task<IActionResult> GetWebAppSetting(string key)
    {
        await Task.CompletedTask;
        var setting = EngineContext.Current.Resolve<WebApiSettings>();
        return Ok(setting);
    }
}

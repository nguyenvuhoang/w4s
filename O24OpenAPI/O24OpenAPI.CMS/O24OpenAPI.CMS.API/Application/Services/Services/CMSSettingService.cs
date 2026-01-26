using LinqToDB;
using O24OpenAPI.CMS.API.Application.Services.Interfaces;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Configuration;
using O24OpenAPI.Framework.Services.Configuration;

namespace O24OpenAPI.CMS.API.Application.Services.Services;

public class CMSSettingService(
    IRepository<Setting> settingRepository,
    IStaticCacheManager staticCacheManager,
    ISettingService settingService
) : ICMSSettingService
{
    private readonly IRepository<Setting> _settingRepository = settingRepository;
    private readonly IStaticCacheManager _staticCacheManager = staticCacheManager;
    private readonly ISettingService _settingService = settingService;

    /// <summary>
    /// Create
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public virtual async Task<Setting> Create(Models.SettingCreateModel model)
    {
        var getSetting = await GetByPrimaryKey(model.Name);
        if (getSetting != null)
        {
            throw new O24OpenAPIException("CMS.Setting.Value.Exist");
        }

        var newModel = new Setting()
        {
            Name = model.Name,
            Value = model.Value,
            OrganizationId = model.OrganizationId,
        };
        await Insert(newModel, model.ReferenceId);
        getSetting = await GetByPrimaryKey(newModel.Name);
        await _staticCacheManager.RemoveByPrefix(O24OpenAPIEntityCacheDefaults<Setting>.Prefix);
        var allSettingKey = O24OpenApiConfigurationDefaults.SettingsAllAsDictionaryCacheKey;
        await _staticCacheManager.RemoveByPrefix(allSettingKey.Key);
        await _settingService.ClearCache();
        return getSetting;
    }

    public Task<Setting> Delete(int id, string referenceId = "")
    {
        throw new NotImplementedException();
    }

    public Task<Setting> GetById(int id)
    {
        throw new NotImplementedException();
    }

    /// Dependencues class
    /// <summary>
    /// GetByPrimaryKey
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public virtual async Task<Setting> GetByPrimaryKey(string name)
    {
        var query = await _settingRepository.Table.Where(s => s.Name == name).FirstOrDefaultAsync();
        return query;
    }

    public async Task<string> GetStringValue(string name)
    {
        var setting = await GetByPrimaryKey(name);
        return setting?.Value ?? string.Empty;
    }

    public Task Insert(Setting value, string referenceId = "")
    {
        throw new NotImplementedException();
    }

    public Task<IPagedList<Setting>> Search(SimpleSearchModel model)
    {
        throw new NotImplementedException();
    }

    public Task<IPagedList<Setting>> Search(Models.SettingSearchModel model)
    {
        throw new NotImplementedException();
    }

    public Task<Models.SettingUpdateModel> Update(
        Models.SettingUpdateModel model,
        string referenceId = ""
    )
    {
        throw new NotImplementedException();
    }

    public Task<Setting> View(int id)
    {
        throw new NotImplementedException();
    }
}

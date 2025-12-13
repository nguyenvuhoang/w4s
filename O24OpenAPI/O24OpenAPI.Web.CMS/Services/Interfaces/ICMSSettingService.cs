using O24OpenAPI.Core.Domain.Configuration;

namespace O24OpenAPI.Web.CMS.Services;

public interface ICMSSettingService
{
    /// <summary>
    /// Create
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<Setting> Create(Models.SettingCreateModel model);

    /// <summary>
    /// Delete
    /// </summary>
    /// <param name="id"></param>
    /// <param name="referenceId"></param>
    /// <returns></returns>
    Task<Setting> Delete(int id, string referenceId = "");

    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Setting> GetById(int id);

    /// <summary>
    /// GetByPrimaryKey
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<Setting> GetByPrimaryKey(string name);

    /// <summary>
    /// Insert
    /// </summary>
    /// <param name="value"></param>
    /// <param name="referenceId"></param>
    /// <returns></returns>
    Task Insert(Setting value, string referenceId = "");

    /// <summary>
    /// Simple search
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<IPagedList<Setting>> Search(SimpleSearchModel model);

    /// <summary>
    /// Advanced Search
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<IPagedList<Setting>> Search(Models.SettingSearchModel model);

    /// <summary>
    /// Update
    /// </summary>
    /// <param name="model"></param>
    /// <param name="referenceId"></param>
    /// <returns></returns>
    Task<Models.SettingUpdateModel> Update(
        Models.SettingUpdateModel model,
        string referenceId = ""
    );

    /// <summary>
    /// View
    /// </summary>
    /// /// <param name="id"></param>
    /// <returns></returns>
    Task<Setting> View(int id);

    Task<string> GetStringValue(string name);
}

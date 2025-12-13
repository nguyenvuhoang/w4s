using O24OpenAPI.Web.CMS.Models;

namespace O24OpenAPI.Web.CMS.Services.Interfaces;

public partial interface IUserInRoleService
{
    /// <summary>
    /// get a role of user by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<UserInRole> GetById(int id);

    /// <summary>
    /// Get a role of user by role id and user id
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="userCode"></param>
    /// <returns></returns>
    Task<UserInRole> GetByRoleIdAndUserCode(int roleId, string userCode);

    /// <summary>
    /// simple search model
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<IPagedList<UserInRoleSearchResponseModel>> Search(SimpleSearchModel model);

    /// <summary>
    /// advanced search role of user
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<IPagedList<UserInRoleSearchResponseModel>> Search(UserInRoleSearchModel model);

    /// <summary>
    /// create a role of user
    /// </summary>
    /// <param name="UserInRole"></param>
    /// <returns></returns>
    Task<UserInRole> Create(UserInRole UserInRole);

    /// <summary>
    /// update a role of user
    /// </summary>
    /// <param name="UserInRole"></param>
    /// <param name="referenceId"></param>
    /// <returns></returns>
    Task Update(UserInRole UserInRole, string referenceId = "");

    /// <summary>
    /// delete a role of user
    /// </summary>
    /// <param name="UserInRoleCode"></param>
    /// <returns></returns>
    Task Delete(UserInRole userInRole);

    /// <summary>
    /// checking role of user
    /// </summary>
    /// <param name="userAccountId"></param>
    /// <returns></returns>
    public Task<bool> IsUserHaveRole(int userAccountId);

    /// <summary>
    /// checking is role have any user
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public Task<bool> IsRoleHavingUser(int roleId);

    /// <summary>
    ///
    /// </summary>
    /// <param name="userCode"></param>
    /// <returns></returns>
    Task<List<UserInRole>> GetListRoleByUserCode(string userCode);

    Task<List<UserInRole>> GetListUserByRoleId(int roleId);
}

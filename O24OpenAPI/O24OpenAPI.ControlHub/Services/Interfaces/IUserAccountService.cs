using O24OpenAPI.APIContracts.Models.CTH;
using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.ControlHub.Models;
using O24OpenAPI.ControlHub.Models.Roles;
using O24OpenAPI.ControlHub.Models.User;
using O24OpenAPI.Core;

namespace O24OpenAPI.ControlHub.Services.Interfaces;

/// <summary>
/// The user account service interface
/// </summary>
public interface IUserAccountService
{
    /// <summary>
    /// Gets the or add user account using the specified user id
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <param name="loginName">The login name</param>
    /// <param name="password">The password</param>
    /// <param name="channelId">The channel id</param>
    /// <returns>A task containing the user account</returns>
    Task<UserAccount> GetLoginAccount(string loginName, string password, string channelId, string language);

    Task<UserAccount> AddAsync(UserAccount user);
    Task<UserAccount> GetByUserIdAsync(string userId);
    /// <summary>
    /// Get User by code
    /// </summary>
    /// <param name="userCode"></param>
    /// <returns></returns>
    Task<UserAccount> GetByUserCodeAsync(string userCode);

    /// <summary>
    /// Get User by Login name
    /// </summary>
    /// <param name="loginName"></param>
    /// <returns></returns>
    Task<UserAccount> GetByLoginNameAsync(string loginName);
    /// <summary>
    /// Get By phone Number Async
    /// </summary>
    /// <param name="phone"></param>
    /// <returns></returns>
    Task<UserAccount> GetByPhoneNumberAsync(string phone);
    /// <summary>
    /// Get User by Login name and Channel ID
    /// </summary>
    /// <param name="loginName"></param>
    /// <param name="channelid"></param>
    /// <returns></returns>
    Task<UserAccount> GetByLoginNameandChannelAsync(string loginName, string channelid);
    /// <summary>
    /// Get User by Login name and Email
    /// </summary>
    /// <param name="loginName"></param>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<UserAccount> GetByLoginNameAndEmailAsync(string loginName, string email, string phonenumber);

    Task<List<UserInRole>> ListOfRole(string usercode);
    /// <summary>
    /// Update UserAccount
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task UpdateAsync(UserAccount entity);
    /// <summary>
    /// Get UserCode By Contract Number
    /// </summary>
    /// <param name="contractnumber"></param>
    /// <returns></returns>
    Task<string> GetUserCodeByContractNumber(string contractnumber);
    /// <summary>
    /// Check if user exists by user name
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    Task<bool> IsExist(string userName);
    /// <summary>
    /// Get User by RoleId
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<IPagedList<UserAccount>> GetUserByRoleIdASync(ModelWithRoleId model);
    /// <summary>
    /// Update User Role ASync
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<bool> UpdateUserRoleASync(UpdateUserRoleModel model);
    /// <summary>
    /// Create User Role ASync
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<bool> CreateUserRoleAsync(CreateUserRoleModel model);

    /// <summary>
    /// Update User ASync
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<UpdateUserResponseModel> UpdateUserAsync(UpdateUserRequestModel model);
    /// <summary>
    /// Is Phone Number Existing ASync
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<bool> IsPhoneNumberExistingAsync(UserAccountPhoneModel model);
    /// <summary>
    /// Is Username Existing ASync
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<bool> IsUserNameExistingAsync(DefaultModel model);
    /// <summary>
    /// Change Device ASync
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<AuthResponseModel> ChangeDeviceAsync(UserAccountChangeDeviceModel model);
    /// <summary>
    /// Verify password
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<bool> VerifyPasswordAsync(VerifyPasswordModel model);
    /// <summary>
    /// Delete user account by user id
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task DeleteUserByUserIdAsync(string userId);
    /// <summary>
    /// Delete user account by contract number
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<bool> DeleteUserByContractAsync(DeleteUserContractModel model);
    /// <summary>
    /// Transition user status asynchronously
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<bool> TransitionUserStatusAsync(TransitionUserStatusModel model);
    /// <summary>
    /// Delete user by id
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<bool> DeleteUserById(DeleteUserModel model);
    /// <summary>
    /// Sync user info
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<bool> SyncUserInfoAsync(SyncUserInfoModel model);
    /// <summary>
    /// Get User by Contract Number
    /// </summary>
    /// <param name="contractnumber"></param>
    /// <returns></returns>
    Task<UserAccount> GetUserByContractNumber(string contractnumber);
    /// <summary>
    /// Unblock User Async
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    Task<bool> UnBlockUserAsync(UnblockUserModel model);
    /// <summary>
    /// Get User Push ID by Contract Number
    /// </summary>
    /// <param name="contractNumber"></param>
    /// <returns></returns>
    Task<CTHUserPushModel> GetUserPushIdByContractNumberAsync(string contractNumber);
    /// <summary>
    /// Update User Avatar Async
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<bool> UpdateUserAvatarAsync(UpdateUserAvatarRequestModel model);
    /// <summary>
    /// Scan Password
    /// </summary>
    /// <returns></returns>
    Task<ScanResult> ScanPassword(string password);
    /// <summary>
    /// Role Scan
    /// </summary>
    /// <param name="roleid"></param>
    /// <returns></returns>
    Task<ScanResult> ScanRole(int roleid);
    /// <summary>
    /// Get User by Phone Number ASync
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<UserInfoModel> GetUserByPhoneNumberASync(UserWithPhoneNumber model);
    /// <summary>
    /// Check User Has Email
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<string> CheckUserHasEmail(DefaultModel model);

}

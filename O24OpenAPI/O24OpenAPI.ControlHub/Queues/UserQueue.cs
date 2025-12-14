using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.ControlHub.Models;
using O24OpenAPI.ControlHub.Models.Roles;
using O24OpenAPI.ControlHub.Models.User;
using O24OpenAPI.ControlHub.Services.Interfaces;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.O24OpenAPIClient.Scheme.Workflow;
using O24OpenAPI.Web.Framework.Extensions;
using O24OpenAPI.Web.Framework.Helpers;
using O24OpenAPI.Web.Framework.Models;
using O24OpenAPI.Web.Framework.Services.Queue;

namespace O24OpenAPI.ControlHub.Queues;

public class UserQueue : BaseQueue
{
    private readonly IUserAccountService _userAccountService =
        EngineContext.Current.Resolve<IUserAccountService>();
    private readonly IUserBannerService _userBannerService =
        EngineContext.Current.Resolve<IUserBannerService>();

    public async Task<WFScheme> GetUserByRoleId(WFScheme wFScheme)
    {
        var model = await wFScheme.ToModel<ModelWithRoleId>();
        return await Invoke<ModelWithRoleId>(
            wFScheme,
            async () =>
            {
                var response = await _userAccountService.GetUserByRoleIdASync(model);
                return response.ToPagedListModel<UserAccount, UserAccountResponseModel>();
            }
        );
    }

    public async Task<WFScheme> UpdateUserRole(WFScheme wFScheme)
    {
        var model = await wFScheme.ToModel<UpdateUserRoleModel>();
        return await Invoke<UpdateUserRoleModel>(
            wFScheme,
            async () =>
            {
                var response = await _userAccountService.UpdateUserRoleASync(model);
                return response;
            }
        );
    }

    /// <summary>
    /// Create a new user role in the system.
    /// </summary>
    /// <param name="wFScheme"></param>
    /// <returns></returns>
    public async Task<WFScheme> CreateUserRole(WFScheme wFScheme)
    {
        var model = await wFScheme.ToModel<CreateUserRoleModel>();
        return await Invoke<CreateUserRoleModel>(
            wFScheme,
            async () =>
            {
                var response = await _userAccountService.CreateUserRoleAsync(model);
                return response;
            }
        );
    }

    public async Task<WFScheme> UpdateUser(WFScheme wFScheme)
    {
        var model = await wFScheme.ToModel<UpdateUserRequestModel>();
        return await Invoke<UpdateUserRequestModel>(
            wFScheme,
            async () =>
            {
                var response = await _userAccountService.UpdateUserAsync(model);
                return response;
            }
        );
    }

    public async Task<WFScheme> IsPhoneNumberExisting(WFScheme wFScheme)
    {
        var model = await wFScheme.ToModel<UserAccountPhoneModel>();
        return await Invoke<UserAccountPhoneModel>(
            wFScheme,
            async () =>
            {
                var response = await _userAccountService.IsPhoneNumberExistingAsync(model);
                return response;
            }
        );
    }

    public async Task<WFScheme> IsUserNameExisting(WFScheme wFScheme)
    {
        var model = await wFScheme.ToModel<DefaultModel>();
        return await Invoke<DefaultModel>(
            wFScheme,
            async () =>
            {
                var response = await _userAccountService.IsUserNameExistingAsync(model);
                return response;
            }
        );
    }

    public async Task<WFScheme> ChangeDevice(WFScheme wFScheme)
    {
        var model = await wFScheme.ToModel<UserAccountChangeDeviceModel>();
        return await Invoke<UserAccountChangeDeviceModel>(
            wFScheme,
            async () =>
            {
                var response = await _userAccountService.ChangeDeviceAsync(model);
                return response;
            }
        );
    }

    public async Task<WFScheme> VerifyPassword(WFScheme wFScheme)
    {
        var model = await wFScheme.ToModel<VerifyPasswordModel>();
        return await Invoke<VerifyPasswordModel>(
            wFScheme,
            async () =>
            {
                var response = await _userAccountService.VerifyPasswordAsync(model);
                return response;
            }
        );
    }

    public async Task<WFScheme> DeleteUser(WFScheme wFScheme)
    {
        var model = await wFScheme.ToModel<DeleteUserModel>();
        return await Invoke<DeleteUserModel>(
            wFScheme,
            async () =>
            {
                var response = await _userAccountService.DeleteUserById(model);
                return response;
            }
        );
    }

    public async Task<WFScheme> TransitionUserStatus(WFScheme wFScheme)
    {
        var model = await wFScheme.ToModel<TransitionUserStatusModel>();
        return await Invoke<TransitionUserStatusModel>(
            wFScheme,
            async () =>
            {
                var response = await _userAccountService.TransitionUserStatusAsync(model);
                return response;
            }
        );
    }

    public async Task<WFScheme> SyncUserInfo(WFScheme wFScheme)
    {
        var model = await wFScheme.ToModel<SyncUserInfoModel>();
        return await Invoke<SyncUserInfoModel>(
            wFScheme,
            async () =>
            {
                var response = await _userAccountService.SyncUserInfoAsync(model);
                return response;
            }
        );
    }

    public async Task<WFScheme> UnblockUser(WFScheme wFScheme)
    {
        var model = await wFScheme.ToModel<UnblockUserModel>();
        return await Invoke<UnblockUserModel>(
            wFScheme,
            async () =>
            {
                var response = await _userAccountService.UnBlockUserAsync(model);
                return response;
            }
        );
    }

    /// <summary>
    /// Update user avatar
    /// </summary>
    /// <param name="wFScheme"></param>
    /// <returns></returns>
    public async Task<WFScheme> UpdateUserAvatar(WFScheme wFScheme)
    {
        var model = await wFScheme.ToModel<UpdateUserAvatarRequestModel>();
        return await Invoke<UpdateUserAvatarRequestModel>(
            wFScheme,
            async () =>
            {
                var response = await _userAccountService.UpdateUserAvatarAsync(model);
                return response;
            }
        );
    }

    /// <summary>
    /// Get user by phone number
    /// </summary>
    /// <param name="wFScheme"></param>
    /// <returns></returns>
    public async Task<WFScheme> GetUserByPhoneNumber(WFScheme wFScheme)
    {
        var model = await wFScheme.ToModel<UserWithPhoneNumber>();
        return await Invoke<UserWithPhoneNumber>(
            wFScheme,
            async () =>
            {
                var response = await _userAccountService.GetUserByPhoneNumberASync(model);
                return response;
            }
        );
    }

    /// <summary>
    /// Update user banner
    /// </summary>
    /// <param name="wFScheme"></param>
    /// <returns></returns>
    public async Task<WFScheme> UpdateUserBanner(WFScheme wFScheme)
    {
        var model = await wFScheme.ToModel<UserBannerModel>();
        return await Invoke<UserBannerModel>(
            wFScheme,
            async () =>
            {
                var response = await _userBannerService.UpdateUserBannerAsync(model);
                return response;
            }
        );
    }

    /// <summary>
    /// Check User Has Email
    /// </summary>
    /// <param name="wFScheme"></param>
    /// <returns></returns>
    public async Task<WFScheme> CheckUserHasEmail(WFScheme wFScheme)
    {
        var model = await wFScheme.ToModel<DefaultModel>();
        return await Invoke<DefaultModel>(
            wFScheme,
            async () =>
            {
                var response = await _userAccountService.CheckUserHasEmail(model);
                return response;
            }
        );
    }

    /// <summary>
    /// Get User by User Code
    /// </summary>
    /// <param name="wFScheme"></param>
    /// <returns></returns>
    public async Task<WFScheme> GetByUserCode(WFScheme wFScheme)
    {
        var model = await wFScheme.ToModel<BaseTransactionModel>();
        return await Invoke<BaseTransactionModel>(
            wFScheme,
            async () =>
            {
                var response = await _userAccountService.GetByUserCodeAsync(model.CurrentUserCode);
                return response;
            }
        );
    }
}

using Newtonsoft.Json.Linq;
using O24OpenAPI.ControlHub.Models;
using O24OpenAPI.ControlHub.Models.Request;
using O24OpenAPI.ControlHub.Models.Response;
using O24OpenAPI.ControlHub.Models.Roles;

namespace O24OpenAPI.ControlHub.Services.Interfaces;

/// <summary>
/// The auth service interface
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Authenticates the model
    /// </summary>
    /// <param name="model">The model</param>
    /// <returns>A task containing the auth response model</returns>
    Task<AuthResponseModel> Authenticate(LoginToO24OpenAPIRequestModel model);
    /// <summary>
    /// Creates the supper admin using the specified model
    /// </summary>
    /// <param name="model">The model</param>
    /// <returns>A task containing the bool</returns>
    Task<bool> CreateSupperAdmin(LoginToO24OpenAPIRequestModel model);
    /// <summary>
    /// Create User Request Model
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<UserResponseModel> CreateUserAsync(CreateUserRequestModel model);

    /// <summary>
    /// Logins the by supper admin using the specified model
    /// </summary>
    /// <param name="model">The model</param>
    /// <returns>A task containing the auth response model</returns>
    Task<AuthResponseModel> LoginBySupperAdmin(LoginToO24OpenAPIRequestModel model);

    /// <summary>
    /// Get Application Info
    /// </summary>
    /// <param name="workflow"></param>
    /// <returns></returns>
    Task<ApplicationInfoResponseModel> ApplicationInfo(ApplicationInfoModel model);
    /// <summary>
    /// Change Password by O24User
    /// </summary>
    /// <param name="workflow"></param>
    /// <returns></returns>
    Task<JToken> ChangePasswordByO24User(ChangePasswordO24OpenAPIRequestModel model);
    /// <summary>
    /// Change Owner Password Async
    /// </summary>
    /// <param name="mode"></param>
    /// <returns></returns>
    Task<bool> ChangeOwnerPasswordAsync(ChangeOwnerRequestModel model);
    /// <summary>
    /// Verify User Async
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<VerifyUserResponseModel> VerifyUserAsync(VerifyUserRequestModel model);
    /// <summary>
    /// Register User Authen
    /// </summary>
    /// <param name="workflow"></param>
    /// <returns></returns>
    Task<bool> RegisterUserAuthenAsync(RegisterUserAuthenModel model);
    /// <summary>
    /// Verify SmartOTP Code
    /// </summary>
    /// <param name="VerifyUserAuthenModel"></param>
    /// <returns></returns>
    Task<VerifySmartOTPResponseModel> VerifySmartOTPCodeAsync(VerifyUserAuthenModel model);

    /// <summary>
    /// RefreshTokenAsync
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<AuthResponseModel> RefreshTokenAsync(RefreshTokenRequest model);
    /// <summary>
    /// Logout
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<bool> LogoutAsync(LogoutO24OpenAPIRequestModel model);
    /// <summary>
    /// Reset Password
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<ResetPasswordResponseModel> ResetPasswordAsync(ResetPasswordRequestModel model);
    /// <summary>
    /// Check Is Login
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<bool> IsLoginAsync(DefaultModel model);
    /// <summary>
    /// RefreshToken
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<AuthResponseModel> RefreshTokenTeller(RefreshTokenTellerRequest model);

    Task<bool> DeactivateSmartOTPAsync(RegisterUserAuthenModel model);
}

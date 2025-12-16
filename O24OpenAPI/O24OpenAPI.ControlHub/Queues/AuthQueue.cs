using O24OpenAPI.Client.Scheme.Workflow;
using O24OpenAPI.ControlHub.Models;
using O24OpenAPI.ControlHub.Models.Request;
using O24OpenAPI.ControlHub.Services.Interfaces;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.Framework.Services.Queue;

namespace O24OpenAPI.ControlHub.Queues;

/// <summary>
/// The auth queue class
/// </summary>
/// <seealso cref="BaseQueue"/>
public class AuthQueue : BaseQueue
{
    /// <summary>
    /// The auth service
    /// </summary>
    private readonly IAuthService _authService = EngineContext.Current.Resolve<IAuthService>();

    /// <summary>
    /// Logins the to o 24 open api using the specified wf scheme
    /// </summary>
    /// <param name="wfScheme">The wf scheme</param>
    /// <returns>A task containing the wf scheme</returns>
    public async Task<WFScheme> LoginToO24OpenAPI(WFScheme wfScheme)
    {
        var model = await wfScheme.ToModel<LoginToO24OpenAPIRequestModel>();
        return await Invoke<LoginToO24OpenAPIRequestModel>(
            wfScheme,
            async () =>
            {
                var response = await _authService.Authenticate(model);
                return response;
            }
        );
    }

    /// <summary>
    /// Creates the s admin using the specified wf scheme
    /// </summary>
    /// <param name="wfScheme">The wf scheme</param>
    /// <returns>A task containing the wf scheme</returns>
    public async Task<WFScheme> CreateSAdmin(WFScheme wfScheme)
    {
        var model = await wfScheme.ToModel<LoginToO24OpenAPIRequestModel>();
        return await Invoke<LoginToO24OpenAPIRequestModel>(
            wfScheme,
            async () =>
            {
                var response = await _authService.CreateSupperAdmin(model);
                return response;
            }
        );
    }

    /// <summary>
    /// Creates the s admin using the specified wf scheme
    /// </summary>
    /// <param name="wfScheme">The wf scheme</param>
    /// <returns>A task containing the wf scheme</returns>
    public async Task<WFScheme> CreateUser(WFScheme wfScheme)
    {
        var model = await wfScheme.ToModel<CreateUserRequestModel>();
        return await Invoke<CreateUserRequestModel>(
            wfScheme,
            async () =>
            {
                var response = await _authService.CreateUserAsync(model);
                return response;
            }
        );
    }

    public async Task<WFScheme> ApplicationInfo(WFScheme workflow)
    {
        var model = await workflow.ToModel<ApplicationInfoModel>();
        return await Invoke<ApplicationInfoModel>(
            workflow,
            async () =>
            {
                var response = await _authService.ApplicationInfo(model);
                return response;
            }
        );
    }

    /// <summary>
    /// Change password
    /// </summary>
    /// <param name="wfScheme">The wf scheme</param>
    /// <returns>A task containing the wf scheme</returns>
    public async Task<WFScheme> ChangePassword(WFScheme wfScheme)
    {
        var model = await wfScheme.ToModel<ChangePasswordO24OpenAPIRequestModel>();
        return await Invoke<ChangePasswordO24OpenAPIRequestModel>(
            wfScheme,
            async () =>
            {
                var response = await _authService.ChangePasswordByO24User(model);
                return response;
            }
        );
    }

    public async Task<WFScheme> ChangeOwnerPassword(WFScheme wfScheme)
    {
        var model = await wfScheme.ToModel<ChangeOwnerRequestModel>();
        return await Invoke<ChangeOwnerRequestModel>(
            wfScheme,
            async () =>
            {
                var response = await _authService.ChangeOwnerPasswordAsync(model);
                return response;
            }
        );
    }

    /// <summary>
    /// Change password
    /// </summary>
    /// <param name="wfScheme">The wf scheme</param>
    /// <returns>A task containing the wf scheme</returns>
    public async Task<WFScheme> VerifyUser(WFScheme wfScheme)
    {
        var model = await wfScheme.ToModel<VerifyUserRequestModel>();
        return await Invoke<VerifyUserRequestModel>(
            wfScheme,
            async () =>
            {
                var response = await _authService.VerifyUserAsync(model);
                return response;
            }
        );
    }

    /// <summary>
    /// Register User Authen
    /// </summary>
    /// <param name="wfScheme">The wf scheme</param>
    /// <returns>A task containing the wf scheme</returns>
    public async Task<WFScheme> RegisterUserAuthen(WFScheme wfScheme)
    {
        var model = await wfScheme.ToModel<RegisterUserAuthenModel>();
        return await Invoke<RegisterUserAuthenModel>(
            wfScheme,
            async () =>
            {
                var response = await _authService.RegisterUserAuthenAsync(model);
                return response;
            }
        );
    }

    /// <summary>
    /// Verify SmartOTP Code
    /// </summary>
    /// <param name="wfScheme">The wf scheme</param>
    /// <returns>A task containing the wf scheme</returns>
    public async Task<WFScheme> VerifySmartOTPCode(WFScheme wfScheme)
    {
        var model = await wfScheme.ToModel<VerifyUserAuthenModel>();
        return await Invoke<VerifyUserAuthenModel>(
            wfScheme,
            async () =>
            {
                var response = await _authService.VerifySmartOTPCodeAsync(model);
                return response;
            }
        );
    }

    /// <summary>
    /// RefreshToken
    /// </summary>
    /// <param name="workflow"></param>
    /// <returns></returns>
    public async Task<WFScheme> RefreshToken(WFScheme wfScheme)
    {
        var model = await wfScheme.ToModel<RefreshTokenRequest>();
        return await Invoke<RefreshTokenRequest>(
            wfScheme,
            async () =>
            {
                var response = await _authService.RefreshTokenAsync(model);
                return response;
            }
        );
    }

    /// <summary>
    /// RefreshTokenTeller
    /// </summary>
    /// <param name="workflow"></param>
    /// <returns></returns>
    public async Task<WFScheme> RefreshTokenTeller(WFScheme wfScheme)
    {
        var model = await wfScheme.ToModel<RefreshTokenTellerRequest>();
        return await Invoke<RefreshTokenTellerRequest>(
            wfScheme,
            async () =>
            {
                var response = await _authService.RefreshTokenTeller(model);
                return response;
            }
        );
    }

    /// <summary>
    /// Logout the O24 open api using the specified wf scheme
    /// </summary>
    /// <param name="wfScheme">The wf scheme</param>
    /// <returns>A task containing the wf scheme</returns>
    public async Task<WFScheme> LogoutO24OpenAPI(WFScheme wfScheme)
    {
        var model = await wfScheme.ToModel<LogoutO24OpenAPIRequestModel>();
        return await Invoke<LogoutO24OpenAPIRequestModel>(
            wfScheme,
            async () =>
            {
                var response = await _authService.LogoutAsync(model);
                return response;
            }
        );
    }

    /// <summary>
    /// ResetPassword
    /// </summary>
    /// <param name="wfScheme">The wf scheme</param>
    /// <returns>A task containing the wf scheme</returns>
    public async Task<WFScheme> ResetPassword(WFScheme wfScheme)
    {
        var model = await wfScheme.ToModel<ResetPasswordRequestModel>();
        return await Invoke<ResetPasswordRequestModel>(
            wfScheme,
            async () =>
            {
                var response = await _authService.ResetPasswordAsync(model);
                return response;
            }
        );
    }

    /// <summary>
    /// IsLogin
    /// </summary>
    /// <param name="wfScheme">The wf scheme</param>
    /// <returns>A task containing the wf scheme</returns>
    public async Task<WFScheme> IsLogin(WFScheme wfScheme)
    {
        var model = await wfScheme.ToModel<DefaultModel>();
        return await Invoke<DefaultModel>(
            wfScheme,
            async () =>
            {
                var response = await _authService.IsLoginAsync(model);
                return response;
            }
        );
    }

    public async Task<WFScheme> DeactivateSmartOTPAsync(WFScheme wfScheme)
    {
        var model = await wfScheme.ToModel<RegisterUserAuthenModel>();
        return await Invoke<RegisterUserAuthenModel>(
            wfScheme,
            async () =>
            {
                bool response = await _authService.DeactivateSmartOTPAsync(model);
                return response;
            }
        );
    }
}

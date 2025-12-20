using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Framework.Services;
using O24OpenAPI.Web.CMS.Constant;
using O24OpenAPI.Web.CMS.Models;
using O24OpenAPI.Web.CMS.Models.ContextModels;
using O24OpenAPI.Web.CMS.Models.Request;
using O24OpenAPI.Web.CMS.Services.Factory;
using O24OpenAPI.Web.CMS.Services.Interfaces;

namespace O24OpenAPI.Web.CMS.Services.WorkflowService.CoreBanking;

public class AuthenticateWorkflow : BaseQueueService
{
    private readonly ICoreAuthenticateService _coreAuthenticateService =
        EngineFactory.Resolve<ICoreAuthenticateService>();
    private readonly IUserSessionsService _userSessionsService =
        EngineContext.Current.Resolve<IUserSessionsService>();
    private readonly JWebUIObjectContextModel _context =
        EngineContext.Current.Resolve<JWebUIObjectContextModel>();

    public async Task<WorkflowScheme> Login(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<AuthenJWTModel>();
        return await Invoke<AuthenJWTModel>(
            workflow,
            async () =>
            {
                if (string.IsNullOrEmpty(model.DeviceId))
                {
                    throw await ErrorUtils.Required(nameof(model.DeviceId));
                }
                var validateSessionModel = await _userSessionsService.CheckValidSingleSession(
                    model.LoginName,
                    true
                );
                if (!validateSessionModel.IsValid)
                {
                    throw await ErrorUtils.CreateException(
                        validateSessionModel.ErrorCode,
                        model.LoginName,
                        validateSessionModel.IpAddress,
                        validateSessionModel.DeviceName
                    );
                }
                var result = await _coreAuthenticateService.Login(model);
                return result;
            }
        );
    }

    public async Task<WorkflowScheme> Logout(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<BaseTransactionModel>();
        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var result = await _coreAuthenticateService.Logout();
                return result.ToJObject();
            }
        );
    }

    public async Task<WorkflowScheme> RefreshToken(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<RefreshTokenRequest>();
        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var userSessions = await _userSessionsService.GetByRefreshToken(model.RefreshToken);
                if (userSessions == null)
                {
                    throw await ErrorUtils.BuildException(
                        "INVALID_SESSION_REFRESH",
                        ModuleCode.TellerApp
                    );
                }
                var loginName = userSessions.LoginName;
                var validateSessionModel = await _userSessionsService.CheckValidSingleSession(
                    loginName,
                    true
                );
                if (!validateSessionModel.IsValid)
                {
                    throw await ErrorUtils.CreateException(
                        validateSessionModel.ErrorCode,
                        loginName,
                        validateSessionModel.IpAddress,
                        validateSessionModel.DeviceName
                    );
                }
                var commandSet = System.Text.Json.JsonSerializer.Deserialize<HashSet<string>>(
                    userSessions.CommandList
                );
                _context.Bo.AddPackFo("rolecommand", commandSet);
                var (newUserSessions, token, refreshToken) =
                    await _userSessionsService.RefreshSession(userSessions);
                var response = new LoginCoreResponse(newUserSessions)
                {
                    Token = token,
                    RefreshToken = refreshToken,
                };
                return response;
            }
        );
    }

    public async Task<WorkflowScheme> ChangePassword(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<ChangePasswordModel>();
        return await Invoke<ChangePasswordModel>(
            workflow,
            async () =>
            {
                var result = await _coreAuthenticateService.ChangePassword(model);
                return result;
            }
        );
    }
}

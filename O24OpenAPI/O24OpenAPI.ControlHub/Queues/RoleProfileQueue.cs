using O24OpenAPI.ControlHub.Models.Roles;
using O24OpenAPI.ControlHub.Services.Interfaces;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.O24OpenAPIClient.Scheme.Workflow;
using O24OpenAPI.Web.Framework.Extensions;
using O24OpenAPI.Web.Framework.Services.Queue;

namespace O24OpenAPI.ControlHub.Queues;

public class RoleProfileQueue : BaseQueue
{
    private readonly IRoleProfileService _roleProfileService =
        EngineContext.Current.Resolve<IRoleProfileService>();

    /// <summary>
    /// LoadOperation
    /// </summary>
    /// <param name="wFScheme"></param>
    /// <returns></returns>
    public async Task<WFScheme> LoadOperation(WFScheme wFScheme)
    {
        var model = await wFScheme.ToModel<UserCommandRequestModel>();
        return await Invoke<UserCommandRequestModel>(
            wFScheme,
            async () =>
            {
                var response = await _roleProfileService.LoadRoleOperationAsync(model);
                return response;
            }
        );
    }

    /// <summary>
    /// LoadMenuByChannel
    /// </summary>
    /// <param name="wFScheme"></param>
    /// <returns></returns>
    public async Task<WFScheme> LoadMenuByChannel(WFScheme wFScheme)
    {
        var model = await wFScheme.ToModel<UserCommandRequestModel>();
        return await Invoke<UserCommandRequestModel>(
            wFScheme,
            async () =>
            {
                var response = await _roleProfileService.LoadMenuByChannelAsync(model);
                return response;
            }
        );
    }

    /// <summary>
    /// UpdateUserRight
    /// </summary>
    /// <param name="wFScheme"></param>
    /// <returns></returns>
    public async Task<WFScheme> UpdateUserRight(WFScheme wFScheme)
    {
        var model = await wFScheme.ToModel<UserRightUpdateModel>();
        return await Invoke<UserRightUpdateModel>(
            wFScheme,
            async () =>
            {
                var response = await _roleProfileService.UpdateUserRightAsync(model);
                return response;
            }
        );
    }
}

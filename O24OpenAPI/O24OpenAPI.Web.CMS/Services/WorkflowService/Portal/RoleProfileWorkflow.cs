using O24OpenAPI.Framework.Services;
using O24OpenAPI.Web.CMS.Models;
using O24OpenAPI.Web.CMS.Models.Portal;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using O24OpenAPI.Web.CMS.Services.Services.Portal;

namespace O24OpenAPI.Web.CMS.Services.WorkflowService.Portal;

public class RoleProfileWorkflow(
    IRoleProfileService roleProfileService,
    IUserInRoleService userInRoleService
) : BaseQueueService
{
    private readonly IRoleProfileService _roleProfileService = roleProfileService;
    private readonly IUserInRoleService _userInRoleService = userInRoleService;

    public async Task<WorkflowScheme> LoadMenuByChannel(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<ModelWithChannel>();
        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var response = await _roleProfileService.LoadMenuByChannel(model);
                return new StatusResponse(response);
            }
        );
    }

    public async Task<WorkflowScheme> LoadOperation(WorkflowScheme workflow)
    {
        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var response = await _roleProfileService.LoadRoleOperation();
                return new StatusResponse(response);
            }
        );
    }

    public async Task<WorkflowScheme> UpdateUserRight(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<UserRightUpdateModel>();
        return await Invoke<UserRightUpdateModel>(
            workflow,
            async () =>
            {
                await _roleProfileService.UpdateUserRight(model);
                return new StatusResponse("true");
            }
        );
    }

    public async Task<WorkflowScheme> UpdateUserInRole(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<UpdateUserInRoleModel>();
        return await Invoke<UpdateUserInRoleModel>(
            workflow,
            async () =>
            {
                await _roleProfileService.UpdateUserInRole(model);
                return new StatusResponse("true");
            }
        );
    }

    public async Task<WorkflowScheme> GetLisUserInRole(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<ModelWithRoleId>();
        return await Invoke<ModelWithRoleId>(
            workflow,
            async () =>
            {
                var r = await _userInRoleService.GetListUserByRoleId(model.RoleId);
                return r;
            }
        );
    }

    public async Task<WorkflowScheme> GetLisRoleByUser(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<ModelWithUserCode>();
        return await Invoke<ModelWithRoleId>(
            workflow,
            async () =>
            {
                var r = await _userInRoleService.GetListRoleByUserCode(model.UserCode);
                return r;
            }
        );
    }
}

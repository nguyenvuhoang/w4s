using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Web.CMS.Constant;
using O24OpenAPI.Web.CMS.Models;
using O24OpenAPI.Web.CMS.Models.ContextModels;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using O24OpenAPI.Framework.Services;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace O24OpenAPI.Web.CMS.Services.WorkflowService;

public class UserCommandWorkflow(IUserCommandService userCommandService) : BaseQueueService
{
    private readonly IUserCommandService _userCommandService = userCommandService;
    private readonly JWebUIObjectContextModel _context =
        EngineContext.Current.Resolve<JWebUIObjectContextModel>();

    /// <summary>
    /// Get Menu By App And Role
    /// </summary>
    /// <param name="workflow"></param>
    /// <returns></returns>
    public async Task<WorkflowScheme> GetMenuByAppAndRole(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<BaseTransactionModel>();
        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                if (
                    _context.InfoUser.UserSession.CommandList.TryParse(
                        out HashSet<string> roleCommand
                    )
                )
                {
                    var menu = await _userCommandService.GetMenuByAppAndRole(
                        model.AppCode,
                        roleCommand
                    );
                    var listResponse = new List<UserCommandModel>();
                    var response = JsonSerializer.Deserialize<List<UserCommandModel>>(
                        menu.ToSerializeSystemText()
                    );
                    _context.Bo.AddPackFo("commandmenu", response);

                    return new StatusResponse(TranStatus.COMPLETED);
                }
                return new StatusResponse(TranStatus.FAILED);
            }
        );
    }


    /// <summary>
    /// Load User Command
    /// </summary>
    /// <param name="workflow"></param>
    /// <returns></returns>

    public async Task<WorkflowScheme> LoadUserCommand(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<BaseTransactionModel>();
        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                return await _userCommandService.LoadUserCommand();
            }
        );
    }
}

using O24OpenAPI.Web.CMS.Configuration;
using O24OpenAPI.Web.CMS.Constant;
using O24OpenAPI.Web.CMS.Models;
using O24OpenAPI.Web.CMS.Models.ContextModels;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using O24OpenAPI.Framework.Services;
using ICodeListService = O24OpenAPI.Web.CMS.Services.Interfaces.ICodeListService;

namespace O24OpenAPI.Web.CMS.Services.WorkflowService.TellerApp;

public class CachingWorkflow(
    ICodeListService codeListService,
    IUserCommandService userCommandService,
    JWebUIObjectContextModel context,
    IParaServerService paraServerService,
    CMSSetting cMSSetting


) : BaseQueueService
{
    public JWebUIObjectContextModel Context { get; set; } = context;
    private readonly ICodeListService _codeListService = codeListService;
    private readonly IUserCommandService _userCommandService = userCommandService;
    private readonly IParaServerService _paraServerService = paraServerService;
    private readonly CMSSetting _cMSSetting = cMSSetting;

    public async Task<WorkflowScheme> ReloadCache(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<BaseTransactionModel>();
        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var cacheCodeList = await _codeListService.GetByApp(model.AppCode);
                List<UserCommandResponse> userCommand = await _userCommandService.LoadUserCommand();
                List<ParaServer> paraServer = await _paraServerService.GetByApp(model.AppCode);
                Context.Bo.AddPackFo("cacheCodeList", cacheCodeList);
                Context.Bo.AddPackFo("cacheUserCommand", userCommand);
                Context.Bo.AddPackFo("cacheParaServer", paraServer);
                Context.Bo.AddPackFo("ListF8Transaction", _cMSSetting.ListF8Transaction.MapToModel<HashSet<String>>());

                return new StatusResponse(TranStatus.COMPLETED);
            }
        );
    }
}

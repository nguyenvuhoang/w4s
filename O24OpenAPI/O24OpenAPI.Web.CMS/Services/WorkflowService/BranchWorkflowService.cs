using O24OpenAPI.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Framework.Services;
using O24OpenAPI.Web.CMS.Models.Digital;
using O24OpenAPI.Web.CMS.Services.Interfaces;

namespace O24OpenAPI.Web.CMS.Services.WorkflowService;

public class BranchWorkflowService : BaseQueueService
{
    private readonly IBranchService _branchService =
        EngineContext.Current.Resolve<IBranchService>();

    public async Task<WorkflowScheme> GetAll(WorkflowScheme workflow)
    {
        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var list = await _branchService.GetAll();
                return list;
            }
        );
    }

    public async Task<WorkflowScheme> GetByBranchCode(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<GetBranchByBranchCodeModel>();

        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var branch = await _branchService.GetByBranchCode(model.BranchID);
                return branch;
            }
        );
    }

    public async Task<WorkflowScheme> Insert(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<BranchModel>();

        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var newBranch = new D_BRANCH();
                newBranch = model.ToEntity(newBranch);
                var branch = await _branchService.Insert(newBranch);
                return branch;
            }
        );
    }

    public async Task<WorkflowScheme> Update(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<BranchUpdateModel>();

        if (string.IsNullOrEmpty(model.BranchID))
        {
            throw new O24OpenAPIException("InvalidBranchCode", "The Branch Code is required");
        }

        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var branch = await _branchService.GetByBranchCode(model.BranchID);

                var updateBranch = model.ToEntity(branch);
                await _branchService.Update(updateBranch);
                return updateBranch;
            }
        );
    }

    public async Task<WorkflowScheme> DeleteById(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<GetBranchByBranchCodeModel>();

        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var branch = await _branchService.DeleteById(model.BranchID);
                return branch;
            }
        );
    }

    public async Task<WorkflowScheme> DeleteByListID(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<DeleteBranchByBranchCodeModel>();

        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                await _branchService.DeleteByListID(model);
                return model;
            }
        );
    }

    public async Task<WorkflowScheme> Search(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<SearchBranchModel>();

        return await Invoke<SearchBranchModel>(
            workflow,
            async () =>
            {
                var list = await _branchService.Search(model);
                return list;
            }
        );
    }
}

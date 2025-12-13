using O24OpenAPI.Web.CMS.Models.Digital;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using O24OpenAPI.Web.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Web.Framework.Services;

namespace O24OpenAPI.Web.CMS.Services.WorkflowService;

public class TelcoWorkflowService : BaseQueueService
{
    private readonly ITelcoService _telcoService = EngineContext.Current.Resolve<ITelcoService>();
    public async Task<WorkflowScheme> GetAll(WorkflowScheme workflow)
    {
        return await Invoke<BaseTransactionModel>(workflow, async ()=>
        {
            var list = await _telcoService.GetAll();
            return list;
        });
    }
    public async Task<WorkflowScheme> GetByTelcoName(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<GetTelcoByTelcoNameModel>();

        return await Invoke<BaseTransactionModel>(workflow, async () =>
        {
            var telco = await _telcoService.GetByTelcoName(model.TelcoName);
            return telco;
        });
    }
    public async Task<WorkflowScheme> Insert(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<TelcoModel>();

        return await Invoke<BaseTransactionModel>(workflow, async () =>
        {
            var newTelco = new D_TOP_TELCO();
            newTelco = model.ToEntity(newTelco);
            var telco = await _telcoService.Insert(newTelco);
            return telco;
        });
    }
    public async Task<WorkflowScheme> Update(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<TelcoUpdateModel>();

        if (string.IsNullOrEmpty(model.TelcoName))
        {

            throw new O24OpenAPIException("InvalidTelcoName", "The Telco Name is required");
        }

        if (string.IsNullOrEmpty(model.TelcoCode))
        {
            throw new O24OpenAPIException("InvalidTelcoCode", "The Telco Code is required");
        }

        return await Invoke<BaseTransactionModel>(workflow, async () =>
        {
            var telco = await _telcoService.GetByTelcoID(model.TelcoID);

            if (telco == null)
            {
                throw new O24OpenAPIException("InvalidCity", "The city does not exist in system");
            }

            var updateTelco = model.ToEntity(telco);
            await _telcoService.Update(updateTelco);
            return updateTelco;
        });
    }
    public async Task<WorkflowScheme> DeleteById(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<GetTelcoByTelcoCodeModel>();

        return await Invoke<BaseTransactionModel>(workflow, async () =>
        {
            var telco = await _telcoService.DeleteById(model.TelcoID);
            return telco;
        });
    }
    public async Task<WorkflowScheme> Search(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<SearchTelcoModel>();

        return await Invoke<SearchTelcoModel>(workflow, async () =>
        {
            var list = await _telcoService.Search(model);
            return list;
        });
    }
}

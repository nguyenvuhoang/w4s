using O24OpenAPI.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Framework.Services;
using O24OpenAPI.Web.CMS.Models.Digital;
using O24OpenAPI.Web.CMS.Services.Interfaces;

namespace O24OpenAPI.Web.CMS.Services.WorkflowService;

public class CurrencyWorkflowService : BaseQueueService
{
    private readonly ICurrencyService _currencyService =
        EngineContext.Current.Resolve<ICurrencyService>();

    public async Task<WorkflowScheme> GetAll(WorkflowScheme workflow)
    {
        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var list = await _currencyService.GetAll();
                return list;
            }
        );
    }

    public async Task<WorkflowScheme> GetByCurrencyCode(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<GetCurrencyByCurrencyCodeModel>();

        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var currency = await _currencyService.GetByCurrencyCode(model.CurrencyCode);
                return currency;
            }
        );
    }

    public async Task<WorkflowScheme> Insert(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<CurrencyModel>();

        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var newCurrency = new D_CURRENCY();
                newCurrency = model.ToEntity(newCurrency);
                var currency = await _currencyService.Insert(newCurrency);
                return currency;
            }
        );
    }

    public async Task<WorkflowScheme> Update(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<CurrencyModel>();

        if (string.IsNullOrEmpty(model.CurrencyCode))
        {
            throw new O24OpenAPIException("InvalidCurrencyCode", "The Currency Code is required");
        }

        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var newCurrency = new D_CURRENCY();
                newCurrency = model.ToEntity(newCurrency);
                var currency = await _currencyService.Update(newCurrency);
                return currency;
            }
        );
    }

    public async Task<WorkflowScheme> DeleteByListID(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<DeleteCurrencyByCurrencyCodeModel>();

        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                await _currencyService.DeleteById(model);
                return model;
            }
        );
    }

    public async Task<WorkflowScheme> Search(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<SearchCurrencyModel>();

        return await Invoke<SearchCurrencyModel>(
            workflow,
            async () =>
            {
                var list = await _currencyService.Search(model);
                return list;
            }
        );
    }
}

using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Helpers;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.W4S.API.Application.Models.Currency;
using O24OpenAPI.W4S.Domain.AggregatesModel.CommonAggregate;

namespace O24OpenAPI.W4S.API.Application.Features.Currencys;

public class SimpleSearchCurrencyCommand
    : SimpleSearchModel,
        ICommand<PagedListModel<CurrencyResponseModel>>
{
    public string StatusOfCurrency { get; set; } = "";
}

[CqrsHandler]
public class SimpleSearchCurrencyHandle(ICurrencyRepository currencyRepository)
    : ICommandHandler<
        SimpleSearchCurrencyCommand,
        PagedListModel<CurrencyResponseModel>
    >
{
    [WorkflowStep(WorkflowStepCode.W4S.WF_STEP_W4S_SIMPLE_SEARCH_CURRENCY)]
    public async Task<PagedListModel<CurrencyResponseModel>> HandleAsync(
        SimpleSearchCurrencyCommand request,
        CancellationToken cancellationToken = default
    )
    {

        IQueryable<Currency> q = currencyRepository.Table;
        if (!string.IsNullOrWhiteSpace(request.SearchText))
        {
            var st = request.SearchText.Trim();
            var status = request.StatusOfCurrency;
            q = q.Where(s => s.CurrencyId == st);
        }
        if (!string.IsNullOrWhiteSpace(request.StatusOfCurrency))
        {
            var status = request.StatusOfCurrency.Trim();
            q = q.Where(s => s.StatusOfCurrency == status);
        }
        q = q.OrderBy(x => x.DisplayOrder);
        return await q.ToPagedListModelAsync(
            request.PageIndex,
            request.PageSize,
            items => items.ToCurrencyResponseModelList(),
            cancellationToken
        );

    }
}

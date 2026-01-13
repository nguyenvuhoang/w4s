using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.W4S.API.Application.Models.Currency;
using O24OpenAPI.W4S.Domain.AggregatesModel.CommonAggregate;

namespace O24OpenAPI.W4S.API.Application.Features.Currencys;

public class SimpleSearchCurrencyCommand
    : SimpleSearchModel,
        ICommand<PagedListModel<Currency, CurrencyResponseModel>>
{ }

[CqrsHandler]
public class SimpleSearchCurrencyHandle(ICurrencyRepository currencyRepository)
    : ICommandHandler<
        SimpleSearchCurrencyCommand,
        PagedListModel<Currency, CurrencyResponseModel>
    >
{
    [WorkflowStep(WorkflowStepCode.W4S.WF_STEP_W4S_SIMPLE_SEARCH_CURRENCY)]
    public async Task<PagedListModel<Currency, CurrencyResponseModel>> HandleAsync(
        SimpleSearchCurrencyCommand request,
        CancellationToken cancellationToken = default
    )
    {
        IPagedList<Currency> currencies = await currencyRepository.GetAllPaged(
            query =>
            {
                if (!string.IsNullOrEmpty(request.SearchText))
                {
                    query = query.Where(c => c.CurrencyId.Contains(request.SearchText));
                }

                query = query.OrderBy(c => c.DisplayOrder);
                return query;
            },
            request.PageIndex,
            request.PageSize
        );

        var result = new PagedListModel<Currency, CurrencyResponseModel>(
            currencies
        );
        return result;
    }
}

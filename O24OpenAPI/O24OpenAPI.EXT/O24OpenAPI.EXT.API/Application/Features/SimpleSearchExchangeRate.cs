using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.EXT.API.Application.Models;
using O24OpenAPI.EXT.Domain.AggregatesModel.ExchangeRateAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Helpers;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.EXT.API.Application.Features;

public class SimpleSearchExchangeRateCommand
    : SimpleSearchModel,
        ICommand<PagedListModel<ExchangeRateResponseModel>>
{ }

[CqrsHandler]
public class SimpleSearchExchangeRateHandle(IExchangeRateRepository exchangeRateRepository)
    : ICommandHandler<
        SimpleSearchExchangeRateCommand,
        PagedListModel<ExchangeRateResponseModel>
    >
{
    [WorkflowStep(WorkflowStepCode.EXT.WF_STEP_EXT_SEARCH_EXCHANGE_RATE)]
    public async Task<PagedListModel<ExchangeRateResponseModel>> HandleAsync(
        SimpleSearchExchangeRateCommand request,
        CancellationToken cancellationToken = default
    )
    {

        IQueryable<ExchangeRate> q = exchangeRateRepository.Table;
        if (!string.IsNullOrWhiteSpace(request.SearchText))
        {
            var st = request.SearchText.Trim();
            q = q.Where(s => s.CurrencyCode == st);
        }
        q = q.OrderBy(x => x.CurrencyCode);
        return await q.ToPagedListModelAsync(
            request.PageIndex,
            request.PageSize,
            items => items.ToExchangeRateResponseModelList(),
            cancellationToken
        );

    }
}

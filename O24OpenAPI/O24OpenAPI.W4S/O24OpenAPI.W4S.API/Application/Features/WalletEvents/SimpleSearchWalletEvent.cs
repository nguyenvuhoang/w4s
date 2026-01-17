using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Helpers;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.W4S.API.Application.Models.Wallet;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

namespace O24OpenAPI.W4S.API.Application.Features.WalletEvents;

public class SimpleSearchWalletEventCommand
    : SimpleSearchModel,
        ICommand<PagedListModel<WalleEventResponseModel>>
{
    public string CurrencyCode { get; set; } = "";
}

[CqrsHandler]
public class SimpleSearchWalletEventHandle(IWalletEventRepository walletEventRepository)
    : ICommandHandler<
        SimpleSearchWalletEventCommand,
        PagedListModel<WalleEventResponseModel>
    >
{
    [WorkflowStep(WorkflowStepCode.W4S.WF_STEP_W4S_RETRIEVE_WALLET_EVENT)]
    public async Task<PagedListModel<WalleEventResponseModel>> HandleAsync(
        SimpleSearchWalletEventCommand request,
        CancellationToken cancellationToken = default
    )
    {
        IQueryable<WalletEvent> q = walletEventRepository.Table;
        if (!string.IsNullOrWhiteSpace(request.SearchText))
        {
            var st = request.SearchText.Trim();
            q = q.Where(s => s.Title == st);
        }
        if (!string.IsNullOrWhiteSpace(request.CurrencyCode))
        {
            var currencyCode = request.CurrencyCode.Trim();
            q = q.Where(s => s.CurrencyCode == currencyCode);
        }
        q = q.OrderBy(x => x.Id);
        return await q.ToPagedListModelAsync(
            request.PageIndex,
            request.PageSize,
            items => items.ToWalleEventResponseModelList(),
            cancellationToken
        );
    }
}

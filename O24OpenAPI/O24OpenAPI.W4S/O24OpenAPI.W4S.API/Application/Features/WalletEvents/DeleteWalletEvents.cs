using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

namespace O24OpenAPI.W4S.API.Application.Features.WalletEvents;

public class DeleteWalletEventsCommand
    : BaseTransactionModel,
        ICommand<bool>
{
    public int EventId
    {
        get; set;
    }
}

[CqrsHandler]
public class DeleteWalletEventsHandler(
    IWalletEventRepository walletEventRepository
) : ICommandHandler<DeleteWalletEventsCommand, bool>
{
    [WorkflowStep(WorkflowStepCode.W4S.WF_STEP_W4S_DELETE_WALLET_EVENT)]
    public async Task<bool> HandleAsync(
        DeleteWalletEventsCommand request,
        CancellationToken ct = default
    )
    {
        var nowUtc = DateTime.UtcNow;

        WalletEvent? evt = await walletEventRepository.GetById(request.EventId);

        if (evt == null)
            return false;

        await walletEventRepository.Delete(evt);

        return true;
    }
}

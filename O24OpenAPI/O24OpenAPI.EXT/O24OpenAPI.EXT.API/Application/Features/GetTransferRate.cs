using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.EXT.API.Application.Models;
using O24OpenAPI.EXT.Domain.AggregatesModel.ExchangeRateAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.EXT.API.Application.Features;

public class GetTransferRateCommand
    : BaseTransactionModel,
        ICommand<List<TransferRateResponseModel>>
{ }

[CqrsHandler]
public class GetTransferRateHandle(IExchangeRateRepository exchangeRateRepository)
    : ICommandHandler<
        GetTransferRateCommand,
        List<TransferRateResponseModel>
    >
{
    [WorkflowStep(WorkflowStepCode.EXT.WF_STEP_EXT_RETRIEVE_TRANSFER_RATE)]
    public async Task<List<TransferRateResponseModel>> HandleAsync(
        GetTransferRateCommand request,
        CancellationToken cancellationToken = default
    )
    {
        var rates = await exchangeRateRepository.Table.OrderBy(x => x.CurrencyCode).ToListAsync(cancellationToken);

        return [.. rates.Select(rate => new TransferRateResponseModel
        {
            CurrencyCode = rate.CurrencyCode,
            Transfer = rate.Transfer
        })];
    }
}

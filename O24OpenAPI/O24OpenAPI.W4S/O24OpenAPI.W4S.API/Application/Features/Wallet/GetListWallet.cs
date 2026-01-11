using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.Logging.Extensions;
using O24OpenAPI.W4S.API.Application.Constants;
using O24OpenAPI.W4S.API.Application.Models.Wallet;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

namespace O24OpenAPI.W4S.API.Application.Features.Wallet;

public class GetListWalletCommand : BaseTransactionModel, ICommand<List<ListWalletResponseModel>>
{
    public string ContractNumber { get; set; } = default!;
}

[CqrsHandler]
public class GetListWalletHandler(IWalletProfileRepository walletProfileRepository)
    : ICommandHandler<GetListWalletCommand, List<ListWalletResponseModel>>
{
    [WorkflowStep(WorkflowStepCode.W4S.WF_STEP_W4S_GET_LIST_WALLET)]
    public async Task<List<ListWalletResponseModel>> HandleAsync(
        GetListWalletCommand request,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.ContractNumber))
                throw await O24Exception.CreateAsync(
                    O24W4SResourceCode.Validation.WalletContractNotFound,
                    request.Language,
                    request.ContractNumber
                );

            string contractNumber = request.ContractNumber.Trim();

            var wallets =
                await walletProfileRepository.GetByContractNumber(contractNumber)
                ?? throw await O24Exception.CreateAsync(
                    O24W4SResourceCode.Validation.WalletContractNotFound,
                    request.Language,
                    contractNumber
                );

            var result = wallets
                .Select(w => new ListWalletResponseModel
                {
                    WalletId = w.Id,
                    ContractNumber = w.ContractNumber,
                    UserCode = w.UserCode ?? string.Empty,
                    WalletName = w.WalletName,
                    WalletType = w.WalletType,
                    DefaultCurrency = w.DefaultCurrency,
                    Status = w.Status,
                })
                .ToList();

            return result;
        }
        catch (Exception ex)
        {
            ex.WriteError();
            throw;
        }
    }
}

using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core.Constants;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.W4S.API.Application.Constants;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;
using O24OpenAPI.W4S.Domain.Constants;

namespace O24OpenAPI.W4S.API.Application.Features.Wallet;

public class RetrieveWalletBalanceCommand : BaseTransactionModel, ICommand<RetrieveWalletBalanceResponseModel>
{
    public string ContractNumber { get; set; }
}

public class RetrieveWalletBalanceResponseModel : BaseO24OpenAPIModel
{
    public int WalletId { get; set; }
    public string CurrencyCode { get; set; }
    public decimal AvailableBalance { get; set; }
}

[CqrsHandler]
public class RetrieveWalletBalanceHandler(
    IWalletProfileRepository walletProfileRepository,
    IWalletAccountProfileRepository walletAccountProfileRepository,
    IWalletBalanceRepository walletBalanceRepository
) : ICommandHandler<RetrieveWalletBalanceCommand, RetrieveWalletBalanceResponseModel>
{
    [WorkflowStep(WorkflowStepCode.W4S.WF_STEP_W4S_RETRIEVE_WALLET_BALANCE)]
    public async Task<RetrieveWalletBalanceResponseModel> HandleAsync(
        RetrieveWalletBalanceCommand request,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            await ValidateRequest(request);

            var contractNumber = request.ContractNumber.Trim();

            var walletProfiles = await walletProfileRepository.GetByContractNumber(contractNumber)
                ?? throw await O24Exception.CreateAsync(
                    O24W4SResourceCode.Validation.WalletContractNotFound,
                    request.Language,
                    contractNumber
                );

            var walletIds = walletProfiles.Select(x => x.Id).ToList();

            var walletAccounts = await walletAccountProfileRepository
                .GetWalletAccountByWalletIdAsync(walletIds);

            if (walletAccounts.Count == 0)
                return new RetrieveWalletBalanceResponseModel
                {
                    WalletId = walletProfiles.First().Id,
                    AvailableBalance = 0,
                    CurrencyCode = string.Empty
                };

            var totalAvailableBalance = await (
                from wa in walletAccountProfileRepository.Table
                join wb in walletBalanceRepository.Table
                    on wa.AccountNumber equals wb.AccountNumber
                where walletIds.Contains(wa.WalletId)
                select
                    (wa.AccountType == WalletAccountType.Income ? 1m : -1m)
                    * wb.AvailableBalance
            ).SumAsync(cancellationToken);

            return new RetrieveWalletBalanceResponseModel
            {
                WalletId = walletProfiles.First().Id,
                AvailableBalance = totalAvailableBalance,
                CurrencyCode = walletAccounts.First().CurrencyCode
            };
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync();
            throw;
        }
    }



    /// <summary>
    /// Validate request
    /// </summary>
    /// <param name="r"></param>
    /// <param name="language"></param>
    /// <returns></returns>
    private async static Task ValidateRequest(
        RetrieveWalletBalanceCommand r
    )
    {
        if (string.IsNullOrWhiteSpace(r.ContractNumber))
            throw await O24Exception.CreateAsync(
                ResourceCode.Validation.RequiredField,
                r.Language,
                "ContractNumber"
            );
    }

}

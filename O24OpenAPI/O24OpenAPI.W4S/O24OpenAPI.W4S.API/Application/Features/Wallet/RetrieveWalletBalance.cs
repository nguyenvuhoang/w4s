using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core.Constants;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.W4S.API.Application.Constants;
using O24OpenAPI.W4S.API.Application.Models.Currency;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;
using O24OpenAPI.W4S.Infrastructure.Configurations;

namespace O24OpenAPI.W4S.API.Application.Features.Wallet;

public class RetrieveWalletBalanceCommand : BaseTransactionModel, ICommand<RetrieveWalletBalanceResponseModel>
{
    public string ContractNumber { get; set; }
    public string CurrencyCode { get; set; }
    public List<TransferRateResponseModel> TransferRates { get; set; } = [];
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
    IWalletBalanceRepository walletBalanceRepository,
    W4SSetting w4SSetting
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

            var baseCurrency = request.CurrencyCode;

            var rateMap = (request.TransferRates ?? [])
            .Where(x => !string.IsNullOrWhiteSpace(x.CurrencyCode))
            .GroupBy(x => x.CurrencyCode.Trim().ToUpperInvariant())
            .ToDictionary(
                g => g.Key,
                g => g.First().Transfer
            );

            var accountNumbers = walletAccounts
            .Select(x => x.AccountNumber!)
            .Distinct()
            .ToList();

            var rows = await walletBalanceRepository.Table
                .Where(wb => accountNumbers.Contains(wb.AccountNumber))
                .Select(wb => new
                {
                    Amount = wb.AvailableBalance,
                    Currency = wb.CurrencyCode ?? baseCurrency
                })
                .ToListAsync(cancellationToken);


            decimal totalAvailableBalance = 0m;

            foreach (var x in rows)
            {
                var currency = (x.Currency ?? baseCurrency).Trim().ToUpperInvariant();

                decimal rate;
                if (string.IsNullOrWhiteSpace(baseCurrency) || currency == baseCurrency)
                {
                    rate = 1m;
                }
                else
                {
                    if (!rateMap.TryGetValue(currency, out var r) || r == null)
                    {
                        throw await O24Exception.CreateAsync(
                            O24W4SResourceCode.Validation.ExchangeRateNotFound,
                            request.Language,
                            $"{currency}->{baseCurrency}"
                        );
                    }

                    rate = r.Value;
                }

                totalAvailableBalance += x.Amount * rate;
            }


            return new RetrieveWalletBalanceResponseModel
            {
                WalletId = walletProfiles.First().Id,
                AvailableBalance = totalAvailableBalance,
                CurrencyCode = baseCurrency
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

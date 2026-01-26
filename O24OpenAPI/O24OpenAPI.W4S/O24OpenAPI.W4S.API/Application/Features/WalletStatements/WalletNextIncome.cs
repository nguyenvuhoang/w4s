using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Localization;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.W4S.API.Application.Constants;
using O24OpenAPI.W4S.API.Application.Models.Currency;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;
using O24OpenAPI.W4S.Domain.AggregatesModel.WalletMasterAggregate;
using O24OpenAPI.W4S.Infrastructure.Configurations;

namespace O24OpenAPI.W4S.API.Application.Features.WalletStatements;

public class WalletNextIncomeCommand : BaseTransactionModel, ICommand<WalletNextIncomeResponseModel>
{
    public string ContractNumber { get; set; } = default!;

    /// <summary>
    /// Rates are passed from outside (e.g. Core/CBG) and used to convert to BaseCurrency.
    /// Expected: amountBase = amount * Transfer
    /// </summary>
    public List<TransferRateResponseModel> TransferRates { get; set; } = [];
}

public class NetBalanceDetailModel
{
    public string Label { get; set; } = default!;
    public decimal Amount { get; set; }
}

public class NetBalanceSummaryModel
{
    public decimal Total { get; set; }
    public List<NetBalanceDetailModel> Details { get; set; } = [];
}

public class WalletNextIncomeResponseModel : BaseO24OpenAPIModel
{
    public NetBalanceSummaryModel NetBalance { get; set; } = new();
    public string BaseCurrencyCode { get; set; } = "VND";
}

[CqrsHandler]
public class WalletNextIncomeHandler(
    IWalletProfileRepository walletProfileRepository,
    IWalletStatementRepository walletStatementRepository,
    W4SSetting w4SSettings,
    ILocalizationService localizationService
) : ICommandHandler<WalletNextIncomeCommand, WalletNextIncomeResponseModel>
{
    [WorkflowStep(WorkflowStepCode.W4S.WF_STEP_W4S_WALLET_NET_INCOME)]
    public async Task<WalletNextIncomeResponseModel> HandleAsync(
        WalletNextIncomeCommand request,
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

            var contractNumber = request.ContractNumber.Trim();

            var walletProfiles =
                await walletProfileRepository.GetByContractNumber(contractNumber)
                ?? throw await O24Exception.CreateAsync(
                    O24W4SResourceCode.Validation.WalletContractNotFound,
                    request.Language,
                    contractNumber
                );

            var walletIds = walletProfiles.Select(x => x.Id).Distinct().ToList();

            if (walletIds.Count == 0)
                throw await O24Exception.CreateAsync(
                    O24W4SResourceCode.Validation.WalletContractNotFound,
                    request.Language,
                    contractNumber
                );

            var baseCurrency = string.IsNullOrWhiteSpace(w4SSettings.BaseCurrency)
                ? "VND"
                : w4SSettings.BaseCurrency.Trim();

            // "Opening" = balance as of now (latest statement before now)
            var now = DateTime.UtcNow;

            // "Closing" = balance at next income checkpoint.
            // Default: first day of next month 00:00 UTC (change if your business logic differs)
            var nextDateUtc = new DateTime(
                now.Year,
                now.Month,
                1,
                0,
                0,
                0,
                DateTimeKind.Utc
            ).AddMonths(1);

            // Build rate map from request.TransferRates
            var rateMap = BuildRateMapFromRequest(request.TransferRates, baseCurrency);

            // Opening balances: last closing before now
            var openingRows = await GetLatestClosingBeforeAsync(walletIds, now, cancellationToken);

            // Closing balances: last closing before nextDateUtc
            var closingRows = await GetLatestClosingBeforeAsync(
                walletIds,
                nextDateUtc,
                cancellationToken
            );

            var openingBase = SumClosingToBase(openingRows, baseCurrency, rateMap);
            var closingBase = SumClosingToBase(closingRows, baseCurrency, rateMap);

            var openingLabel = await localizationService.GetByName(
                O24W4SResourceCode.WalletStatement.OpeningBalance,
                request.Language
            );

            var closingLabel = await localizationService.GetByName(
                O24W4SResourceCode.WalletStatement.ClosingBalance,
                request.Language
            );

            return new WalletNextIncomeResponseModel
            {
                BaseCurrencyCode = baseCurrency,
                NetBalance = new NetBalanceSummaryModel
                {
                    Total = closingBase - openingBase,
                    Details =
                    [
                        new NetBalanceDetailModel
                        {
                            Label = openingLabel.ResourceValue,
                            Amount = openingBase,
                        },
                        new NetBalanceDetailModel
                        {
                            Label = closingLabel.ResourceValue,
                            Amount = closingBase,
                        },
                    ],
                },
            };
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync();
            throw;
        }
    }

    /// <summary>
    /// For each (WalletId, CurrencyCode) get latest ClosingBalance before a cutoff time.
    /// </summary>
    private async Task<List<(string CurrencyCode, decimal? Closing)>> GetLatestClosingBeforeAsync(
        List<int> walletIds,
        DateTime beforeUtc,
        CancellationToken cancellationToken
    )
    {
        var rows = await walletStatementRepository
            .Table.Where(x => walletIds.Contains(x.WalletId) && x.TransactionOnUtc < beforeUtc)
            .GroupBy(x => new { x.WalletId, x.CurrencyCode })
            .Select(g => new
            {
                g.Key.CurrencyCode,
                Closing = g.OrderByDescending(s => s.TransactionOnUtc)
                    .ThenByDescending(s => s.Id)
                    .Select(s => s.ClosingBalance)
                    .FirstOrDefault(),
            })
            .ToListAsync(cancellationToken);

        return [.. rows.Select(x => (x.CurrencyCode, Closing: (decimal?)x.Closing))];
    }

    private static decimal SumClosingToBase(
        IEnumerable<(string CurrencyCode, decimal? Closing)> rows,
        string baseCurrency,
        IReadOnlyDictionary<string, decimal> rateMap
    )
    {
        decimal sum = 0m;

        foreach (var r in rows)
        {
            var amt = r.Closing ?? 0m;
            sum += ConvertToBase(amt, r.CurrencyCode, baseCurrency, rateMap);
        }

        return sum;
    }

    /// <summary>
    /// Build rate map from request.TransferRates.
    /// Expected: amountBase = amount * rate
    /// baseCurrency is always 1.
    /// </summary>
    private static Dictionary<string, decimal> BuildRateMapFromRequest(
        IList<TransferRateResponseModel> transferRates,
        string baseCurrency
    )
    {
        var map = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase)
        {
            [baseCurrency] = 1m,
        };

        if (transferRates == null || transferRates.Count == 0)
            return map;

        foreach (var r in transferRates)
        {
            if (r == null)
                continue;

            var ccy = r.CurrencyCode?.Trim();
            if (string.IsNullOrWhiteSpace(ccy))
                continue;

            if (ccy.Equals(baseCurrency, StringComparison.OrdinalIgnoreCase))
            {
                map[ccy] = 1m;
                continue;
            }

            var rate = r.Transfer;
            if (!rate.HasValue || rate.Value <= 0m)
                continue;

            map[ccy] = rate.Value;
        }

        return map;
    }

    private static decimal GetRateToBase(
        string fromCurrency,
        string baseCurrency,
        IReadOnlyDictionary<string, decimal> rateMap
    )
    {
        if (fromCurrency.Equals(baseCurrency, StringComparison.OrdinalIgnoreCase))
            return 1m;

        if (!rateMap.TryGetValue(fromCurrency, out var rate) || rate <= 0m)
            throw new InvalidOperationException(
                $"Missing exchange rate for {fromCurrency}->{baseCurrency}"
            );

        return rate;
    }

    private static decimal ConvertToBase(
        decimal amount,
        string currencyCode,
        string baseCurrency,
        IReadOnlyDictionary<string, decimal> rateMap
    )
    {
        var ccy = string.IsNullOrWhiteSpace(currencyCode) ? baseCurrency : currencyCode.Trim();

        if (ccy.Equals(baseCurrency, StringComparison.OrdinalIgnoreCase))
            return amount;

        var rate = GetRateToBase(ccy, baseCurrency, rateMap);
        return amount * rate;
    }
}

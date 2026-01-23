using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.W4S.API.Application.Constants;
using O24OpenAPI.W4S.API.Application.Helpers;
using O24OpenAPI.W4S.API.Application.Models.Currency;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;
using O24OpenAPI.W4S.Domain.AggregatesModel.WalletMasterAggregate;
using O24OpenAPI.W4S.Infrastructure.Configurations;

namespace O24OpenAPI.W4S.API.Application.Features.WalletStatements;

public class WalletIncomeExpenseSummaryCommand
    : BaseTransactionModel,
        ICommand<WalletIncomeExpenseSummaryResponseModel>
{
    public string ContractNumber { get; set; } = default!;

    /// <summary>D=Day, M=Month, Q=Quarter, H=Half-year, Y=Year</summary>
    public string PeriodType { get; set; } = "M";
    public string PeriodUnit { get; set; }

    /// <summary>
    /// Rates are passed from outside (e.g. Core/CBG) and used to convert to BaseCurrency.
    /// </summary>
    public List<TransferRateResponseModel> TransferRates { get; set; } = [];
}


public class WalletIncomeExpenseSummaryResponseModel : BaseO24OpenAPIModel
{
    public IncomeExpenseSummaryModel IncomeExpenseSummary { get; set; } = new();
}

public class IncomeExpenseSummaryModel
{
    public SummaryItemModel Expense { get; set; } = new();
    public SummaryItemModel Income { get; set; } = new();
}

public class SummaryItemModel
{
    public decimal Total { get; set; }
    public decimal ChangePercent { get; set; }
}

[CqrsHandler]
public class WalletIncomeExpenseSummaryHandler(
    IWalletProfileRepository walletProfileRepository,
    IWalletStatementRepository walletStatementRepository,
    W4SSetting w4SSettings
) : ICommandHandler<WalletIncomeExpenseSummaryCommand, WalletIncomeExpenseSummaryResponseModel>
{
    [WorkflowStep(WorkflowStepCode.W4S.WF_STEP_W4S_WALLET_INCOME_EXPENSE_SUMMARY)]
    public async Task<WalletIncomeExpenseSummaryResponseModel> HandleAsync(
        WalletIncomeExpenseSummaryCommand request,
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

            var walletProfiles = await walletProfileRepository.GetByContractNumber(contractNumber)
                ?? throw await O24Exception.CreateAsync(
                    O24W4SResourceCode.Validation.WalletContractNotFound,
                    request.Language,
                    contractNumber
                );

            // User confirmed: walletProfiles.Id is WalletId
            var walletIds = walletProfiles
                .Select(x => x.Id)
                .Distinct()
                .ToList();

            if (walletIds.Count == 0)
                throw await O24Exception.CreateAsync(
                    O24W4SResourceCode.Validation.WalletContractNotFound,
                    request.Language,
                    contractNumber
                );

            var baseCurrency = string.IsNullOrWhiteSpace(w4SSettings.BaseCurrency)
                ? "VND"
                : w4SSettings.BaseCurrency.Trim();

            var now = DateTime.UtcNow;
            var (thisFrom, thisTo, prevFrom, prevTo) =
                WalletPeriodType.BuildPeriodRangeUtc(now, request.PeriodType);

            // Pull minimal rows (tracker: no ledger)
            var thisRows = await walletStatementRepository.Table
                .Where(x =>
                    walletIds.Contains(x.WalletId) &&
                    x.StatementOnUtc >= thisFrom &&
                    x.StatementOnUtc < thisTo
                )
                .Select(x => new
                {
                    x.EntryType,      // Code.EntryType.DEBIT / CREDIT
                    x.Amount,
                    x.CurrencyCode
                })
                .ToListAsync(cancellationToken);

            var prevRows = await walletStatementRepository.Table
                .Where(x =>
                    walletIds.Contains(x.WalletId) &&
                    x.StatementOnUtc >= prevFrom &&
                    x.StatementOnUtc < prevTo
                )
                .Select(x => new
                {
                    x.EntryType,
                    x.Amount,
                    x.CurrencyCode
                })
                .ToListAsync(cancellationToken);

            // Build rate map from request.TransferRates
            var rateMap = RateHelper.BuildRateMapFromRequest(request.TransferRates, baseCurrency);

            // Sum after conversion to base
            decimal thisIncomeBase = 0m, thisExpenseBase = 0m;

            foreach (var r in thisRows)
            {
                var amtBase = ConvertToBase(r.Amount, r.CurrencyCode, baseCurrency, rateMap);
                AccumulateByEntryType(r.EntryType, amtBase, ref thisIncomeBase, ref thisExpenseBase);
            }


            decimal prevIncomeBase = 0m, prevExpenseBase = 0m;

            foreach (var r in prevRows)
            {
                var amtBase = ConvertToBase(r.Amount, r.CurrencyCode, baseCurrency, rateMap);
                AccumulateByEntryType(r.EntryType, amtBase, ref prevIncomeBase, ref prevExpenseBase);
            }


            return new WalletIncomeExpenseSummaryResponseModel
            {
                IncomeExpenseSummary = new IncomeExpenseSummaryModel
                {
                    Expense = new SummaryItemModel
                    {
                        Total = thisExpenseBase,
                        ChangePercent = CalcChangePercent(thisExpenseBase, prevExpenseBase)
                    },
                    Income = new SummaryItemModel
                    {
                        Total = thisIncomeBase,
                        ChangePercent = CalcChangePercent(thisIncomeBase, prevIncomeBase)
                    }
                }
            };
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync();
            throw;
        }
    }

    private static decimal ConvertToBase(
        decimal amount,
        string? currencyCode,
        string baseCurrency,
        IReadOnlyDictionary<string, decimal> rateMap
    )
    {
        var ccy = string.IsNullOrWhiteSpace(currencyCode) ? baseCurrency : currencyCode.Trim();

        if (ccy.Equals(baseCurrency, StringComparison.OrdinalIgnoreCase))
            return amount;

        var rate = RateHelper.GetRateToBase(ccy, baseCurrency, rateMap);
        return amount * rate;
    }

    private static decimal CalcChangePercent(decimal current, decimal previous)
    {
        if (previous == 0m)
            return current == 0m ? 0m : 100m;

        return Math.Round((current - previous) / previous * 100m, 1, MidpointRounding.AwayFromZero);
    }


    private static void AccumulateByEntryType(
        string? entryType,
        decimal amountBase,
        ref decimal income,
        ref decimal expense
    )
    {
        if (string.IsNullOrWhiteSpace(entryType))
            return;

        var type = entryType.Trim();

        if (string.Equals(type, Code.EntryType.CREDIT, StringComparison.OrdinalIgnoreCase))
        {
            income += amountBase;
        }
        else if (string.Equals(type, Code.EntryType.DEBIT, StringComparison.OrdinalIgnoreCase))
        {
            expense += amountBase;
        }
    }


}

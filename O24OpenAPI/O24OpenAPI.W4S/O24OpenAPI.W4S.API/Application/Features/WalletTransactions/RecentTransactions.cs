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
using O24OpenAPI.W4S.API.Application.Models.WalletTransactions;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;
using O24OpenAPI.W4S.Domain.AggregatesModel.WalletMasterAggregate;

namespace O24OpenAPI.W4S.API.Application.Features.WalletStatements;

public class RecentTransactionsCommand
    : BaseTransactionModel,
        ICommand<RecentTransactionsResponseModel>
{
    public string ContractNumber { get; set; } = default!;
    public int Take { get; set; } = 5;

    /// <summary>Base currency to return amounts (e.g. VND, USD)</summary>
    public string CurrencyCode { get; set; } = "VND";

    /// <summary>Raw rates from outside. Typically quote to VND: 1 CCY = Transfer VND</summary>
    public List<TransferRateResponseModel> TransferRates { get; set; } = [];
}

[CqrsHandler]
public class RecentTransactionsCommandHandler(
    IWalletProfileRepository walletProfileRepository,
    IWalletStatementRepository walletStatementRepository,
    IWalletCategoryRepository walletCategoryRepository
) : ICommandHandler<RecentTransactionsCommand, RecentTransactionsResponseModel>
{
    [WorkflowStep(WorkflowStepCode.W4S.WF_STEP_W4S_WALLET_RECENT_TRANSACTIONS)]
    public async Task<RecentTransactionsResponseModel> HandleAsync(
        RecentTransactionsCommand request,
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

            var walletIds = walletProfiles.Select(x => x.Id).Distinct().ToList();
            if (walletIds.Count == 0)
                throw await O24Exception.CreateAsync(
                    O24W4SResourceCode.Validation.WalletContractNotFound,
                    request.Language,
                    contractNumber
                );

            var take = request.Take <= 0 ? 5 : Math.Min(request.Take, 50);
            var baseCurrency = (request.CurrencyCode ?? "VND").Trim().ToUpperInvariant();

            // --- Build rate map: rawVndRateMap (1 CCY = Transfer VND) -> rateMap (1 CCY = ? baseCurrency)
            var rawVndRateMap = (request.TransferRates ?? [])
                .Where(x =>
                    !string.IsNullOrWhiteSpace(x.CurrencyCode) &&
                    x.Transfer.HasValue &&
                    x.Transfer.Value > 0m
                )
                .GroupBy(x => x.CurrencyCode!.Trim().ToUpperInvariant())
                .ToDictionary(g => g.Key, g => g.Last().Transfer!.Value);

            rawVndRateMap.TryAdd("VND", 1m);

            var rateMap = RateHelper.BuildRateMapByBase(baseCurrency, rawVndRateMap);
            // --- End rate map

            // Pull recent statements
            var rows = await walletStatementRepository.Table
                .Where(x => walletIds.Contains(x.WalletId))
                .OrderByDescending(x => x.StatementOnUtc)
                .Take(take)
                .Select(x => new
                {
                    x.Id,
                    x.ReferenceId,
                    x.EntryType,
                    x.CategoryId,      // int?
                    x.Description,
                    x.Amount,
                    x.CurrencyCode,
                    x.StatementOnUtc
                })
                .ToListAsync(cancellationToken);

            // Load category meta
            var categoryIds = rows
                .Where(x => x.CategoryId > 0)
                .Select(x => x.CategoryId!)
                .Distinct()
                .ToList();

            var categories = categoryIds.Count == 0
                ? []
                : await walletCategoryRepository.GetByIds(categoryIds);

            var categoryMap = categories.ToDictionary(x => x.Id);

            // Convert time to +07:00 string
            var tz = TimeZoneInfo.FindSystemTimeZoneById("Asia/Ho_Chi_Minh");

            var result = new RecentTransactionsResponseModel();

            foreach (var r in rows)
            {
                WalletCategory? cat = null;
                if (r.CategoryId > 0)
                    categoryMap.TryGetValue(r.CategoryId, out cat);

                var type = string.Equals(r.EntryType, Code.EntryType.DEBIT, StringComparison.OrdinalIgnoreCase)
                    ? "EXPENSE"
                    : "INCOME";

                // occurredAt ISO8601 +07:00
                var local = TimeZoneInfo.ConvertTimeFromUtc(
                    DateTime.SpecifyKind(r.StatementOnUtc, DateTimeKind.Utc),
                    tz
                );
                var occurredAt = new DateTimeOffset(local, tz.GetUtcOffset(local)).ToString("o");

                // Convert amount to baseCurrency
                var amtBase = RateHelper.ConvertToBase(r.Amount, r.CurrencyCode, baseCurrency, rateMap);

                result.RecentTransactions.Add(new RecentTransactionItem
                {
                    TransactionId = !string.IsNullOrWhiteSpace(r.ReferenceId) ? r.ReferenceId : r.Id.ToString(),
                    Type = type,
                    CategoryId = r.CategoryId,
                    CategoryName = cat?.CategoryName,
                    Amount = amtBase,
                    Currency = baseCurrency,
                    OccurredAt = occurredAt,
                    Icon = cat?.Icon ?? (type == "EXPENSE" ? "bag" : "cash"),
                    Color = cat?.Color ?? (type == "EXPENSE" ? "#2196F3" : "#4CAF50"),
                    Title = !string.IsNullOrWhiteSpace(r.Description) ? cat?.CategoryName + " " + r.Description : cat?.CategoryName
                });
            }

            return result;
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync();
            throw;
        }
    }
}

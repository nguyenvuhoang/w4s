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

namespace O24OpenAPI.W4S.API.Application.Features.WalletCategorys;

public class TopSpendingCategoriesCommand
    : BaseTransactionModel,
        ICommand<List<TopSpendingCategoriesResponseModel>>
{
    public string ContractNumber { get; set; } = default!;

    /// <summary>D=Day, M=Month, Q=Quarter, H=Half-year, Y=Year</summary>
    public string PeriodType { get; set; } = "M";

    /// <summary>Base currency to show result. e.g. VND, USD</summary>
    public string? CurrencyCode { get; set; } = "VND";

    /// <summary>
    /// Raw rates from outside. Typically quote to VND: 1 CCY = Transfer VND
    /// </summary>
    public List<TransferRateResponseModel> TransferRates { get; set; } = [];
}

public class TopSpendingCategoriesResponseModel : BaseO24OpenAPIModel
{
    public List<TopSpendingCategoryItem> TopCategories { get; set; } = [];
}

public class TopSpendingCategoryItem
{
    public int CategoryId { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Icon { get; set; } = default!;
    public string Color { get; set; } = default!;
    public int TransactionCount { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal Percentage { get; set; }
}

[CqrsHandler]
public class TopSpendingCategoriesCommandHandler(
    IWalletProfileRepository walletProfileRepository,
    IWalletStatementRepository walletStatementRepository,
    IWalletCategoryRepository walletCategoryRepository
) : ICommandHandler<TopSpendingCategoriesCommand, List<TopSpendingCategoriesResponseModel>>
{
    [WorkflowStep(WorkflowStepCode.W4S.WF_STEP_W4S_TOP_SPENDING_CATEGORIES)]
    public async Task<List<TopSpendingCategoriesResponseModel>> HandleAsync(
        TopSpendingCategoriesCommand request,
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

            var baseCurrency = (request.CurrencyCode ?? "VND").Trim().ToUpperInvariant();

            var now = DateTime.UtcNow;
            var (fromUtc, toUtc, _, _) = WalletPeriodType.BuildPeriodRangeUtc(now, request.PeriodType);

            // rates: build raw VND map (1 CCY = Transfer VND)
            var rawVndRateMap = (request.TransferRates ?? [])
                .Where(x =>
                    !string.IsNullOrWhiteSpace(x.CurrencyCode) &&
                    x.Transfer.HasValue &&
                    x.Transfer.Value > 0m
                )
                .GroupBy(x => x.CurrencyCode!.Trim().ToUpperInvariant())
                .ToDictionary(g => g.Key, g => g.Last().Transfer!.Value);

            // optional: support VND in statement even if rate list doesn't include it
            rawVndRateMap.TryAdd("VND", 1m);

            // map: 1 CCY => baseCurrency
            var rateMap = RateHelper.BuildRateMapByBase(baseCurrency, rawVndRateMap);

            // pull expense rows for period
            var rows = await walletStatementRepository.Table
                .Where(x =>
                    walletIds.Contains(x.WalletId) &&
                    x.StatementOnUtc >= fromUtc &&
                    x.StatementOnUtc < toUtc &&
                    x.EntryType == Code.EntryType.DEBIT &&
                    x.CategoryId != null
                )
                .Select(x => new
                {
                    CategoryId = x.CategoryId!,
                    x.Amount,
                    x.CurrencyCode
                })
                .ToListAsync(cancellationToken);

            // aggregate by category (after convert)
            var categoryAgg = new Dictionary<int, (decimal total, int count)>();
            decimal totalExpenseBase = 0m;

            foreach (var r in rows)
            {
                var amountBase = RateHelper.ConvertToBase(r.Amount, r.CurrencyCode, baseCurrency, rateMap);
                totalExpenseBase += amountBase;

                if (!categoryAgg.TryGetValue(r.CategoryId, out var agg))
                    agg = (0m, 0);

                categoryAgg[r.CategoryId] = (agg.total + amountBase, agg.count + 1);
            }

            // pick top 5
            var top = categoryAgg
                .OrderByDescending(x => x.Value.total)
                .Take(5)
                .Select(x => new { CategoryId = x.Key, Total = x.Value.total, Count = x.Value.count })
                .ToList();

            // load category meta
            var categoryIds = top.Select(x => x.CategoryId).ToList();
            var categories = await walletCategoryRepository.GetByIds(categoryIds);

            var categoryMap = categories.ToDictionary(x => x.Id);

            var response = new TopSpendingCategoriesResponseModel();

            foreach (var t in top)
            {
                // fallback if not found
                categoryMap.TryGetValue(t.CategoryId, out var cat);

                response.TopCategories.Add(new TopSpendingCategoryItem
                {
                    CategoryId = t.CategoryId,
                    Name = cat?.CategoryName,
                    Icon = cat?.Icon ?? "pricetag-outline",
                    Color = cat?.Color ?? "#9E9E9E",
                    TransactionCount = t.Count,
                    TotalAmount = t.Total,
                    Percentage = totalExpenseBase == 0m
                        ? 0m
                        : Math.Round(t.Total / totalExpenseBase, 2, MidpointRounding.AwayFromZero)
                });
            }

            // keep your signature: List<TopSpendingCategoriesResponseModel>
            return [response];
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync();
            throw;
        }
    }
}

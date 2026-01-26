using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.W4S.API.Application.Constants;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;
using O24OpenAPI.W4S.Domain.AggregatesModel.WalletMasterAggregate;

namespace O24OpenAPI.W4S.API.Application.Features.WalletStatements;

public class RecentTransactionsCommand
    : BaseTransactionModel,
        ICommand<RecentTransactionsResponseModel>
{
    public string ContractNumber { get; set; } = default!;

    /// <summary>Max items. Default 5.</summary>
    public int Take { get; set; } = 5;
}

public class RecentTransactionsResponseModel : BaseO24OpenAPIModel
{
    public List<RecentTransactionItem> RecentTransactions { get; set; } = [];
}

public class RecentTransactionItem
{
    public string TransactionId { get; set; } = default!;
    public string Type { get; set; } = default!; // EXPENSE / INCOME
    public int Category { get; set; } = default!;
    public string Title { get; set; } = default!;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = default!;
    public string OccurredAt { get; set; } = default!; // ISO8601 +07:00
    public string Icon { get; set; } = default!;
    public string Color { get; set; } = default!;
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
                    x.CategoryId,
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
                categoryMap.TryGetValue(r.CategoryId, out var cat);

                var type = string.Equals(r.EntryType, Code.EntryType.DEBIT, StringComparison.OrdinalIgnoreCase)
                    ? "EXPENSE"
                    : "INCOME";

                // occurredAt ISO8601 with offset
                var local = TimeZoneInfo.ConvertTimeFromUtc(
                    DateTime.SpecifyKind(r.StatementOnUtc, DateTimeKind.Utc),
                    tz
                );
                var occurredAt = new DateTimeOffset(local, tz.GetUtcOffset(local)).ToString("o");

                result.RecentTransactions.Add(new RecentTransactionItem
                {
                    TransactionId = !string.IsNullOrWhiteSpace(r.ReferenceId) ? r.ReferenceId : r.Id.ToString(),
                    Type = type,
                    Category = r.CategoryId,
                    Title = r.Description,
                    Amount = r.Amount,
                    Currency = string.IsNullOrWhiteSpace(r.CurrencyCode) ? "VND" : r.CurrencyCode.Trim().ToUpperInvariant(),
                    OccurredAt = occurredAt,
                    Icon = cat?.Icon ?? (type == "EXPENSE" ? "bag" : "cash"),
                    Color = cat?.Color ?? (type == "EXPENSE" ? "#2196F3" : "#4CAF50")
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

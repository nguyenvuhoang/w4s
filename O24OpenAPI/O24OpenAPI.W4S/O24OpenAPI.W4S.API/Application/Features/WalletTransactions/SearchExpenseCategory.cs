using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

namespace O24OpenAPI.W4S.API.Application.Features.WalletTransactions;

public class SearchExpenseCategoryQuery : BaseSearch, IQuery<PagedListModel<ExpenseCategory>>
{
    public int WalletId { get; set; }
}

public class ExpenseCategory : BaseO24OpenAPIModel
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public int TotalTransaction { get; set; }
    public decimal? TotalAmount { get; set; }
}

[CqrsHandler]
public class SearchExpenseCategoryHandler(
    IWalletTransactionRepository walletTransactionRepository,
    IWalletCategoryRepository walletCategoryRepository
) : IQueryHandler<SearchExpenseCategoryQuery, PagedListModel<ExpenseCategory>>
{
    [WorkflowStep(WorkflowStepCode.W4S.WF_STEP_W4S_SEARCH_EXPENSE_CATEGORY)]
    public async Task<PagedListModel<ExpenseCategory>> HandleAsync(
        SearchExpenseCategoryQuery request,
        CancellationToken cancellationToken = default
    )
    {
        var fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

        var query = walletTransactionRepository
            .Table.Where(t =>
                t.USERID == request.CurrentUserCode
                && t.TRANSACTIONDATE >= fromDate
                && t.NUM01 > 0
                && !t.DELETED
                && !string.IsNullOrEmpty(t.CHAR02)
            )
            .GroupBy(t => t.CHAR02)
            .Join(
                walletCategoryRepository.Table,
                g => int.Parse(g.Key),
                c => c.Id,
                (g, c) =>
                    new ExpenseCategory
                    {
                        CategoryId = c.Id,
                        CategoryName = c.CategoryName,
                        TotalAmount = g.Sum(x => x.NUM01),
                        TotalTransaction = g.Count(),
                    }
            )
            .OrderByDescending(x => x.TotalAmount);
        var pagedList = await query.ToPagedList(request.PageIndex, request.PageSize);
        return new PagedListModel<ExpenseCategory>(pagedList);
    }
}

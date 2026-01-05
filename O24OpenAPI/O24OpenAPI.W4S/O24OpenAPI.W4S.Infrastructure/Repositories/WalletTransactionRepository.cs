using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

namespace O24OpenAPI.W4S.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class WalletTransactionRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<WalletTransaction>(dataProvider, staticCacheManager), IWalletTransactionRepository
{
    /// <summary>
    /// Insert a new wallet transaction record
    /// </summary>
    /// <param name="transaction"></param>
    /// <returns></returns>
    public async Task<WalletTransaction> AddAsync(WalletTransaction transaction)
    {
        return await InsertAsync(transaction);
    }

    /// <summary>
    /// Get wallet transactions by wallet id with optional date range and pagination
    /// </summary>
    /// <param name="walletId"></param>
    /// <param name="fromUtc"></param>
    /// <param name="toUtc"></param>
    /// <param name="skip"></param>
    /// <param name="take"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>

    public async Task<IList<WalletTransaction>> GetByWalletIdAsync(Guid walletId, DateTime? fromUtc = null, DateTime? toUtc = null, int skip = 0, int take = 50, CancellationToken cancellationToken = default)
    {
        var wid = walletId.ToString("D").ToUpperInvariant();

        var q = Table.Where(x => x.SOURCETRANREF == wid);

        if (fromUtc.HasValue)
            q = q.Where(x => x.TRANSACTIONDATE >= fromUtc.Value);

        if (toUtc.HasValue)
            q = q.Where(x => x.TRANSACTIONDATE < toUtc.Value);

        return (IList<WalletTransaction>)await q
        .OrderByDescending(x => x.TRANSACTIONDATE)
        .ThenByDescending(x => x.Id)
        .Skip(skip)
        .Take(take)
        .ToListAsync(cancellationToken)
        ?? [];
    }
}

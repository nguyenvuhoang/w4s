using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.W4S.Domain.AggregatesModel.WalletMasterAggregate;

namespace O24OpenAPI.W4S.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class WalletStatementRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
)
    : EntityRepository<WalletStatement>(dataProvider, staticCacheManager),
        IWalletStatementRepository
{
    public async Task<decimal?> GetLastClosingBalanceAsync(
        int walletId,
        string accountNumber,
        string currencyCode,
        DateTime beforeUtc,
        CancellationToken cancellationToken)
    {
        accountNumber = accountNumber?.Trim() ?? "";
        currencyCode = currencyCode?.Trim() ?? "";

        return await Table
            .Where(ws =>
                ws.WalletId == walletId &&
                ws.AccountNumber == accountNumber &&
                ws.CurrencyCode == currencyCode &&
                ws.TransactionOnUtc < beforeUtc
            )
            .OrderByDescending(ws => ws.TransactionOnUtc)
            .ThenByDescending(ws => ws.Id)
            .Select(ws => (decimal?)ws.ClosingBalance)
            .FirstOrDefaultAsync(cancellationToken);
    }

}

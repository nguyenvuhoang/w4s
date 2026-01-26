using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.W4S.Domain.AggregatesModel.WalletMasterAggregate;

public interface IWalletStatementRepository : IRepository<WalletStatement>
{
    Task<decimal?> GetLastClosingBalanceAsync(int walletId, string accountNumber, string currencyCode, DateTime beforeUtc, CancellationToken cancellationToken);
}

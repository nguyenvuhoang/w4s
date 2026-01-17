using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.W4S.Domain.AggregatesModel.WalletMasterAggregate;

public interface IWalletLedgerEntryRepository : IRepository<WalletLedgerEntry>
{
    Task<WalletLedgerEntry> AddAsync(WalletLedgerEntry entity);
    Task PostingEntry(List<WalletLedgerEntry> entity);
}

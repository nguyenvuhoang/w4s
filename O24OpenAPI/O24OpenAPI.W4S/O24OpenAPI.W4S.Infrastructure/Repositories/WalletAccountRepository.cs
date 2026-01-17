using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;
using O24OpenAPI.W4S.Domain.Constants;

namespace O24OpenAPI.W4S.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class WalletAccountRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
)
    : EntityRepository<WalletAccount>(dataProvider, staticCacheManager),
        IWalletAccountProfileRepository
{
    public async Task<List<WalletAccount>> CreateDefaultAccount(int walletId, string currencycode)
    {
        var now = DateTime.UtcNow;

        var accounts = new List<WalletAccount>
        {
            new() {
                WalletId = walletId,
                AccountType = WalletAccountType.Income, // INCOME
                CurrencyCode = currencycode,
                IsPrimary = true,
                Status = "A",
                AccountNumber = Generate(WalletAccountType.Income, 1, now)
            },
            new ()
            {
                WalletId = walletId,
                AccountType = WalletAccountType.Expense, // EXPENSE
                CurrencyCode = currencycode,
                IsPrimary = false,
                Status = "A",
                AccountNumber = Generate(WalletAccountType.Expense, 1, now)
            },
            new ()
            {
                WalletId = walletId,
                AccountType = WalletAccountType.Loan, // LOAN
                CurrencyCode = currencycode,
                IsPrimary = false,
                Status = "A",
                AccountNumber = Generate( WalletAccountType.Loan, 1, now)
            }
        };

        await BulkInsert(accounts);
        return accounts;
    }



    public async Task<List<WalletAccount>> GetWalletAccountByWalletIdAsync(
     List<int> walletIds
    )
    {
        if (walletIds == null || walletIds.Count == 0)
            return [];

        return await Table
            .Where(x => walletIds.Contains(x.WalletId))
            .ToListAsync();
    }

    public static string Generate(
     string type,
     int sequence,
     DateTime? utcNow = null)
    {
        var time = (utcNow ?? DateTime.UtcNow)
            .ToString("yyyyMMddHHmmss");

        var typeCode = type;
        var seq = sequence.ToString("D6");

        return $"{time}{typeCode}{seq}";
    }

    /// <summary>
    /// Get Default Expense Account
    /// </summary>
    /// <param name="walletId"></param>
    /// <returns></returns>
    public Task<WalletAccount> GetDefaultExpenseAccountAsync(int walletId)
    {
        return Table
            .Where(x => x.WalletId == walletId && x.AccountType == WalletAccountType.Expense)
            .FirstOrDefaultAsync();
    }
}

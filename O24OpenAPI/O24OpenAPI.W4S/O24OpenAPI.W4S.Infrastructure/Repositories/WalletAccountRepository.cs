using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

namespace O24OpenAPI.W4S.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class WalletAccountRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
)
    : EntityRepository<WalletAccount>(dataProvider, staticCacheManager),
        IWalletAccountProfileRepository
{
    public async Task CreateDefaultAccount(int walletId)
    {
        var now = DateTime.UtcNow;

        var accounts = new List<WalletAccount>
        {
            new() {
                WalletId = walletId,
                AccountType = "01", // INCOME
                CurrencyCode = "VND",
                IsPrimary = true,
                Status = "A",
                AccountNumber = Generate(WalletAccountType.Income, 1, now)
            },
            new ()
            {
                WalletId = walletId,
                AccountType = "02", // EXPENSE
                CurrencyCode = "VND",
                IsPrimary = false,
                Status = "A",
                AccountNumber = Generate(WalletAccountType.Expense, 1, now)
            },
            new ()
            {
                WalletId = walletId,
                AccountType = "03", // LOAN
                CurrencyCode = "VND",
                IsPrimary = false,
                Status = "A",
                AccountNumber = Generate( WalletAccountType.Loan, 1, now)
            }
        };

        await BulkInsert(accounts);
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

    public enum WalletAccountType
    {
        Income = 1,
        Expense = 2,
        Loan = 3
    }
    public static string Generate(
     WalletAccountType type,
     int sequence,
     DateTime? utcNow = null)
    {
        var time = (utcNow ?? DateTime.UtcNow)
            .ToString("yyyyMMddHHmmss");

        var typeCode = ((int)type).ToString("D2");
        var seq = sequence.ToString("D6");

        return $"{time}{typeCode}{seq}";
    }

}

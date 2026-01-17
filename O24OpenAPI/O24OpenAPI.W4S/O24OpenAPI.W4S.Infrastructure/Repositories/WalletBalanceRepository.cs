using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

namespace O24OpenAPI.W4S.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class WalletBalanceRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager
) : EntityRepository<WalletBalance>(dataProvider, staticCacheManager), IWalletBalanceRepository
{
    public async Task<List<WalletBalance>> GetByAccountNumbersAsync(List<string> accountNumbers)
    {
        if (accountNumbers == null || accountNumbers.Count == 0)
            return [];

        return await Table
            .Where(x => accountNumbers.Contains(x.AccountNumber))
            .ToListAsync();
    }

    public async Task CreditBalanceAsync(string accountNumber, decimal amount)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);

        var now = DateTime.UtcNow;

        var affected = await Table
            .Where(x => x.AccountNumber == accountNumber)
            .Set(x => x.Balance, x => x.Balance + amount)
            .Set(x => x.AvailableBalance, x => x.AvailableBalance + amount)
            .Set(x => x.UpdatedOnUtc, now)
            .UpdateAsync();

        if (affected == 0)
        {
            try
            {
                await InsertAsync(new WalletBalance
                {
                    AccountNumber = accountNumber,
                    Balance = 0m,
                    AvailableBalance = 0m,
                    UpdatedOnUtc = now
                });

                await Table
                    .Where(x => x.AccountNumber == accountNumber)
                    .Set(x => x.Balance, x => x.Balance + amount)
                    .Set(x => x.AvailableBalance, x => x.AvailableBalance + amount)
                    .Set(x => x.UpdatedOnUtc, now)
                    .UpdateAsync();
            }
            catch
            {
                await Table
                    .Where(x => x.AccountNumber == accountNumber)
                    .Set(x => x.Balance, x => x.Balance + amount)
                    .Set(x => x.AvailableBalance, x => x.AvailableBalance + amount)
                    .Set(x => x.UpdatedOnUtc, now)
                    .UpdateAsync();
            }
        }
    }


    public async Task DebitBalanceAsync(string accountNumber, decimal amount)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);

        var now = DateTime.UtcNow;

        // guard: available >= amount
        var affected = await Table
            .Where(x => x.AccountNumber == accountNumber && x.AvailableBalance >= amount)
            .Set(x => x.Balance, x => x.Balance - amount)
            .Set(x => x.AvailableBalance, x => x.AvailableBalance - amount)
            .Set(x => x.UpdatedOnUtc, now)
            .UpdateAsync();

        if (affected > 0) return;

        var exists = await Table
            .Where(x => x.AccountNumber == accountNumber)
            .AnyAsync();

        if (!exists)
        {
            try
            {
                await InsertAsync(new WalletBalance
                {
                    AccountNumber = accountNumber,
                    Balance = 0m,
                    AvailableBalance = 0m,
                    UpdatedOnUtc = now
                });
            }
            catch
            {
                // ignore race
            }
        }

        affected = await Table
            .Where(x => x.AccountNumber == accountNumber && x.AvailableBalance >= amount)
            .Set(x => x.Balance, x => x.Balance - amount)
            .Set(x => x.AvailableBalance, x => x.AvailableBalance - amount)
            .Set(x => x.UpdatedOnUtc, now)
            .UpdateAsync();

        if (affected == 0)
            throw new InvalidOperationException($"Insufficient funds or account not found: {accountNumber}");
    }


}

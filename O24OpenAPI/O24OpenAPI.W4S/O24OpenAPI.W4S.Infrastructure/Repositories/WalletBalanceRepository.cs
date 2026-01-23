using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

namespace O24OpenAPI.W4S.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class WalletBalanceRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager,
    IWalletAccountProfileRepository walletAccountProfileRepository
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

    public async Task EnsureBalanceAsync(string accountNumber, string currencyCode)
    {
        var exists = await Table.AnyAsync(x => x.AccountNumber == accountNumber && x.CurrencyCode == currencyCode);
        if (exists) return;

        var now = DateTime.UtcNow;

        await InsertAsync(new WalletBalance
        {
            AccountNumber = accountNumber,
            CurrencyCode = currencyCode,
            Balance = 0m,
            AvailableBalance = 0m,
            CreatedOnUtc = now,
            UpdatedOnUtc = now
        });
    }

    public async Task CreditBalanceAsync(string accountNumber, decimal amount, string currencyCode)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);

        if (string.IsNullOrWhiteSpace(accountNumber))
            throw new ArgumentException("AccountNumber is required.", nameof(accountNumber));

        if (string.IsNullOrWhiteSpace(currencyCode))
            throw new ArgumentException("CurrencyCode is required.", nameof(currencyCode));

        var now = DateTime.UtcNow;
        currencyCode = currencyCode.Trim().ToUpperInvariant();

        var affected = await Table
            .Where(x => x.AccountNumber == accountNumber)
            .Set(x => x.Balance, x => x.Balance + amount)
            .Set(x => x.AvailableBalance, x => x.AvailableBalance + amount)
            .Set(x => x.CurrencyCode, x => x.CurrencyCode ?? currencyCode)
            .Set(x => x.UpdatedOnUtc, now)
            .UpdateAsync();

        if (affected > 0) return;

        try
        {
            await InsertAsync(new WalletBalance
            {
                AccountNumber = accountNumber,
                CurrencyCode = currencyCode,
                Balance = 0m,
                AvailableBalance = 0m,
                UpdatedOnUtc = now
            });
        }
        catch
        {
        }

        affected = await Table
            .Where(x => x.AccountNumber == accountNumber)
            .Set(x => x.Balance, x => x.Balance + amount)
            .Set(x => x.AvailableBalance, x => x.AvailableBalance + amount)
            .Set(x => x.CurrencyCode, x => x.CurrencyCode ?? currencyCode)
            .Set(x => x.UpdatedOnUtc, now)
            .UpdateAsync();

        if (affected == 0)
            throw new InvalidOperationException($"Account not found: {accountNumber}");
    }


    /// <summary>
    /// Debit balance
    /// </summary>
    /// <param name="accountNumber"></param>
    /// <param name="amount"></param>
    /// <param name="currencyCode"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task DebitBalanceAsync(
        string accountNumber,
        decimal amount,
        string currencyCode
    )
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);

        var isAllowNegativeBalance = await walletAccountProfileRepository.IsAllowNegativeBalanceAsync(accountNumber);

        if (!isAllowNegativeBalance)
        {
            throw new InvalidOperationException("Cannot debit a non-expense or non-loan account.");
        }

        var now = DateTime.UtcNow;
        await EnsureBalanceAsync(accountNumber, currencyCode);

        IQueryable<WalletBalance> q = Table.Where(x => x.AccountNumber == accountNumber);

        var affected = await q
            .Set(x => x.Balance, x => x.Balance - amount)
            .Set(x => x.AvailableBalance, x => x.AvailableBalance - amount)
            .Set(x => x.UpdatedOnUtc, now)
            .UpdateAsync();

        if (affected == 0)
            throw new InvalidOperationException($"Insufficient funds: {accountNumber}");
    }


}

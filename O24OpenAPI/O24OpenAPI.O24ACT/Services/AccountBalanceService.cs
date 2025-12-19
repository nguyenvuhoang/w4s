using LinqToDB;
using O24OpenAPI.Core;
using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.SeedWork;
using O24OpenAPI.Data.Extensions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.O24ACT.Common;
using O24OpenAPI.O24ACT.Configuration;
using O24OpenAPI.O24ACT.Domain;
using O24OpenAPI.O24ACT.Services.Interfaces;

namespace O24OpenAPI.O24ACT.Services;

/// <summary>
/// AccountBalanceService
/// </summary>
/// <remarks>
/// Contructor
/// </remarks>
/// <param name="accountBalanceRepository"></param>
/// <param name="accountingSeting"></param>
/// <param name="grpcAdminClient"></param>
/// <param name="staticCacheManager"></param>
public partial class AccountBalanceService(
    IRepository<AccountBalance> accountBalanceRepository,
    AccountingSettings accountingSeting,
    IStaticCacheManager staticCacheManager
) : IAccountBalanceService
{
    #region  Fields
    private readonly IRepository<AccountBalance> _accountBalanceRepository =
        accountBalanceRepository;
    private readonly AccountingSettings _accountingSeting = accountingSeting;
    private readonly IStaticCacheManager _staticCacheManager = staticCacheManager;

    #endregion
    #region Ctor

    #endregion

    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual async Task<AccountBalance> GetById(int id)
    {
        return await _accountBalanceRepository.GetById(id, cache => default);
    }

    /// <summary>
    /// Get By Account Number
    /// </summary>
    /// <param name="accountNumber"></param>
    /// <returns></returns>
    public virtual async Task<AccountBalance> GetByAccountNumber(string accountNumber)
    {
        // var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(NeptuneEntityCacheDefaults<AccountBalance>.ByCodeCacheKey, accountNumber);
        // var accountBalance = await _staticCacheManager.Get(cacheKey, async () =>
        // {
        var accountBalance = await _accountBalanceRepository
            .Table.Where(c => c.AccountNumber == accountNumber)
            .FirstOrDefaultAsync();
        if (accountBalance is null)
        {
            accountBalance = new AccountBalance { AccountNumber = accountNumber };
            await accountBalance.Insert();
        }
        return accountBalance;
        // });
        // return accountBalance;
    }

    /// <summary>
    /// Check Balance Equal Zero
    /// </summary>
    /// <param name="accountNumber"></param>
    /// <returns></returns>
    public virtual async Task<bool> IsBalanceEqualZero(string accountNumber)
    {
        var accountBalance = await GetByAccountNumber(accountNumber);
        if (accountBalance == null)
        {
            return true;
        }

        return (accountBalance.Balance == 0);
    }

    /// <summary>
    /// Credit Balance
    /// </summary>
    /// <param name="transaction"></param>
    /// <param name="accountNumber"></param>
    /// <param name="amount"></param>
    /// <param name="valueDate"></param>
    /// <param name="referenceId"></param>
    /// <returns></returns>
    /// <exception cref="O24OpenAPIException"></exception>
    public virtual async Task CreditAccount(
        BaseTransactionModel transaction,
        string accountNumber,
        decimal amount,
        DateTime? valueDate = null,
        string referenceId = ""
    )
    {
        var accountbalance =
            await GetByAccountNumber(accountNumber)
            ?? throw new O24OpenAPIException(nameof(AccountBalance), accountNumber);
        if (!transaction.IsReverse)
        {
            accountbalance.Balance += amount;
            accountbalance.DailyCredit += amount;
            accountbalance.WeeklyCredit += amount;
            accountbalance.MonthlyCredit += amount;
            accountbalance.QuarterlyCredit += amount;
            accountbalance.HalfYearlyCredit += amount;
            accountbalance.YearlyCredit += amount;
        }
        else
        {
            accountbalance.Balance -= amount;
            accountbalance.DailyCredit -= amount;
            accountbalance.WeeklyCredit -= amount;
            accountbalance.MonthlyCredit -= amount;
            accountbalance.QuarterlyCredit -= amount;
            accountbalance.HalfYearlyCredit -= amount;
            accountbalance.YearlyCredit -= amount;
        }
        await accountbalance.Update(referenceId);
    }

    /// <summary>
    /// CreditAccountProcess
    /// </summary>
    /// <param name="accountBalance"></param>
    /// <param name="amount"></param>
    /// <param name="valueDate"></param>
    /// <param name="referenceId"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public virtual async Task CreditAccountProcess(
        AccountBalance accountBalance,
        decimal amount,
        DateTime? valueDate = null,
        string referenceId = ""
    )
    {
        accountBalance.Balance += amount;
        accountBalance.DailyCredit += amount;
        accountBalance.WeeklyCredit += amount;
        accountBalance.MonthlyCredit += amount;
        accountBalance.QuarterlyCredit += amount;
        accountBalance.HalfYearlyCredit += amount;
        accountBalance.YearlyCredit += amount;
        await accountBalance.Update(referenceId);
    }

    /// <summary>
    /// Debit Balance
    /// </summary>
    /// <param name="transaction"></param>
    /// <param name="accountNumber"></param>
    /// <param name="amount"></param>
    /// <param name="valueDate"></param>
    /// <param name="referenceId"></param>
    /// <returns></returns>
    /// <exception cref="O24OpenAPIException"></exception>
    public virtual async Task DebitAccount(
        BaseTransactionModel transaction,
        string accountNumber,
        decimal amount,
        DateTime? valueDate = null,
        string referenceId = ""
    )
    {
        var accountbalance =
            await GetByAccountNumber(accountNumber)
            ?? throw new O24OpenAPIException(nameof(AccountBalance), accountNumber);

        if (!transaction.IsReverse)
        {
            accountbalance.Balance -= amount;
            accountbalance.DailyDebit -= amount;
            accountbalance.WeeklyDebit -= amount;
            accountbalance.MonthlyDebit -= amount;
            accountbalance.QuarterlyDebit -= amount;
            accountbalance.HalfYearlyDebit -= amount;
            accountbalance.YearlyDebit -= amount;
        }
        else
        {
            accountbalance.Balance += amount;
            accountbalance.DailyDebit += amount;
            accountbalance.WeeklyDebit += amount;
            accountbalance.MonthlyDebit += amount;
            accountbalance.QuarterlyDebit += amount;
            accountbalance.HalfYearlyDebit += amount;
            accountbalance.YearlyDebit += amount;
        }
        await accountbalance.Update(referenceId);
    }

    /// <summary>
    /// DebitAccountProcess
    /// </summary>
    /// <param name="accountBalance"></param>
    /// <param name="amount"></param>
    /// <param name="valueDate"></param>
    /// <param name="referenceId"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public virtual async Task DebitAccountProcess(
        AccountBalance accountBalance,
        decimal amount,
        DateTime? valueDate = null,
        string referenceId = ""
    )
    {
        accountBalance.Balance -= amount;
        accountBalance.DailyDebit += amount;
        accountBalance.WeeklyDebit += amount;
        accountBalance.MonthlyDebit += amount;
        accountBalance.QuarterlyDebit += amount;
        accountBalance.HalfYearlyDebit += amount;
        accountBalance.YearlyDebit += amount;
        await accountBalance.Update(referenceId);
    }

    /// <summary>
    /// Update Account
    /// </summary>
    /// <param name="accountNumber"></param>
    /// <param name="action"></param>
    /// <param name="amount"></param>
    /// <param name="valueDate"></param>
    /// <param name="referenceId"></param>
    /// <returns></returns>
    public virtual async Task UpdateAccount(
        BaseTransactionModel transaction,
        string accountNumber,
        string action,
        decimal amount,
        DateTime? valueDate = null,
        string referenceId = ""
    )
    {
        if (_accountingSeting.EnableBalance)
        {
            if (action.Equals(Constants.Checkmsgtransaction.Credit, Constants.ICIC))
            {
                await CreditAccount(transaction, accountNumber, amount, valueDate, referenceId);
            }
            else
            {
                await DebitAccount(transaction, accountNumber, amount, valueDate, referenceId);
            }
        }
    }

    /// <summary>
    /// UpdateAccountProcess
    /// </summary>
    /// <param name="accountNumber"></param>
    /// <param name="action"></param>
    /// <param name="amount"></param>
    /// <param name="valueDate"></param>
    /// <param name="referenceId"></param>
    /// <returns></returns>
    public virtual async Task UpdateAccountProcess(
        string accountNumber,
        string action,
        decimal amount,
        DateTime? valueDate = null,
        string referenceId = ""
    )
    {
        var accountBalance = await GetByAccountNumber(accountNumber);
        if (_accountingSeting.EnableBalance)
        {
            if (accountBalance == null)
            {
                throw new O24OpenAPIException(nameof(AccountBalance), accountNumber);
            }

            if (action.Equals(Constants.Checkmsgtransaction.Credit, Constants.ICIC))
            {
                await CreditAccountProcess(accountBalance, amount, valueDate, referenceId);
            }
            else
            {
                await DebitAccountProcess(accountBalance, amount, valueDate, referenceId);
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public virtual IQueryable<AccountBalance> Table => _accountBalanceRepository.Table;
}

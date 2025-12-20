using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.O24ACT.Domain;

namespace O24OpenAPI.O24ACT.Services.Interfaces;

/// <summary>
/// IAccountBalanceService
/// </summary>
public partial interface IAccountBalanceService
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<AccountBalance> GetById(int id);

    /// <summary>
    /// Get By Account Number
    /// </summary>
    /// <param name="accountNumber"></param>
    /// <returns></returns>
    Task<AccountBalance> GetByAccountNumber(string accountNumber);

    /// <summary>
    ///
    /// </summary>
    /// <param name="accountNumber"></param>
    /// <returns></returns>
    Task<bool> IsBalanceEqualZero(string accountNumber);

    /// <summary>
    ///
    /// </summary>
    /// <param name="accountNumber"></param>
    /// <param name="amount"></param>
    /// <param name="valueDate"></param>
    /// <param name="referenceId"></param>
    /// <returns></returns>
    Task CreditAccount(
        BaseTransactionModel transaction,
        string accountNumber,
        decimal amount,
        DateTime? valueDate = null,
        string referenceId = ""
    );

    /// <summary>
    /// DebitAccount
    /// </summary>
    /// <param name="accountNumber"></param>
    /// <param name="amount"></param>
    /// <param name="valueDate"></param>
    /// <param name="referenceId"></param>
    /// <returns></returns>
    Task DebitAccount(
        BaseTransactionModel transaction,
        string accountNumber,
        decimal amount,
        DateTime? valueDate = null,
        string referenceId = ""
    );

    /// <summary>
    /// UpdateAccount
    /// </summary>
    /// <param name="accountNumber"></param>
    /// <param name="action"></param>
    /// <param name="amount"></param>
    /// <param name="valueDate"></param>
    /// <param name="referenceId"></param>
    /// <returns></returns>
    Task UpdateAccount(
        BaseTransactionModel transaction,
        string accountNumber,
        string action,
        decimal amount,
        DateTime? valueDate = null,
        string referenceId = ""
    );

    /// <summary>
    /// UpdateAccountProcess
    /// </summary>
    /// <param name="accountNumber"></param>
    /// <param name="action"></param>
    /// <param name="amount"></param>
    /// <param name="valueDate"></param>
    /// <param name="referenceId"></param>
    /// <returns></returns>
    Task UpdateAccountProcess(
        string accountNumber,
        string action,
        decimal amount,
        DateTime? valueDate = null,
        string referenceId = ""
    );

    /// <summary>
    /// DebitAccountProcess
    /// </summary>
    /// <param name="accountBalance"></param>
    /// <param name="amount"></param>
    /// <param name="valueDate"></param>
    /// <param name="referenceId"></param>
    /// <returns></returns>
    Task DebitAccountProcess(
        AccountBalance accountBalance,
        decimal amount,
        DateTime? valueDate = null,
        string referenceId = ""
    );

    /// <summary>
    /// CreditAccountProcess
    /// </summary>
    /// <param name="accountBalance"></param>
    /// <param name="amount"></param>
    /// <param name="valueDate"></param>
    /// <param name="referenceId"></param>
    /// <returns></returns>
    Task CreditAccountProcess(
        AccountBalance accountBalance,
        decimal amount,
        DateTime? valueDate = null,
        string referenceId = ""
    );

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    IQueryable<AccountBalance> Table { get; }
}

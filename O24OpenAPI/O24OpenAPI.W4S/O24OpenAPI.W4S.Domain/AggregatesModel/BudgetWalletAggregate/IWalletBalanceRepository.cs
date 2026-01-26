using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

public interface IWalletBalanceRepository : IRepository<WalletBalance>
{
    Task<List<WalletBalance>> GetByAccountNumbersAsync(List<string> accountNumbers);
    Task EnsureBalanceAsync(string accountNumber, string currencyCode);
    Task CreditBalanceAsync(string accountNumber, decimal amount, string currencyCode);
    Task DebitBalanceAsync(
       string accountNumber,
       decimal amount,
       string currencyCode
   );
}

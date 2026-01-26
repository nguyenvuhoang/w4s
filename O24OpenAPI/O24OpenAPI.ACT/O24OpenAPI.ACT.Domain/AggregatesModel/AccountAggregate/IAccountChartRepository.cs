using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.ACT.Domain.AggregatesModel.AccountAggregate;

public interface IAccountChartRepository : IRepository<AccountChart>
{
    Task<AccountChart?> GetByAccountNumberAsync(string accountNumber);
    Task<IReadOnlyList<AccountChart>> GetByBranchCodeCurrencyCodeAsync(
        string branchCode,
        string currencyCode
    );
    Task<bool> IsAccountNumberExist(string acno);
}

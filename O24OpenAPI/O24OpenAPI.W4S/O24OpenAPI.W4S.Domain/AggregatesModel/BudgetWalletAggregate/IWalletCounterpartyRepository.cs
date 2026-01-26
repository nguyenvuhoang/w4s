using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

public interface IWalletCounterpartyRepository : IRepository<WalletCounterparty>
{
    public Task<WalletCounterparty?> FindByPhoneOrEmailAsync(string userCode, string? phone, string? email);
}

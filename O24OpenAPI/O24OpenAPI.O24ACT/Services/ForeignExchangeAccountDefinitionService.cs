using LinqToDB;
using O24OpenAPI.Data;
using O24OpenAPI.O24ACT.Domain;
using O24OpenAPI.O24ACT.Services.Interfaces;

namespace O24OpenAPI.O24ACT.Services;

public class ForeignExchangeAccountDefinitionService(IRepository<ForeignExchangeAccountDefinition> foreignExchangeAccountDefinitionRepository) : IForeignExchangeAccountDefinitionService
{
    private readonly IRepository<ForeignExchangeAccountDefinition> _foreignExchangeAccountDefinitionRepository = foreignExchangeAccountDefinitionRepository;

    public virtual async Task<ForeignExchangeAccountDefinition> GetByUniqueKey(string branchCode, string accountCurrency, string clearingCurrency, string clearingType)
    {
        return await _foreignExchangeAccountDefinitionRepository.Table.Where(c => c.BranchCode.Equals(branchCode)
                                                                                && c.AccountCurrency.Equals(accountCurrency)
                                                                                && c.ClearingCurrency.Equals(clearingCurrency)
                                                                                && c.ClearingType.Equals(clearingType)).FirstOrDefaultAsync();
    }

}

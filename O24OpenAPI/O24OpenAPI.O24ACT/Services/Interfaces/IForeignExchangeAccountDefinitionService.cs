using O24OpenAPI.O24ACT.Domain;

namespace O24OpenAPI.O24ACT.Services.Interfaces;

public interface IForeignExchangeAccountDefinitionService
{
    Task<ForeignExchangeAccountDefinition> GetByUniqueKey(string branchCode, string accountCurrency, string clearingCurrency, string clearingType);
}

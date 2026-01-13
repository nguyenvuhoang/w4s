using O24OpenAPI.O24ACT.Domain;

namespace O24OpenAPI.O24ACT.Services.Interfaces;

public interface IRuleDefinitionService
{
    Task<RuleDefinition> GetByRuleName(string ruleName);
}

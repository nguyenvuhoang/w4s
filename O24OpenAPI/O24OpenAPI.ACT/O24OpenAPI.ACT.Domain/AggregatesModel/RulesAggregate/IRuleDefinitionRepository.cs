using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.ACT.Domain.AggregatesModel.RulesAggregate;

public interface IRuleDefinitionRepository : IRepository<RuleDefinition>
{
    Task<RuleDefinition?> GetByCodeAsync(string code);
}

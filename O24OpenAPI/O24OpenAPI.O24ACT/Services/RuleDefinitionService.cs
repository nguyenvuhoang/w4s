using LinqToDB;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.O24ACT.Domain;
using O24OpenAPI.O24ACT.Services.Interfaces;

namespace O24OpenAPI.O24ACT.Services;

public class RuleDefinitionService(IStaticCacheManager staticCacheManager, IRepository<RuleDefinition> ruleRepository) : IRuleDefinitionService
{
    private readonly IStaticCacheManager _staticCacheManager = staticCacheManager;
    private readonly IRepository<RuleDefinition> _ruleRepository = ruleRepository;

    public virtual async Task<RuleDefinition> GetByRuleName(string ruleName)
    {
        var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(O24OpenAPIEntityCacheDefaults<RuleDefinition>.FunctionCacheKey, "GetByRuleName", ruleName);
        var rule = await _staticCacheManager.Get(cacheKey, async () =>
        {
            return await _ruleRepository.Table.FirstOrDefaultAsync(x => x.RuleName == ruleName);
        });
        return rule;
    }
}

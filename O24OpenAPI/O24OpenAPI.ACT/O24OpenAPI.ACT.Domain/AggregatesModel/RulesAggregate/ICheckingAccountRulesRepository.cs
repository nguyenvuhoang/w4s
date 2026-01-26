using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.ACT.Domain.AggregatesModel.RulesAggregate;

public interface ICheckingAccountRulesRepository : IRepository<CheckingAccountRules>
{
    Task<IReadOnlyList<CheckingAccountRules>> GetActiveAsync();
    Task CheckingRuleAccount(
        string accls,
        string rbal,
        string bside,
        string pside,
        string acgrp,
        string accat
    );
}

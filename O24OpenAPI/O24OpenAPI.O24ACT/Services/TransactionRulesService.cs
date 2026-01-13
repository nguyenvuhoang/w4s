using LinqToDB;
using Newtonsoft.Json;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Core.Helper;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.O24ACT.Domain;
using O24OpenAPI.O24ACT.Models;
using O24OpenAPI.O24ACT.Services.Interfaces;
using O24OpenAPI.O24ACT.Utils;
using System.Text.RegularExpressions;

namespace O24OpenAPI.O24ACT.Services;

public class TransactionRulesService(
    IStaticCacheManager staticCacheManager,
    IRepository<TransactionRules> transactionRulesRepository,
    IRuleDefinitionService ruleService
) : ITransactionRulesService
{
    private readonly IStaticCacheManager _staticCacheManager = staticCacheManager;
    private readonly IRepository<TransactionRules> _transactionRulesRepository =
        transactionRulesRepository;
    private readonly IRuleDefinitionService _ruleService = ruleService;

    async Task<List<TransactionRules>> GetByWorkflowId(string workflowId)
    {
        CacheKey cacheKey = CachingKey.SessionKey(workflowId);
        List<TransactionRules> lst = await _staticCacheManager.Get(
            cacheKey,
            async () =>
            {
                return await _transactionRulesRepository
                    .Table.Where(x => x.IsEnable == true && x.WorkflowId == workflowId)
                    .ToListAsync();
            }
        );
        return lst;
    }

    public virtual async Task Validate(BaseTransactionModel model, object data)
    {
        if (model.IsReverse)
        {
            return;
        }

        List<TransactionRules> lstTransRule = await GetByWorkflowId(model.TransactionCode);
        foreach (TransactionRules item in lstTransRule)
        {
            RuleDefinition ruleDef = await _ruleService.GetByRuleName(item.RuleName);
            if (ruleDef != null)
            {
                BaseValidatorModel rule = new BaseValidatorModel()
                {
                    Parameter = item.Parameter,
                    Workflowid = item.WorkflowId,
                    MethodName = ruleDef.MethodName,
                    NameSpace = ruleDef.FullClassName,
                    Fields = data.ToDictionary(),
                };

                if (
                    rule.Fields is not null
                    && !rule.Fields.ContainsKey(nameof(model.CurrentBranchCode).ToUnderscoreCase())
                )
                {
                    rule.Fields?.Add(
                        nameof(model.CurrentBranchCode).ToUnderscoreCase(),
                        model.CurrentBranchCode
                    );
                }

                Dictionary<string, object> param = JsonConvert.DeserializeObject<Dictionary<string, object>>(
                    rule.Parameter
                );

                var lst =
                    from d in rule.Fields
                    from e in param.Where(x => x.Value.ToString().Contains(d.Key))
                    select new
                    {
                        d.Key,
                        d.Value,
                        Field = e.Key,
                    };

                foreach (var field in lst)
                {
                    Regex regex = new Regex(@"\b" + field.Key + @"\b");
                    string value = regex.Replace(
                        Convert.ToString(param[field.Field]),
                        Convert.ToString(field.Value)
                    );
                    param[field.Field] = value;
                }

                bool result = true;
                if (param.ContainsKey(nameof(BaseValidatorModel.Condition)))
                {
                    object cdt = param[nameof(BaseValidatorModel.Condition)];
                    string condition = Regex.Unescape(cdt.ToString() ?? string.Empty);

                    if (condition.HasValue()) { }
                }

                if (result)
                {
                    param?.Add(nameof(model.Postings)?.ToUnderscoreCase(), model.Postings);
                    await ReflectionHelper.DynamicInvokeAsync(
                        rule.NameSpace,
                        rule.MethodName,
                        new object[] { param }
                    );
                }
            }
        }
    }
}

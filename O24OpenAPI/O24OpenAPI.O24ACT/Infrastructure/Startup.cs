using O24OpenAPI.ControlHub.Services.Interfaces;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.O24ACT.Services;
using O24OpenAPI.O24ACT.Services.Interfaces;

namespace O24OpenAPI.O24ACT.Infrastructure;

public class Startup : IO24OpenAPIStartup
{
    public int Order => 2000;

    public void Configure(IApplicationBuilder application) { }

    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAccountChartService, AccountChartService>();
        services.AddScoped<IAccountBalanceService, AccountBalanceService>();
        services.AddScoped<IAccountCommonService, AccountCommonService>();
        services.AddScoped<IBranchService, BranchService>();
        services.AddScoped<ICheckingAccountRulesService, CheckingAccountRulesService>();
        services.AddScoped<ICurrencyService, CurrencyService>();
        services.AddScoped<IValidatorService, ValidatorService>();
        services.AddScoped<IFOTransactionServices, FOTransactionServices>();
        services.AddScoped<IEntryjounalDataService, EntryJounalDataService>();
        services.AddScoped<IAccountingRuleDefinitionService, AccountingRuleDefinitionService>();
        services.AddScoped<ITransactionRulesService, TransactionRulesService>();
        services.AddScoped<IProccessTransRefActService, ProccessTransRefActService>();
        services.AddScoped<IAccountClearingService, AccountClearingService>();
        services.AddScoped<IForeignExchangeAccountDefinitionService, ForeignExchangeAccountDefinitionService>();
        services.AddScoped<IRuleDefinitionService, RuleDefinitionService>();
        services.AddScoped<IAccountingService, AccountingService>();
    }
}

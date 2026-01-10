using O24OpenAPI.Framework.Abstractions;

namespace O24OpenAPI.W4S.API.Application;

internal static class ApplicationExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddLinKitCqrs("w4s");
        services.AddKeyedSingleton<IWorkflowStepInvoker, Workflow.Generated.WorkflowStepInvoker>(serviceKey: "w4s");
        services.AddScoped<O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate.IWalletAccountProfileRepository, O24OpenAPI.W4S.Infrastructure.Repositories.WalletAccountRepository>();
        services.AddScoped<O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate.IWalletCategoryRepository, O24OpenAPI.W4S.Infrastructure.Repositories.WalletCategoryRepository>();
        services.AddScoped<O24OpenAPI.W4S.Domain.AggregatesModel.WalletMasterAggregate.IWalletCategoryDefaultRepository, O24OpenAPI.W4S.Infrastructure.Repositories.WalletCategoryDefaultRepository>();
        services.AddScoped<O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate.IWalletBudgetRepository, O24OpenAPI.W4S.Infrastructure.Repositories.WalletBudgetRepository>();
        services.AddScoped<O24OpenAPI.W4S.Domain.AggregatesModel.WalletMasterAggregate.IWalletContractRepository, O24OpenAPI.W4S.Infrastructure.Repositories.WalletContractRepository>();
        services.AddScoped<O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate.IWalletGoalRepository, O24OpenAPI.W4S.Infrastructure.Repositories.WalletGoalRepository>();
        services.AddScoped<O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate.IWalletTransactionRepository, O24OpenAPI.W4S.Infrastructure.Repositories.WalletTransactionRepository>();
        services.AddScoped<O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate.IWalletProfileRepository, O24OpenAPI.W4S.Infrastructure.Repositories.WalletProfileRepository>();
        services.AddScoped<O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate.IWalletBalanceRepository, O24OpenAPI.W4S.Infrastructure.Repositories.WalletBalanceRepository>();
        services.AddScoped<O24OpenAPI.W4S.Domain.AggregatesModel.WalletMasterAggregate.IWalletStatementRepository, O24OpenAPI.W4S.Infrastructure.Repositories.WalletStatementRepository>();
        services.AddScoped<O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate.IWalletEventRepository, O24OpenAPI.W4S.Infrastructure.Repositories.WalletEventRepository>();
        return services;
    }
}

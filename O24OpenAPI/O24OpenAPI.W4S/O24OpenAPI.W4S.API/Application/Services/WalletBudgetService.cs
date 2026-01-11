using LinKit.Core.Abstractions;

namespace O24OpenAPI.W4S.API.Application.Services;

public interface IWalletBudgetService { }

[RegisterService(Lifetime.Scoped)]
public class WalletBudgetService : IWalletBudgetService { }

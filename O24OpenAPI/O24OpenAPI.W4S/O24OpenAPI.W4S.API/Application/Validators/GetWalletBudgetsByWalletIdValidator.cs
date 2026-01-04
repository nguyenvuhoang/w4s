using LinKit.Core.Abstractions;
using O24OpenAPI.Framework.Abstractions;
using O24OpenAPI.W4S.API.Application.Features.WalletBugets;
using System.ComponentModel.DataAnnotations;

namespace O24OpenAPI.W4S.API.Application.Validators;

[RegisterService(Lifetime.Scoped)]
public class GetWalletBudgetsByWalletIdValidator : IValidator<GetWalletBudgetsByWalletId>
{
    public async Task ValidateAsync(GetWalletBudgetsByWalletId request)
    {
        await Task.CompletedTask;
        if (string.IsNullOrWhiteSpace(request.WalletId))
        {
            throw new ValidationException("WalletId cannot be null or empty.");
        }
    }
}

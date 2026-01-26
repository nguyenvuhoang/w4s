using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.W4S.API.Application.Helpers;
using O24OpenAPI.W4S.API.Application.Models.WalletCategorys;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

namespace O24OpenAPI.W4S.API.Application.Features.WalletCategorys;

public class CreateWalletCategoryCommand
    : BaseTransactionModel,
        ICommand<CreateWalletCategoryResponseModel>
{
    public int WalletId { get; set; }
    public int ParentCategoryId { get; set; }
    public string CategoryGroup { get; set; }
    public string CategoryType { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
}

[CqrsHandler]
public class CreateWalletCategoryHandler(
    IWalletCategoryRepository walletCategoryRepository
) : ICommandHandler<CreateWalletCategoryCommand, CreateWalletCategoryResponseModel>
{
    [WorkflowStep(WorkflowStepCode.W4S.WF_STEP_W4S_CREATE_WALLET_CATEGORY)]
    public async Task<CreateWalletCategoryResponseModel> HandleAsync(
        CreateWalletCategoryCommand request,
        CancellationToken ct = default
    )
    {
        // 1) Basic validation
        if (request.WalletId <= 0)
            throw new InvalidOperationException("WalletId is invalid");

        // normalize group/type
        var group = (request.CategoryGroup ?? string.Empty).Trim().ToUpperInvariant();
        var type = (request.CategoryType ?? string.Empty).Trim().ToUpperInvariant();

        if (string.IsNullOrWhiteSpace(group))
            throw new InvalidOperationException("CategoryGroup is required");

        if (string.IsNullOrWhiteSpace(type))
            throw new InvalidOperationException("CategoryType is required");

        // only allow supported values (adjust if you have more)
        if (group is not ("EXPENSE" or "INCOME"))
            throw new InvalidOperationException("CategoryGroup must be EXPENSE or INCOME");

        if (type is not ("EXPENSE" or "INCOME"))
            throw new InvalidOperationException("CategoryType must be EXPENSE or INCOME");

        // enforce consistent group/type (if that's your rule)
        if (!string.Equals(group, type, StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("CategoryGroup and CategoryType must match");

        // validate name
        var name = (request.CategoryName ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidOperationException("CategoryName is required");

        // If CategoryName is JSON multi-lang, ensure it's valid JSON.
        // If not JSON, you can remove this block.
        if (name.StartsWith('{'))
        {
            try
            {
                using var _ = System.Text.Json.JsonDocument.Parse(name);
            }
            catch
            {
                throw new InvalidOperationException("CategoryName must be a valid JSON string (e.g. {\"vi\":\"...\",\"en\":\"...\"}).");
            }
        }

        var existed = await walletCategoryRepository.ExistsByWalletGroupAndNameAsync(
            walletId: request.WalletId,
            categoryGroup: group,
            categoryName: name,
            cancellationToken: ct
        );

        if (existed)
            throw new InvalidOperationException("CategoryName already exists in this group for this wallet");

        // 3) Build entity (code auto-generated)
        var entity = new WalletCategory
        {
            CategoryCode = WalletProfileHelper.GenerateCategoryCode(group),
            WalletId = request.WalletId,
            ParentCategoryId = request.ParentCategoryId,
            CategoryGroup = group,
            CategoryType = type,
            CategoryName = name,
            Icon = request.Icon?.Trim() ?? string.Empty,
            Color = request.Color?.Trim() ?? string.Empty,
            CreatedOnUtc = DateTime.UtcNow,
            UpdatedOnUtc = null
        };

        await walletCategoryRepository.InsertAsync(entity);

        return new CreateWalletCategoryResponseModel
        {
            Id = entity.Id,
            CategoryCode = entity.CategoryCode
        };
    }
}



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
    public int ParentCategoryId { get; set; }
    public string CategoryGroup { get; set; }
    public string CategoryType { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string ContractNumber { get; set; } = string.Empty;
}

[CqrsHandler]
public class CreateWalletCategoryHandler(
    IWalletProfileRepository walletProfileRepository,
    IWalletCategoryRepository walletCategoryRepository
) : ICommandHandler<CreateWalletCategoryCommand, CreateWalletCategoryResponseModel>
{
    [WorkflowStep(WorkflowStepCode.W4S.WF_STEP_W4S_CREATE_WALLET_CATEGORY)]
    public async Task<CreateWalletCategoryResponseModel> HandleAsync(
        CreateWalletCategoryCommand request,
        CancellationToken ct = default
    )
    {
        // 1) Validate contract
        var contractNumber = (request.ContractNumber ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(contractNumber))
            throw new InvalidOperationException("ContractNumber is required");

        // 2) Normalize group/type
        var group = (request.CategoryGroup ?? string.Empty).Trim().ToUpperInvariant();
        var type = (request.CategoryType ?? string.Empty).Trim().ToUpperInvariant();

        if (string.IsNullOrWhiteSpace(group))
            throw new InvalidOperationException("CategoryGroup is required");
        if (string.IsNullOrWhiteSpace(type))
            throw new InvalidOperationException("CategoryType is required");

        if (group is not ("EXPENSE" or "INCOME"))
            throw new InvalidOperationException("CategoryGroup must be EXPENSE or INCOME");
        if (type is not ("EXPENSE" or "INCOME"))
            throw new InvalidOperationException("CategoryType must be EXPENSE or INCOME");
        if (!string.Equals(group, type, StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("CategoryGroup and CategoryType must match");

        // 3) Validate name
        var name = (request.CategoryName ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidOperationException("CategoryName is required");

        if (name.StartsWith('{'))
        {
            try { using var _ = System.Text.Json.JsonDocument.Parse(name); }
            catch
            {
                throw new InvalidOperationException(
                    "CategoryName must be a valid JSON string (e.g. {\"vi\":\"...\",\"en\":\"...\"})."
                );
            }
        }

        // 4) Load walletIds by contract
        var walletProfiles = await walletProfileRepository.GetByContractNumber(contractNumber)
            ?? throw new InvalidOperationException($"Wallet contract not found: {contractNumber}");

        var walletIds = walletProfiles.Select(x => x.Id).Distinct().ToList();
        if (walletIds.Count == 0)
            throw new InvalidOperationException($"No wallet found for contract: {contractNumber}");

        // 5) Insert category for each wallet
        var resp = new CreateWalletCategoryResponseModel();

        foreach (var walletId in walletIds)
        {
            // duplicate check per wallet
            var existed = await walletCategoryRepository.ExistsByWalletGroupAndNameAsync(
                walletId: walletId,
                categoryGroup: group,
                categoryName: name,
                cancellationToken: ct
            );

            if (existed)
                continue; // hoặc throw nếu bạn muốn "đã tồn tại thì fail"

            var entity = new WalletCategory
            {
                CategoryCode = WalletProfileHelper.GenerateCategoryCode(group),
                WalletId = walletId,
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

            resp.Items.Add(new CreateWalletCategoryItemResult
            {
                WalletId = walletId,
                CategoryId = entity.Id,
                CategoryCode = entity.CategoryCode
            });
        }

        resp.CreatedCount = resp.Items.Count;
        return resp;
    }
}




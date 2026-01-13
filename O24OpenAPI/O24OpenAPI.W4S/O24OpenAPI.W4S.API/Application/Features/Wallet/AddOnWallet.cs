using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core.Constants;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.W4S.API.Application.Constants;
using O24OpenAPI.W4S.API.Application.Helpers;
using O24OpenAPI.W4S.API.Application.Models.Wallet;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;
using O24OpenAPI.W4S.Domain.AggregatesModel.WalletMasterAggregate;
using O24OpenAPI.W4S.Infrastructure.Configurations;

namespace O24OpenAPI.W4S.API.Application.Features.Wallet;

public class AddOnWalletCommand : BaseTransactionModel, ICommand<AddOnWalletResponseModel>
{
    public string ContractNumber { get; set; }
    public string Color { get; set; }
    public string Icon { get; set; }
    public bool IsIncludeReport { get; set; }
    public string BaseCurrency { get; set; }
    public string WalletType { get; set; }
    public string Classification { get; set; }
    public string Phone { get; set; }
}

[CqrsHandler]
public class AddOnWalletHandler(
    IWalletProfileRepository walletProfileRepository,
    IWalletContractRepository walletContractRepository,
    IWalletCategoryDefaultRepository walletCategoryDefaultRepository,
    IWalletCategoryRepository walletCategoryRepository,
    IWalletAccountProfileRepository walletAccountProfileRepository,
    W4SSetting w4SSetting
) : ICommandHandler<AddOnWalletCommand, AddOnWalletResponseModel>
{
    [WorkflowStep(WorkflowStepCode.W4S.WF_STEP_W4S_ADD_ON_WALLET)]
    public async Task<AddOnWalletResponseModel> HandleAsync(
        AddOnWalletCommand request,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            await ValidateRequest(request);

            var contractNumber = request.ContractNumber;

            var contractInfo = await walletContractRepository.GetByContractNumberAsync(contractNumber: request.ContractNumber) ??
              throw await O24Exception.CreateAsync(
                    O24W4SResourceCode.Validation.WalletContractNotFound,
                    request.Language,
                    contractNumber
                ); ;


            WalletProfile profile = WalletProfile.Create(
                walletProfileCode: WalletProfileHelper.GenerateWalletProfileCode(
                    request.WalletType,
                    request.Classification
                ),
                contractNumber: contractNumber,
                userCode: request.Phone,
                walletName: contractInfo.FullName,
                walletType: request.WalletType,
                defaultCurrency: request.BaseCurrency,
                icon: request.Icon ?? w4SSetting.DefaultWalletIcon,
                color: request.Color ?? w4SSetting.DefaultWalletColor
            );

            profile = await walletProfileRepository.InsertAsync(profile);

            if (request.WalletType == "TWDR")
            {
                await CloneDefaultCategoriesToWalletAsync(profile.Id);
                await walletAccountProfileRepository.CreateDefaultAccount(profile.Id);
            }


            return new AddOnWalletResponseModel
            {
                WalletId = profile.Id,
                ContractNumber = contractInfo.ContractNumber
            };
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync();
            throw;
        }
    }

    /// <summary>
    /// Validate request
    /// </summary>
    /// <param name="r"></param>
    /// <param name="language"></param>
    /// <returns></returns>
    private async static Task ValidateRequest(
        AddOnWalletCommand r
    )
    {
        if (string.IsNullOrWhiteSpace(r.ContractNumber))
            throw await O24Exception.CreateAsync(
                ResourceCode.Validation.RequiredField,
                r.Language,
                "ContractNumber"
            );

        if (string.IsNullOrWhiteSpace(r.WalletType))
            throw await O24Exception.CreateAsync(
                ResourceCode.Validation.RequiredField,
                r.Language,
                "WalletType"
            );
    }

    /// <summary>
    /// Clone default categories to wallet
    /// </summary>
    /// <param name="walletId"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    private async Task CloneDefaultCategoriesToWalletAsync(int walletId)
    {
        IList<WalletCategoryDefault> defaults =
            await walletCategoryDefaultRepository.GetActiveAsync();

        if (defaults is null || defaults.Count == 0)
            return;

        List<WalletCategory> toInsert = new(defaults.Count);

        foreach (WalletCategoryDefault d in defaults.OrderBy(x => x.SortOrder))
        {
            string categoryCode = d.CategoryCode;

            bool exists = await walletCategoryRepository.ExistsAsync(walletId, categoryCode);
            if (exists)
                continue;

            WalletCategory entity = WalletCategory.Create(
                categoryCode: WalletProfileHelper.GenerateCategoryCode(d.CategoryGroup),
                walletId: walletId,
                parentCategoryId: 0,
                categoryGroup: d.CategoryGroup,
                categoryType: d.CategoryType,
                categoryName: d.CategoryName,
                icon: d.Icon,
                color: d.Color
            );

            toInsert.Add(entity);
        }

        if (toInsert.Count > 0)
            await walletCategoryRepository.BulkInsertAsync(toInsert);
    }
}

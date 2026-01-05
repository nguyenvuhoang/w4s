using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.Framework.Services.Configuration;
using O24OpenAPI.W4S.API.Application.Constants;
using O24OpenAPI.W4S.API.Application.Models.Wallet;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;
using O24OpenAPI.W4S.Domain.AggregatesModel.WalletMasterAggregate;

namespace O24OpenAPI.W4S.API.Application.Features.Wallet;

public class RetrieveWalletInformationCommand : BaseTransactionModel, ICommand<WalletInformationResponseModel>
{
    public string ContractNumber { get; set; } = default!;
}

[CqrsHandler]
public class RetrieveWalletInformationHandler(
    IWalletProfileRepository walletProfileRepository,
    IWalletContractRepository walletContractRepository,
    IWalletAccountProfileRepository walletAccountRepository,
    IWalletBalanceRepository walletBalanceRepository,
    IWalletCategoryRepository walletCategoryRepository,
    IWalletBudgetRepository walletBudgetRepository,
    IWalletGoalRepository walletGoalRepository,
    ICodeListService systemCodeService
) : ICommandHandler<RetrieveWalletInformationCommand, WalletInformationResponseModel>
{
    [WorkflowStep(WorkflowStepCode.W4S.WF_STEP_W4S_RETRIEVE_WALLET_INFORMATION)]
    public async Task<WalletInformationResponseModel> HandleAsync(
        RetrieveWalletInformationCommand request,
        CancellationToken cancellationToken = default
    )
    {
        IList<WalletProfile> wallets;
        if (string.IsNullOrEmpty(request.ContractNumber))
            return new WalletInformationResponseModel();
        var contractNumber = request.ContractNumber;
        wallets = await walletProfileRepository.GetByContractNumber(contractNumber) ?? throw await O24Exception.CreateAsync(
               O24W4SResourceCode.Validation.WalletContractNotFound,
               request.Language,
               contractNumber
           );
        var contract = await walletContractRepository.GetByContractNumberAsync(contractNumber);

        var walletIdStrings = wallets.Select(x => x.WalletId.ToString()).Distinct().ToList();

        var accounts = await walletAccountRepository.GetWalletAccountByWalletIdAsync(walletIdStrings);
        var accountNumbers = accounts.Select(a => a.AccountNumber).Distinct().ToList();

        var balances = await walletBalanceRepository.GetByAccountNumbersAsync(accountNumbers);
        var categories = await walletCategoryRepository.GetByWalletIdsAsync(walletIdStrings);
        var budgets = await walletBudgetRepository.GetByWalletIdsAsync(walletIdStrings);
        var goals = await walletGoalRepository.GetByWalletIdsAsync(walletIdStrings);

        var balancesByAcc = balances.ToDictionary(x => x.AccountNumber, x => x);
        var accountsByWallet = accounts.GroupBy(x => x.WalletId).ToDictionary(g => g.Key, g => g.ToList());
        var categoriesByWallet = categories.GroupBy(x => x.WalletId).ToDictionary(g => g.Key, g => g.ToList());
        var budgetsByWallet = budgets.GroupBy(x => x.WalletId).ToDictionary(g => g.Key, g => g.ToList());
        var goalsByWallet = goals.GroupBy(x => x.WalletId).ToDictionary(g => g.Key, g => g.ToList());

        var contractType = (int?)contract.ContractType;
        var contractTypeCaption = await systemCodeService.GetCaption(contractType.ToString(), "CONTRACTTYPE", "W4S", request.Language);
        var contractStatus = contract.Status.ToString();
        var contractStatusCaption = await systemCodeService.GetCaption(contractStatus, "CONTRACTSTATUS", "BO", request.Language);


        var response = new WalletInformationResponseModel
        {
            Contract = contract == null
                ? null
                : new WalletContractResponseModel
                {
                    Id = contract.Id,
                    ContractNumber = contract.ContractNumber,
                    ContractType = contractType,
                    ContractTypeCaption = contractTypeCaption,
                    Status = contractStatus,
                    StatusCaption = contractStatusCaption,
                    CreatedOnUtc = contract.CreatedOnUtc,
                    UpdatedOnUtc = contract.UpdatedOnUtc
                }
        };

        foreach (var w in wallets.OrderBy(x => x.Id))
        {
            var wid = w.WalletId.ToString();
            var walletTypeCaption = await systemCodeService.GetCaption(w.WalletType, "WALLETTYPE", "W4S", request.Language);
            var walletStatus = w.Status.ToString();
            var walletStatusCaption = await systemCodeService.GetCaption(walletStatus, "WALLETSTATUS", "BO", request.Language);
            var walletDetail = new WalletProfileDetailResponseModel
            {
                Id = w.Id,
                WalletId = w.WalletId,
                UserCode = w.UserCode,
                ContractNumber = w.ContractNumber,
                WalletName = w.WalletName,
                WalletType = w.WalletType,
                WalletTypeCaption = walletTypeCaption,
                DefaultCurrency = w.DefaultCurrency,
                Status = walletStatus,
                StatusCaption = walletStatusCaption,
                CreatedOnUtc = (DateTime)w.CreatedOnUtc,
                UpdatedOnUtc = w.UpdatedOnUtc
            };

            // accounts + balance
            if (accountsByWallet.TryGetValue(wid, out var accs))
            {
                var ordered = accs
                    .OrderByDescending(a => a.IsPrimary)
                    .ThenBy(a => a.Id)
                    .ToList();

                var accountTasks = ordered.Select(async a =>
                {
                    balancesByAcc.TryGetValue(a.AccountNumber, out var bal);

                    var accountTypeCaption = await systemCodeService.GetCaption(
                        a.AccountType,
                        "ACCOUNTTYPE",
                        "W4S",
                        request.Language
                    );

                    var statusCaption = await systemCodeService.GetCaption(
                        a.Status,
                        "ACCOUNTSTATUS",
                        "W4S",
                        request.Language
                    );

                    return new WalletAccountWithBalanceResponseModel
                    {
                        Id = a.Id,
                        WalletId = a.WalletId,
                        AccountNumber = a.AccountNumber,
                        AccountType = a.AccountType,
                        AccountTypeCaption = accountTypeCaption,
                        CurrencyCode = a.CurrencyCode,
                        IsPrimary = a.IsPrimary,
                        Status = a.Status,
                        StatusCaption = statusCaption,
                        Balance = bal == null
                            ? null
                            : new WalletBalanceResponseModel
                            {
                                Id = bal.Id,
                                Balance = bal.Balance,
                                BonusBalance = bal.BonusBalance,
                                LockedBalance = bal.LockedBalance,
                                AvailableBalance = bal.AvailableBalance
                            }
                    };
                });

                walletDetail.Accounts = [.. (await Task.WhenAll(accountTasks))];
            }


            // categories
            if (categoriesByWallet.TryGetValue(wid, out var cats))
            {
                walletDetail.Categories = [.. cats
                    .OrderBy(x => x.Id)
                    .Select(c => new WalletCategoryResponseModel(c))];
            }

            // budgets
            if (budgetsByWallet.TryGetValue(wid, out var bgs))
            {
                walletDetail.Budgets = [.. bgs
                    .OrderByDescending(x => x.StartDate)
                    .ThenByDescending(x => x.Id)
                    .Select(x => new WalletBudgetResponseModel
                    {
                        Id = x.Id,
                        BudgetId = x.BudgetId,
                        WalletId = x.WalletId,
                        CategoryId = x.CategoryId,
                        Amount = x.Amount,
                        SourceBudget = x.SourceBudget,
                        SouceTracker = x.SouceTracker,
                        PeriodType = x.PeriodType,
                        StartDate = x.StartDate,
                        EndDate = x.EndDate
                    })];
            }

            // goals
            if (goalsByWallet.TryGetValue(wid, out var gls))
            {
                walletDetail.Goals = [.. gls
                    .OrderByDescending(x => x.Id)
                    .Select(x => new WalletGoalResponseModel
                    {
                        Id = x.Id,
                        GoalId = x.GoalId,
                        WalletId = x.WalletId,
                        GoalName = x.GoalName,
                        TargetAmount = x.TargetAmount ?? 0,
                        CurrentAmount = x.CurrentAmount ?? 0,
                        TargetDate = x.TargetDate
                    })];
            }

            response.Wallets.Add(walletDetail);
        }

        return response;
    }
}

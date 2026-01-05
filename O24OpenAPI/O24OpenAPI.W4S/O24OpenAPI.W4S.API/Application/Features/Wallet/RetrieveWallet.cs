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
        if (string.IsNullOrWhiteSpace(request.ContractNumber))
            throw await O24Exception.CreateAsync(
               O24W4SResourceCode.Validation.WalletContractNotFound,
               request.Language,
               request.ContractNumber
           );

        var contractNumber = request.ContractNumber.Trim();

        var captionCache = new Dictionary<string, string>(StringComparer.Ordinal);

        Task<string> GetCaptionCached(string? code, string group, string app)
        {
            code ??= string.Empty;
            var key = $"{app}|{group}|{request.Language}|{code}";

            if (captionCache.TryGetValue(key, out var cached))
                return Task.FromResult(cached);

            return LoadAndCache();

            async Task<string> LoadAndCache()
            {
                var v = await systemCodeService.GetCaption(code, group, app, request.Language);
                captionCache[key] = v;
                return v;
            }
        }

        static Guid ParseGuidOrEmpty(string? s)
        {
            return Guid.TryParse(s, out var g) ? g : Guid.Empty;
        }

        var wallets =
            await walletProfileRepository.GetByContractNumber(contractNumber)
            ?? throw await O24Exception.CreateAsync(
                O24W4SResourceCode.Validation.WalletContractNotFound,
                request.Language,
                contractNumber
            );

        var contract = await walletContractRepository.GetByContractNumberAsync(contractNumber);

        var walletIds = wallets.Select(x => x.WalletId).Distinct().ToList();

        var walletIdStrings = walletIds.Select(x => x.ToString()).ToList();

        var accountsTask = walletAccountRepository.GetWalletAccountByWalletIdAsync(walletIdStrings);
        await Task.WhenAll(accountsTask);

        var accounts = accountsTask.Result ?? [];

        var accountNumbers = accounts
            .Select(a => a.AccountNumber)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct()
            .ToList();

        var balancesTask = walletBalanceRepository.GetByAccountNumbersAsync(accountNumbers);
        var categoriesTask = walletCategoryRepository.GetByWalletIdsAsync(walletIdStrings);
        var budgetsTask = walletBudgetRepository.GetByWalletIdsAsync(walletIdStrings);
        var goalsTask = walletGoalRepository.GetByWalletIdsAsync(walletIdStrings);

        await Task.WhenAll(balancesTask, categoriesTask, budgetsTask, goalsTask);

        var balances = balancesTask.Result ?? [];
        var categories = categoriesTask.Result ?? [];
        var budgets = budgetsTask.Result ?? [];
        var goals = goalsTask.Result ?? [];

        var balancesByAcc = balances
            .GroupBy(x => x.AccountNumber)
            .ToDictionary(g => g.Key, g => g.OrderByDescending(x => x.Id).First(), StringComparer.Ordinal);

        var accountsByWallet = accounts
            .GroupBy(a => ParseGuidOrEmpty(a.WalletId))
            .Where(g => g.Key != Guid.Empty)
            .ToDictionary(g => g.Key, g => g.ToList());

        var categoriesByWallet = categories
            .GroupBy(c => ParseGuidOrEmpty(c.WalletId))
            .Where(g => g.Key != Guid.Empty)
            .ToDictionary(g => g.Key, g => g.ToList());

        var budgetsByWallet = budgets
            .GroupBy(b => ParseGuidOrEmpty(b.WalletId))
            .Where(g => g.Key != Guid.Empty)
            .ToDictionary(g => g.Key, g => g.ToList());

        var goalsByWallet = goals
            .GroupBy(gl => ParseGuidOrEmpty(gl.WalletId))
            .Where(g => g.Key != Guid.Empty)
            .ToDictionary(g => g.Key, g => g.ToList());

        // Contract captions (only if contract exists)
        int? contractType = (int?)(contract?.ContractType);
        string? contractStatus = contract?.Status.ToString();

        string? contractTypeCaption = null;
        string? contractStatusCaption = null;

        if (contract != null)
        {
            contractTypeCaption = await GetCaptionCached(contractType?.ToString(), "CONTRACTTYPE", "W4S");

            contractStatusCaption = await GetCaptionCached(contractStatus, "CONTRACTSTATUS", "BO");
        }

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
                    OpenDateUtc = contract.OpenDateUtc,
                    CloseDateUtc = contract.CloseDateUtc,
                    CreatedOnUtc = contract.CreatedOnUtc,
                    UpdatedOnUtc = contract.UpdatedOnUtc
                }
        };

        var walletDetailTasks = wallets
            .OrderBy(x => x.Id)
            .Select(async w =>
            {
                var wid = w.WalletId;

                var walletTypeCaption = await GetCaptionCached(w.WalletType, "WALLETTYPE", "W4S");
                var walletStatusCode = w.Status.ToString() ?? string.Empty;
                var walletStatusCaption = await GetCaptionCached(walletStatusCode, "WALLETSTATUS", "BO");

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
                    Status = walletStatusCode,
                    StatusCaption = walletStatusCaption,
                    CreatedOnUtc = w.CreatedOnUtc ?? default,
                    UpdatedOnUtc = w.UpdatedOnUtc
                };

                // accounts + balance
                if (accountsByWallet.TryGetValue(wid, out var accs) && accs.Count > 0)
                {
                    var ordered = accs
                        .OrderByDescending(a => a.IsPrimary)
                        .ThenBy(a => a.Id)
                        .ToList();

                    var accountTasks = ordered.Select(async a =>
                    {
                        balancesByAcc.TryGetValue(a.AccountNumber, out var bal);

                        var accountTypeCaption = await GetCaptionCached(a.AccountType, "ACCOUNTTYPE", "W4S");
                        var accountStatusCaption = await GetCaptionCached(a.Status, "ACCOUNTSTATUS", "W4S");

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
                            StatusCaption = accountStatusCaption,
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
                if (categoriesByWallet.TryGetValue(wid, out var cats) && cats.Count > 0)
                {
                    walletDetail.Categories = [.. cats
                        .OrderBy(x => x.Id)
                        .Select(c => new WalletCategoryResponseModel(c))];
                }

                // budgets
                if (budgetsByWallet.TryGetValue(wid, out var bgs) && bgs.Count > 0)
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
                if (goalsByWallet.TryGetValue(wid, out var gls) && gls.Count > 0)
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

                return walletDetail;
            });

        response.Wallets = [.. await Task.WhenAll(walletDetailTasks)];
        return response;
    }
}

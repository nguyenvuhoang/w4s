using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.Framework.Services.Configuration;
using O24OpenAPI.W4S.API.Application.Constants;
using O24OpenAPI.W4S.API.Application.Models.Wallet;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;
using O24OpenAPI.W4S.Domain.AggregatesModel.WalletMasterAggregate;

namespace O24OpenAPI.W4S.API.Application.Features.Wallet;

public class RetrieveWalletInformationCommand
    : BaseTransactionModel,
        ICommand<WalletInformationResponseModel>
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
    ICodeListService systemCodeService,
    IWalletTransactionRepository walletTransactionRepository
) : ICommandHandler<RetrieveWalletInformationCommand, WalletInformationResponseModel>
{
    [WorkflowStep(WorkflowStepCode.W4S.WF_STEP_W4S_RETRIEVE_WALLET_INFORMATION)]
    public async Task<WalletInformationResponseModel> HandleAsync(
        RetrieveWalletInformationCommand request,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.ContractNumber))
                throw await O24Exception.CreateAsync(
                    O24W4SResourceCode.Validation.WalletContractNotFound,
                    request.Language,
                    request.ContractNumber
                );

            string contractNumber = request.ContractNumber.Trim();

            Dictionary<string, string> captionCache = new(StringComparer.Ordinal);

            Task<string> GetCaptionCached(string code, string group, string app)
            {
                code ??= string.Empty;
                string key = $"{app}|{group}|{request.Language}|{code}";

                if (captionCache.TryGetValue(key, out string cached))
                    return Task.FromResult(cached);

                return LoadAndCache();

                async Task<string> LoadAndCache()
                {
                    string v = await systemCodeService.GetCaption(
                        code,
                        group,
                        app,
                        request.Language
                    );
                    captionCache[key] = v;
                    return v;
                }
            }

            List<WalletProfile> wallets =
                await walletProfileRepository.GetByContractNumber(contractNumber)
                ?? throw await O24Exception.CreateAsync(
                    O24W4SResourceCode.Validation.WalletContractNotFound,
                    request.Language,
                    contractNumber
                );

            WalletContract contract = await walletContractRepository.GetByContractNumberAsync(
                contractNumber
            );

            List<int> walletIds = wallets.Select(x => x.Id).Distinct().ToList();

            Task<List<WalletAccount>> accountsTask =
                walletAccountRepository.GetWalletAccountByWalletIdAsync(walletIds);
            await Task.WhenAll(accountsTask);

            List<WalletAccount> accounts = accountsTask.Result ?? [];

            List<string> accountNumbers = [.. accounts
                .Select(a => a.AccountNumber)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct()];

            Task<List<WalletBalance>> balancesTask =
                walletBalanceRepository.GetByAccountNumbersAsync(accountNumbers);
            Task<List<WalletCategory>> categoriesTask =
                walletCategoryRepository.GetByWalletIdsAsync(walletIds);
            Task<List<WalletBudget>> budgetsTask = walletBudgetRepository.GetByWalletIdsAsync(
                walletIds
            );
            Task<List<WalletGoal>> goalsTask = walletGoalRepository.GetByWalletIdsAsync(walletIds);

            List<(int WalletId, Task<IList<WalletTransaction>> Task)> txTaskList = [.. walletIds
                .Distinct()
                .Select(wid =>
                    (
                        WalletId: wid,
                        Task: walletTransactionRepository.GetByWalletIdAsync(
                            wid,
                            fromUtc: null,
                            toUtc: null,
                            skip: 0,
                            take: 50,
                            cancellationToken: cancellationToken
                        )
                    )
                )];

            try
            {
                await Task.WhenAll(txTaskList.Select(x => x.Task));
            }
            catch
            {
                var failed = txTaskList
                    .Where(x => x.Task.IsFaulted)
                    .Select(x => new
                    {
                        x.WalletId,
                        Error = x.Task.Exception?.GetBaseException().Message,
                        Full = x.Task.Exception?.ToString(),
                    })
                    .ToList();

                throw;
            }

            Dictionary<int, IList<WalletTransaction>> txsByWallet = txTaskList.ToDictionary(
                x => x.WalletId,
                x => x.Task.Result ?? []
            );

            txsByWallet = txTaskList.ToDictionary(x => x.WalletId, x => x.Task.Result ?? []);

            await Task.WhenAll(balancesTask, categoriesTask, budgetsTask, goalsTask);

            List<WalletBalance> balances = balancesTask.Result ?? [];
            List<WalletCategory> categories = categoriesTask.Result ?? [];
            List<WalletBudget> budgets = budgetsTask.Result ?? [];
            List<WalletGoal> goals = goalsTask.Result ?? [];

            Dictionary<string, WalletBalance> balancesByAcc = balances
                .GroupBy(x => x.AccountNumber)
                .ToDictionary(
                    g => g.Key,
                    g => g.OrderByDescending(x => x.Id).First(),
                    StringComparer.Ordinal
                );

            Dictionary<int, List<WalletAccount>> accountsByWallet = accounts
                .GroupBy(a => a.WalletId)
                .ToDictionary(g => g.Key, g => g.ToList());

            Dictionary<int, List<WalletCategory>> categoriesByWallet = categories
                .GroupBy(a => a.WalletId)
                .ToDictionary(g => g.Key, g => g.ToList());

            Dictionary<int, List<WalletBudget>> budgetsByWallet = budgets
                .GroupBy(a => a.WalletId)
                .ToDictionary(g => g.Key, g => g.ToList());

            Dictionary<int, List<WalletGoal>> goalsByWallet = goals
                .GroupBy(a => a.WalletId)
                .ToDictionary(g => g.Key, g => g.ToList());

            // Contract captions (only if contract exists)
            int? contractType = (int?)(contract?.ContractType);
            string contractStatus = contract?.Status.ToString();

            string contractTypeCaption = null;
            string contractStatusCaption = null;

            if (contract != null)
            {
                contractTypeCaption = await GetCaptionCached(
                    contractType?.ToString(),
                    "CONTRACTTYPE",
                    "W4S"
                );

                contractStatusCaption = await GetCaptionCached(
                    contractStatus,
                    "CONTRACTSTATUS",
                    "BO"
                );
            }

            WalletInformationResponseModel response = new()
            {
                Contract =
                    contract == null
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
                            UpdatedOnUtc = contract.UpdatedOnUtc,
                        },
            };

            IEnumerable<Task<WalletProfileDetailResponseModel>> walletDetailTasks = wallets
                .OrderBy(x => x.Id)
                .Select(async w =>
                {
                    int wid = w.Id;

                    string walletTypeCaption = await GetCaptionCached(
                        w.WalletType,
                        "WALLETTYPE",
                        "W4S"
                    );
                    string walletStatusCode = w.Status.ToString() ?? string.Empty;
                    string walletStatusCaption = await GetCaptionCached(
                        walletStatusCode,
                        "WALLETSTATUS",
                        "BO"
                    );

                    WalletProfileDetailResponseModel walletDetail = new()
                    {
                        Id = w.Id,
                        WalletId = w.Id,
                        UserCode = w.UserCode,
                        ContractNumber = w.ContractNumber,
                        WalletName = w.WalletName,
                        WalletType = w.WalletType,
                        WalletTypeCaption = walletTypeCaption,
                        DefaultCurrency = w.DefaultCurrency,
                        Status = walletStatusCode,
                        StatusCaption = walletStatusCaption,
                        CreatedOnUtc = w.CreatedOnUtc ?? default,
                        UpdatedOnUtc = w.UpdatedOnUtc,
                    };

                    // accounts + balance
                    if (
                        accountsByWallet.TryGetValue(wid, out List<WalletAccount> accs)
                        && accs.Count > 0
                    )
                    {
                        List<WalletAccount> ordered = [.. accs.OrderByDescending(a => a.IsPrimary).ThenBy(a => a.Id)];

                        IEnumerable<Task<WalletAccountWithBalanceResponseModel>> accountTasks =
                            ordered.Select(async a =>
                            {
                                balancesByAcc.TryGetValue(a.AccountNumber, out WalletBalance bal);

                                string accountTypeCaption = await GetCaptionCached(
                                    a.AccountType,
                                    "ACCOUNTTYPE",
                                    "W4S"
                                );
                                string accountStatusCaption = await GetCaptionCached(
                                    a.Status,
                                    "ACCOUNTSTATUS",
                                    "W4S"
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
                                    StatusCaption = accountStatusCaption,
                                    Balance =
                                        bal == null
                                            ? null
                                            : new WalletBalanceResponseModel
                                            {
                                                Id = bal.Id,
                                                Balance = bal.Balance,
                                                BonusBalance = bal.BonusBalance,
                                                LockedBalance = bal.LockedBalance,
                                                AvailableBalance = bal.AvailableBalance,
                                            },
                                };
                            });

                        walletDetail.Accounts = [.. await Task.WhenAll(accountTasks)];
                    }

                    // categories
                    if (
                        categoriesByWallet.TryGetValue(wid, out List<WalletCategory> cats)
                        && cats.Count > 0
                    )
                    {
                        walletDetail.Categories =
                        [
                            .. cats.OrderBy(x => x.Id)
                                .Select(c => new WalletCategoryResponseModel(c)),
                        ];
                    }

                    // budgets
                    if (
                        budgetsByWallet.TryGetValue(wid, out List<WalletBudget> bgs)
                        && bgs.Count > 0
                    )
                    {
                        walletDetail.Budgets =
                        [
                            .. bgs.OrderByDescending(x => x.StartDate)
                                .ThenByDescending(x => x.Id)
                                .Select(x => new WalletBudgetResponseModel
                                {
                                    Id = x.Id,
                                    BudgetId = x.Id,
                                    WalletId = x.WalletId,
                                    CategoryId = x.CategoryId,
                                    Amount = x.Amount,
                                    SourceBudget = x.SourceBudget,
                                    SouceTracker = x.SouceTracker,
                                    PeriodType = x.PeriodType,
                                    StartDate = x.StartDate,
                                    EndDate = x.EndDate,
                                }),
                        ];
                    }

                    // goals
                    if (goalsByWallet.TryGetValue(wid, out List<WalletGoal> gls) && gls.Count > 0)
                    {
                        walletDetail.Goals =
                        [
                            .. gls.OrderByDescending(x => x.Id)
                                .Select(x => new WalletGoalResponseModel
                                {
                                    Id = x.Id,
                                    WalletId = x.WalletId,
                                    GoalName = x.GoalName,
                                    TargetAmount = x.TargetAmount ?? 0,
                                    CurrentAmount = x.CurrentAmount ?? 0,
                                    TargetDate = x.TargetDate,
                                }),
                        ];
                    }

                    if (txsByWallet.TryGetValue(wid, out var txs) && txs.Count > 0)
                    {
                        walletDetail.Transactions =
                        [
                            .. txs.OrderByDescending(x => x.TRANSACTIONDATE)
                                .Select(x => new WalletTransactionResponseModel
                                {
                                    TransactionId = x.TRANSACTIONID,
                                    Name = x.TRANSACTIONNAME ?? x.CHAR01 ?? string.Empty,
                                    Category = x.CHAR02 ?? string.Empty,
                                    Amount = x.NUM01 ?? 0,
                                    Currency = x.CCYID ?? "VND",
                                    TransactionDate = x.TRANSACTIONDATE,
                                    Description = x.TRANDESC,
                                    Status = x.STATUS,
                                }),
                        ];
                    }

                    return walletDetail;
                });

            response.Wallets = [.. await Task.WhenAll(walletDetailTasks)];
            return response;
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync();
            throw;
        }
    }
}

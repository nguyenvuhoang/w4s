using LinKit.Core.Cqrs;
using LinqToDB;
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
using O24OpenAPI.W4S.Domain.AggregatesModel.CommonAggregate;
using O24OpenAPI.W4S.Domain.AggregatesModel.WalletMasterAggregate;
using O24OpenAPI.W4S.Domain.Constants;
using O24OpenAPI.W4S.Infrastructure.Configurations;
using static O24OpenAPI.W4S.API.Application.Constants.Code;

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
    public decimal Amount { get; set; }
    public string WalletName { get; set; }
}

[CqrsHandler]
public class AddOnWalletHandler(
    IWalletProfileRepository walletProfileRepository,
    IWalletContractRepository walletContractRepository,
    IWalletCategoryDefaultRepository walletCategoryDefaultRepository,
    IWalletCategoryRepository walletCategoryRepository,
    IWalletAccountProfileRepository walletAccountProfileRepository,
    IWalletBalanceRepository walletBalanceRepository,
    IWalletLedgerEntryRepository walletLedgerEntryRepository,
    IWalletAccountGLsRepository walletAccountGLsRepository,
    IWalletTransactionRepository walletTransactionRepository,
    IWalletCategoryGLsRepository walletCateoryGLsRepository,
    ICurrencyRepository currencyRepository,
    IWalletStatementRepository walletStatementRepository,
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
                walletName: request.WalletName,
                walletType: request.WalletType,
                defaultCurrency: request.BaseCurrency,
                icon: request.Icon ?? w4SSetting.DefaultWalletIcon,
                color: request.Color ?? w4SSetting.DefaultWalletColor
            );

            profile = await walletProfileRepository.InsertAsync(profile);
            var entriesInitBalance = new List<WalletLedgerEntry>();
            if (request.WalletType == "TWDR")
            {
                await CloneDefaultCategoriesToWalletAsync(profile.Id);
                var listAccount = await walletAccountProfileRepository.CreateDefaultAccount(profile.Id, request.BaseCurrency);

                var accountIncome = listAccount?.FirstOrDefault(x => x.AccountType == WalletAccountType.Income);

                if (accountIncome != null && request.Amount > 0)
                {
                    var refid = string.IsNullOrWhiteSpace(request.RefId)
                        ? Guid.NewGuid().ToString("N")
                        : request.RefId;

                    entriesInitBalance = await InitBalanceAsync(
                     refid: refid,
                     walletId: profile.Id.ToString(),
                     phone: request.Phone,
                     accountNumber: accountIncome.AccountNumber!,
                     currency: request.BaseCurrency,
                     amount: request.Amount,
                     language: request.Language
                 );
                }
            }

            return new AddOnWalletResponseModel
            {
                WalletId = profile.Id,
                ContractNumber = contractInfo.ContractNumber,
                WalletLedgerEntries = entriesInitBalance

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

    /// <summary>
    /// Init Balance
    /// </summary>
    /// <param name="walletId"></param>
    /// <param name="accountNumber"></param>
    /// <param name="currency"></param>
    /// <param name="amount"></param>
    /// <param name="language"></param>
    /// <returns></returns>
    private async Task<List<WalletLedgerEntry>> InitBalanceAsync(
        string refid,
        string walletId,
        string phone,
        string accountNumber,
        string currency,
        decimal amount,
        string language
    )
    {
        if (amount <= 0) return [];

        var now = DateTime.UtcNow;

        if (!int.TryParse(walletId, out var walletIdInt))
            throw new ArgumentException($"Invalid walletId: {walletId}", nameof(walletId));

        var transactionId = new SnowflakeTransactionNumberGenerator(machineId: 1)
            .GenerateTransactionNumber();

        var tran = WalletTransaction.Create(
            transactionId: transactionId,
            transactionCode: WalletTranCode.WALLET_OPENING,
            sourceTranRef: refid,
            userId: phone,
            walletId: walletId,
            walletAccount: accountNumber,
            amount: amount,
            currency: currency,
            description: $"Init tracking wallet: {walletId}"
        );

        var gl = await ResolveOpeningGlAsync(currency, language);

        var entries = BuildOpeningEntries(
            transactionId: transactionId,
            currency: currency,
            amount: amount,
            postingUtc: now,
            virtualSourceGl: gl.VirtualSourceGL,
            walletAccountGl: gl.WalletAccountGL,
            virtualSourceEntryType: gl.VirtualSourceSysName,
            walletAccountEntryType: gl.WalletAccountSysName
        );


        var opening = 0m;

        await walletBalanceRepository.CreditBalanceAsync(accountNumber, amount);

        var statements = new List<WalletStatement>();

        var walletCreditEntry = entries.First(x => x.DrCr == DrCr.C);
        statements.Add(WalletStatement.Create(
            walletId: walletIdInt,
            accountNumber: accountNumber,
            statementOnUtc: walletCreditEntry.PostingDateUtc,
            drCr: DrCr.C,
            amount: walletCreditEntry.Amount,
            currencyCode: walletCreditEntry.Currency,
            openingBalance: opening,
            description: $"Opening wallet {walletId}",
            referenceType: "WalletTransaction",
            referenceId: transactionId,
            externalRef: refid,
            source: walletId
        ));

        var glAccounts = BuildGlMapping(
            walletAccount: accountNumber,
            currency: currency,
            walletAccountSysName: gl.WalletAccountSysName,
            walletAccountCatalog: gl.WalletAccountCatalog,
            walletAccountGl: gl.WalletAccountGL,
            virtualSourceSysName: gl.VirtualSourceSysName,
            virtualSourceCatalog: gl.VirtualSourceCatalog,
            virtualSourceGl: gl.VirtualSourceGL
        );

        await walletTransactionRepository.AddAsync(tran);
        await walletAccountGLsRepository.BulkInsert(glAccounts);
        await walletLedgerEntryRepository.BulkInsert(entries);

        await walletStatementRepository.BulkInsert(statements);

        return entries;
    }

    // ===== Helpers =====

    private sealed record OpeningGlResolved(
        string WalletAccountGL,
        string WalletAccountSysName,
        string WalletAccountCatalog,
        string VirtualSourceGL,
        string VirtualSourceSysName,
        string VirtualSourceCatalog
    );

    private async Task<OpeningGlResolved> ResolveOpeningGlAsync(string currency, string language)
    {
        var shortCurrency = await currencyRepository.GetShortCurrencyIdAsync(currency)
            ?? throw await O24Exception.CreateAsync(
                O24W4SResourceCode.Accounting.CurrencyIsNotDefined,
                language,
                currency);

        var walletAccountGLAlias = await walletCateoryGLsRepository.GetAccountingAccount("WALVN000", "WALLETACCOUNT")
            ?? throw await O24Exception.CreateAsync(
                O24W4SResourceCode.Accounting.AccountNumberIsNotDefined,
                language,
                "WALVN000");

        var virtualSourceGLAlias = await walletCateoryGLsRepository.GetAccountingAccount("VTSVN000", "VIRTUALSOURCE")
            ?? throw await O24Exception.CreateAsync(
                O24W4SResourceCode.Accounting.AccountNumberIsNotDefined,
                language,
                "VTSVN000");

        var dict = new Dictionary<string, string> { ["CUR"] = shortCurrency };

        var walletAccountGL = WalletAccountGLHelper.ResolveAccountAlias(walletAccountGLAlias.AccountAlias, dict);
        var virtualSourceGL = WalletAccountGLHelper.ResolveAccountAlias(virtualSourceGLAlias.AccountAlias, dict);

        return new OpeningGlResolved(
            WalletAccountGL: walletAccountGL,
            WalletAccountSysName: walletAccountGLAlias.SysAccountName,
            WalletAccountCatalog: walletAccountGLAlias.CatalogCode,
            VirtualSourceGL: virtualSourceGL,
            VirtualSourceSysName: virtualSourceGLAlias.SysAccountName,
            VirtualSourceCatalog: virtualSourceGLAlias.CatalogCode
        );
    }

    private static List<WalletLedgerEntry> BuildOpeningEntries(
        string transactionId,
        string currency,
        decimal amount,
        DateTime postingUtc,
        string virtualSourceGl,
        string walletAccountGl,
        string virtualSourceEntryType,
        string walletAccountEntryType)
    {
        return
        [
            // Debit: Virtual Source
            new()
            {
                TRANSACTIONID = transactionId,
                AccountNumber = virtualSourceGl,
                Currency = currency,
                Group = 1,
                Index = 1,
                DrCr = DrCr.D,
                Amount = amount,
                EntryType = virtualSourceEntryType,
                PostingDateUtc = postingUtc
            },

            // Credit: Wallet Account
            new()
            {
                TRANSACTIONID = transactionId,
                AccountNumber = walletAccountGl,
                Currency = currency,
                Group = 1,
                Index = 2,
                DrCr = DrCr.C,
                Amount = amount,
                EntryType = walletAccountEntryType,
                PostingDateUtc = postingUtc
            }
        ];
    }

    private static List<WalletAccountGLs> BuildGlMapping(
        string walletAccount,
        string currency,
        string walletAccountSysName,
        string walletAccountCatalog,
        string walletAccountGl,
        string virtualSourceSysName,
        string virtualSourceCatalog,
        string virtualSourceGl)
    {
        return
    [
        new()
        {
            WalletAccount = walletAccount,
            SysAccountName = walletAccountSysName,
            CatalogCode = walletAccountCatalog,
            GLAccount = walletAccountGl
        },
        new()
        {
            WalletAccount = walletAccount,
            SysAccountName = virtualSourceSysName,
            CatalogCode = virtualSourceCatalog ?? currency,
            GLAccount = virtualSourceGl
        }
    ];
    }

}

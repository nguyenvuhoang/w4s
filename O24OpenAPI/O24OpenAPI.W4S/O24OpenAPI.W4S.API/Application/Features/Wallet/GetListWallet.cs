using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.Logging.Extensions;
using O24OpenAPI.W4S.API.Application.Constants;
using O24OpenAPI.W4S.API.Application.Helpers;
using O24OpenAPI.W4S.API.Application.Models.Wallet;
using O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;
using O24OpenAPI.W4S.Infrastructure.Configurations;

namespace O24OpenAPI.W4S.API.Application.Features.Wallet;

public class GetListWalletCommand : BaseTransactionModel, ICommand<List<ListWalletResponseModel>>
{
    public string ContractNumber { get; set; } = default!;
}

[CqrsHandler]
public class GetListWalletHandler(IWalletProfileRepository walletProfileRepository,
    IWalletAccountProfileRepository walletAccountProfileRepository,
    IWalletBalanceRepository walletBalanceRepository,
    W4SSetting w4SSettings

    )
    : ICommandHandler<GetListWalletCommand, List<ListWalletResponseModel>>
{
    [WorkflowStep(WorkflowStepCode.W4S.WF_STEP_W4S_GET_LIST_WALLET)]
    public async Task<List<ListWalletResponseModel>> HandleAsync(
        GetListWalletCommand request,
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

            var wallets =
                await walletProfileRepository.GetByContractNumber(contractNumber)
                ?? throw await O24Exception.CreateAsync(
                    O24W4SResourceCode.Validation.WalletContractNotFound,
                    request.Language,
                    contractNumber
                );

            var walletAccountList =
                await walletAccountProfileRepository.GetWalletAccountByWalletIdAsync(
                    [.. wallets.Select(w => w.Id)]
                );

            var walletAccountBalance = await walletBalanceRepository.GetByAccountNumbersAsync(
                [.. walletAccountList.Select(wa => wa.AccountNumber)]
            );

            var baseCurrency = string.IsNullOrWhiteSpace(w4SSettings.BaseCurrency)
                ? "VND"
                : w4SSettings.BaseCurrency.Trim();

            var balanceByAccount = walletAccountBalance
            .GroupBy(b => b.AccountNumber)
            .ToDictionary(g => g.Key, g => g.First());


            var balanceByWallet = walletAccountList
             .GroupBy(wa => wa.WalletId)
             .ToDictionary(
                 g => g.Key,
                 g => g.Sum(a =>
                 {
                     var bal = balanceByAccount.TryGetValue(a.AccountNumber, out var b)
                         ? b.AvailableBalance
                         : 0m;

                     return bal;
                 })
             );

            var result = wallets
                .Select(w => new ListWalletResponseModel
                {
                    WalletId = w.Id,
                    ContractNumber = w.ContractNumber,
                    UserCode = w.UserCode ?? string.Empty,
                    WalletName = w.WalletName,
                    WalletType = w.WalletType,
                    AvailableBalance = balanceByWallet.TryGetValue(w.Id, out var available) ? available : 0,
                    DefaultCurrency = w.DefaultCurrency,
                    Status = w.Status,
                    Icon = w.Icon,
                    Color = w.Color,
                    Account = [.. walletAccountList
                        .Where(wa => wa.WalletId == w.Id)
                        .Select(wa => new WalletAccountWithBalanceResponseModel
                        {
                            Id = wa.Id,
                            WalletId = wa.WalletId,
                            AccountNumber = wa.AccountNumber,
                            AccountType = wa.AccountType,
                            AccountTypeCaption = WalletAccountTypeHelper.GetDisplayName(wa.AccountType),
                            CurrencyCode = wa.CurrencyCode,
                            IsPrimary = wa.IsPrimary,
                            Status = wa.Status,
                            Balance = walletAccountBalance.Where(b => b.AccountNumber == wa.AccountNumber)
                                .Select(b => new WalletBalanceResponseModel
                                {
                                    Balance = b.Balance,
                                    AvailableBalance = b.AvailableBalance,
                                    BonusBalance = b.BonusBalance,
                                    LockedBalance = b.LockedBalance
                                })
                                .FirstOrDefault()

                        })]
                })
                .ToList();

            return result;
        }
        catch (Exception ex)
        {
            ex.WriteError();
            throw;
        }
    }
}

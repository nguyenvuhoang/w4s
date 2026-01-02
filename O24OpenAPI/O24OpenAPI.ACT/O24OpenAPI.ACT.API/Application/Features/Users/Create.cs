using System.Text.Json;
using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.ACT.API.Application.Models;
using O24OpenAPI.ACT.Config;
using O24OpenAPI.ACT.Domain;
using O24OpenAPI.ACT.Domain.AggregatesModel.AccountAggregate;
using O24OpenAPI.ACT.Domain.AggregatesModel.CommonAggregate;
using O24OpenAPI.ACT.Domain.AggregatesModel.RulesAggregate;
using O24OpenAPI.ACT.Infrastructure.Repositories;
using O24OpenAPI.Core;
using O24OpenAPI.Data.Extensions;
using O24OpenAPI.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.ACT.API.Application.Features.Users
{
    public class CreateCommand : BaseTransactionModel, ICommand<AccountChartCRUDReponseModel>
    {
        /// <summary>
        /// AccountNumber
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// CurrencyCode
        /// </summary>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// BranchCode
        /// </summary>
        public string BranchCode { get; set; }

        /// <summary>
        /// ParentAccountId
        /// </summary>
        public string ParentAccountId { get; set; }

        /// <summary>
        /// AccountLevel
        /// </summary>
        public int AccountLevel { get; set; }

        /// <summary>
        /// IsAccountLeave
        /// </summary>
        public bool IsAccountLeave { get; set; }

        /// <summary>
        /// BalanceSide
        /// </summary>
        public string BalanceSide { get; set; }

        /// <summary>
        /// ReverseBalance
        /// </summary>
        public string ReverseBalance { get; set; }

        /// <summary>
        /// PostingSide
        /// </summary>
        public string PostingSide { get; set; }

        /// <summary>
        /// AccountName
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// ShortAccountName
        /// </summary>
        public string ShortAccountName { get; set; }

        /// <summary>
        /// MultiValueName
        /// </summary>
        public string MultiValueName { get; set; }

        /// <summary>
        /// AccountClassification
        /// </summary>
        public string AccountClassification { get; set; }

        /// <summary>
        /// AccountCategories
        /// </summary>
        public string AccountCategories { get; set; }

        /// <summary>
        /// AccountGroup
        /// </summary>
        public string AccountGroup { get; set; }

        /// <summary>
        /// DirectPosting
        /// </summary>
        public string DirectPosting { get; set; }

        /// <summary>
        /// IsVisible
        /// </summary>
        public string IsVisible { get; set; }

        /// <summary>
        /// IsMultiCurrency
        /// </summary>
        public string IsMultiCurrency { get; set; }

        /// <summary>
        /// JobProcessOption
        /// </summary>
        public string JobProcessOption { get; set; }

        /// <summary>
        /// RefAccountNumber
        /// </summary>
        public string RefAccountNumber { get; set; }

        /// <summary>
        /// ReferencesNumber
        /// </summary>
        public string ReferencesNumber { get; set; }

        /// <summary>
        /// Last update date
        /// </summary>
        public DateTime? CreatedOnUtc { get; set; }

        /// <summary>
        /// Last update date
        /// </summary>
        public DateTime? UpdatedOnUtc { get; set; }
        public string referenceId { get; set; }
    }

    public class CreateHandle(
        IAccountChartRepository accountChartRepository,
        ICurrencyRepository currencyRepository,
        ICheckingAccountRulesRepository checkingAccountRulesRepository,
        AccountingSettings accountingSettings
    ) : ICommandHandler<CreateCommand, AccountChartCRUDReponseModel>
    {
        public async Task<AccountChartCRUDReponseModel> HandleAsync(
            CreateCommand request,
            CancellationToken cancellationToken = default
        )
        {
            if (accountChartRepository.IsAccountNumberExist(request.AccountNumber))
            {
                throw new O24OpenAPIException("Bank Account Number " + request.AccountNumber);
            }

            if (request.AccountLevel == 8 || request.AccountLevel == 9)
            {
                var currency = await GetCurrency(request.CurrencyCode);
                if (currency.Id == 0)
                {
                    throw new O24OpenAPIException("Currency", request.CurrencyCode);
                }
            }

            await checkingAccountRulesRepository.CheckingRuleAccount(
                request.AccountClassification,
                request.ReverseBalance,
                request.BalanceSide,
                request.PostingSide,
                request.AccountGroup,
                request.AccountCategories
            );
            int sysLeaveLevel = accountingSettings.AccountLeaveLevel;

            request.ParentAccountId = request.AccountLevel switch
            {
                9 => request.AccountNumber.Substring(
                    accountingSettings.LengthBranch,
                    request.AccountNumber.Length - accountingSettings.LengthBranch
                ),
                1 => null,
                _ => request.AccountNumber.Substring(0, request.AccountNumber.Length - 2),
            };
            await request.Insert(request.referenceId);
            if (request.AccountLevel.Equals(sysLeaveLevel))
            {
                AccountBalance accountBalance = new AccountBalance
                {
                    AccountNumber = request.AccountNumber,
                    BranchCode = request.BranchCode,
                    CurrencyCode = request.CurrencyCode,
                };
                await accountBalance.Insert();
                request.IsAccountLeave = true;
                await accountChartRepository.Update(request);
            }
            var chartResult = request.ToModel<AccountChartCRUDReponseModel>();
            chartResult.MultiValueNameLang = JsonSerializer.Deserialize<MultiValueName>(
                request.MultiValueName
            );
            return chartResult;
        }

        public async Task<GetCurrencyResponse> GetCurrency(string currencyId)
        {
            var currency = await currencyRepository
                .Table.Where(c => c.CurrencyId.Equals(currencyId) && c.StatusOfCurrency.Equals("A"))
                .FirstOrDefaultAsync();
            if (currency == null)
            {
                var nullResponse = new GetCurrencyResponse { Id = 0 };
                return nullResponse;
            }
            var response = new GetCurrencyResponse
            {
                Id = currency.Id,
                CurrencyId = currency.CurrencyId,
                ShortCurrencyId = currency.ShortCurrencyId,
                StatusOfCurrency = currency.StatusOfCurrency,
                RoundingDigits = currency.RoundingDigits,
                DecimalDigits = currency.DecimalDigits,
                CurrencyNumber = currency.CurrencyNumber,
            };
            return response;
        }
    }
}

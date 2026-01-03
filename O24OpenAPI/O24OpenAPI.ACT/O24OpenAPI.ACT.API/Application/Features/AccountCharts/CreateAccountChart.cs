using System.Text.Json;
using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.ACT.API.Application.Models;
using O24OpenAPI.ACT.Config;
using O24OpenAPI.ACT.Domain.AggregatesModel.AccountAggregate;
using O24OpenAPI.ACT.Domain.AggregatesModel.CommonAggregate;
using O24OpenAPI.ACT.Domain.AggregatesModel.RulesAggregate;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core;
using O24OpenAPI.Data.Extensions;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.ACT.API.Application.Features.AccountCharts;

public class CreateAccountChartCommand
    : BaseTransactionModel,
        ICommand<AccountChartCRUDReponseModel>
{
    public string AccountNumber { get; set; }

    public string CurrencyCode { get; set; }

    public string BranchCode { get; set; }

    public string ParentAccountId { get; set; }

    public int AccountLevel { get; set; }

    public bool IsAccountLeave { get; set; }

    public string BalanceSide { get; set; }

    public string ReverseBalance { get; set; }

    public string PostingSide { get; set; }

    public string AccountName { get; set; }

    public string ShortAccountName { get; set; }

    public string MultiValueName { get; set; }

    public string AccountClassification { get; set; }

    public string AccountCategories { get; set; }

    public string AccountGroup { get; set; }

    public string DirectPosting { get; set; }

    public string IsVisible { get; set; }

    public string IsMultiCurrency { get; set; }

    public string JobProcessOption { get; set; }

    public string RefAccountNumber { get; set; }

    public string ReferencesNumber { get; set; }

    public DateTime? CreatedOnUtc { get; set; }

    public DateTime? UpdatedOnUtc { get; set; }
    public string referenceId { get; set; }
}

[CqrsHandler]
public class CreateHandle(
    IAccountChartRepository accountChartRepository,
    ICurrencyRepository currencyRepository,
    ICheckingAccountRulesRepository checkingAccountRulesRepository,
    AccountingSettings accountingSettings
) : ICommandHandler<CreateAccountChartCommand, AccountChartCRUDReponseModel>
{
    [WorkflowStep(WorkflowStepCode.ACT.WF_STEP_ACT_CREATE_ACCOUNTCHART)]
    public async Task<AccountChartCRUDReponseModel> HandleAsync(
        CreateAccountChartCommand request,
        CancellationToken cancellationToken = default
    )
    {
        AccountChart chart = request.ToAccountChart();
        if (accountChartRepository.IsAccountNumberExist(chart.AccountNumber))
        {
            throw new O24OpenAPIException("Bank Account Number " + chart.AccountNumber);
        }

        if (chart.AccountLevel == 8 || chart.AccountLevel == 9)
        {
            GetCurrencyResponse currency = await GetCurrency(chart.CurrencyCode);
            if (currency.Id == 0)
            {
                throw new O24OpenAPIException("Currency", chart.CurrencyCode);
            }
        }

        await checkingAccountRulesRepository.CheckingRuleAccount(
            chart.AccountClassification,
            chart.ReverseBalance,
            chart.BalanceSide,
            chart.PostingSide,
            chart.AccountGroup,
            chart.AccountCategories
        );
        int sysLeaveLevel = accountingSettings.AccountLeaveLevel;

        chart.ParentAccountId = chart.AccountLevel switch
        {
            9 => chart.AccountNumber.Substring(
                accountingSettings.LengthBranch,
                chart.AccountNumber.Length - accountingSettings.LengthBranch
            ),
            1 => null,
            _ => chart.AccountNumber.Substring(0, chart.AccountNumber.Length - 2),
        };
        await chart.Insert();
        if (chart.AccountLevel.Equals(sysLeaveLevel))
        {
            AccountBalance accountBalance = new()
            {
                AccountNumber = chart.AccountNumber,
                BranchCode = chart.BranchCode,
                CurrencyCode = chart.CurrencyCode,
            };
            await accountBalance.Insert();
            chart.IsAccountLeave = true;
            await accountChartRepository.Update(chart);
        }
        AccountChartCRUDReponseModel chartResult = chart.ToModel<AccountChartCRUDReponseModel>();
        chartResult.MultiValueNameLang = JsonSerializer.Deserialize<MultiValueName>(
            chart.MultiValueName
        );
        return chartResult;
    }

    public async Task<GetCurrencyResponse> GetCurrency(string currencyId)
    {
        Currency? currency = await currencyRepository
            .Table.Where(c => c.CurrencyId.Equals(currencyId) && c.StatusOfCurrency.Equals("A"))
            .FirstOrDefaultAsync();
        if (currency == null)
        {
            GetCurrencyResponse nullResponse = new() { Id = 0 };
            return nullResponse;
        }
        GetCurrencyResponse response = new()
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

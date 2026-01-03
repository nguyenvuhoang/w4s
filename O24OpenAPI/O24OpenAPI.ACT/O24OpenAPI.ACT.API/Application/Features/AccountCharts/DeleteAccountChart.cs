using LinKit.Core.Cqrs;
using O24OpenAPI.ACT.Domain.AggregatesModel.AccountAggregate;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core;
using O24OpenAPI.Data.Extensions;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Localization;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.ACT.API.Application.Features.AccountCharts
{
    public class DeleteAccountChartCommand : BaseTransactionModel, ICommand<bool>
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
    public class DeleteAccountHandle(
        IAccountBalanceRepository accountBalanceRepository,
        ILocalizationService localizationService
    ) : ICommandHandler<DeleteAccountChartCommand, bool>
    {
        [WorkflowStep(WorkflowStepCode.ACT.WF_STEP_ACT_DELETE_ACCOUNTCHART)]
        public async Task<bool> HandleAsync(
            DeleteAccountChartCommand request,
            CancellationToken cancellationToken = default
        )
        {
            AccountChart chart = request.ToAccountChart();
            if (chart == null || chart.Id == 0)
            {
                throw new ArgumentNullException("Bank Account Definition is not exist");
            }

            if (chart.IsAccountLeave)
            {
                AccountBalance accBalance = await accountBalanceRepository.GetByAccountNumber(
                    chart.AccountNumber
                );
                if (
                    !(
                        accBalance.DailyCredit == 0
                        && accBalance.WeeklyCredit == 0
                        && accBalance.WeeklyDebit == 0
                        && accBalance.MonthlyCredit == 0
                        && accBalance.MonthlyDebit == 0
                        && accBalance.QuarterlyCredit == 0
                        && accBalance.QuarterlyDebit == 0
                        && accBalance.HalfYearlyCredit == 0
                        && accBalance.HalfYearlyDebit == 0
                        && accBalance.YearlyCredit == 0
                        && accBalance.YearlyDebit == 0
                        && accBalance.WeekAverageBalance == 0
                        && accBalance.MonthAverageBalance == 0
                        && accBalance.HalfYearAverageBalance == 0
                        && accBalance.HalfYearAverageBalance == 0
                    )
                )
                {
                    throw new O24OpenAPIException(
                        string.Format(
                            await localizationService.GetResource(
                                "Accounting.Validation.ACT_INVALID_INACTIVE_ACC"
                            ),
                            chart.AccountNumber
                        )
                    );
                }

                await accBalance.Delete();
            }
            await chart.Delete();
            return true;
        }
    }
}

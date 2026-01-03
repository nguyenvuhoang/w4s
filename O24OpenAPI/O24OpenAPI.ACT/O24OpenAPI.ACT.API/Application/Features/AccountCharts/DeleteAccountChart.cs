using LinKit.Core.Cqrs;
using O24OpenAPI.ACT.Domain;
using O24OpenAPI.ACT.Domain.AggregatesModel.AccountAggregate;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core;
using O24OpenAPI.Data.Extensions;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Localization;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.ACT.API.Application.Features.AccountCharts
{
    public class DeleteAccountChartCommand : BaseTransactionModel, ICommand
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

    [CqrsHandler]
    public class DeleteAccountHandle(
        IAccountBalanceRepository accountBalanceRepository,
        ILocalizationService localizationService
    ) : ICommandHandler<DeleteAccountChartCommand>
    {
        [WorkflowStep(WorkflowStepCode.ACT.WF_STEP_ACT_DELETE_ACCOUNTCHART)]
        public async Task<Unit> HandleAsync(
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
            return Unit.Value;
        }
    }
}

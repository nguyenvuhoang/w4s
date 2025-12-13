using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.Web.Report.Domain.MIS;

namespace O24OpenAPI.Web.Report.Migrations.Builder;

public partial class TrialBalanceBuilder : O24OpenAPIEntityBuilder<TrialBalance>
{
    /// <summary>
    /// Map entity TrialBalance table
    /// </summary>
    /// <param name="table"></param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(TrialBalance.SubCOA))
            .AsString(50)
            .WithColumn(nameof(TrialBalance.BranchCode))
            .AsString(50)
            .WithColumn(nameof(TrialBalance.StatementDate))
            .AsDateTime()
            .WithColumn(nameof(TrialBalance.OpenBalance))
            .AsDecimal(18, 2)
            .WithColumn(nameof(TrialBalance.MovingDebit))
            .AsDecimal(18, 2)
            .WithColumn(nameof(TrialBalance.MovingCredit))
            .AsDecimal(18, 2)
            .WithColumn(nameof(TrialBalance.ClosingBalance))
            .AsDecimal(18, 2)
            .WithColumn(nameof(TrialBalance.Currency))
            .AsString(10)
            .WithColumn(nameof(TrialBalance.ImportDate))
            .AsDateTime()
            .WithColumn(nameof(TrialBalance.BalanceSide))
            .AsString(10);
    }
}

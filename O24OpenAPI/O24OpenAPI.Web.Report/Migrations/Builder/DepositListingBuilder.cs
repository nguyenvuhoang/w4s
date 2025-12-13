
using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.Web.Report.Domain.MIS;

namespace O24OpenAPI.Web.Report.Migrations.Builder;

public partial class DepositListingBuilder : O24OpenAPIEntityBuilder<DepositListing>
{
    /// <summary>
    /// Map entity DepositListing table
    /// </summary>
    /// <param name="table"></param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(DepositListing.AccountNumber))
            .AsString(50)
            .WithColumn(nameof(DepositListing.OpenDate))
            .AsDateTime()
            .WithColumn(nameof(DepositListing.CloseDate))
            .AsDateTime()
            .Nullable()
            .WithColumn(nameof(DepositListing.CurrentBalance))
            .AsDecimal(18, 2)
            .WithColumn(nameof(DepositListing.StatementDate))
            .AsDateTime()
            .WithColumn(nameof(DepositListing.CustomerCode))
            .AsString(50)
            .WithColumn(nameof(DepositListing.DepositStatus))
            .AsString(50)
            .WithColumn(nameof(DepositListing.DepositType))
            .AsString(50)
            .WithColumn(nameof(DepositListing.DepositTypeName))
            .AsString(100);
    }
}

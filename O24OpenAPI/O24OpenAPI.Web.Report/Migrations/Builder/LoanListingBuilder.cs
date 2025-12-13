using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.Web.Report.Domain.MIS;

namespace O24OpenAPI.Web.Report.Migrations.Builder;

public partial class LoanListingBuilder : O24OpenAPIEntityBuilder<LoanListing>
{
    /// <summary>
    /// Map entity LoanListing table
    /// </summary>
    /// <param name="table"></param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(LoanListing.AccountNumber))
            .AsString(50)
            .WithColumn(nameof(LoanListing.OpenDate))
            .AsDateTime()
            .WithColumn(nameof(LoanListing.CloseDate))
            .AsDateTime()
            .Nullable()
            .WithColumn(nameof(LoanListing.CurrentBalance))
            .AsDecimal(18, 2)
            .WithColumn(nameof(LoanListing.StatementDate))
            .AsDateTime()
            .WithColumn(nameof(LoanListing.CustomerCode))
            .AsString(50)
            .WithColumn(nameof(LoanListing.LoanStatus))
            .AsString(50)
            .WithColumn(nameof(LoanListing.ClassificationStatus))
            .AsString(50)
            .WithColumn(nameof(LoanListing.LatePayment))
            .AsString(50)
            .Nullable();
    }
}

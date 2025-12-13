using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.Web.Report.Domain.MIS;

namespace O24OpenAPI.Web.Report.Migrations.Builder;

public partial class CustomerListingBuilder : O24OpenAPIEntityBuilder<CustomerListing>
{
    /// <summary>
    /// Map entity CustomerListing table
    /// </summary>
    /// <param name="table"></param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(CustomerListing.CustomerCode))
            .AsString(50)
            .WithColumn(nameof(CustomerListing.CreateDate))
            .AsDateTime()
            .WithColumn(nameof(CustomerListing.EconomicSector))
            .AsString(100)
            .WithColumn(nameof(CustomerListing.Districts))
            .AsString(100)
            .WithColumn(nameof(CustomerListing.Villages))
            .AsString(100)
            .WithColumn(nameof(CustomerListing.Province))
            .AsString(100)
            .WithColumn(nameof(CustomerListing.Gender))
            .AsString(10)
            .WithColumn(nameof(CustomerListing.CustomerType))
            .AsString(50);
    }
}

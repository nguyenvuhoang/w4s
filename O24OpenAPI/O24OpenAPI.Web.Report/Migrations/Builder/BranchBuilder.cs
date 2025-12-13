using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.Web.Report.Domain.MIS;

namespace O24OpenAPI.Web.Report.Migrations.Builder;

public partial class BranchBuilder : O24OpenAPIEntityBuilder<Branch>
{
    /// <summary>
    /// Map entity Branch table
    /// </summary>
    /// <param name="table"></param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(Branch.BranchCode))
            .AsString(50)
            .WithColumn(nameof(Branch.BranchName))
            .AsString(100)
            .WithColumn(nameof(Branch.BranchAddress))
            .AsString(255)
            .WithColumn(nameof(Branch.Phone))
            .AsString(20);
    }
}

using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.DataWarehouse.Migrations;

public class D_REGIONBuilder : O24OpenAPIEntityBuilder<D_REGION>
{
    /// <summary>
    /// MapEntity
    /// </summary>
    /// <param name="table">The table.</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(D_REGION.RegionCode))
            .AsString(255)
            .Nullable()
            .WithColumn(nameof(D_REGION.RegionName))
            .AsString(255)
            .Nullable()
            .WithColumn(nameof(D_REGION.RegionSpecial))
            .AsString(5)
            .Nullable()
            .WithColumn(nameof(D_REGION.Description))
            .AsString(-1)
            .Nullable()
            .WithColumn(nameof(D_REGION.UserCreate))
            .AsString(50)
            .Nullable()
            .WithColumn(nameof(D_REGION.DateCreate))
            .AsDateTime()
            .Nullable()
            .WithColumn(nameof(D_REGION.UserModify))
            .AsString(50)
            .Nullable()
            .WithColumn(nameof(D_REGION.DateModify))
            .AsDateTime()
            .Nullable()
            .WithColumn(nameof(D_REGION.Status))
            .AsString(10)
            .Nullable();
    }
}

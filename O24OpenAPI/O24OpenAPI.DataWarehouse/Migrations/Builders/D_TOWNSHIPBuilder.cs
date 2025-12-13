using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.DataWarehouse.Migrations.Builders;

public class D_TOWNSHIPBuilder : O24OpenAPIEntityBuilder<D_TOWNSHIP>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(D_TOWNSHIP.TownshipCode))
            .AsInt64()
            .NotNullable()
            .WithColumn(nameof(D_TOWNSHIP.TownshipName))
            .AsString(1000)
            .NotNullable()
            .WithColumn(nameof(D_TOWNSHIP.DistCode))
            .AsString(50)
            .NotNullable()
            .WithColumn(nameof(D_TOWNSHIP.TownshipNameMM))
            .AsString(255)
            .Nullable();
    }
}

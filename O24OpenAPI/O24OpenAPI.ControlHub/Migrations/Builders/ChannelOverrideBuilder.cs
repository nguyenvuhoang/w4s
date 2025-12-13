using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.Data;
using O24OpenAPI.Data.Attributes;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.ControlHub.Migrations.Builders;

[DatabaseType(DataProviderType.SqlServer)]
public class ChannelOverrideBuilder : O24OpenAPIEntityBuilder<ChannelOverride>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(ChannelOverride.ChannelIdRef)).AsInt32().NotNullable()
            .WithColumn(nameof(ChannelOverride.Date)).AsDate().NotNullable()
            .WithColumn(nameof(ChannelOverride.IsClosedAllDay)).AsBoolean().NotNullable();
    }
}

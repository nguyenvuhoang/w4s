using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.ControlHub.Migrations.Builders;

public class ChannelScheduleIntervalBuilder : O24OpenAPIEntityBuilder<ChannelScheduleInterval>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(ChannelScheduleInterval.ChannelScheduleIdRef)).AsInt32().NotNullable()
            .WithColumn(nameof(ChannelScheduleInterval.StartTime)).AsTime().NotNullable()
            .WithColumn(nameof(ChannelScheduleInterval.EndTime)).AsTime().NotNullable()
            .WithColumn(nameof(ChannelScheduleInterval.SortOrder)).AsInt32().Nullable();
    }
}

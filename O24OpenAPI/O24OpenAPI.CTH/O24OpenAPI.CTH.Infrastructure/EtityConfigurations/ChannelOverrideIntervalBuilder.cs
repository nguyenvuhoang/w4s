using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.CTH.Domain.AggregatesModel.ChannelAggregate;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.CTH.Infrastructure.EtityConfigurations;

public class ChannelOverrideIntervalBuilder : O24OpenAPIEntityBuilder<ChannelOverrideInterval>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(ChannelOverrideInterval.ChannelOverrideIdRef)).AsInt32().NotNullable()
            .WithColumn(nameof(ChannelOverrideInterval.StartTime)).AsTime().NotNullable()
            .WithColumn(nameof(ChannelOverrideInterval.EndTime)).AsTime().NotNullable()
            .WithColumn(nameof(ChannelOverrideInterval.SortOrder)).AsInt32().Nullable();
    }
}

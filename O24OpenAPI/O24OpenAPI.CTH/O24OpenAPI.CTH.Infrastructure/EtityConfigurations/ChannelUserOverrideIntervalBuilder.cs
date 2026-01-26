using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.CTH.Domain.AggregatesModel.ChannelAggregate;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.CTH.Infrastructure.EtityConfigurations;

public class ChannelUserOverrideIntervalBuilder : O24OpenAPIEntityBuilder<ChannelUserOverrideInterval>
{
    public override void MapEntity(CreateTableExpressionBuilder t)
    {
        t.WithColumn(nameof(ChannelUserOverrideInterval.ChannelUserOverrideIdRef)).AsInt32().NotNullable()
         .WithColumn(nameof(ChannelUserOverrideInterval.StartTime)).AsTime().NotNullable()
         .WithColumn(nameof(ChannelUserOverrideInterval.EndTime)).AsTime().NotNullable()
         .WithColumn(nameof(ChannelUserOverrideInterval.SortOrder)).AsInt32().Nullable();
    }
}

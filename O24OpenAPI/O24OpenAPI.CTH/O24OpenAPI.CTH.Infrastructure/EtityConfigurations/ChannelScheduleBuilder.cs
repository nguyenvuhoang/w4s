using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.CTH.Domain.AggregatesModel.ChannelAggregate;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.CTH.Infrastructure.EtityConfigurations;

public class ChannelScheduleBuilder : O24OpenAPIEntityBuilder<ChannelSchedule>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(ChannelSchedule.ChannelIdRef)).AsInt32().NotNullable()
            .WithColumn(nameof(ChannelSchedule.DayOfWeek)).AsInt32().NotNullable()
            .WithColumn(nameof(ChannelSchedule.EffectiveFrom)).AsDate().Nullable()
            .WithColumn(nameof(ChannelSchedule.EffectiveTo)).AsDate().Nullable()
            .WithColumn(nameof(ChannelSchedule.IsClosed)).AsBoolean().NotNullable();
    }
}

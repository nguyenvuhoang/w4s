using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.ControlHub.Migrations.Builders;

public class ChannelUserOverrideBuilder : O24OpenAPIEntityBuilder<ChannelUserOverride>
{
    public override void MapEntity(CreateTableExpressionBuilder t)
    {
        t.WithColumn(nameof(ChannelUserOverride.ChannelIdRef)).AsInt32().NotNullable()
         .WithColumn(nameof(ChannelUserOverride.UserId)).AsString(100).NotNullable()
         .WithColumn(nameof(ChannelUserOverride.EffectiveFrom)).AsDateTime().Nullable()
         .WithColumn(nameof(ChannelUserOverride.EffectiveTo)).AsDateTime().Nullable()
         .WithColumn(nameof(ChannelUserOverride.IsAllowedAllDay)).AsBoolean().NotNullable()
         .WithColumn(nameof(ChannelUserOverride.AllowWhenDisabled)).AsBoolean().NotNullable()
         .WithColumn(nameof(ChannelUserOverride.Note)).AsString(500).Nullable();
    }
}

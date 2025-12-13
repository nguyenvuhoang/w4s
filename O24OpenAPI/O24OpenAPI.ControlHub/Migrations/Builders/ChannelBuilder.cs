using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.ControlHub.Migrations.Builders;

/// <summary>
/// The channel builder class
/// </summary>
/// <seealso cref="O24OpenAPIEntityBuilder{Channel}"/>
public class ChannelBuilder : O24OpenAPIEntityBuilder<Channel>
{
    /// <summary>
    /// Maps the entity using the specified table
    /// </summary>
    /// <param name="table">The table</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(Channel.ChannelId))
            .AsString(50)
            .NotNullable()
            .WithColumn(nameof(Channel.ChannelName))
            .AsString(250)
            .NotNullable()
            .WithColumn(nameof(Channel.Description))
            .AsString(1000)
            .Nullable()
            .WithColumn(nameof(Channel.SortOrder))
            .AsInt16()
            .Nullable()
            .WithColumn(nameof(Channel.Status))
            .AsBoolean()
            .NotNullable()
            .WithColumn(nameof(Channel.IsAlwaysOpen)).AsBoolean().NotNullable()
            .WithColumn(nameof(Channel.TimeZoneId)).AsString(100).Nullable(); ;
    }
}

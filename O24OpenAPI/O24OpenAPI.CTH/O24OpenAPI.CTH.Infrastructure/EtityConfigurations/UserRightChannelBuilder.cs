using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.CTH.Infrastructure.EtityConfigurations;

/// <summary>
/// The user right builder class
/// </summary>
/// <seealso cref="O24OpenAPIEntityBuilder{UserRightChannel}"/>
public class UserRightChannelBuilder : O24OpenAPIEntityBuilder<UserRightChannel>
{
    /// <summary>
    /// Maps the entity using the specified table
    /// </summary>
    /// <param name="table">The table</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(UserRightChannel.RoleId))
            .AsString(100)
            .NotNullable()
            .WithColumn(nameof(UserRightChannel.ChannelId))
            .AsString(100)
            .NotNullable()
            .WithColumn(nameof(UserRightChannel.Invoke))
            .AsBoolean()
            .NotNullable()
            .WithColumn(nameof(UserRightChannel.CreatedOnUtc))
            .AsDateTime2()
            .Nullable()
            .WithColumn(nameof(UserRightChannel.UpdatedOnUtc))
            .AsDateTime2()
            .Nullable();
    }
}

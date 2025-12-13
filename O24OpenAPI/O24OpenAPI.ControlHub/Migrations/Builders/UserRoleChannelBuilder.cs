using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.ControlHub.Migrations.Builders;

/// <summary>
/// The user role builder class
/// </summary>
/// <seealso cref="O24OpenAPIEntityBuilder{UserRoleChannel}"/>
public class UserRoleChannelBuilder : O24OpenAPIEntityBuilder<UserRoleChannel>
{
    /// <summary>
    /// Maps the entity using the specified table
    /// </summary>
    /// <param name="table">The table</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(UserRoleChannel.RoleId))
            .AsString(100)
            .NotNullable()
            .WithColumn(nameof(UserRoleChannel.RoleName))
            .AsString(100)
            .NotNullable()
            .WithColumn(nameof(UserRoleChannel.RoleDescription))
            .AsString()
            .Nullable()
            .WithColumn(nameof(UserRoleChannel.UserCreated))
            .AsString()
            .Nullable()
            .WithColumn(nameof(UserRoleChannel.UserModified))
            .AsString()
            .Nullable()
            .WithColumn(nameof(UserRoleChannel.Status))
            .AsBoolean()
            .NotNullable()
            .WithColumn(nameof(UserRoleChannel.UpdatedOnUtc))
            .AsDateTime2()
            .Nullable()
            .WithColumn(nameof(UserRoleChannel.CreatedOnUtc))
            .AsDateTime2()
            .Nullable();
    }
}

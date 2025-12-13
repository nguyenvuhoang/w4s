using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.ControlHub.Migrations.Builders;

/// <summary>
/// The user password builder class
/// </summary>
/// <seealso cref="O24OpenAPIEntityBuilder{UserPassword}"/>
public class UserPasswordBuilder : O24OpenAPIEntityBuilder<UserPassword>
{
    /// <summary>
    /// Maps the entity using the specified table
    /// </summary>
    /// <param name="table">The table</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(UserPassword.UserId))
            .AsString(100)
            .NotNullable()
            .WithColumn(nameof(UserPassword.Password))
            .AsString(100)
            .NotNullable()
            .WithColumn(nameof(UserPassword.Salt))
            .AsString(100)
            .NotNullable()
            .WithColumn(nameof(UserPassword.CreatedOnUtc))
            .AsDateTime2()
            .Nullable()
            .WithColumn(nameof(UserPassword.UpdatedOnUtc))
            .AsDateTime2()
            .Nullable()
            .WithColumn(nameof(UserPassword.ChannelId))
            .AsString(10)
            .Nullable();
    }
}

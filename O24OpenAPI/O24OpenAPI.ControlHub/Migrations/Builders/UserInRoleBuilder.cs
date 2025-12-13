using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.ControlHub.Migrations.Builders;

public class UserInRoleBuilder : O24OpenAPIEntityBuilder<UserInRole>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(UserInRole.RoleId))
            .AsInt64()
            .NotNullable()
            .WithColumn(nameof(UserInRole.UserCode))
            .AsString(50)
            .Nullable()
            .WithColumn(nameof(UserInRole.IsMain))
            .AsString(1) // char(1)
            .Nullable()
            .WithColumn(nameof(UserInRole.UpdatedOnUtc))
            .AsDateTime2()
            .Nullable()
            .WithColumn(nameof(UserInRole.CreatedOnUtc))
            .AsDateTime2()
            .Nullable();
    }
}

using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.Web.CMS.Migrations.Builder;

public class UserRoleBuilder : O24OpenAPIEntityBuilder<UserRole>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(UserRole.RoleId))
            .AsInt32()
            .NotNullable()
            .WithColumn(nameof(UserRole.RoleName))
            .AsString(1000)
            .NotNullable()
            .WithColumn(nameof(UserRole.RoleDescription))
            .AsString(4000)
            .Nullable()
            .WithColumn(nameof(UserRole.UserType))
            .AsString(50)
            .Nullable()
            .WithColumn(nameof(UserRole.ContractNo))
            .AsString(50)
            .Nullable()
            .WithColumn(nameof(UserRole.UserCreated))
            .AsString(50)
            .Nullable()
            .WithColumn(nameof(UserRole.DateCreated))
            .AsDateTime()
            .Nullable()
            .WithColumn(nameof(UserRole.UserModified))
            .AsString(50)
            .Nullable()
            .WithColumn(nameof(UserRole.DateModified))
            .AsDateTime()
            .Nullable()
            .WithColumn(nameof(UserRole.ServiceID))
            .AsString(50)
            .Nullable()
            .WithColumn(nameof(UserRole.RoleType))
            .AsString(50)
            .Nullable()
            .WithColumn(nameof(UserRole.Status))
            .AsString(50)
            .Nullable()
            .WithColumn(nameof(UserRole.IsShow))
            .AsString(5)
            .Nullable()
            .WithColumn(nameof(UserRole.Order))
            .AsDecimal(3, 0)
            .NotNullable();
    }
}

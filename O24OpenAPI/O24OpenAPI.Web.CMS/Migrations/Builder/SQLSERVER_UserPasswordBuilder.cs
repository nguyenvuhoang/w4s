using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Attributes;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.Web.CMS.Migrations.Builder;

[DatabaseType(DataProviderType.SqlServer)]
public class SQLSERVER_UserPasswordBuilder : O24OpenAPIEntityBuilder<UserPassword>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(UserPassword.UserCode))
            .AsString(50)
            .NotNullable()
            .WithColumn(nameof(UserPassword.Password))
            .AsString(500)
            .NotNullable()
            .WithColumn(nameof(UserPassword.LastLogin))
            .AsDateTime2()
            .Nullable()
            .WithColumn(nameof(UserPassword.FailureCount))
            .AsInt32()
            .NotNullable()
            .WithColumn(nameof(UserPassword.PasswordSalt))
            .AsString(int.MaxValue) // nvarchar(max)
            .Nullable()
            .WithColumn(nameof(UserPassword.UpdatedOnUtc))
            .AsDateTime2()
            .Nullable()
            .WithColumn(nameof(UserPassword.CreatedOnUtc))
            .AsDateTime2()
            .Nullable();
    }
}

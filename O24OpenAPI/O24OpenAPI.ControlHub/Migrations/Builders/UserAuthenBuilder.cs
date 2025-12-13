using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.ControlHub.Migrations.Builders;

public class UserAuthenBuilder : O24OpenAPIEntityBuilder<UserAuthen>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(UserAuthen.ChannelId))
            .AsString(10)
            .NotNullable()
            .WithColumn(nameof(UserAuthen.AuthenType))
            .AsString(10)
            .NotNullable()
            .WithColumn(nameof(UserAuthen.UserCode))
            .AsString(100)
            .NotNullable()
            .WithColumn(nameof(UserAuthen.Key))
            .AsString(500)
            .Nullable()
            .WithColumn(nameof(UserAuthen.SmartOTP))
            .AsString(500)
            .Nullable()
            .WithColumn(nameof(UserAuthen.PinCode))
            .AsString(500)
            .Nullable()
            .WithColumn(nameof(UserAuthen.SMSOTP))
            .AsString(500)
            .Nullable()
            .WithColumn(nameof(UserAuthen.Phone))
            .AsString(50)
            .Nullable()
            .WithColumn(nameof(UserAuthen.CreatedOnUtc))
            .AsDateTime2()
            .Nullable()
            .WithColumn(nameof(UserAuthen.UpdatedOnUtc))
            .AsDateTime2()
            .Nullable()
            .WithColumn(nameof(UserAuthen.IsActive))
            .AsBoolean()
            .Nullable();
    }
}

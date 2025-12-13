using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.ControlHub.Migrations.Builder;

public class UserCommandBuilder : O24OpenAPIEntityBuilder<UserCommand>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(UserCommand.ApplicationCode))
            .AsString(50)
            .NotNullable()
            .WithColumn(nameof(UserCommand.CommandId))
            .AsString(50)
            .NotNullable()
            .WithColumn(nameof(UserCommand.ParentId))
            .AsString(50)
            .Nullable()
            .WithColumn(nameof(UserCommand.CommandName))
            .AsString(500)
            .Nullable()
            .WithColumn(nameof(UserCommand.CommandNameLanguage))
            .AsString(1000)
            .Nullable()
            .WithColumn(nameof(UserCommand.CommandType))
            .AsString(1)
            .NotNullable()
            .WithColumn(nameof(UserCommand.CommandURI))
            .AsString(200)
            .Nullable()
            .WithColumn(nameof(UserCommand.Enabled))
            .AsBoolean()
            .NotNullable()
            .WithColumn(nameof(UserCommand.IsVisible))
            .AsBoolean()
            .Nullable()
            .WithColumn(nameof(UserCommand.DisplayOrder))
            .AsInt32()
            .NotNullable()
            .WithColumn(nameof(UserCommand.GroupMenuIcon))
            .AsString(700)
            .Nullable()
            .WithColumn(nameof(UserCommand.GroupMenuVisible))
            .AsString(700)
            .Nullable()
            .WithColumn(nameof(UserCommand.GroupMenuListAuthorizeForm))
            .AsString(700)
            .Nullable()
            .WithColumn(nameof(UserCommand.GroupMenuId))
            .AsString(700)
            .Nullable()
            .WithColumn(nameof(UserCommand.CreatedOnUtc))
            .AsDateTime()
            .Nullable()
            .WithColumn(nameof(UserCommand.UpdatedOnUtc))
            .AsDateTime()
            .Nullable();
    }
}

using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.ControlHub.Migrations.Builders;

public class UserLevelBuilder : O24OpenAPIEntityBuilder<UserLevel>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(UserLevel.LevelCode))
            .AsString(10)
            .NotNullable()
            .WithColumn(nameof(UserLevel.LevelName))
            .AsString(100)
            .NotNullable()
            .WithColumn(nameof(UserLevel.Description))
            .AsString(500)
            .Nullable();
    }
}

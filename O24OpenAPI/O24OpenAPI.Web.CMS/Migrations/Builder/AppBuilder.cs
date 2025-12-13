using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.Web.CMS.Migrations.Builder;

public class AppBuilder : O24OpenAPIEntityBuilder<App>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(App.AppCode))
            .AsString(50)
            .NotNullable()
            .WithColumn(nameof(App.AppName))
            .AsString(250)
            .NotNullable()
            .WithColumn(nameof(App.Description))
            .AsString(1000)
            .NotNullable()
            .WithColumn(nameof(App.Order))
            .AsInt16()
            .Nullable()
            .WithColumn(nameof(App.Status))
            .AsBoolean()
            .NotNullable()
            .WithColumn(nameof(App.CreatedOnUtc))
            .AsDateTime2()
            .Nullable()
            .WithColumn(nameof(App.UpdatedOnUtc))
            .AsDateTime2()
            .Nullable();
    }
}

using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.Web.CMS.Migrations.Builder;

public class AppTypeConfigBuilder : O24OpenAPIEntityBuilder<AppTypeConfig>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(AppTypeConfig.AppCode))
            .AsString(50)
            .NotNullable()
            .WithColumn(nameof(AppTypeConfig.AppName))
            .AsString(100)
            .NotNullable()
            .WithColumn(nameof(AppTypeConfig.AppTypeDescription))
            .AsString(200)
            .Nullable()
            .WithColumn(nameof(AppTypeConfig.AppTypeIcon))
            .AsString(200)
            .Nullable()
            .WithColumn(nameof(AppTypeConfig.OrderIndex))
            .AsInt32()
            .NotNullable()
            .WithColumn(nameof(AppTypeConfig.RedirectPage))
            .AsString(200)
            .Nullable()
            .WithColumn(nameof(AppTypeConfig.CreatedOnUtc))
            .AsDateTime()
            .NotNullable()
            .WithColumn(nameof(AppTypeConfig.UpdatedOnUtc))
            .AsDateTime()
            .Nullable();
    }
}

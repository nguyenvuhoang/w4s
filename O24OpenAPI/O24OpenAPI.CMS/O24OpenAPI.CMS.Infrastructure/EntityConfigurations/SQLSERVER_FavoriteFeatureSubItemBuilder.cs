using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.CMS.Domain;
using O24OpenAPI.Data;
using O24OpenAPI.Data.Attributes;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.CMS.Infrastructure.EntityConfigurations;

[DatabaseType(DataProviderType.SqlServer)]
public class SQLSERVER_FavoriteFeatureSubItemBuilder
    : O24OpenAPIEntityBuilder<FavoriteFeatureSubItem>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(FavoriteFeatureSubItem.Id))
            .AsInt32()
            .PrimaryKey()
            .Identity()
            .WithColumn(nameof(FavoriteFeatureSubItem.SubItemCode))
            .AsString(50)
            .NotNullable()
            .WithColumn(nameof(FavoriteFeatureSubItem.Icon))
            .AsString(2000)
            .NotNullable()
            .WithColumn(nameof(FavoriteFeatureSubItem.Label))
            .AsString(2000)
            .NotNullable()
            .WithColumn(nameof(FavoriteFeatureSubItem.Url))
            .AsString(500)
            .Nullable()
            .WithColumn(nameof(FavoriteFeatureSubItem.Description))
            .AsString(int.MaxValue)
            .Nullable()
            .WithColumn(nameof(FavoriteFeatureSubItem.CreatedOnUtc))
            .AsDateTime2()
            .Nullable()
            .WithColumn(nameof(FavoriteFeatureSubItem.UpdatedOnUtc))
            .AsDateTime2()
            .Nullable();
    }
}

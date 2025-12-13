using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.Web.CMS.Migrations.Builder;

public class FavoriteFeatureBuilder : O24OpenAPIEntityBuilder<FavoriteFeature>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(FavoriteFeature.Id))
            .AsInt32()
            .PrimaryKey()
            .Identity()
            .WithColumn(nameof(FavoriteFeature.FavoriteFeatureCode))
            .AsString(50)
            .NotNullable()
            .WithColumn(nameof(FavoriteFeature.FavoriteFeatureName))
            .AsString(500)
            .NotNullable()
            .WithColumn(nameof(FavoriteFeature.SubItemCode))
            .AsString(2000)
            .Nullable()
            .WithColumn(nameof(FavoriteFeature.CommandId))
            .AsString(50)
            .NotNullable()
            .WithColumn(nameof(FavoriteFeature.Description))
            .AsString(2000)
            .Nullable()
            .WithColumn(nameof(FavoriteFeature.CreatedOnUtc))
            .AsDateTime2()
            .Nullable()
            .WithColumn(nameof(FavoriteFeature.UpdatedOnUtc))
            .AsDateTime2()
            .Nullable();
    }
}

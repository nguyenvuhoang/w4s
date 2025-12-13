using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.Web.CMS.Migrations.Builder;

public class UserFavoriteFeatureBuilder : O24OpenAPIEntityBuilder<UserFavoriteFeature>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(UserFavoriteFeature.Id))
            .AsInt32()
            .PrimaryKey()
            .Identity()
            .WithColumn(nameof(UserFavoriteFeature.UserCode))
            .AsString(50)
            .NotNullable()
            .WithColumn(nameof(UserFavoriteFeature.FavoriteFeatureID))
            .AsInt32()
            .NotNullable()
            .WithDefaultValue(0)
            .WithColumn(nameof(UserFavoriteFeature.Favorite))
            .AsBoolean()
            .NotNullable()
            .WithDefaultValue(false)
            .WithColumn(nameof(UserFavoriteFeature.Description))
            .AsString(2000)
            .Nullable()
            .WithColumn(nameof(UserFavoriteFeature.CreatedOnUtc))
            .AsDateTime2()
            .Nullable()
            .WithColumn(nameof(UserFavoriteFeature.UpdatedOnUtc))
            .AsDateTime2()
            .Nullable();
    }
}

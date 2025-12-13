using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.ControlHub.Migrations.Builders;

/// <summary>
/// The user limit builder class
/// </summary>
public class UserBannerBuilder : O24OpenAPIEntityBuilder<UserBanner>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(UserBanner.UserCode)).AsString(50).NotNullable()
            .WithColumn(nameof(UserBanner.BannerSource)).AsString(1000).NotNullable()
            .WithColumn(nameof(UserBanner.CreatedOnUTC)).AsDateTime().Nullable()
            .WithColumn(nameof(UserBanner.UpdatedOnUTC)).AsDateTime().Nullable();
    }
}

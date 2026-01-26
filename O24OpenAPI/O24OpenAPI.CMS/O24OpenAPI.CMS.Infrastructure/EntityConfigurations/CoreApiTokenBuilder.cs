using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.CMS.Domain.AggregateModels;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.CMS.Infrastructure.EntityConfigurations;

public class CoreApiTokenBuilder : O24OpenAPIEntityBuilder<CoreApiToken>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(CoreApiToken.ClientId)).AsString(100).NotNullable()
            .WithColumn(nameof(CoreApiToken.Token)).AsString(1000).NotNullable()
            .WithColumn(nameof(CoreApiToken.RefreshToken)).AsString(200).Nullable()
            .WithColumn(nameof(CoreApiToken.RefreshTokenExpiredAt)).AsDateTime2().Nullable()
            .WithColumn(nameof(CoreApiToken.Scopes)).AsString(500).Nullable()
            .WithColumn(nameof(CoreApiToken.CreatedAt)).AsDateTime2().NotNullable()
            .WithColumn(nameof(CoreApiToken.ExpiredAt)).AsDateTime2().NotNullable()
            .WithColumn(nameof(CoreApiToken.IsRevoked)).AsBoolean().NotNullable().WithDefaultValue(false)
            .WithColumn(nameof(CoreApiToken.LastUsedAt)).AsDateTime2().Nullable()
            .WithColumn(nameof(CoreApiToken.UsageCount)).AsInt32().NotNullable().WithDefaultValue(0)
            .WithColumn(nameof(CoreApiToken.BICCD)).AsString(500).Nullable();
    }
}

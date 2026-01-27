using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.NCH.Domain.AggregatesModel.ZaloAggregate;

namespace O24OpenAPI.NCH.Infrastructure.EntityCongurations;

/// <summary>
/// The Zalo OA token builder class
/// </summary>
/// <seealso cref="O24OpenAPIEntityBuilder{ZaloOAToken}"/>
public class ZaloOATokenBuilder : O24OpenAPIEntityBuilder<ZaloOAToken>
{
    /// <summary>
    /// Maps the entity using the specified table
    /// </summary>
    /// <param name="table">The table</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table

            .WithColumn(nameof(ZaloOAToken.OaId))
                .AsString(50)
                .NotNullable()

            .WithColumn(nameof(ZaloOAToken.AppId))
                .AsString(50)
                .NotNullable()

            .WithColumn(nameof(ZaloOAToken.AccessToken))
                .AsString(int.MaxValue)
                .NotNullable()

            .WithColumn(nameof(ZaloOAToken.RefreshToken))
                .AsString(int.MaxValue)
                .Nullable()

            .WithColumn(nameof(ZaloOAToken.ExpiresIn))
                .AsInt32()
                .NotNullable()

            .WithColumn(nameof(ZaloOAToken.ExpiresAtUtc))
                .AsDateTime2()
                .NotNullable()

            .WithColumn(nameof(ZaloOAToken.IsActive))
                .AsBoolean()
                .NotNullable()
                .WithDefaultValue(true)

            .WithColumn(nameof(ZaloOAToken.LastUsedAtUtc))
                .AsDateTime2()
                .Nullable();
    }
}

using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Attributes;
using O24OpenAPI.Data.Extensions;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.Web.CMS.Migrations.Builder;

[DatabaseType(DataProviderType.Oracle)]

public class ORACLE_QRDataBuilder : O24OpenAPIEntityBuilder<QRData>
{
    /// <summary>
    /// MapEntity
    /// </summary>
    /// <param name="table">The table.</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(QRData.ChannelId))
            .AsString(50)
            .NotNullable()
            .WithColumn(nameof(QRData.HashedId))
            .AsString(255)
            .NotNullable()
            .WithColumn(nameof(QRData.Data))
            .AsNCLOB()
            .NotNullable()
            .WithColumn(nameof(QRData.ExpirationUtc))
            .AsDateTime2()
            .Nullable()
            .WithColumn(nameof(QRData.CreatedOnUtc))
            .AsDateTime2()
            .Nullable()
            .WithColumn(nameof(QRData.UpdatedOnUtc))
            .AsDateTime2()
            .Nullable();
    }
}

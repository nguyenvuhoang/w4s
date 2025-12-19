using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.CMS.Domain;
using O24OpenAPI.Data;
using O24OpenAPI.Data.Attributes;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.CMS.Infrastructure.EntityConfigurations;

[DatabaseType(DataProviderType.SqlServer)]
public class SQLSERVER_QRDataBuilder : O24OpenAPIEntityBuilder<QRData>
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
            .AsString(int.MaxValue)
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

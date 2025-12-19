using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.CMS.Domain;
using O24OpenAPI.Data;
using O24OpenAPI.Data.Attributes;
using O24OpenAPI.Data.Mapping.Builders;

namespace O24OpenAPI.CMS.Infrastructure.EntityConfigurations;

[DatabaseType(DataProviderType.SqlServer)]
public partial class SQLSERVER_HttpLogMessageBuilder : O24OpenAPIEntityBuilder<HttpLogMessage>
{
    /// <summary>
    /// MapEntity
    /// </summary>
    /// <param name="table">The table.</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(HttpLogMessage.BeginTime))
            .AsString(100)
            .Nullable()
            .WithColumn(nameof(HttpLogMessage.EndTime))
            .AsString(100)
            .Nullable()
            .WithColumn(nameof(HttpLogMessage.RequestBody))
            .AsString(int.MaxValue)
            .Nullable()
            .WithColumn(nameof(HttpLogMessage.RequestHeader))
            .AsString(int.MaxValue)
            .Nullable()
            .WithColumn(nameof(HttpLogMessage.ResponseBody))
            .AsString(int.MaxValue)
            .Nullable();
    }
}

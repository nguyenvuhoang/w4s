using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data;
using O24OpenAPI.Data.Attributes;
using O24OpenAPI.Data.Extensions;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.Logger.Domain;

namespace O24OpenAPI.Logger.Migrations.Builders;

[DatabaseType(DataProviderType.SqlServer)]
public class ApplicationLogBuilder : O24OpenAPIEntityBuilder<ApplicationLog>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table.WithColumn(nameof(ApplicationLog.LogTimestamp)).AsDateTimeOffset().Nullable();
        table.WithColumn(nameof(ApplicationLog.LogLevel)).AsString(50).Nullable();
        table.WithColumn(nameof(ApplicationLog.ServiceName)).AsString(100).Nullable();
        table.WithColumn(nameof(ApplicationLog.CorrelationId)).AsString(100).NotNullable();
        table.WithColumn(nameof(ApplicationLog.LogType)).AsString(50).Nullable();
        table.WithColumn(nameof(ApplicationLog.Direction)).AsString(50).Nullable();
        table.WithColumn(nameof(ApplicationLog.ActionName)).AsString(100).Nullable();
        table.WithColumn(nameof(ApplicationLog.Flow)).AsString(255).Nullable();
        table.WithColumn(nameof(ApplicationLog.DurationMs)).AsInt64().Nullable();
        table.WithColumn(nameof(ApplicationLog.Message)).AsString(int.MaxValue).Nullable();
        table.WithColumn(nameof(ApplicationLog.Headers)).AsString(int.MaxValue).Nullable();
        table.WithColumn(nameof(ApplicationLog.RequestPayload)).AsString(int.MaxValue).Nullable();
        table.WithColumn(nameof(ApplicationLog.ResponsePayload)).AsString(int.MaxValue).Nullable();
        table.WithColumn(nameof(ApplicationLog.ExceptionDetails)).AsString(int.MaxValue).Nullable();
        table.WithColumn(nameof(ApplicationLog.Properties)).AsString(int.MaxValue).Nullable();
        table.WithColumn(nameof(ApplicationLog.CreatedOnUtc)).AsDateTime2().Nullable();
    }
}

[DatabaseType(DataProviderType.Oracle)]
public class ApplicationLogBuilder1 : O24OpenAPIEntityBuilder<ApplicationLog>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table.WithColumn(nameof(ApplicationLog.LogTimestamp)).AsDateTime2().Nullable();
        table.WithColumn(nameof(ApplicationLog.LogLevel)).AsString(50).Nullable();
        table.WithColumn(nameof(ApplicationLog.ServiceName)).AsString(100).Nullable();
        table.WithColumn(nameof(ApplicationLog.CorrelationId)).AsString(100).Nullable();
        table.WithColumn(nameof(ApplicationLog.LogType)).AsString(50).Nullable();
        table.WithColumn(nameof(ApplicationLog.Direction)).AsString(50).Nullable();
        table.WithColumn(nameof(ApplicationLog.ActionName)).AsString(100).Nullable();
        table.WithColumn(nameof(ApplicationLog.DurationMs)).AsInt64().Nullable();
        table.WithColumn(nameof(ApplicationLog.Message)).AsNCLOB().Nullable();
        table.WithColumn(nameof(ApplicationLog.Headers)).AsNCLOB().Nullable();
        table.WithColumn(nameof(ApplicationLog.RequestPayload)).AsNCLOB().Nullable();
        table.WithColumn(nameof(ApplicationLog.ResponsePayload)).AsNCLOB().Nullable();
        table.WithColumn(nameof(ApplicationLog.ExceptionDetails)).AsNCLOB().Nullable();
        table.WithColumn(nameof(ApplicationLog.Properties)).AsNCLOB().Nullable();
        table.WithColumn(nameof(ApplicationLog.CreatedOnUtc)).AsDateTime2().Nullable();
    }
}

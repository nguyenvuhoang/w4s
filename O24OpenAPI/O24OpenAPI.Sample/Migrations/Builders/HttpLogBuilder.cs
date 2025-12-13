using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.Web.Framework.Domain.Logging;

namespace O24OpenAPI.Sample.Migrations.Builders;

public class HttpLogBuilder : O24OpenAPIEntityBuilder<HttpLog>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(HttpLog.RequestId))
            .AsString(255)
            .NotNullable()
            .WithColumn(nameof(HttpLog.HttpMethod))
            .AsString(10)
            .NotNullable()
            .WithColumn(nameof(HttpLog.RequestUrl))
            .AsString(2000)
            .NotNullable()
            .WithColumn(nameof(HttpLog.RequestHeaders))
            .AsString(int.MaxValue)
            .Nullable()
            .WithColumn(nameof(HttpLog.RequestBody))
            .AsString(int.MaxValue)
            .Nullable()
            .WithColumn(nameof(HttpLog.ClientIp))
            .AsString(45)
            .Nullable()
            .WithColumn(nameof(HttpLog.UserAgent))
            .AsString(1000)
            .Nullable()
            .WithColumn(nameof(HttpLog.ResponseStatusCode))
            .AsInt32()
            .NotNullable()
            .WithColumn(nameof(HttpLog.ResponseHeaders))
            .AsString(int.MaxValue)
            .Nullable()
            .WithColumn(nameof(HttpLog.ResponseBody))
            .AsString(int.MaxValue)
            .Nullable()
            .WithColumn(nameof(HttpLog.ServiceId))
            .AsString(255)
            .NotNullable()
            .WithColumn(nameof(HttpLog.Reference))
            .AsString(255)
            .Nullable()
            .WithColumn(nameof(HttpLog.ExceptionMessage))
            .AsString(int.MaxValue)
            .Nullable()
            .WithColumn(nameof(HttpLog.StackTrace))
            .AsString(int.MaxValue)
            .Nullable()
            .WithColumn(nameof(HttpLog.BeginOnUtc))
            .AsDateTime2()
            .NotNullable()
            .WithColumn(nameof(HttpLog.FinishOnUtc))
            .AsDateTime2()
            .Nullable();
    }
}

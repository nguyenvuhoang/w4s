using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.Logger.Domain.AggregateModels.ServiceLogAggregate;

namespace O24OpenAPI.Logger.API.Migrations.Builders;

/// <summary>
/// The service log builder class
/// </summary>
/// <seealso cref="O24OpenAPIEntityBuilder{ServiceLog}"/>
public class ServiceLogBuilder : O24OpenAPIEntityBuilder<ServiceLog>
{
    /// <summary>
    /// Maps the entity using the specified table
    /// </summary>
    /// <param name="table">The table</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(ServiceLog.ExecutionLogId))
            .AsString(36)
            .Nullable()
            .WithColumn(nameof(ServiceLog.LogLevelId))
            .AsInt32()
            .NotNullable()
            .WithColumn(nameof(ServiceLog.ServiceId))
            .AsString(100)
            .NotNullable()
            .WithColumn(nameof(ServiceLog.ChannelId))
            .AsString(50)
            .Nullable()
            .WithColumn(nameof(ServiceLog.Status))
            .AsString(50)
            .Nullable()
            .WithColumn(nameof(ServiceLog.Code))
            .AsString(100)
            .Nullable()
            .WithColumn(nameof(ServiceLog.ShortMessage))
            .AsString(2000)
            .Nullable()
            .WithColumn(nameof(ServiceLog.FullMessage))
            .AsString(int.MaxValue)
            .Nullable()
            .WithColumn(nameof(ServiceLog.Data))
            .AsString(int.MaxValue)
            .Nullable()
            .WithColumn(nameof(ServiceLog.UserId))
            .AsString(100)
            .Nullable()
            .WithColumn(nameof(ServiceLog.Reference))
            .AsString(100)
            .Nullable()
            .WithColumn(nameof(ServiceLog.IpAddress))
            .AsString(50)
            .Nullable()
            .WithColumn(nameof(ServiceLog.UserAgent))
            .AsString(500)
            .Nullable()
            .WithColumn(nameof(ServiceLog.CreatedOnUtc))
            .AsDateTime2()
            .Nullable();
    }
}

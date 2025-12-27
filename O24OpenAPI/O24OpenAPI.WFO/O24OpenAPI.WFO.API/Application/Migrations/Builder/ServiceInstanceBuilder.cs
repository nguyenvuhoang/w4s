using FluentMigrator.Builders.Create.Table;
using O24OpenAPI.Data.Mapping.Builders;
using O24OpenAPI.WFO.Domain.AggregateModels.ServiceAggregate;

namespace O24OpenAPI.WFO.API.Application.Migrations.Builder;

public class ServiceInstanceBuilder : O24OpenAPIEntityBuilder<ServiceInstance>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table
            .WithColumn(nameof(ServiceInstance.InstanceID))
            .AsString(255)
            .NotNullable()
            .WithColumn(nameof(ServiceInstance.GrpcUrl))
            .AsString(1000)
            .NotNullable()
            .WithColumn(nameof(ServiceInstance.GrpcTimeout))
            .AsInt64()
            .Nullable()
            .WithDefaultValue(0)
            .WithColumn(nameof(ServiceInstance.ServiceCode))
            .AsString(50)
            .NotNullable()
            .WithColumn(nameof(ServiceInstance.ServiceHandleName))
            .AsString(100)
            .Nullable()
            .WithColumn(nameof(ServiceInstance.EventQueueName))
            .AsString(255)
            .NotNullable()
            .WithColumn(nameof(ServiceInstance.CommandQueueName))
            .AsString(255)
            .NotNullable()
            .WithColumn(nameof(ServiceInstance.ConcurrentLimit))
            .AsInt64()
            .NotNullable()
            .WithColumn(nameof(ServiceInstance.Status))
            .AsString(10)
            .Nullable()
            .WithColumn(nameof(ServiceInstance.AssemblyName))
            .AsString(1000)
            .Nullable();
    }
}

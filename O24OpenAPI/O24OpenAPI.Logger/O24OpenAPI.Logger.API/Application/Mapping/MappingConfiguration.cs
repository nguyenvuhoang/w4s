using LinKit.Core.Mapping;
using O24OpenAPI.Contracts.Models.Log;
using O24OpenAPI.Framework.Domain.Logging;
using O24OpenAPI.Logger.API.Application.Features;
using O24OpenAPI.Logger.Domain.AggregateModels.ServiceLogAggregate;

namespace O24OpenAPI.Logger.API.Application.Mapping;

[MapperContext]
public class MappingConfiguration : IMappingConfigurator
{
    public void Configure(IMapperConfigurationBuilder builder)
    {
        builder.CreateMap<LogEntryModel, ApplicationLog>();
        builder.CreateMap<CreateHttLogCommand, HttpLog>();
        builder.CreateMap<
            CreateWorkflowLogCommand,
            Domain.AggregateModels.WorkflowLogAggregate.WorkflowLog
        >();
        builder.CreateMap<
            CreateWorkflowStepLogCommand,
            Domain.AggregateModels.WorkflowLogAggregate.WorkflowStepLog
        >();
        builder.CreateMap<CreateServiceLogCommand, ServiceLog>();
    }
}

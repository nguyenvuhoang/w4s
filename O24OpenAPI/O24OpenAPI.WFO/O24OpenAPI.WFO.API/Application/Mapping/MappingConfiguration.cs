using LinKit.Core.Mapping;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.WFO.API.Application.Models;
using O24OpenAPI.WFO.Domain.AggregateModels.WorkflowAggregate;

namespace O24OpenAPI.WFO.API.Application.Mapping;

[MapperContext]
public class MappingConfiguration : IMappingConfigurator
{
    public void Configure(IMapperConfigurationBuilder builder)
    {
        builder
            .CreateMap<WorkflowInfoModel, WorkflowInfo>()
            .ForMember(d => d.input, o => o.MapFrom(s => s.input.WriteIndentedJson()))
            .ForMember(
                d => d.response_content,
                o => o.MapFrom(s => s.response_content.WriteIndentedJson())
            );

        builder
            .CreateMap<WorkflowStepInfoModel, WorkflowStepInfo>()
            .ForMember(
                d => d.sending_condition,
                o => o.MapFrom(s => s.sending_condition.WriteIndentedJson())
            )
            .ForMember(d => d.p1_content, o => o.MapFrom(s => s.p1_content.WriteIndentedJson()))
            .ForMember(d => d.p1_request, o => o.MapFrom(s => s.p1_request.WriteIndentedJson()))
            .ForMember(d => d.p2_content, o => o.MapFrom(s => s.p2_content.WriteIndentedJson()))
            .ForMember(d => d.p2_request, o => o.MapFrom(s => s.p2_request.WriteIndentedJson()));

        builder.CreateMap<WorkflowStep, WorkflowStep>().ForMember(d => d.Id, o => o.Ignore());
        builder.CreateMap<WorkflowDef, WorkflowDef>().ForMember(d => d.Id, o => o.Ignore());
    }
}

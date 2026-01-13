using LinKit.Core.Mapping;
using LinKit.Json.Runtime;
using O24OpenAPI.Contracts.Models;
using O24OpenAPI.WFO.API.Application.Features.WorkflowLogs;
using O24OpenAPI.WFO.API.Application.Models;
using O24OpenAPI.WFO.API.Application.Models.WorkflowStepModels;
using O24OpenAPI.WFO.Domain.AggregateModels.WorkflowAggregate;

namespace O24OpenAPI.WFO.API.Application.Mapping;

[MapperContext]
public class MappingConfiguration : IMappingConfigurator
{
    public void Configure(IMapperConfigurationBuilder builder)
    {
        builder
            .CreateMap<WorkflowInfoModel, WorkflowInfo>()
            .ForMember(d => d.input, o => o.MapFrom(s => s.input.ToJson(null, null)))
            .ForMember(
                d => d.response_content,
                o => o.MapFrom(s => s.response_content.ToJson(null, null))
            )
            .ForMember(d => d.error_info, o => o.MapFrom(s => s.error_info.ToJson(null, null)));

        builder
            .CreateMap<WorkflowInfo, WorkflowInfoModel>()
            .ForMember(d => d.input, o => o.MapFrom(s => s.input.FromJson<object>()))
            .ForMember(
                d => d.response_content,
                o => o.MapFrom(s => s.response_content.FromJson<object>())
            )
            .ForMember(d => d.error_info, o => o.MapFrom(s => s.error_info.FromJson<ErrorInfo>()));

        builder.CreateMap<WorkflowInfo, AdvancedSearchWorkflowLogResponse>();

        builder
            .CreateMap<WorkflowStepInfoModel, WorkflowStepInfo>()
            .ForMember(
                d => d.sending_condition,
                o => o.MapFrom(s => s.sending_condition.ToJson(null, null))
            )
            .ForMember(d => d.p1_content, o => o.MapFrom(s => s.p1_content.ToJson(null, null)))
            .ForMember(d => d.p1_request, o => o.MapFrom(s => s.p1_request.ToJson(null, null)))
            .ForMember(d => d.p2_content, o => o.MapFrom(s => s.p2_content.ToJson(null, null)))
            .ForMember(d => d.p2_request, o => o.MapFrom(s => s.p2_request.ToJson(null, null)));

        builder
            .CreateMap<WorkflowStepInfo, WorkflowStepInfoModel>()
            .ForMember(
                d => d.sending_condition,
                o => o.MapFrom(s => s.sending_condition.FromJson<object>())
            )
            .ForMember(d => d.p1_content, o => o.MapFrom(s => s.p1_content.FromJson<object>()))
            .ForMember(d => d.p1_request, o => o.MapFrom(s => s.p1_request.FromJson<object>()))
            .ForMember(d => d.p2_content, o => o.MapFrom(s => s.p2_content.FromJson<object>()))
            .ForMember(d => d.p2_request, o => o.MapFrom(s => s.p2_request.FromJson<object>()));

        builder.CreateMap<WorkflowStep, WorkflowStep>().ForMember(d => d.Id, o => o.Ignore());
        builder.CreateMap<WorkflowDef, WorkflowDef>().ForMember(d => d.Id, o => o.Ignore());

        builder.CreateMap<WorkflowStep, WorkflowStepModel>();
        builder.CreateMap<WorkflowStepModel, WorkflowStep>();
    }
}

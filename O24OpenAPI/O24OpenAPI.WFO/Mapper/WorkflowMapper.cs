using Linh.JsonKit.Json;
using O24OpenAPI.WFO.Domain;
using O24OpenAPI.WFO.Models;
using Riok.Mapperly.Abstractions;

namespace O24OpenAPI.WFO.Mapper;

[Mapper(
    RequiredMappingStrategy = RequiredMappingStrategy.None,
    RequiredEnumMappingStrategy = RequiredMappingStrategy.None
)]
public static partial class WorkflowMapper
{
    private static string ToJson(object obj)
    {
        return obj.ToJson();
    }

    [MapProperty(nameof(WorkflowInfo.input), nameof(WorkflowInfoModel.input), Use = nameof(ToJson))]
    [MapProperty(
        nameof(WorkflowInfo.response_content),
        nameof(WorkflowInfoModel.response_content),
        Use = nameof(ToJson)
    )]
    public static partial WorkflowInfo MapToWorkflowInfo(this WorkflowInfoModel model);

    [MapProperty(
        nameof(WorkflowStepInfo.sending_condition),
        nameof(WorkflowStepInfo.sending_condition),
        Use = nameof(ToJson)
    )]
    [MapProperty(
        nameof(WorkflowStepInfo.p1_content),
        nameof(WorkflowStepInfo.p1_content),
        Use = nameof(ToJson)
    )]
    [MapProperty(
        nameof(WorkflowStepInfo.p1_request),
        nameof(WorkflowStepInfo.p1_request),
        Use = nameof(ToJson)
    )]
    [MapProperty(
        nameof(WorkflowStepInfo.p2_content),
        nameof(WorkflowStepInfo.p2_content),
        Use = nameof(ToJson)
    )]
    [MapProperty(
        nameof(WorkflowStepInfo.p2_request),
        nameof(WorkflowStepInfo.p2_request),
        Use = nameof(ToJson)
    )]
    public static partial WorkflowStepInfo MapToWorkflowStepInfo(this WorkflowStepInfoModel model);
}

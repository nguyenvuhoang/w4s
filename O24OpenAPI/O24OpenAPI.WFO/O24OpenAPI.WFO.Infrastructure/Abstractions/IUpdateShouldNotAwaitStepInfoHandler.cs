using O24OpenAPI.Client.Scheme.Workflow;

namespace O24OpenAPI.WFO.Infrastructure.Abstractions;

public interface IUpdateShouldNotAwaitStepInfoHandler
{
    Task UpdateShouldNotAwaitStepInfo(WFScheme wFScheme);
}

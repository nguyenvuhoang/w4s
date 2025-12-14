using LinKit.Core.Cqrs;
using O24OpenAPI.O24OpenAPIClient.Scheme.Workflow;

namespace O24OpenAPI.Web.Framework.Abstractions;

public interface IWorkflowStepInvoker
{
    Task<object> InvokeAsync(
        string stepCode,
        WFScheme scheme,
        IMediator mediator,
        CancellationToken cancellationToken
    );
}

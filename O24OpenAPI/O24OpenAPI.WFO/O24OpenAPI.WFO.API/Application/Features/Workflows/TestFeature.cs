using LinKit.Core.Cqrs;
using LinKit.Core.Endpoints;

namespace O24OpenAPI.WFO.API.Application.Features.Workflows;

[ApiEndpoint(ApiMethod.Post, "workflows/test")]
public class Testcomand : ICommand<bool>
{
    public string WorkflowId { get; set; } = string.Empty;
}
[CqrsHandler]
public class TestFeatureHandler : ICommandHandler<Testcomand, bool>
{
    public async Task<bool> HandleAsync(
        Testcomand request,
        CancellationToken cancellationToken = default
    )
    {
        return await Task.FromResult(true);
    }

}

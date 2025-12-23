using LinKit.Core.Cqrs;
using O24OpenAPI.Framework.Attributes;

namespace O24OpenAPI.CMS.API.Application.Features.Hello;

public class HelloCommand : BaseTransactionModel, ICommand<HelloResponse>
{
    public string Name { get; set; }
}

public class HelloResponse
{
    public string Message { get; set; } = string.Empty;
}

public class Step
{
    public const string StepHello = "CMS_HELLO";
}

[CqrsHandler]
public class HelloHandler : ICommandHandler<HelloCommand, HelloResponse>
{
    [WorkflowStep(Step.StepHello)]
    public Task<HelloResponse> HandleAsync(
        HelloCommand request,
        CancellationToken cancellationToken = default
    )
    {
        return Task.FromResult(new HelloResponse() { Message = $"Hello {request.Name}" });
    }
}

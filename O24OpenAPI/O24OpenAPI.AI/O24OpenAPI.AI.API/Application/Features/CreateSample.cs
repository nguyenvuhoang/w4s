using LinKit.Core.Cqrs;
using O24OpenAPI.AI.Domain.AggregatesModel.SampleAggregate;
using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.AI.API.Application.Features;

public class CreateSampleCommand : BaseTransactionModel, ICommand<CreateSampleResponse>
{
    public string? SampleData { get; set; }
}

public record CreateSampleResponse();

[CqrsHandler]
public class CreateSampleCommandHandler(ISampleRepository sampleRepository)
    : ICommandHandler<CreateSampleCommand, CreateSampleResponse>
{
    [WorkflowStep("CreateSample")]
    public Task<CreateSampleResponse> HandleAsync(
        CreateSampleCommand request,
        CancellationToken cancellationToken = default
    )
    {
        throw new NotImplementedException();
    }
}

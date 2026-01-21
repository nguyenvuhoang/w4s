using LinKit.Core.Cqrs;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.PMT.Domain.AggregatesModel.PMTAggregate;

namespace O24OpenAPI.PMT.API.Application.Features;

public class CreatePMTCommand : BaseTransactionModel, ICommand<CreatePMTResponse>
{
    public string Data { get; set; }
}

public record CreatePMTResponse();

[CqrsHandler]
public class CreatePMTCommandHandler(IPMTRepository repository)
    : ICommandHandler<CreatePMTCommand, CreatePMTResponse>
{
    [WorkflowStep("CreatePMT")]
    public Task<CreatePMTResponse> HandleAsync(
        CreatePMTCommand request,
        CancellationToken cancellationToken = default
    )
    {
        // TODO: Implement
        throw new NotImplementedException();
    }
}

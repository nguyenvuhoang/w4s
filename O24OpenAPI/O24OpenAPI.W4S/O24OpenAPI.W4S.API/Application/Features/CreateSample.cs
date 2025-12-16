using LinKit.Core.Cqrs;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.W4S.API.Application.Features;

public class CreateSampleCommand : BaseTransactionModel, ICommand<CreateSampleResponse>
{
    public string? SampleData { get; set; }
}

public record CreateSampleResponse();

[CqrsHandler]
public class CreateSampleCommandHandler : ICommandHandler<CreateSampleCommand, CreateSampleResponse>
{
    public Task<CreateSampleResponse> HandleAsync(
        CreateSampleCommand request,
        CancellationToken cancellationToken = default
    )
    {
        throw new NotImplementedException();
    }
}

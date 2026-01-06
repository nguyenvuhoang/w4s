using LinKit.Core.Cqrs;
using O24OpenAPI.Logger.Domain.AggregateModels.ServiceLogAggregate;

namespace O24OpenAPI.Logger.API.Application.Features;

[CqrsHandler]
public class CreateServiceLog(IServiceLogRepository serviceLogRepository)
    : ICommandHandler<CreateServiceLogCommand>
{
    public async Task<Unit> HandleAsync(
        CreateServiceLogCommand request,
        CancellationToken cancellationToken = default
    )
    {
        var serviceLog = request.ToServiceLog();
        await serviceLogRepository.InsertAsync(serviceLog);
        return Unit.Value;
    }
}

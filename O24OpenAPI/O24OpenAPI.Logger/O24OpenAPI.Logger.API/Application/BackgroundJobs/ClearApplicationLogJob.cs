using LinKit.Core.BackgroundJobs;
using LinKit.Core.Cqrs;
using O24OpenAPI.Logger.Domain.AggregateModels.ServiceLogAggregate;

namespace O24OpenAPI.Logger.API.Application.BackgroundJobs;

[BackgroundJob("ClearApplicationLog")]
public class ClearApplicationLogCommand : BackgroundJobCommand { }

[CqrsHandler]
public class ClearApplicationLogHandler(IApplicationLogRepository applicationLogRepository)
    : ICommandHandler<ClearApplicationLogCommand>
{
    public async Task<Unit> HandleAsync(
        ClearApplicationLogCommand request,
        CancellationToken cancellationToken = default
    )
    {
        await applicationLogRepository.ClearApplicationLogAsync();
        return Unit.Value;
    }
}

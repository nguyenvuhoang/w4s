using LinKit.Core.BackgroundJobs;
using LinKit.Core.Cqrs;

namespace O24OpenAPI.PMT.API.Application.BackgroundJobs;

[BackgroundJob("HealthCheck")]
public class HealthCheckJob : BackgroundJobCommand { }

[CqrsHandler]
public class HealthCheckJobHandler : ICommandHandler<HealthCheckJob>
{
    public async Task<Unit> HandleAsync(
        HealthCheckJob request,
        CancellationToken cancellationToken = default
    )
    {
        // TODO: Implement health check logic
        return Unit.Value;
    }
}

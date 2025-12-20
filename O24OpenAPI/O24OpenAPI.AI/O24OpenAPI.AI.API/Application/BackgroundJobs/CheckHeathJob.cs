using LinKit.Core.BackgroundJobs;
using LinKit.Core.Cqrs;

namespace O24OpenAPI.AI.API.Application.BackgroundJobs;

[BackgroundJob("CheckHeath")]
public class CheckHeathJob : BackgroundJobCommand
{

}

[CqrsHandler]
public class CheckHeathJobHandler : ICommandHandler<CheckHeathJob>
{
    public async Task<Unit> HandleAsync(CheckHeathJob request, CancellationToken cancellationToken = default)
    {
        //.......
        return Unit.Value;
    }
}

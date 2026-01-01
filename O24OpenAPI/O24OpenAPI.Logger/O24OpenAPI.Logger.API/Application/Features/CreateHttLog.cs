using LinKit.Core.Cqrs;
using O24OpenAPI.Framework.Domain.Logging;
using O24OpenAPI.Logger.Domain.AggregateModels.HttpLogAggregate;

namespace O24OpenAPI.Logger.API.Application.Features;

[CqrsHandler]
public class CreateHttLog(IHttpLogRepository httpLogRepository)
    : ICommandHandler<CreateHttLogCommand>
{
    public async Task<Unit> HandleAsync(
        CreateHttLogCommand request,
        CancellationToken cancellationToken = default
    )
    {
        HttpLog httpLog = request.ToHttpLog();
        await httpLogRepository.InsertAsync(httpLog);
        return Unit.Value;
    }
}

using LinKit.Core.Cqrs;
using LinKit.Core.Endpoints;

namespace O24OpenAPI.CMS.API.Application.Features.Hello;

[ApiEndpoint(ApiMethod.Post, "say-hello-1", MediatorKey = "cms")]
public class Hello1Command : ICommand<bool> { }

[CqrsHandler]
public class Hello1Handler : ICommandHandler<Hello1Command, bool>
{
    public Task<bool> HandleAsync(Hello1Command request, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(true);
    }
}

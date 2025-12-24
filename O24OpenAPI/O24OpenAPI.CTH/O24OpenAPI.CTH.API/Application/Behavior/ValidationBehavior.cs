using LinKit.Core.Cqrs;
using O24OpenAPI.Framework.Abstractions;

namespace O24OpenAPI.CTH.API.Application.Behavior;

[CqrsBehavior(typeof(IRequest<>), 1)]
public class ValidationBehavior<TRequest, TResponse>(IServiceProvider serviceProvider)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> HandleAsync(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        var validator = serviceProvider.GetService<IValidator<IRequest>>();
        if (validator is not null)
            await validator.ValidateAsync(request);
        return await next();
    }
}

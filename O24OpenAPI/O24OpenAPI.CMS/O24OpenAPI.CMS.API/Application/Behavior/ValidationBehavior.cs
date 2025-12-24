using LinKit.Core.Cqrs;
using O24OpenAPI.Framework.Abstractions;

namespace O24OpenAPI.CMS.API.Application.Behavior;

[CqrsBehavior(typeof(ICommand), 1)]
public class ValidationBehavior<TRequest, TResponse>(IValidator<TRequest> validator)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{
    public async Task<TResponse> HandleAsync(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        await validator.ValidateAsync(request);
        return await next();
    }
}

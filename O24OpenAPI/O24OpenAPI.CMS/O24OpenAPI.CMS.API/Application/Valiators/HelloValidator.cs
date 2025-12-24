using LinKit.Core.Abstractions;
using O24OpenAPI.CMS.API.Application.Features.Hello;
using O24OpenAPI.Framework.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace O24OpenAPI.CMS.API.Application.Valiators;

[RegisterService(Lifetime.Scoped)]
public class HelloValidator : IValidator<HelloCommand>
{
    public async Task ValidateAsync(HelloCommand request)
    {
        await Task.CompletedTask;
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new ValidationException("Name cannot be empty.");
        }
    }
}

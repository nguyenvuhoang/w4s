using System.ComponentModel.DataAnnotations;
using LinKit.Core.Abstractions;
using O24OpenAPI.CMS.API.Application.Features.Hello;
using O24OpenAPI.Framework.Abstractions;

namespace O24OpenAPI.CMS.API.Application.Validators;

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

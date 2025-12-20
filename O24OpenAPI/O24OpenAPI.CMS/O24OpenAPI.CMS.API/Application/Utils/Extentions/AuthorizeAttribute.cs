using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using O24OpenAPI.CMS.API.Application.Services.Interfaces;

namespace O24OpenAPI.CMS.API.Application.Utils.Extentions;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var _userSessionService =
            context.HttpContext.RequestServices.GetService<ICoreAPIService>()
            ?? throw new InvalidOperationException(
                "ICoreAPIService is not registered in DI container."
            );
        var authorizationHeader =
            context.HttpContext.Request.Headers.Authorization.FirstOrDefault();

        if (string.IsNullOrEmpty(authorizationHeader))
        {
            context.Result = CreateUnauthorizedResponse(
                context,
                "No Authorization header provided. Please include a valid Bearer token."
            );
            return;
        }

        if (!authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            context.Result = new BadRequestObjectResult(
                new { Message = "Invalid Authorization header format. Expected 'Bearer <token>'" }
            );
            return;
        }

        string token = authorizationHeader["Bearer ".Length..].Trim();

        var isValid = await ValidateToken(token, _userSessionService);

        if (!isValid)
        {
            context.Result = CreateUnauthorizedResponse(
                context,
                "Invalid Token. Please try login again!!!!!"
            );
            return;
        }

        context.HttpContext.Items["authorization"] = token;
    }

    private static async Task<bool> ValidateToken(string token, ICoreAPIService coreAPIService)
    {
        if (string.IsNullOrEmpty(token))
        {
            return false;
        }
        var userToken = await coreAPIService.GetByToken(token);
        return userToken != null;
    }

    private static UnauthorizedObjectResult CreateUnauthorizedResponse(
        AuthorizationFilterContext context,
        string detailMessage
    )
    {
        var traceid = context.HttpContext.TraceIdentifier;
        var response = new
        {
            title = "CoreAPI Unauthorized",
            status = StatusCodes.Status401Unauthorized,
            traceId = traceid,
            detail = detailMessage,
        };

        return new UnauthorizedObjectResult(response);
    }
}

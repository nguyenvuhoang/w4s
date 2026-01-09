namespace O24OpenAPI.CMS.API.Application.Features.Requests;

public interface IRequestHandler
{
    Task<ResponseModel> ProcessAsync(RequestModel request, HttpContext httpContext);
}

using O24OpenAPI.CMS.API.Application.Models.Response;

namespace O24OpenAPI.CMS.API.Application.Features.Requests;

public interface IRequestHandler
{
    Task<ActionsResponseModel<object>> HandleAsync(BoRequestModel bo, HttpContext httpContext);
    Task<ResponseModel> ProcessAsync(RequestModel request, HttpContext httpContext);
}

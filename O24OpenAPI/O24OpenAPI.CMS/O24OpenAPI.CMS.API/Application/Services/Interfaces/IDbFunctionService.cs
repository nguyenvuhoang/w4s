namespace O24OpenAPI.CMS.API.Application.Services.Interfaces;

public interface IDbFunctionService
{
    Task<int> ExportToFile();
}

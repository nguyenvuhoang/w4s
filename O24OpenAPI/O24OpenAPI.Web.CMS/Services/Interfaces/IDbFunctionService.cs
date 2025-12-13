namespace O24OpenAPI.Web.CMS.Services.Interfaces;

public interface IDbFunctionService
{
    Task<int> ExportToFile();
}

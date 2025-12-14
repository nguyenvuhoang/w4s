namespace O24OpenAPI.Web.Framework.Services.CDC;

public interface ICdcKeyConfigService
{
    Task<string[]> GetKeyColumnsAsync(string tableName);
}

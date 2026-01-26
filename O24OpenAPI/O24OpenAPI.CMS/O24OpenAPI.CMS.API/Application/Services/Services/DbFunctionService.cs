using Newtonsoft.Json;
using O24OpenAPI.CMS.API.Application.Services.Interfaces;
using O24OpenAPI.CMS.API.Application.Utils;
using O24OpenAPI.Data.Configuration;
using O24OpenAPI.Framework.Services;

namespace O24OpenAPI.CMS.API.Application.Services.Services;

public class DbFunction
{
    [JsonProperty("objectname")]
    public string ObjectName { get; set; }

    [JsonProperty("objecttype")]
    public string ObjectType { get; set; }

    [JsonProperty("script")]
    public string Script { get; set; }
}

public class DbFunctionService(IExecuteQueryService executeQueryService) : IDbFunctionService
{
    IExecuteQueryService _executeQueryService = executeQueryService;

    public async Task<List<DbFunction>> GetAll()
    {
        var model = new ModelWithQuery() { CommandName = "GetDbFunctions" };
        var resultQuery = await _executeQueryService.SqlQuery(model);
        var list = resultQuery.JsonConvertToModel<List<DbFunction>>();
        return list;
    }

    public async Task<int> ExportToFile()
    {
        var list = await GetAll();
        foreach (DbFunction dbFunction in list)
        {
            string folderPath =
                dbFunction.ObjectType == "Stored Procedure"
                    ? $"Migrations/Scripts/{Singleton<DataConfig>.Instance.DataProvider}/StoredProcedures"
                    : $"Migrations/Scripts/{Singleton<DataConfig>.Instance.DataProvider}/Functions";

            string filePath = Path.Combine(folderPath, $"{dbFunction.ObjectName}.sql");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            await File.WriteAllTextAsync(filePath, dbFunction.Script);
        }
        return list.Count;
    }
}

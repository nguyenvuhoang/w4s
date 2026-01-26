using Newtonsoft.Json.Linq;

namespace O24OpenAPI.CMS.API.Application.Services.Interfaces;

public interface IDataService
{
    Task<JObject> ExportAllData(string host);
}

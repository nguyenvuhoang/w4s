using Newtonsoft.Json.Linq;

namespace O24OpenAPI.Web.CMS.Services.Interfaces;

public interface IDataService
{
    Task<JObject> ExportAllData(string host);
}

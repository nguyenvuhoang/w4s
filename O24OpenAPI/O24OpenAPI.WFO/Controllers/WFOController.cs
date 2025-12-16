using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using O24OpenAPI.Core.Domain;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Data.Utils;
using O24OpenAPI.Framework.Controllers;
using O24OpenAPI.Framework.Models.UtilityModels;
using O24OpenAPI.Framework.Utils;
using O24OpenAPI.WFO.Domain;

namespace O24OpenAPI.WFO.Controllers;

public class WFOController : BaseController
{
    [HttpPost]
    public async Task<IActionResult> ExportAllData(string host)
    {
        host ??= HttpContext.Request.Host.ToString();
        JObject result = new();
        var baseType = typeof(BaseEntity);

        var derivedTypes = Singleton<ITypeFinder>
            .Instance.FindClassesOfType<BaseEntity>()
            .Where(s =>
                (
                    s.FullName.Contains("O24OpenAPI.WFO.Domain")
                    || s.FullName.Contains("O24OpenAPI.Core.Domain.Configuration")
                )
                && !s.FullName.Contains("Log")
                && !s.FullName.Contains(nameof(WorkflowStepInfo))
                && !s.FullName.Contains(nameof(WorkflowInfo))
            );

        foreach (var type in derivedTypes)
        {
            var method = typeof(DataUtils).GetMethod(nameof(DataUtils.ExportAll));
            var genericMethod = method.MakeGenericMethod(type);

            try
            {
                var task = (Task<FileModel>)genericMethod.Invoke(null, [host]);
                var file = await task;

                if (file.FileContent is not null)
                {
                    string path = "Migrations/DataJson/All";

                    FileUtils.CreateDirectoryIfNotExist(path);

                    string originalFileName = file.FileName;

                    if (!originalFileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                    {
                        originalFileName += ".json";
                    }

                    string fullPath = Path.Combine(path, originalFileName);

                    bool flag = FileUtils.FileWriter(fullPath, file.FileContent);
                    if (flag)
                    {
                        result.Add(file.FileName, "Success");
                    }
                }
            }
            catch
            {
                continue;
            }
        }
        return Ok(result);
    }
}

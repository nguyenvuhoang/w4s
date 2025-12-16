using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Framework.Controllers;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Web.CMS.Models;
using O24OpenAPI.Web.CMS.Models.RabbitMQ;
using O24OpenAPI.Web.CMS.Models.Tools;
using O24OpenAPI.Web.CMS.Services.Interfaces;

namespace O24OpenAPI.Web.CMS.Controllers;

public class ToolsController(
    IFormService formService,
    IDataService dataService,
    IDbFunctionService dbFunctionService
) : BaseController
{
    private readonly IFormService _formService = formService;
    private readonly IDataService _dataService = dataService;
    private readonly IDbFunctionService _dbFunctionService = dbFunctionService;

    [HttpPost]
    public async Task<IActionResult> FeedFormDataRequestMapping()
    {
        await _formService.FeedDataRequestMapping();

        return Ok("Successfully");
    }

    [HttpPost]
    public virtual async Task<IActionResult> ExportWorkflowStepToFile(
        List<ExportWorkflowStepModel> listFields,
        string fileName = null,
        bool isReplace = true
    )
    {
        return await ExportToFile<WorkflowStep, ExportWorkflowStepModel>(
            listFields,
            fileName,
            isReplace,
            nameof(WorkflowStep)
        );
    }

    [HttpPost]
    public virtual async Task<IActionResult> ExportUserCommandToFile(
        List<ExportUserCommandModel> listFields,
        string fileName = null,
        bool isReplace = true
    )
    {
        return await ExportToFile<UserCommand, ExportUserCommandModel>(
            listFields,
            fileName,
            isReplace,
            nameof(UserCommand)
        );
    }

    [HttpPost]
    public virtual async Task<IActionResult> ExportCodeListToFile(
        List<ExportCodeListModel> listFields,
        string fileName = null,
        bool isReplace = true
    )
    {
        return await ExportToFile<C_CODELIST, ExportCodeListModel>(
            listFields,
            fileName,
            isReplace,
            nameof(C_CODELIST)
        );
    }

    private async Task<IActionResult> ExportToFile<TEntity, TModel>(
        List<TModel> listFields,
        string fileName,
        bool isReplace,
        string folderName
    )
        where TEntity : BaseEntity
        where TModel : BaseO24OpenAPIModel
    {
        try
        {
            var file = await Utils.Utils.ExportData<TEntity, TModel>(
                HttpContext.Request.Host.ToString(),
                listFields
            );

            if (file.FileContent is null)
            {
                return Ok("No data found");
            }
            string path = $"Migrations/DataJson/{folderName}";

            Utils.Utils.CreateDirectoryIfNotExist(path);

            string originalFileName = fileName ?? file.FileName;

            if (!originalFileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
            {
                originalFileName += ".json";
            }

            string fullPath = Path.Combine(path, originalFileName);

            // Check if file already exists
            if (System.IO.File.Exists(fullPath) && !isReplace)
            {
                int count = 1;
                string fileExtension = Path.GetExtension(originalFileName);
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(
                    originalFileName
                );
                string newFullPath;

                do
                {
                    string newFileName = $"{fileNameWithoutExtension} ({count}){fileExtension}";
                    newFullPath = Path.Combine(path, newFileName);
                    count++;
                } while (System.IO.File.Exists(newFullPath));

                fullPath = newFullPath;
            }

            bool flag = Utils.Utils.FileWriter(fullPath, file.FileContent);
            return Ok("Successfully!");
        }
        catch (Exception ex)
        {
            return Ok(ex);
        }
    }

    private static bool SaveToFile(List<FileModel> files, string tableName)
    {
        try
        {
            foreach (var file in files)
            {
                string path = $"Migrations/DataJson/{tableName}";

                Utils.Utils.CreateDirectoryIfNotExist(path);

                string fullPath = Path.Combine(path, file.FileName);
                bool flag = Utils.Utils.FileWriter(fullPath, file.FileContent);
            }
            return true;
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpPost]
    public virtual async Task<IActionResult> ExportWorkflowStepToFiles(
        List<ExportWorkflowStepModel> listFields
    )
    {
        return await ExportFiles<WorkflowStep, ExportWorkflowStepModel>(listFields);
    }

    private async Task<IActionResult> ExportFiles<TEntity, TModel>(List<TModel> listFields)
        where TEntity : BaseEntity
        where TModel : BaseO24OpenAPIModel
    {
        var files = await Utils.Utils.ExportListFiles<TEntity, TModel>(
            HttpContext.Request.Host.ToString(),
            listFields
        );
        SaveToFile(files, typeof(TEntity).Name);

        return Ok($"Converted {files.Count} files");
    }

    [HttpPost]
    public async Task<IActionResult> ExportAll()
    {
        var result = await _dataService.ExportAllData("192.168.1.138");

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> ExportFunction()
    {
        var count = await _dbFunctionService.ExportToFile();

        return Ok($"Exported: {count}");
    }

    [HttpPost]
    public async Task<IActionResult> ExportFunctionLuuTest()
    {
        await Task.CompletedTask;
        return Ok("Luu Test Successful");
    }

    [HttpPost]
    public async Task<IActionResult> ImportTransaction(IFormFile txftag, IFormFile rulefunc)
    {
        await Task.CompletedTask;
        var ftags = Utils.Utils.ReadCSV<TXFTagModel>(txftag);
        var rules = Utils.Utils.ReadCSV<RuleFunc>(rulefunc);

        return Ok("Successful");
    }

    [HttpGet]
    public async Task<IActionResult> SeedDataWorkflowDef()
    {
        var service = EngineContext.Current.Resolve<IWorkflowDefinitionService>();
        var response = await service.SeedDataWorkflowDef();
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateAllFormForNewWorkflow()
    {
        var response = new List<UpdateFormForNewWorkflowResponse>();
        try
        {
            var service = EngineContext.Current.Resolve<IFormService>();
            var forms = await service.GetAll();
            foreach (var form in forms)
            {
                var update = new UpdateFormForNewWorkflowResponse { FormId = form.FormId };
                try
                {
                    var layoutArray = JArray.Parse(form.ListLayout);
                    foreach (var layout in layoutArray)
                    {
                        if (layout["list_view"] is JArray listViewArray)
                        {
                            foreach (var view in listViewArray)
                            {
                                if (view["list_input"] is JArray listInputArray)
                                {
                                    foreach (var input in listInputArray)
                                    {
                                        if (input["config"]?["txFo"] != null)
                                        {
                                            string txFoString = input["config"]["txFo"].ToString();
                                            if (string.IsNullOrEmpty(txFoString))
                                            {
                                                continue;
                                            }

                                            JToken txFoToken = JToken.Parse(txFoString);

                                            // Xử lý khi txFo là một JArray
                                            if (txFoToken is JArray txFoArray)
                                            {
                                                foreach (var tx in txFoArray)
                                                {
                                                    if (tx["input"] is JObject inputObj)
                                                    {
                                                        if (
                                                            inputObj.ContainsKey("fields")
                                                            && inputObj["fields"].HasValues
                                                        )
                                                        {
                                                            continue;
                                                        }

                                                        string learnApi = inputObj["learn_api"]
                                                            ?.ToString();
                                                        string workflowId = inputObj["workflowid"]
                                                            ?.ToString();
                                                        if (
                                                            string.IsNullOrEmpty(learnApi)
                                                            || string.IsNullOrEmpty(workflowId)
                                                        )
                                                        {
                                                            continue;
                                                        }

                                                        JObject fieldsObject = new JObject();
                                                        foreach (var prop in inputObj.Properties())
                                                        {
                                                            if (
                                                                prop.Name != "learn_api"
                                                                && prop.Name != "workflowid"
                                                            )
                                                            {
                                                                fieldsObject[prop.Name] =
                                                                    prop.Value;
                                                            }
                                                        }

                                                        JObject newInput = new JObject
                                                        {
                                                            ["fields"] = fieldsObject,
                                                            ["learn_api"] = learnApi,
                                                            ["workflowid"] = workflowId,
                                                        };

                                                        tx["input"] = newInput;
                                                    }
                                                }

                                                input["config"]["txFo"] =
                                                    JsonConvert.SerializeObject(
                                                        txFoArray,
                                                        Formatting.Indented
                                                    );
                                            }
                                            // Xử lý khi txFo là một JObject và có key "bo" chứa một JArray
                                            else if (txFoToken is JObject txFoObject)
                                            {
                                                if (txFoObject["bo"] is JArray boArray)
                                                {
                                                    foreach (var boItem in boArray)
                                                    {
                                                        if (boItem["input"] is JObject inputObj)
                                                        {
                                                            if (
                                                                inputObj.ContainsKey("fields")
                                                                && inputObj["fields"].HasValues
                                                            )
                                                            {
                                                                continue;
                                                            }

                                                            string learnApi = inputObj["learn_api"]
                                                                ?.ToString();
                                                            string workflowId = inputObj[
                                                                "workflowid"
                                                            ]
                                                                ?.ToString();
                                                            if (
                                                                string.IsNullOrEmpty(learnApi)
                                                                || string.IsNullOrEmpty(workflowId)
                                                            )
                                                            {
                                                                continue;
                                                            }

                                                            JObject fieldsObject = new JObject();
                                                            foreach (
                                                                var prop in inputObj.Properties()
                                                            )
                                                            {
                                                                if (
                                                                    prop.Name != "learn_api"
                                                                    && prop.Name != "workflowid"
                                                                )
                                                                {
                                                                    fieldsObject[prop.Name] =
                                                                        prop.Value;
                                                                }
                                                            }

                                                            JObject newInput = new JObject
                                                            {
                                                                ["fields"] = fieldsObject,
                                                                ["learn_api"] = learnApi,
                                                                ["workflowid"] = workflowId,
                                                            };

                                                            boItem["input"] = newInput;
                                                        }
                                                    }
                                                }

                                                input["config"]["txFo"] =
                                                    JsonConvert.SerializeObject(
                                                        txFoObject,
                                                        Formatting.Indented
                                                    );
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    form.ListLayout = layoutArray.ToSerialize();
                    await service.Update(form);
                    update.Status = true;
                    response.Add(update);
                }
                catch (Exception e)
                {
                    update.Status = false;
                    update.Message = e.Message;
                    response.Add(update);
                }
            }
        }
        catch (Exception ex)
        {
            return Ok(ex);
        }

        return Ok(response);
    }

    private class UpdateFormForNewWorkflowResponse
    {
        public string FormId { get; set; }
        public bool Status { get; set; } = true;
        public string Message { get; set; }
    }

    [HttpPost]
    public async Task<IActionResult> UpdateFormForNewWorkflow(string formId, string app)
    {
        try
        {
            var service = EngineContext.Current.Resolve<IFormService>();
            var form = await service.GetByIdAndApp(formId, app);
            var layoutArray = JArray.Parse(form.ListLayout.ToSerialize());
            foreach (var layout in layoutArray)
            {
                if (layout["list_view"] is JArray listViewArray)
                {
                    foreach (var view in listViewArray)
                    {
                        if (view["list_input"] is JArray listInputArray)
                        {
                            foreach (var input in listInputArray)
                            {
                                if (input["config"]?["txFo"] != null)
                                {
                                    string txFoString = input["config"]["txFo"].ToString();
                                    if (string.IsNullOrEmpty(txFoString))
                                    {
                                        continue;
                                    }

                                    JArray txFoArray = JArray.Parse(txFoString);

                                    foreach (var tx in txFoArray)
                                    {
                                        if (tx["input"] is JObject inputObj)
                                        {
                                            string learnApi = inputObj["learn_api"]?.ToString();
                                            string workflowId = inputObj["workflowid"]?.ToString();
                                            if (
                                                string.IsNullOrEmpty(learnApi)
                                                || string.IsNullOrEmpty(workflowId)
                                            )
                                            {
                                                continue;
                                            }

                                            JObject fieldsObject = new JObject();
                                            foreach (var prop in inputObj.Properties())
                                            {
                                                if (
                                                    prop.Name != "learn_api"
                                                    && prop.Name != "workflowid"
                                                )
                                                {
                                                    fieldsObject[prop.Name] = prop.Value;
                                                }
                                            }

                                            JObject newInput = new JObject
                                            {
                                                ["fields"] = fieldsObject,
                                                ["learn_api"] = learnApi,
                                                ["workflowid"] = workflowId,
                                            };

                                            tx["input"] = newInput;
                                        }
                                    }

                                    input["config"]["txFo"] = JsonConvert.SerializeObject(
                                        txFoArray,
                                        Formatting.Indented
                                    );
                                }
                            }
                        }
                    }
                }
            }
            return Ok(layoutArray);
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync();
            throw;
        }
    }

    [HttpGet]
    public virtual async Task<IActionResult> GetQueueConsumers(
        string queueHost,
        string username,
        string password,
        string queueName,
        string vhost = "/"
    )
    {
        if (string.IsNullOrWhiteSpace(queueName))
        {
            return BadRequest(new { Message = "queueName is required" });
        }

        var baseUrl = $"http://{queueHost}:15672/api/";

        var vhostEncoded = Uri.EscapeDataString(vhost);
        var queueEncoded = Uri.EscapeDataString(queueName);
        var url = $"{baseUrl}queues/{vhostEncoded}/{queueEncoded}";

        using var client = new HttpClient();

        var raw = $"{username}:{password}";
        var bytes = Encoding.ASCII.GetBytes(raw);
        var base64 = Convert.ToBase64String(bytes);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64);

        try
        {
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                return StatusCode(
                    (int)response.StatusCode,
                    new
                    {
                        Message = "RabbitMQ API error",
                        Status = response.StatusCode,
                        Body = body,
                    }
                );
            }

            var json = await response.Content.ReadAsStringAsync();
            var queue = System.Text.Json.JsonSerializer.Deserialize<RabbitQueueDetail>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            var consumerCount = queue?.Consumers ?? 0;

            return Ok(new { queue = queue?.Name ?? queueName, consumers = consumerCount });
        }
        catch (Exception ex)
        {
            return StatusCode(
                500,
                new { Message = "Error calling RabbitMQ API", Error = ex.Message }
            );
        }
    }
}

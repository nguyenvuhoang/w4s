using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O24OpenAPI.CMS.API.Application.Models;
using O24OpenAPI.CMS.API.Application.Models.RabbitMQ;
using O24OpenAPI.CMS.API.Application.Models.Tools;
using O24OpenAPI.CMS.API.Application.Services.Interfaces;
using O24OpenAPI.CMS.Domain.AggregateModels.Digital;
using O24OpenAPI.CMS.Domain.AggregateModels.FormAggregate;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Framework.Controllers;
using O24OpenAPI.Framework.Extensions;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace O24OpenAPI.CMS.API.Controllers;

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
            FileModel file = await Application.Utils.Utils.ExportData<TEntity, TModel>(
                HttpContext.Request.Host.ToString(),
                listFields
            );

            if (file.FileContent is null)
            {
                return Ok("No data found");
            }
            string path = $"Migrations/DataJson/{folderName}";

            Application.Utils.Utils.CreateDirectoryIfNotExist(path);

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

            bool flag = Application.Utils.Utils.FileWriter(fullPath, file.FileContent);
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
            foreach (FileModel file in files)
            {
                string path = $"Migrations/DataJson/{tableName}";

                Application.Utils.Utils.CreateDirectoryIfNotExist(path);

                string fullPath = Path.Combine(path, file.FileName);
                bool flag = Application.Utils.Utils.FileWriter(fullPath, file.FileContent);
            }
            return true;
        }
        catch (Exception)
        {
            throw;
        }
    }

    private async Task<IActionResult> ExportFiles<TEntity, TModel>(List<TModel> listFields)
        where TEntity : BaseEntity
        where TModel : BaseO24OpenAPIModel
    {
        List<FileModel> files = await Application.Utils.Utils.ExportListFiles<TEntity, TModel>(
            HttpContext.Request.Host.ToString(),
            listFields
        );
        SaveToFile(files, typeof(TEntity).Name);

        return Ok($"Converted {files.Count} files");
    }

    [HttpPost]
    public async Task<IActionResult> ExportAll()
    {
        JObject result = await _dataService.ExportAllData("192.168.1.138");

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> ExportFunction()
    {
        int count = await _dbFunctionService.ExportToFile();

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
        Task<List<TXFTagModel>> ftags = Application.Utils.Utils.ReadCSV<TXFTagModel>(txftag);
        Task<List<RuleFunc>> rules = Application.Utils.Utils.ReadCSV<RuleFunc>(rulefunc);

        return Ok("Successful");
    }

    [HttpPost]
    public async Task<IActionResult> UpdateAllFormForNewWorkflow()
    {
        List<UpdateFormForNewWorkflowResponse> response = [];
        try
        {
            IFormService service = EngineContext.Current.Resolve<IFormService>();
            List<Form> forms = await service.GetAll();
            foreach (Form form in forms)
            {
                UpdateFormForNewWorkflowResponse update = new() { FormId = form.FormId };
                try
                {
                    JArray layoutArray = JArray.Parse(form.ListLayout);
                    foreach (JToken layout in layoutArray)
                    {
                        if (layout["list_view"] is JArray listViewArray)
                        {
                            foreach (JToken view in listViewArray)
                            {
                                if (view["list_input"] is JArray listInputArray)
                                {
                                    foreach (JToken input in listInputArray)
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
                                                foreach (JToken tx in txFoArray)
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

                                                        JObject fieldsObject = [];
                                                        foreach (
                                                            JProperty prop in inputObj.Properties()
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

                                                        JObject newInput = new()
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
                                                    foreach (JToken boItem in boArray)
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

                                                            JObject fieldsObject = [];
                                                            foreach (
                                                                JProperty prop in inputObj.Properties()
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

                                                            JObject newInput = new()
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
            IFormService service = EngineContext.Current.Resolve<IFormService>();
            FormModel form = await service.GetByIdAndApp(formId, app);
            JArray layoutArray = JArray.Parse(form.ListLayout.ToSerialize());
            foreach (JToken layout in layoutArray)
            {
                if (layout["list_view"] is JArray listViewArray)
                {
                    foreach (JToken view in listViewArray)
                    {
                        if (view["list_input"] is JArray listInputArray)
                        {
                            foreach (JToken input in listInputArray)
                            {
                                if (input["config"]?["txFo"] != null)
                                {
                                    string txFoString = input["config"]["txFo"].ToString();
                                    if (string.IsNullOrEmpty(txFoString))
                                    {
                                        continue;
                                    }

                                    JArray txFoArray = JArray.Parse(txFoString);

                                    foreach (JToken tx in txFoArray)
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

                                            JObject fieldsObject = [];
                                            foreach (JProperty prop in inputObj.Properties())
                                            {
                                                if (
                                                    prop.Name != "learn_api"
                                                    && prop.Name != "workflowid"
                                                )
                                                {
                                                    fieldsObject[prop.Name] = prop.Value;
                                                }
                                            }

                                            JObject newInput = new()
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

        string baseUrl = $"http://{queueHost}:15672/api/";

        string vhostEncoded = Uri.EscapeDataString(vhost);
        string queueEncoded = Uri.EscapeDataString(queueName);
        string url = $"{baseUrl}queues/{vhostEncoded}/{queueEncoded}";

        using HttpClient client = new();

        string raw = $"{username}:{password}";
        byte[] bytes = Encoding.ASCII.GetBytes(raw);
        string base64 = Convert.ToBase64String(bytes);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64);

        try
        {
            HttpResponseMessage response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                string body = await response.Content.ReadAsStringAsync();
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

            string json = await response.Content.ReadAsStringAsync();
            RabbitQueueDetail queue =
                System.Text.Json.JsonSerializer.Deserialize<RabbitQueueDetail>(
                    json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

            int consumerCount = queue?.Consumers ?? 0;

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

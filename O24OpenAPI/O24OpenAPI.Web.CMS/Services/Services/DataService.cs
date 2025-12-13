using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.O24OpenAPIClient.Enums;
using O24OpenAPI.Web.CMS.Constant;
using O24OpenAPI.Web.CMS.Models;
using O24OpenAPI.Web.CMS.Models.Tools;
using O24OpenAPI.Web.CMS.Services.Interfaces;

namespace O24OpenAPI.Web.CMS.Services.Services
{
    public class DataService(
        IWorkflowStepService workflowStepService,
        IWorkflowDefinitionService workflowDefinitionService
    ) : IDataService
    {
        private readonly IWorkflowStepService _workflowStepService = workflowStepService;
        private readonly IWorkflowDefinitionService _workflowDefinitionService =
            workflowDefinitionService;

        public async Task<JObject> ExportAllData(string host)
        {
            JObject result = new();
            var baseType = typeof(BaseEntity);

            var derivedTypes = Singleton<ITypeFinder>
                .Instance.FindClassesOfType<BaseEntity>()
                .Where(s =>
                    (
                        s.FullName.Contains("O24OpenAPI.Web.CMS.Domain")
                        || s.FullName.Contains("O24OpenAPI.Core.Domain.Configuration")
                    )
                    && !s.FullName.Contains("Log")
                    && !s.FullName.Contains("UserSessions")
                    && !s.FullName.Contains("D_DEVICE")
                );

            foreach (var type in derivedTypes)
            {
                var method = typeof(Utils.Utils).GetMethod(nameof(Utils.Utils.ExportAll));
                var genericMethod = method.MakeGenericMethod(type);

                try
                {
                    var task = (Task<FileModel>)genericMethod.Invoke(null, [host]);
                    var file = await task;

                    if (file.FileContent is not null)
                    {
                        string path = "Migrations/DataJson/All";

                        Utils.Utils.CreateDirectoryIfNotExist(path);

                        string originalFileName = file.FileName;

                        if (!originalFileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                        {
                            originalFileName += ".json";
                        }

                        string fullPath = Path.Combine(path, originalFileName);

                        bool flag = Utils.Utils.FileWriter(fullPath, file.FileContent);
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
            return result;
        }

        private async Task CreateWorkflow(
            List<TXFTagModel> tXFTagModels,
            List<RuleFunc> ruleFunc,
            string appCode = AppCode.TellerApp,
            ActionType actionType = ActionType.CreateFo
        )
        {
            await Task.CompletedTask;
            var tranCode = tXFTagModels[0].TXCODE;
            Dictionary<string, string> fields = [];
            Dictionary<string, string> mapResponse = [];
            foreach (var tag in tXFTagModels)
            {
                _ = tag.CAPTION.TryParse<JObject>(out var caption);
                if (caption != null)
                {
                    var key = caption["en"].ToLowerCase();
                    fields[key] = $"MapS.dataS(request.{key})";
                    mapResponse[key] = $"MapS.dataS({tag.FTAG.ToLower()})";
                }
            }

            var sendingTemplate = new SendingTemplate()
            {
                WorkflowFunc = tranCode,
                ActionType = actionType.ToString(),
                Fields = fields,
            };
            var workflowStep = new WorkflowStep()
            {
                WFId = tranCode,
                StepOrder = 1,
                StepCode = tranCode,
                Description = tranCode,
                AppCode = appCode,
                SendingTemplate = sendingTemplate.ToSerialize(),
                ProcessingNumber = ProcessNumber.Core,
                IsReverse = false,
            };
            // await _workflowStepService.
        }

        public async Task CreateDataTransactionConfig(
            List<TXFTagModel> tXFTagModel,
            List<RuleFunc> ruleFunc,
            string appCode = AppCode.TellerApp
        )
        {
            await CreateWorkflow(tXFTagModel, ruleFunc, appCode);
        }
    }
}

public class SendingTemplate
{
    [JsonProperty("lang")]
    public string Lang { get; set; } = "MapS.infoRequest(lang)"; // Tương ứng với "MapS.infoRequest(lang)"

    [JsonProperty("token")]
    public string Token { get; set; } = "MapS.context(token)"; // Tương ứng với "MapS.context(token)"

    [JsonProperty("workflow_func")]
    public string WorkflowFunc { get; set; } = "CRD_APR"; // Giá trị mặc định

    [JsonProperty("action_type")]
    public string ActionType { get; set; } = "CreateFo"; // Giá trị mặc định

    [JsonProperty("fields")]
    public Dictionary<string, string> Fields { get; set; } // Danh sách các field khác
}

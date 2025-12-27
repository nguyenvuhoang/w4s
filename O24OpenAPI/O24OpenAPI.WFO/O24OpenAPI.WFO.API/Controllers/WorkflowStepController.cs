using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using O24OpenAPI.Client.Workflow;
using O24OpenAPI.Core;
using O24OpenAPI.Core.Domain;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.WFO.API.Application.Models;
using O24OpenAPI.WFO.Domain.AggregateModels.WorkflowAggregate;
using System.Reflection;
using WorkflowInput = O24OpenAPI.WFO.API.Application.Models.WorkflowInput;

namespace O24OpenAPI.WFO.API.Controllers;

public class WorkflowStepController(IWorkflowStepRepository workflowStepRepository)
    : WFOBaseController
{
    [HttpPost]
    public async Task<IActionResult> GetMappingResponse([FromBody] JObject req)
    {
        string txCode = req["txCode"]?.ToString();
        string stepCode = req["stepCode"]?.ToString();

        WorkflowStep r = await workflowStepRepository.GetByWorkflowIdAndStepCode(txCode, stepCode);
        return Ok(r.MappingResponse);
    }

    [HttpPost]
    public async Task<IActionResult> SimpleSearch([FromBody] WorkflowInput request)
    {
        string searchText = request.Fields["searchtext"].ToString();
        string pageIndex = request.Fields["pageindex"].ToString();
        string pageSize = request.Fields["pagesize"].ToString();

        SimpleSearchModel model = new()
        {
            SearchText = searchText,
            PageIndex = pageIndex.ToInt(),
            PageSize = pageSize.ToInt(),
        };

        IPagedList<WorkflowStep> r = await workflowStepRepository.SimpleSearch(model);

        List<JObject> jObjects = r.Select(x => JObject.FromObject(x)).ToList();
        PagedList<JObject> pagedJObjects = new(jObjects, r.PageIndex, r.PageSize, r.TotalCount);

        PageListModel pageListModel = new(pagedJObjects, r.TotalCount);

        JObject result = new() { ["data"] = JObject.FromObject(pageListModel) };

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> GetByWorkflowId([FromBody] WorkflowInput request)
    {
        string workflowid = request.Fields["workflowid"].ToString();

        IPagedList<WorkflowStep> r = await workflowStepRepository.GetByWorkflowId(workflowid);

        List<JObject> jObjects = r.Select(x => JObject.FromObject(x)).ToList();
        PagedList<JObject> pagedJObjects = new(jObjects, r.PageIndex, r.PageSize, r.TotalCount);

        PageListModel pageListModel = new(pagedJObjects, r.TotalCount);

        JObject result = new() { ["data"] = JObject.FromObject(pageListModel) };

        return Ok(result);
    }

    [HttpPost]
    public async Task<WorkflowResponse> Update([FromBody] WorkflowInput request)
    {
        WorkflowResponse response = await InvokeAsync<WorkflowResponse>(async () =>
        {
            if (request.Fields["wfstep"] is not JObject wfStepObj)
            {
                WorkflowResponse rs = new()
                {
                    error_code = "ERR_INVALID_FORMAT",
                    error_message = "wfstep is invalid format!",
                };
                return rs;
            }

            string workflowId = wfStepObj["WorkflowId"]?.ToString();
            string stepCode = wfStepObj["StepCode"]?.ToString();

            if (string.IsNullOrEmpty(workflowId))
            {
                WorkflowResponse rs = new()
                {
                    error_code = "ERR_MISSING_FIELD",
                    error_message = "WorkflowId is missing!",
                };
                return rs;
            }

            if (string.IsNullOrEmpty(stepCode))
            {
                WorkflowResponse rs = new()
                {
                    error_code = "ERR_MISSING_FIELD",
                    error_message = "StepCode is missing!",
                };
                return rs;
            }

            WorkflowStep wfStep = await workflowStepRepository.GetByWorkflowIdAndStepCode(
                workflowId,
                stepCode
            );
            if (wfStep == null)
            {
                WorkflowResponse rs = new()
                {
                    error_code = "ERR_NOT_EXISTS_DATA",
                    error_message = "Can not find WorkflowStep to update!",
                };
                return rs;
            }

            foreach (KeyValuePair<string, JToken> kv in wfStepObj)
            {
                PropertyInfo prop = wfStep.GetType().GetProperty(kv.Key);
                if (prop != null && kv.Value != null)
                {
                    try
                    {
                        object convertedValue;

                        if (prop.PropertyType == typeof(string) && kv.Value is JObject jObj)
                        {
                            convertedValue = NormalizeToken(jObj);
                        }
                        else
                        {
                            convertedValue = Convert.ChangeType(kv.Value, prop.PropertyType);
                        }

                        prop.SetValue(wfStep, convertedValue);
                    }
                    catch { }
                }
            }
            await workflowStepRepository.Update(wfStep);
            WorkflowResponse result = new();
            Dictionary<string, object> dict = JObject
                .FromObject(wfStep)
                .ToObject<Dictionary<string, object>>();
            result.data = dict;
            return result;
        });
        return response;
    }

    [HttpPost]
    public async Task<WorkflowResponse> Insert([FromBody] WorkflowInput request)
    {
        WorkflowResponse response = await InvokeAsync<WorkflowResponse>(async () =>
        {
            if (request.Fields["wfstep"] is not JObject wfStepObj)
            {
                WorkflowResponse rs = new()
                {
                    error_code = "ERR_INVALID_FORMAT",
                    error_message = "wfstep is invalid format!",
                };
                return rs;
            }

            string workflowId = wfStepObj["WorkflowId"]?.ToString();
            string stepCode = wfStepObj["StepCode"]?.ToString();
            int stepOrder = wfStepObj["StepOrder"]?.ToObject<int>() ?? 0;

            if (string.IsNullOrEmpty(workflowId))
            {
                WorkflowResponse rs = new()
                {
                    error_code = "ERR_MISSING_FIELD",
                    error_message = "WorkflowId is missing!",
                };
                return rs;
            }

            if (string.IsNullOrEmpty(stepCode))
            {
                WorkflowResponse rs = new()
                {
                    error_code = "ERR_MISSING_FIELD",
                    error_message = "StepCode is missing!",
                };
                return rs;
            }

            if (stepOrder == 0)
            {
                WorkflowResponse rs = new()
                {
                    error_code = "ERR_MISSING_FIELD",
                    error_message = "StepOrder is missing!",
                };
                return rs;
            }

            WorkflowStep checkExists =
                await workflowStepRepository.GetByWorkflowIdAndStepCodeAndStepOrder(
                    workflowId,
                    stepCode,
                    stepOrder
                );
            if (checkExists != null)
            {
                WorkflowResponse rs = new()
                {
                    error_code = "ERR_EXISTS_DATA",
                    error_message = "This WorkflowStep already exists!",
                };
                return rs;
            }

            WorkflowStep wfStep = new()
            {
                WorkflowId = workflowId,
                StepCode = stepCode,
                StepOrder = stepOrder,
                ServiceId = wfStepObj["ServiceId"]?.ToString() ?? "",
                Status = wfStepObj["Status"]?.ToObject<bool>() ?? true,
                Description = wfStepObj["Description"]?.ToString() ?? "",
                SendingTemplate = NormalizeToken(wfStepObj["SendingTemplate"]),
                MappingResponse = NormalizeToken(wfStepObj["MappingResponse"]),
                SubSendingTemplate = NormalizeToken(wfStepObj["SubSendingTemplate"]),
                SendingCondition = NormalizeToken(wfStepObj["SendingCondition"]),
                StepTimeout = wfStepObj["StepTimeout"]?.Value<long?>() ?? 60000,
                IsReverse = wfStepObj["IsReverse"]?.ToObject<bool>() ?? false,
                ShouldAwaitStep = wfStepObj["ShouldAwaitStep"]?.ToObject<bool>() ?? true,
                ProcessingNumber = (Client.Enums.ProcessNumber)(
                    wfStepObj["ProcessingNumber"]?.ToObject<int>() ?? 0
                ),
            };
            await workflowStepRepository.InsertAsync(wfStep);
            WorkflowResponse result = new();
            Dictionary<string, object> dict = JObject
                .FromObject(wfStep)
                .ToObject<Dictionary<string, object>>();
            result.data = dict;
            return result;
        });
        return response;
    }

    [HttpPost]
    public async Task<WorkflowResponse> Delete([FromBody] WorkflowInput request)
    {
        WorkflowResponse response = await InvokeAsync<WorkflowResponse>(async () =>
        {
            if (request.Fields["listWfStep"] is not JArray listWfStep)
            {
                WorkflowResponse rs = new()
                {
                    error_code = "ERR_INVALID_FORMAT",
                    error_message = "wfstep is invalid format!",
                };
                return rs;
            }
            if (listWfStep.Count > 0)
            {
                List<Dictionary<string, object>> deletedItems = [];
                foreach (JToken item in listWfStep)
                {
                    string value = (string)item;
                    string[] parts = value.Split('#');

                    string workflowId = parts[0];
                    string stepCode = parts.Length > 1 ? parts[1] : null;
                    int stepOrder = parts.Length > 2 ? parts[2].ToInt() : 0;

                    if (string.IsNullOrEmpty(workflowId))
                    {
                        WorkflowResponse rs = new()
                        {
                            error_code = "ERR_MISSING_FIELD",
                            error_message = "WorkflowId is missing!",
                        };
                        return rs;
                    }

                    if (string.IsNullOrEmpty(stepCode))
                    {
                        WorkflowResponse rs = new()
                        {
                            error_code = "ERR_MISSING_FIELD",
                            error_message = "StepCode is missing!",
                        };
                        return rs;
                    }

                    if (stepOrder == 0)
                    {
                        WorkflowResponse rs = new()
                        {
                            error_code = "ERR_MISSING_FIELD",
                            error_message = "StepOrder is missing!",
                        };
                        return rs;
                    }

                    WorkflowStep wfStep =
                        await workflowStepRepository.GetByWorkflowIdAndStepCodeAndStepOrder(
                            workflowId,
                            stepCode,
                            stepOrder
                        );
                    if (wfStep == null)
                    {
                        WorkflowResponse rs = new()
                        {
                            error_code = "ERR_NOT_EXISTS_DATA",
                            error_message = "Can not find WorkflowStep to delete!",
                        };
                        return rs;
                    }

                    await workflowStepRepository.Delete(wfStep);
                    Dictionary<string, object> dict = JObject
                        .FromObject(wfStep)
                        .ToObject<Dictionary<string, object>>();
                    deletedItems.Add(dict);
                }
                return new WorkflowResponse
                {
                    data = new Dictionary<string, object> { ["deletedItems"] = deletedItems },
                };
            }
            else
            {
                WorkflowResponse rs = new()
                {
                    error_code = "ERR_EMPTY_DATA",
                    error_message = "listWfStep is empty!",
                };
                return rs;
            }
        });
        return response;
    }

    private static string NormalizeToken(JToken token)
    {
        return (token is JObject obj && !obj.HasValues)
            ? null
            : token?.ToString(Newtonsoft.Json.Formatting.None);
    }
}

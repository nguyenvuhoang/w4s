using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using O24OpenAPI.Client.Workflow;
using O24OpenAPI.Core.Domain;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.WFO.Domain;
using O24OpenAPI.WFO.Lib;
using O24OpenAPI.WFO.Models;
using O24OpenAPI.WFO.Services.Interfaces;
using WorkflowInput = O24OpenAPI.WFO.Models.WorkflowInput;

namespace O24OpenAPI.WFO.Controllers;

public class WorkflowStepController(IWorkflowStepService workflowStepService) : WFOBaseController
{
    private readonly IWorkflowStepService _workflowStepService = workflowStepService;

    [HttpPost]
    public async Task<IActionResult> GetMappingResponse([FromBody] JObject req)
    {
        var txCode = req["txCode"]?.ToString();
        var stepCode = req["stepCode"]?.ToString();

        var r = await _workflowStepService.GetByWorkflowIdAndStepCode(txCode, stepCode);
        return Ok(r.MappingResponse);
    }

    [HttpPost]
    public async Task<IActionResult> SimpleSearch([FromBody] WorkflowInput request)
    {
        var searchText = request.Fields["searchtext"].ToString();
        var pageIndex = request.Fields["pageindex"].ToString();
        var pageSize = request.Fields["pagesize"].ToString();

        var model = new SimpleSearchModel()
        {
            SearchText = searchText,
            PageIndex = pageIndex.ToInt(),
            PageSize = pageSize.ToInt(),
        };

        var r = await _workflowStepService.SimpleSearch(model);

        var jObjects = r.Select(x => JObject.FromObject(x)).ToList();
        var pagedJObjects = new PagedList<JObject>(jObjects, r.PageIndex, r.PageSize, r.TotalCount);

        var pageListModel = new PageListModel(pagedJObjects, r.TotalCount);

        var result = new JObject { ["data"] = JObject.FromObject(pageListModel) };

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> GetByWorkflowId([FromBody] WorkflowInput request)
    {
        var workflowid = request.Fields["workflowid"].ToString();

        var r = await _workflowStepService.GetByWorkflowId(workflowid);

        var jObjects = r.Select(x => JObject.FromObject(x)).ToList();
        var pagedJObjects = new PagedList<JObject>(jObjects, r.PageIndex, r.PageSize, r.TotalCount);

        var pageListModel = new PageListModel(pagedJObjects, r.TotalCount);

        var result = new JObject { ["data"] = JObject.FromObject(pageListModel) };

        return Ok(result);
    }

    [HttpPost]
    public async Task<WorkflowResponse> Update([FromBody] WorkflowInput request)
    {
        var response = await InvokeAsync<WorkflowResponse>(async () =>
        {
            if (request.Fields["wfstep"] is not JObject wfStepObj)
            {
                var rs = new WorkflowResponse
                {
                    error_code = "ERR_INVALID_FORMAT",
                    error_message = "wfstep is invalid format!",
                };
                return rs;
            }

            var workflowId = wfStepObj["WorkflowId"]?.ToString();
            var stepCode = wfStepObj["StepCode"]?.ToString();

            if (string.IsNullOrEmpty(workflowId))
            {
                var rs = new WorkflowResponse
                {
                    error_code = "ERR_MISSING_FIELD",
                    error_message = "WorkflowId is missing!",
                };
                return rs;
            }

            if (string.IsNullOrEmpty(stepCode))
            {
                var rs = new WorkflowResponse
                {
                    error_code = "ERR_MISSING_FIELD",
                    error_message = "StepCode is missing!",
                };
                return rs;
            }

            var wfStep = await _workflowStepService.GetByWorkflowIdAndStepCode(
                workflowId,
                stepCode
            );
            if (wfStep == null)
            {
                var rs = new WorkflowResponse
                {
                    error_code = "ERR_NOT_EXISTS_DATA",
                    error_message = "Can not find WorkflowStep to update!",
                };
                return rs;
            }

            foreach (var kv in wfStepObj)
            {
                var prop = wfStep.GetType().GetProperty(kv.Key);
                if (prop != null && kv.Value != null)
                {
                    try
                    {
                        object convertedValue;

                        if (prop.PropertyType == typeof(string) && kv.Value is JObject jObj)
                        {
                            convertedValue = JTokenHelper.NormalizeToken(jObj);
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
            await _workflowStepService.UpdateAsync(wfStep);
            var result = new WorkflowResponse();
            var dict = JObject.FromObject(wfStep).ToObject<Dictionary<string, object>>();
            result.data = dict;
            return result;
        });
        return response;
    }

    [HttpPost]
    public async Task<WorkflowResponse> Insert([FromBody] WorkflowInput request)
    {
        var response = await InvokeAsync<WorkflowResponse>(async () =>
        {
            if (request.Fields["wfstep"] is not JObject wfStepObj)
            {
                var rs = new WorkflowResponse
                {
                    error_code = "ERR_INVALID_FORMAT",
                    error_message = "wfstep is invalid format!",
                };
                return rs;
            }

            var workflowId = wfStepObj["WorkflowId"]?.ToString();
            var stepCode = wfStepObj["StepCode"]?.ToString();
            var stepOrder = wfStepObj["StepOrder"]?.ToObject<int>() ?? 0;

            if (string.IsNullOrEmpty(workflowId))
            {
                var rs = new WorkflowResponse
                {
                    error_code = "ERR_MISSING_FIELD",
                    error_message = "WorkflowId is missing!",
                };
                return rs;
            }

            if (string.IsNullOrEmpty(stepCode))
            {
                var rs = new WorkflowResponse
                {
                    error_code = "ERR_MISSING_FIELD",
                    error_message = "StepCode is missing!",
                };
                return rs;
            }

            if (stepOrder == 0)
            {
                var rs = new WorkflowResponse
                {
                    error_code = "ERR_MISSING_FIELD",
                    error_message = "StepOrder is missing!",
                };
                return rs;
            }

            var checkExists = await _workflowStepService.GetByWorkflowIdAndStepCodeAndStepOrder(
                workflowId,
                stepCode,
                stepOrder
            );
            if (checkExists != null)
            {
                var rs = new WorkflowResponse
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
                SendingTemplate = JTokenHelper.NormalizeToken(wfStepObj["SendingTemplate"]),
                MappingResponse = JTokenHelper.NormalizeToken(wfStepObj["MappingResponse"]),
                SubSendingTemplate = JTokenHelper.NormalizeToken(wfStepObj["SubSendingTemplate"]),
                SendingCondition = JTokenHelper.NormalizeToken(wfStepObj["SendingCondition"]),
                StepTimeout = wfStepObj["StepTimeout"]?.Value<long?>() ?? 60000,
                IsReverse = wfStepObj["IsReverse"]?.ToObject<bool>() ?? false,
                ShouldAwaitStep = wfStepObj["ShouldAwaitStep"]?.ToObject<bool>() ?? true,
                ProcessingNumber = (Client.Enums.ProcessNumber)(
                    wfStepObj["ProcessingNumber"]?.ToObject<int>() ?? 0
                ),
            };
            await _workflowStepService.AddAsync(wfStep);
            var result = new WorkflowResponse();
            var dict = JObject.FromObject(wfStep).ToObject<Dictionary<string, object>>();
            result.data = dict;
            return result;
        });
        return response;
    }

    [HttpPost]
    public async Task<WorkflowResponse> Delete([FromBody] WorkflowInput request)
    {
        var response = await InvokeAsync<WorkflowResponse>(async () =>
        {
            if (request.Fields["listWfStep"] is not JArray listWfStep)
            {
                var rs = new WorkflowResponse
                {
                    error_code = "ERR_INVALID_FORMAT",
                    error_message = "wfstep is invalid format!",
                };
                return rs;
            }
            if (listWfStep.Count > 0)
            {
                var deletedItems = new List<Dictionary<string, object>>();
                foreach (var item in listWfStep)
                {
                    string value = (string)item;
                    var parts = value.Split('#');

                    string workflowId = parts[0];
                    string stepCode = parts.Length > 1 ? parts[1] : null;
                    int stepOrder = parts.Length > 2 ? parts[2].ToInt() : 0;

                    if (string.IsNullOrEmpty(workflowId))
                    {
                        var rs = new WorkflowResponse
                        {
                            error_code = "ERR_MISSING_FIELD",
                            error_message = "WorkflowId is missing!",
                        };
                        return rs;
                    }

                    if (string.IsNullOrEmpty(stepCode))
                    {
                        var rs = new WorkflowResponse
                        {
                            error_code = "ERR_MISSING_FIELD",
                            error_message = "StepCode is missing!",
                        };
                        return rs;
                    }

                    if (stepOrder == 0)
                    {
                        var rs = new WorkflowResponse
                        {
                            error_code = "ERR_MISSING_FIELD",
                            error_message = "StepOrder is missing!",
                        };
                        return rs;
                    }

                    var wfStep = await _workflowStepService.GetByWorkflowIdAndStepCodeAndStepOrder(
                        workflowId,
                        stepCode,
                        stepOrder
                    );
                    if (wfStep == null)
                    {
                        var rs = new WorkflowResponse
                        {
                            error_code = "ERR_NOT_EXISTS_DATA",
                            error_message = "Can not find WorkflowStep to delete!",
                        };
                        return rs;
                    }

                    await _workflowStepService.DeleteAsync(wfStep);
                    var dict = JObject.FromObject(wfStep).ToObject<Dictionary<string, object>>();
                    deletedItems.Add(dict);
                }
                return new WorkflowResponse
                {
                    data = new Dictionary<string, object> { ["deletedItems"] = deletedItems },
                };
            }
            else
            {
                var rs = new WorkflowResponse
                {
                    error_code = "ERR_EMPTY_DATA",
                    error_message = "listWfStep is empty!",
                };
                return rs;
            }
        });
        return response;
    }
}

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using O24OpenAPI.Client.Workflow;
using O24OpenAPI.Core.Domain;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.WFO.Domain;
using O24OpenAPI.WFO.Models;
using O24OpenAPI.WFO.Services.Interfaces;
using WorkflowInput = O24OpenAPI.WFO.Models.WorkflowInput;

namespace O24OpenAPI.WFO.Controllers;

public class WorkflowDefController(IWorkflowDefService workflowDefService) : WFOBaseController
{
    private readonly IWorkflowDefService _workflowDefService = workflowDefService;

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

        var r = await _workflowDefService.SimpleSearch(model);

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
            if (request.Fields["wfdef"] is not JObject wfDefObj)
            {
                var rs = new WorkflowResponse
                {
                    error_code = "ERR_INVALID_FORMAT",
                    error_message = "wfdef is invalid format!",
                };
                return rs;
            }

            var workflowId = wfDefObj["WorkflowId"]?.ToString();
            var channelId = wfDefObj["ChannelId"]?.ToString();

            if (string.IsNullOrEmpty(workflowId))
            {
                var rs = new WorkflowResponse
                {
                    error_code = "ERR_MISSING_FIELD",
                    error_message = "WorkflowId is missing!",
                };
                return rs;
            }
            if (string.IsNullOrEmpty(channelId))
            {
                var rs = new WorkflowResponse
                {
                    error_code = "ERR_MISSING_FIELD",
                    error_message = "ChannelId is missing!",
                };
                return rs;
            }

            var wfDef = await _workflowDefService.GetByWorkflowIdAsync(workflowId, channelId);
            if (wfDef == null)
            {
                var rs = new WorkflowResponse
                {
                    error_code = "ERR_NOT_EXISTS_DATA",
                    error_message = "Can not find WorkflowDef to update!",
                };
                return rs;
            }

            foreach (var kv in wfDefObj)
            {
                var prop = wfDef.GetType().GetProperty(kv.Key);
                if (prop != null && kv.Value != null)
                {
                    try
                    {
                        object convertedValue;

                        if (prop.PropertyType == typeof(string) && kv.Value is JObject jObj)
                        {
                            convertedValue = jObj.ToString(Newtonsoft.Json.Formatting.None);
                        }
                        else
                        {
                            convertedValue = Convert.ChangeType(kv.Value, prop.PropertyType);
                        }

                        prop.SetValue(wfDef, convertedValue);
                    }
                    catch { }
                }
            }
            await _workflowDefService.UpdateAsync(wfDef);
            var result = new WorkflowResponse();
            var dict = JObject.FromObject(wfDef).ToObject<Dictionary<string, object>>();
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
            if (request.Fields["wfdef"] is not JObject wfDefObj)
            {
                var rs = new WorkflowResponse
                {
                    error_code = "ERR_INVALID_FORMAT",
                    error_message = "wfdef is invalid format!",
                };
                return rs;
            }

            var workflowId = wfDefObj["WorkflowId"]?.ToString();
            var channelId = wfDefObj["ChannelId"]?.ToString();

            if (string.IsNullOrEmpty(workflowId))
            {
                var rs = new WorkflowResponse
                {
                    error_code = "ERR_MISSING_FIELD",
                    error_message = "WorkflowId is missing!",
                };
                return rs;
            }
            if (string.IsNullOrEmpty(channelId))
            {
                var rs = new WorkflowResponse
                {
                    error_code = "ERR_MISSING_FIELD",
                    error_message = "ChannelId is missing!",
                };
                return rs;
            }

            var checkExists = await _workflowDefService.GetByWorkflowIdAsync(workflowId, channelId);
            if (checkExists != null)
            {
                var rs = new WorkflowResponse
                {
                    error_code = "ERR_EXISTS_DATA",
                    error_message = "This WorkflowDef already exists!",
                };
                return rs;
            }

            WorkflowDef wfDef = new()
            {
                WorkflowId = workflowId,
                ChannelId = channelId,
                WorkflowName = wfDefObj["WorkflowName"]?.ToString() ?? "",
                Description = wfDefObj["Description"]?.ToString() ?? "",
                Status = wfDefObj["Status"]?.ToObject<bool>() ?? true,
                IsReverse = wfDefObj["IsReverse"]?.ToObject<bool>() ?? true,
                Timeout = wfDefObj["Timeout"]?.Value<long?>() ?? 60000,
                WorkflowEvent = wfDefObj["WorkflowEvent"]?.ToString() ?? "",
                TemplateResponse =
                    wfDefObj["TemplateResponse"]?.ToString(Newtonsoft.Json.Formatting.None) ?? "",
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow,
            };
            await _workflowDefService.AddAsync(wfDef);
            var result = new WorkflowResponse();
            var dict = JObject.FromObject(wfDef).ToObject<Dictionary<string, object>>();
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
            if (request.Fields["listWfDef"] is not JArray listWfDef)
            {
                var rs = new WorkflowResponse
                {
                    error_code = "ERR_INVALID_FORMAT",
                    error_message = "wfdef is invalid format!",
                };
                return rs;
            }
            if (listWfDef.Count > 0)
            {
                var deletedItems = new List<Dictionary<string, object>>();
                foreach (var item in listWfDef)
                {
                    string value = (string)item;
                    var parts = value.Split('#');

                    string workflowId = parts[0];
                    string channelId = parts.Length > 1 ? parts[1] : null;

                    if (string.IsNullOrEmpty(workflowId))
                    {
                        var rs = new WorkflowResponse
                        {
                            error_code = "ERR_MISSING_FIELD",
                            error_message = "WorkflowId is missing!",
                        };
                        return rs;
                    }
                    if (string.IsNullOrEmpty(channelId))
                    {
                        var rs = new WorkflowResponse
                        {
                            error_code = "ERR_MISSING_FIELD",
                            error_message = "ChannelId is missing!",
                        };
                        return rs;
                    }

                    var wfDef = await _workflowDefService.GetByWorkflowIdAsync(
                        workflowId,
                        channelId
                    );
                    if (wfDef == null)
                    {
                        var rs = new WorkflowResponse
                        {
                            error_code = "ERR_NOT_EXISTS_DATA",
                            error_message = "Can not find WorkflowDef to delete!",
                        };
                        return rs;
                    }

                    await _workflowDefService.DeleteAsync(wfDef);
                    var dict = JObject.FromObject(wfDef).ToObject<Dictionary<string, object>>();
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
                    error_message = "listWfDef is empty!",
                };
                return rs;
            }
        });
        return response;
    }
}

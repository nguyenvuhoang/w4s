using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using O24OpenAPI.Client.Workflow;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.WFO.API.Application.Features.Worklfows;
using O24OpenAPI.WFO.Domain.AggregateModels.WorkflowAggregate;
using System.Reflection;
using WorkflowInput = O24OpenAPI.WFO.API.Application.Models.WorkflowInput;

namespace O24OpenAPI.WFO.API.Controllers;

public class WorkflowDefController(
    ISearchWorkflowHandler searchWorkflowHandler,
    IWorkflowDefRepository workflowDefRepository
) : WFOBaseController
{
    [HttpPost]
    public async Task<IActionResult> SimpleSearch([FromBody] SimpleSearchModel request)
    {
        string searchText = request.SearchText;
        int pageIndex = request.PageIndex;
        int pageSize = request.PageSize;

        SimpleSearchModel model = new()
        {
            SearchText = searchText,
            PageIndex = pageIndex.ToInt(),
            PageSize = pageSize.ToInt(),
        };

        WorkflowResponse response = await searchWorkflowHandler.SimpleSearch(model);

        return Ok(response);
    }

    [HttpPost]
    public async Task<WorkflowResponse> Update([FromBody] WorkflowInput request)
    {
        WorkflowResponse response = await InvokeAsync<WorkflowResponse>(async () =>
        {
            if (request.Fields["wfdef"] is not JObject wfDefObj)
            {
                WorkflowResponse rs = new()
                {
                    error_code = "ERR_INVALID_FORMAT",
                    error_message = "wfdef is invalid format!",
                };
                return rs;
            }

            string workflowId = wfDefObj["WorkflowId"]?.ToString();
            string channelId = wfDefObj["ChannelId"]?.ToString();

            if (string.IsNullOrEmpty(workflowId))
            {
                WorkflowResponse rs = new()
                {
                    error_code = "ERR_MISSING_FIELD",
                    error_message = "WorkflowId is missing!",
                };
                return rs;
            }
            if (string.IsNullOrEmpty(channelId))
            {
                WorkflowResponse rs = new()
                {
                    error_code = "ERR_MISSING_FIELD",
                    error_message = "ChannelId is missing!",
                };
                return rs;
            }

            WorkflowDef wfDef = await workflowDefRepository.GetByWorkflowIdAndChannelIdAsync(
                workflowId,
                channelId
            );
            if (wfDef == null)
            {
                WorkflowResponse rs = new()
                {
                    error_code = "ERR_NOT_EXISTS_DATA",
                    error_message = "Can not find WorkflowDef to update!",
                };
                return rs;
            }

            foreach (KeyValuePair<string, JToken> kv in wfDefObj)
            {
                PropertyInfo prop = wfDef.GetType().GetProperty(kv.Key);
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
            await workflowDefRepository.Update(wfDef);
            WorkflowResponse result = new();
            Dictionary<string, object> dict = JObject
                .FromObject(wfDef)
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
            if (request.Fields["wfdef"] is not JObject wfDefObj)
            {
                WorkflowResponse rs = new()
                {
                    error_code = "ERR_INVALID_FORMAT",
                    error_message = "wfdef is invalid format!",
                };
                return rs;
            }

            string workflowId = wfDefObj["WorkflowId"]?.ToString();
            string channelId = wfDefObj["ChannelId"]?.ToString();

            if (string.IsNullOrEmpty(workflowId))
            {
                WorkflowResponse rs = new()
                {
                    error_code = "ERR_MISSING_FIELD",
                    error_message = "WorkflowId is missing!",
                };
                return rs;
            }
            if (string.IsNullOrEmpty(channelId))
            {
                WorkflowResponse rs = new()
                {
                    error_code = "ERR_MISSING_FIELD",
                    error_message = "ChannelId is missing!",
                };
                return rs;
            }

            WorkflowDef checkExists = await workflowDefRepository.GetByWorkflowIdAndChannelIdAsync(
                workflowId,
                channelId
            );
            if (checkExists != null)
            {
                WorkflowResponse rs = new()
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
            await workflowDefRepository.InsertAsync(wfDef);
            WorkflowResponse result = new();
            Dictionary<string, object> dict = JObject
                .FromObject(wfDef)
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
            if (request.Fields["listWfDef"] is not JArray listWfDef)
            {
                WorkflowResponse rs = new()
                {
                    error_code = "ERR_INVALID_FORMAT",
                    error_message = "wfdef is invalid format!",
                };
                return rs;
            }
            if (listWfDef.Count > 0)
            {
                List<Dictionary<string, object>> deletedItems = [];
                foreach (JToken item in listWfDef)
                {
                    string value = (string)item;
                    string[] parts = value.Split('#');

                    string workflowId = parts[0];
                    string channelId = parts.Length > 1 ? parts[1] : null;

                    if (string.IsNullOrEmpty(workflowId))
                    {
                        WorkflowResponse rs = new()
                        {
                            error_code = "ERR_MISSING_FIELD",
                            error_message = "WorkflowId is missing!",
                        };
                        return rs;
                    }
                    if (string.IsNullOrEmpty(channelId))
                    {
                        WorkflowResponse rs = new()
                        {
                            error_code = "ERR_MISSING_FIELD",
                            error_message = "ChannelId is missing!",
                        };
                        return rs;
                    }

                    WorkflowDef wfDef =
                        await workflowDefRepository.GetByWorkflowIdAndChannelIdAsync(
                            workflowId,
                            channelId
                        );
                    if (wfDef == null)
                    {
                        WorkflowResponse rs = new()
                        {
                            error_code = "ERR_NOT_EXISTS_DATA",
                            error_message = "Can not find WorkflowDef to delete!",
                        };
                        return rs;
                    }

                    await workflowDefRepository.Delete(wfDef);
                    Dictionary<string, object> dict = JObject
                        .FromObject(wfDef)
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
                    error_message = "listWfDef is empty!",
                };
                return rs;
            }
        });
        return response;
    }
}

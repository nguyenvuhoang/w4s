using Newtonsoft.Json;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Web.CMS.Services.Interfaces;

namespace O24OpenAPI.Web.CMS.Services.Services;

/// <summary>
/// The web portal service class
/// </summary>
/// <seealso cref="IWebPortalService"/>
public class WebPortalService : IWebPortalService
{
    /// <summary>
    /// The workflow
    /// </summary>
    private readonly IWorkflowService _workflow;

    /// <summary>
    /// The learn api
    /// </summary>
    private readonly ILearnApiService _learnApi;

    /// <summary>
    /// Initializes a new instance of the <see cref="WebPortalService"/> class
    /// </summary>
    /// <param name="workflow">The workflow</param>
    /// <param name="learnApi">The learn api</param>
    public WebPortalService(IWorkflowService workflow, ILearnApiService learnApi)
    {
        _workflow = workflow;
        _learnApi = learnApi;
    }

    /// <summary>
    /// Processes the request
    /// </summary>
    /// <param name="request">The request</param>
    /// <returns>A task containing the web portal response</returns>
    public async Task<WebPortalResponse> Process(WebPortalRequest request)
    {
        switch (request.Type)
        {
            case "learnapi":
                return await LearnApi(request);

            case "workflow":
                return await Workflow(request);

            default:
                return new WebPortalResponse()
                {
                    Status = false,
                    Message = $@"Not found type {request.Type}",
                };
        }
    }

    private async Task<WebPortalResponse> Workflow(WebPortalRequest request)
    {
        try
        {
            switch (request.Function)
            {
                case "add":
                    request.Data = await _workflow.Insert(
                        JsonConvert.DeserializeObject<WorkflowStep>(
                            JsonConvert.SerializeObject(request.Data)
                        )
                    );
                    break;
                case "searchAll":
                    request.Data = await _workflow.GetAll();
                    break;
                case "searchById":
                    request.Data = await _workflow.GetById((int)request.Data);
                    break;
                case "update":
                    request.Data = await _workflow.Update(
                        JsonConvert.DeserializeObject<WorkflowStep>(
                            JsonConvert.SerializeObject(request.Data)
                        )
                    );
                    break;
                case "deleteById":
                    request.Data = await _workflow.DeleteById((int)request.Data);
                    break;
                default:
                    return new WebPortalResponse()
                    {
                        Status = false,
                        Message = $@"Not found function {request.Function}",
                    };
            }

            return new WebPortalResponse() { Data = request };
        }
        catch (Exception e)
        {
            return new WebPortalResponse() { Status = false, Message = $@"Error {e.Message}" };
        }
    }

    private async Task<WebPortalResponse> LearnApi(WebPortalRequest request)
    {
        try
        {
            switch (request.Function)
            {
                case "add":
                    request.Data = await _learnApi.Insert(
                        JsonConvert.DeserializeObject<LearnApi>(
                            JsonConvert.SerializeObject(request.Data)
                        )
                    );
                    break;
                case "searchAll":
                    var text = request.Data.ToSerialize();
                    var model = System.Text.Json.JsonSerializer.Deserialize<SimpleSearchModel>(
                        text
                    );
                    request.Data = await _learnApi.GetAll(model);

                    break;
                case "searchById":
                    int id = Convert.ToInt32(request.Data);
                    request.Data = await _learnApi.GetById(id);
                    break;
                case "update":
                    request.Data = await _learnApi.Update(
                        JsonConvert.DeserializeObject<LearnApi>(
                            JsonConvert.SerializeObject(request.Data)
                        )
                    );
                    break;
                case "deleteById":
                    request.Data = await _learnApi.DeleteById((int)request.Data);
                    break;
                default:
                    return new WebPortalResponse()
                    {
                        Status = false,
                        Message = $@"Not found function {request.Function}",
                    };
            }

            return new WebPortalResponse() { Data = request };
        }
        catch (Exception e)
        {
            return new WebPortalResponse() { Status = false, Message = $@"Error {e.Message}" };
        }
    }
}

/// <summary>
/// The web portal request class
/// </summary>
public class WebPortalRequest
{
    /// <summary>
    /// Gets or sets the value of the type
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Gets or sets the value of the function
    /// </summary>
    public string Function { get; set; }

    /// <summary>
    /// Gets or sets the value of the data
    /// </summary>
    public object Data { get; set; }
}

/// <summary>
/// The web portal response class
/// </summary>
public class WebPortalResponse
{
    /// <summary>
    /// Gets or sets the value of the status
    /// </summary>
    public bool Status { get; set; } = true;

    /// <summary>
    /// Gets or sets the value of the message
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Gets or sets the value of the data
    /// </summary>
    public WebPortalRequest Data { get; set; }
}

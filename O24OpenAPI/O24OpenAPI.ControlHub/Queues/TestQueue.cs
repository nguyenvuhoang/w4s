using Newtonsoft.Json.Linq;
using O24OpenAPI.O24OpenAPIClient.Scheme.Workflow;
using O24OpenAPI.Web.Framework.Models;
using O24OpenAPI.Web.Framework.Models.O24OpenAPI;
using O24OpenAPI.Web.Framework.Services.Queue;

namespace O24OpenAPI.ControlHub.Queues;

/// <summary>
/// The test queue class
/// </summary>
/// <seealso cref="BaseQueue"/>
public class TestQueue : BaseQueue
{
    /// <summary>
    /// Processes the workflow scheme
    /// </summary>
    /// <param name="workflowScheme">The workflow scheme</param>
    /// <returns>A task containing the wf scheme</returns>
    public async Task<WFScheme> Process(WFScheme workflowScheme)
    {
        var model = await workflowScheme.ToModel<TestModel>();
        return await Invoke<BaseTransactionModel>(
            workflowScheme,
            async () =>
            {
                // Do something
                Console.WriteLine("Processing workflow scheme");
                var jsonObject = new JObject();
                jsonObject["hello1"] = "hello from Logger";
                jsonObject["data_test"] = "Logger saw :" + model.DataTest;
                return await Task.FromResult(jsonObject);
            }
        );
    }
}

/// <summary>
/// The test model class
/// </summary>
/// <seealso cref="BaseTransactionModel"/>
public class TestModel : BaseTransactionModel
{
    /// <summary>
    /// Gets or sets the value of the data test
    /// </summary>
    public string DataTest { get; set; }
}

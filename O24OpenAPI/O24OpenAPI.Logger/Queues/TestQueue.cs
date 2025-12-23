using System.Text.Json.Nodes;
using O24OpenAPI.Client.Scheme.Workflow;
using O24OpenAPI.Core.Helper;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.Framework.Services.Queue;

namespace O24OpenAPI.Logger.Queues;

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
        var model = await workflowScheme.ToModel<TestModel>(
            SerializerOptions.JsonSerializerOptions
        );
        return await Invoke<BaseTransactionModel>(
            workflowScheme,
            async () =>
            {
                // Do something
                Console.WriteLine("Processing workflow scheme");
                var jsonObject = new JsonObject
                {
                    ["hello1"] = "hello from Logger",
                    ["data_test"] = "Logger saw :" + model.DataTest,
                    ["device"] = model.Device,
                };
                var arr = new JsonArray(jsonObject);
                return await Task.FromResult(arr);
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

    /// <summary>
    /// Gets or sets the value of the device
    /// </summary>
    public string Device { get; set; }
}

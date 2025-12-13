using Newtonsoft.Json.Linq;
using O24OpenAPI.O24OpenAPIClient.Scheme.Workflow;
using O24OpenAPI.Web.Framework.Models;
using O24OpenAPI.Web.Framework.Models.O24OpenAPI;
using O24OpenAPI.Web.Framework.Services.Queue;

namespace O24OpenAPI.Sample.Queues;

public class TestQueue : BaseQueue
{
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

public class TestModel : BaseTransactionModel
{
    public string DataTest { get; set; }
}

using System.Text.Json.Nodes;
using O24OpenAPI.GrpcContracts.GrpcClientServices.LOG;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Services.Queue;
using ILogger = O24OpenAPI.Framework.Services.Logging.ILogger;

namespace O24OpenAPI.Web.CMS.Queues;

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
                var jsonObject = new JsonObject
                {
                    ["hello"] = "hello " + model.Name, //JObject.FromObject(model);
                };
                var logger = EngineContext.Current.Resolve<ILogger>();
                _ = logger.LogInfoAsync("hello logger from cms");
                var logGrpcClient = EngineContext.Current.Resolve<ILOGGrpcClientService>();
                var testResult = await logGrpcClient.TestAsync("Linh CMS");
                jsonObject["testGrpc"] = testResult;
                //  throw new O24OpenAPIException("hello linh test lỗi nè");
                return await Task.FromResult(jsonObject);
            }
        );
    }
}

public class TestModel : BaseTransactionModel
{
    public string Name { get; set; }
}

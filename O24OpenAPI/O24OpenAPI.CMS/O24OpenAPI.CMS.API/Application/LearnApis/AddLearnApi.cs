using System.Text.Json.Serialization;
using LinKit.Core.Cqrs;
using LinKit.Core.Endpoints;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.CMS.Domain.AggregateModels.LearnApiAggregate;

namespace O24OpenAPI.CMS.API.Application.LearnApis;

[ApiEndpoint(ApiMethod.Post, "api/learnapis/create", MediatorKey = MediatorKey.CMS)]
public class AddLearnApiCommand : ICommand<bool>
{
    [JsonPropertyName("learn_api_id")]
    public string LearnApiId { get; set; }

    [JsonPropertyName("learn_api_name")]
    public string LearnApiName { get; set; }

    [JsonPropertyName("learn_api_data")]
    public string LearnApiData { get; set; }

    [JsonPropertyName("learn_api_node_data")]
    public string LearnApiNodeData { get; set; }

    [JsonPropertyName("learn_api_method")]
    public string LearnApiMethod { get; set; }

    [JsonPropertyName("learn_api_header")]
    public string LearnApiHeader { get; set; }

    [JsonPropertyName("learn_api_mapping")]
    public string LearnApiMapping { get; set; }

    [JsonPropertyName("is_cache")]
    public bool IsCache { get; set; }

    [JsonPropertyName("key_read_data")]
    public string KeyReadData { get; set; }

    [JsonPropertyName("learn_api_id_clear")]
    public string LearnApiIdClear { get; set; }

    [JsonPropertyName("channel")]
    public string Channel { get; set; }

    [JsonPropertyName("learn_api_mapping_response")]
    public string LearnApiMappingResponse { get; set; }

    [JsonPropertyName("full_interface_name")]
    public string FullInterfaceName { get; set; }

    [JsonPropertyName("method_name")]
    public string MethodName { get; set; }

    [JsonPropertyName("uri")]
    public string URI { get; set; } = string.Empty;
}

[CqrsHandler]
public class AddLearnApiHandler(ILearnApiRepository learnApiRepository)
    : ICommandHandler<AddLearnApiCommand, bool>
{
    public async Task<bool> HandleAsync(
        AddLearnApiCommand request,
        CancellationToken cancellationToken = default
    )
    {
        LearnApi entity = request.ToLearnApi();
        await learnApiRepository.InsertAsync(entity);
        return true;
    }
}

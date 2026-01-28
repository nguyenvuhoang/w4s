using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.NCH.Domain.AggregatesModel.ZaloAggregate;
using O24OpenAPI.NCH.Infrastructure.Configurations;
using System.Net.Http.Headers;
using System.Text.Json;

namespace O24OpenAPI.NCH.API.Application.Features.Zalo;

public class GetZnsTemplateListCommand : BaseTransactionModel, ICommand<GetZnsTemplateListResponseModel>
{
    // Paging
    public int Offset { get; set; } = 0;
    public int Limit { get; set; } = 100;

    // Filter
    // status: theo docs API (vd: 2) - bạn đang dùng status=2
    public int? Status { get; set; } = 2;

    // filterPreset: 0=all templates of OA, 1=templates created by app of access_token
    public int? FilterPreset { get; set; } = 1;

    // Optional: nếu muốn override token theo từng request
    public string? AccessToken { get; set; }
}

public class GetZnsTemplateListResponseModel : BaseO24OpenAPIModel
{
    public int Error { get; set; }
    public string Message { get; set; } = string.Empty;

    public List<ZnsTemplateListItem> Data { get; set; } = new();

    public ZnsTemplateListMetadata Metadata { get; set; } = new();
}

public class ZnsTemplateListItem
{
    public long TemplateId { get; set; }
    public string TemplateName { get; set; } = string.Empty;

    // Zalo trả createdTime dạng unix time (đôi khi 0)
    public long CreatedTime { get; set; }

    public string Status { get; set; } = string.Empty;          // e.g. PENDING_REVIEW
    public string TemplateQuality { get; set; } = string.Empty;  // e.g. UNDEFINED
}

public class ZnsTemplateListMetadata
{
    public int Total { get; set; }
}


[CqrsHandler]
public class GetZnsTemplateListHandler(
    IHttpClientFactory httpClientFactory,
    IZaloZNSTemplateRepository templateRepo,
    O24NCHSetting zaloConfig
) : ICommandHandler<GetZnsTemplateListCommand, GetZnsTemplateListResponseModel>
{
    // TODO: đặt đúng step code của bạn
    [WorkflowStep(WorkflowStepCode.NCH.WF_STEP_NCH_GET_ZBS_TEMPLATE_LIST)]
    public async Task<GetZnsTemplateListResponseModel> HandleAsync(GetZnsTemplateListCommand request, CancellationToken ct)
    {
        // 1) Resolve access token
        var accessToken = (request.AccessToken ?? GetAccessTokenFromConfig(zaloConfig))?.Trim();
        if (string.IsNullOrWhiteSpace(accessToken))
        {
            return new GetZnsTemplateListResponseModel
            {
                Error = -1,
                Message = "Zalo access_token is missing."
            };
        }

        // 2) Build URL
        var offset = request.Offset < 0 ? 0 : request.Offset;
        var limit = request.Limit <= 0 ? 100 : request.Limit;

        var qs = new List<string>
        {
            $"offset={offset}",
            $"limit={limit}"
        };

        if (request.Status.HasValue) qs.Add($"status={request.Status.Value}");
        if (request.FilterPreset.HasValue) qs.Add($"filterPreset={request.FilterPreset.Value}");

        var url = $"template/all?{string.Join("&", qs)}";

        // 3) Call Zalo
        var client = httpClientFactory.CreateClient();

        // BaseAddress: nếu bạn đã cấu hình sẵn ở HttpClientFactory thì bỏ dòng này
        var baseUrl = GetBaseUrlFromConfig(zaloConfig) ?? "https://business.openapi.zalo.me/";
        client.BaseAddress = new Uri(baseUrl.EndsWith("/") ? baseUrl : baseUrl + "/");

        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        // Zalo business API dùng header access_token
        client.DefaultRequestHeaders.Remove("access_token");
        client.DefaultRequestHeaders.Add("access_token", accessToken);

        using var resp = await client.GetAsync(url, ct);
        var raw = await resp.Content.ReadAsStringAsync(ct);

        // 4) Parse JSON (kể cả khi HTTP != 200, Zalo vẫn có thể trả body chuẩn)
        var result = DeserializeZalo<GetZnsTemplateListResponseModel>(raw)
                     ?? new GetZnsTemplateListResponseModel
                     {
                         Error = (int)resp.StatusCode,
                         Message = $"Failed to call Zalo API. HTTP {(int)resp.StatusCode}",
                         Data = [],
                         Metadata = new()
                     };

        //if (result.Error == 0 && result.Data.Count > 0)
        //{
        //    await UpsertTemplatesAsync(templateRepo, result.Data, ct);
        //}

        return result;
    }

    private static T? DeserializeZalo<T>(string json)
    {
        try
        {
            return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch
        {
            return default;
        }
    }

    //private static async Task UpsertTemplatesAsync(
    //    IZaloZNSTemplateRepository templateRepo,
    //    List<ZnsTemplateListItem> items,
    //    CancellationToken ct)
    //{

    //    var entity = await templateRepo.FindByTemplateIdAsync(item.TemplateId);



    //    foreach (var item in items)
    //    {
    //        // Ví dụ minimal: nếu repo có method nào đó thì dùng.
    //        // await templateRepo.UpsertAsync(..., ct);
    //        await Task.CompletedTask;
    //    }

    //    await Task.CompletedTask;
    //}

    private static string? GetBaseUrlFromConfig(O24NCHSetting config)
    {
        // TODO: đổi theo cấu trúc config thật của bạn
        // ví dụ: config.Zalo?.BusinessApiBaseUrl
        return null;
    }

    private static string? GetAccessTokenFromConfig(O24NCHSetting config)
    {
        // TODO: đổi theo cấu trúc config thật của bạn
        // ví dụ: config.Zalo?.ZnsAccessToken
        return null;
    }
}

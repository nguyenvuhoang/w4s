using LinKit.Core.Cqrs;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.NCH.Domain.AggregatesModel.ZaloAggregate;
using O24OpenAPI.NCH.Infrastructure.Configurations;
using System.Text;
using System.Text.Json;

namespace O24OpenAPI.NCH.API.Application.Features.Zalo
{
    public class CreateZnsTemplateCommand : BaseTransactionModel, ICommand<ZaloZNSTemplateResponseModel>
    {
        public string OaId { get; set; } = default!;
        public string TemplateCode { get; set; } = default!;
        public string TemplateName { get; set; } = default!;
        public string TemplateType { get; set; } = "OTP";
        // raw template definition theo Zalo spec
        public object TemplateDefinition { get; set; } = default!;
    }

    public class ZaloZNSTemplateResponseModel : BaseO24OpenAPIModel
    {
        public string OaId { get; set; } = default!;
        public string TemplateId { get; set; } = default!;
        public string TemplateCode { get; set; } = default!;

        public string TemplateName { get; set; } = default!;
        public string TemplateType { get; set; } = default!;

        public string Status { get; set; } = default!;
        public string? RejectReason { get; set; }

        public string RequestPayload { get; set; } = default!;
        public string? ResponsePayload { get; set; }
        public DateTime CreatedOnUtc { get; set; } = DateTime.UtcNow;
    }

    [CqrsHandler]
    public class CreateZnsTemplateHandler(
        IHttpClientFactory httpClientFactory,
        IZaloOATokenRepository tokenRepo,
        IZaloZNSTemplateRepository templateRepo,
        O24NCHSetting zaloConfig
    ) : ICommandHandler<CreateZnsTemplateCommand, ZaloZNSTemplateResponseModel>
    {
        [WorkflowStep(WorkflowStepCode.NCH.WF_STEP_NCH_CREATE_ZBS_TEMPLATE)]
        public async Task<ZaloZNSTemplateResponseModel> HandleAsync(CreateZnsTemplateCommand request, CancellationToken ct)
        {
            var token = await tokenRepo.GetActiveByOaIdAsync(request.OaId, ct)
                ?? throw new InvalidOperationException("No active OA token");

            var endpoint = zaloConfig.ZnsCreateTemplateEndpoint
                ?? throw new InvalidOperationException("Missing ZnsCreateTemplateEndpoint");

            var payloadJson = JsonSerializer.Serialize(request.TemplateDefinition);

            var client = httpClientFactory.CreateClient("ZaloZns");

            using var http = new HttpRequestMessage(HttpMethod.Post, endpoint);

            http.Headers.TryAddWithoutValidation("access_token", token.AccessToken);

            http.Content = new StringContent(payloadJson, Encoding.UTF8, "application/json");

            var resp = await client.SendAsync(http, ct);
            var raw = await resp.Content.ReadAsStringAsync(ct);

            if (!resp.IsSuccessStatusCode)
                throw new Exception($"Create template failed (HTTP {(int)resp.StatusCode}): {raw}");

            using var doc = JsonDocument.Parse(raw);
            var root = doc.RootElement;

            var error = root.TryGetProperty("error", out var errEl) && errEl.ValueKind == JsonValueKind.Number
                ? errEl.GetInt32()
                : -1;

            var message = root.TryGetProperty("message", out var msgEl) ? msgEl.GetString() : null;

            if (error != 0)
                throw new Exception($"Create template failed (error={error}): {message ?? raw}");

            var data = root.GetProperty("data");

            var templateId = data.GetProperty("template_id").GetString()!;
            var status = data.TryGetProperty("status", out var stEl) ? stEl.GetString() : "PENDING_REVIEW";

            var entity = new ZaloZNSTemplate
            {
                OaId = request.OaId,
                TemplateId = templateId,
                TemplateCode = request.TemplateCode,
                TemplateName = request.TemplateName,
                TemplateType = request.TemplateType, // (internal) hoặc map từ response nếu muốn
                Status = status ?? "PENDING_REVIEW",
                RejectReason = null,
                RequestPayload = payloadJson,
                ResponsePayload = raw,
                CreatedOnUtc = DateTime.UtcNow
            };

            await templateRepo.InsertAsync(entity);

            return entity.ToZaloZNSTemplateResponseModel();
        }

    }

}

using LinKit.Core.Cqrs;
using Microsoft.AspNetCore.Mvc;
using O24OpenAPI.AI.API.Application.Features;
using O24OpenAPI.AI.API.Application.Features.RAG;
using O24OpenAPI.AI.Domain.AggregatesModel.AskAggreate;
using O24OpenAPI.AI.Domain.AggregatesModel.QdrantAggreate;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Framework.Controllers;

namespace O24OpenAPI.AI.API.Controllers;

public partial class RagQdrantController([FromKeyedServices(MediatorKey.AI)] IMediator mediator) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Upsert([FromBody] UpsertPointRequest req, CancellationToken ct)
    {
        var pointId = Guid.NewGuid().ToString("N");
        var cmd = new UpsertPointCommand
        {
            TransactionCode = Guid.NewGuid().ToString("N"),
            Language = req.Language,
            PointId = req.PointId ?? pointId,
            TenantId = req.TenantId,
            DocType = req.DocType,
            DocId = req.DocId,
            Title = req.Title,
            Content = req.Content,
            Extra = req.Extra,
        };
        var upsertPointResponse = await mediator.SendAsync(cmd, ct);
        return Ok(upsertPointResponse);
    }

    [HttpPost]
    public async Task<IActionResult> Search([FromBody] SearchPointRequest req, CancellationToken ct)
    {
        var searchPointCommand = new SearchPointsCommand
        {
            TransactionCode = Guid.NewGuid().ToString("N"),
            TenantId = req.TenantId,
            QueryText = req.QueryText,
            DocType = req.DocType,
            Language = req.Language ?? "en",
            TopK = req.TopK <= 0 ? 5 : req.TopK,
            Collection = string.IsNullOrWhiteSpace(req.Collection)
                ? "o24_static_knowledge_v1"
                : req.Collection!,
        };

        var result = await mediator.SendAsync(searchPointCommand, ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Ask([FromBody] AskRequest req, CancellationToken ct)
    {
        var query = new AskCommand
        {
            TransactionCode = Guid.NewGuid().ToString("N"),
            Language = req.Language ?? "vi",

            TenantId = req.TenantId,
            Question = req.Question,
            DocType = req.DocType,
            LanguageFilter = req.Language,
            TopK = req.TopK <= 0 ? 5 : req.TopK,
            MinScore = req.MinScore <= 0 ? 0.70f : req.MinScore,
            Collection = string.IsNullOrWhiteSpace(req.Collection)
                ? "o24_static_knowledge_v1"
                : req.Collection!,
        };

        var result = await mediator.SendAsync(query, ct);
        return Ok(result);
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    [DisableRequestSizeLimit]
    [RequestFormLimits(MultipartBodyLengthLimit = 1024L * 1024L * 200L)] // 200MB
    public async Task<IActionResult> Upload([FromForm] List<IFormFile> files, CancellationToken ct)
    {
        if (files == null || files.Count == 0) return BadRequest("No files.");

        var cmd = new UpsertFileCommand { Files = files };
        var upsertPointResponse = await mediator.SendAsync(cmd, ct);
        return Ok(upsertPointResponse);
    }
}

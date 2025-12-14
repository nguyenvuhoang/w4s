using LinKit.Core.Cqrs;
using Microsoft.AspNetCore.Mvc;
using O24OpenAPI.AI.API.Application.Features;
using O24OpenAPI.AI.Domain.AggregatesModel.AskAggreate;
using O24OpenAPI.AI.Domain.AggregatesModel.QdrantAggreate;
using O24OpenAPI.Web.Framework.Controllers;

namespace O24OpenAPI.AI.API.Controllers;

public partial class RagQdrantController([FromKeyedServices("AIService")] IMediator mediator) : BaseController
{
    private readonly IMediator _mediator = mediator;

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
            Extra = req.Extra
        };
        var upsertPointResponse = await _mediator.SendAsync(cmd, ct);
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
            Collection = string.IsNullOrWhiteSpace(req.Collection) ? "o24_static_knowledge_v1" : req.Collection!
        };

        var result = await _mediator.SendAsync(searchPointCommand, ct);
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
            Collection = string.IsNullOrWhiteSpace(req.Collection) ? "o24_static_knowledge_v1" : req.Collection!
        };

        var result = await _mediator.SendAsync(query, ct);
        return Ok(result);
    }
}

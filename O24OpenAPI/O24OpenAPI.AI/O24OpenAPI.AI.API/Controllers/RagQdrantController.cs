using Microsoft.AspNetCore.Mvc;
using O24OpenAPI.AI.Domain.AggregatesModel.QdrantAggreate;
using O24OpenAPI.Web.Framework.Controllers;

namespace O24OpenAPI.AI.API.Controllers;

public partial class RagQdrantController(IHttpClientFactory factory) : BaseController
{
    private readonly HttpClient _http = factory.CreateClient("qdrant");
    private const string Collection = "o24_static_knowledge_v1";

    [HttpPost]
    public async Task<IActionResult> Upsert([FromBody] QdrantUpsertRequest req, CancellationToken ct)
    {
        if (req.Embedding is null || req.Embedding.Length != 1536)
            return BadRequest("Embedding must be length 1536.");

        var pointId = Guid.NewGuid().ToString("N");

        var payload = new Dictionary<string, object>
        {
            ["tenant_id"] = req.TenantId,
            ["source_system"] = "o24ai",
            ["doc_type"] = req.DocType,
            ["language"] = req.Language,
            ["doc_id"] = req.DocId,
            ["title"] = req.Title,
            ["chunk_index"] = req.ChunkIndex,
            ["content"] = req.Content,
            ["source_uri"] = req.SourceUri,
            ["version"] = req.Version,
            ["tags"] = req.Tags ?? [],
            ["acl_roles"] = req.AclRoles ?? []
        };

        var body = new
        {
            points = new[]
            {
                new { id = pointId, vector = req.Embedding, payload }
            }
        };

        var res = await _http.PutAsJsonAsync($"/collections/{Collection}/points?wait=true", body, ct);
        var text = await res.Content.ReadAsStringAsync(ct);

        if (!res.IsSuccessStatusCode) return StatusCode((int)res.StatusCode, text);
        return Ok(new { pointId });
    }

    [HttpPost]
    public async Task<IActionResult> Search([FromBody] QdrantSearchRequest req, CancellationToken ct)
    {
        if (req.QueryEmbedding is null || req.QueryEmbedding.Length != 1536)
            return BadRequest("QueryEmbedding must be length 1536.");

        // must filters
        var must = new List<object>
        {
            new { key = "tenant_id", match = new { value = req.TenantId } }
        };

        if (!string.IsNullOrWhiteSpace(req.DocType))
            must.Add(new { key = "doc_type", match = new { value = req.DocType } });

        if (!string.IsNullOrWhiteSpace(req.Language))
            must.Add(new { key = "language", match = new { value = req.Language } });

        // ACL: user có ít nhất 1 role trong RequireAnyRole
        // Qdrant: match any values bằng match.any
        object? filter = null;
        if (req.RequireAnyRole is { Length: > 0 })
        {
            var should = new[]
            {
                new
                {
                    key = "acl_roles",
                    match = new { any = req.RequireAnyRole }
                }
            };

            filter = new { must, should };
        }
        else
        {
            filter = new { must };
        }

        var body = new
        {
            vector = req.QueryEmbedding,
            limit = req.TopK,
            with_payload = true,
            filter
        };

        var res = await _http.PostAsJsonAsync($"/collections/{Collection}/points/search", body, ct);
        var text = await res.Content.ReadAsStringAsync(ct);

        if (!res.IsSuccessStatusCode) return StatusCode((int)res.StatusCode, text);
        return Content(text, "application/json");
    }
}

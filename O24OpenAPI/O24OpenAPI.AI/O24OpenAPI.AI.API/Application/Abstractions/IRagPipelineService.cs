namespace O24OpenAPI.AI.API.Application.Abstractions
{
    public interface IRagPipelineService
    {
        Task IngestAsync(IEnumerable<(string text, string? source)> chunks, CancellationToken ct = default);
    }
}

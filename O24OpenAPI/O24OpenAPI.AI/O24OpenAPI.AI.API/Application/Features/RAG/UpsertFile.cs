using LinKit.Core.Cqrs;
using O24OpenAPI.AI.API.Application.Abstractions;
using O24OpenAPI.AI.API.Application.Utils;
using O24OpenAPI.AI.Infrastructure.Configurations;
using O24OpenAPI.Core.Configuration;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.AI.API.Application.Features.RAG;

public class UpsertFileCommand : BaseTransactionModel, ICommand<UpsertFileResponse>
{
    public List<IFormFile> Files { get; set; } = [];
}

public class UpsertFileResponse
{
    public string ErrorCode { get; set; } = "0";
    public string ErrorMessage { get; set; } = "Success";
    public int Ingested { get; set; }
    public int Files { get; set; }

};

[CqrsHandler]
public class UpsertFileCommandHandler(IRagPipelineService qdrantService) : ICommandHandler<UpsertFileCommand, UpsertFileResponse>
{
    public async Task<UpsertFileResponse> HandleAsync(
        UpsertFileCommand request,
        CancellationToken cancellationToken = default
    )
    {
        RagConfig? ragConfig = Singleton<AppSettings>.Instance?.Get<RagConfig>();

        var chunks = new List<(string text, string? source)>();
        foreach (var file in request.Files)
        {
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (ext is not (".pdf" or ".txt" or ".md")) continue;

            var tmp = Path.GetTempFileName();
            await using (var fs = System.IO.File.Create(tmp))
                await file.CopyToAsync(fs, cancellationToken);

            string raw = ext switch
            {
                ".pdf" => PdfTextExtractor.Extract(tmp),
                ".txt" or ".md" => await System.IO.File.ReadAllTextAsync(tmp, cancellationToken),
                _ => ""
            };
            System.IO.File.Delete(tmp);

            if (string.IsNullOrWhiteSpace(raw)) continue;

            foreach (var ch in TextChunker.Chunk(raw, ragConfig.ChunkSize, ragConfig.ChunkOverlap))
                chunks.Add((ch, file.FileName));
        }

        if (chunks.Count == 0)
        {
            return new UpsertFileResponse
            {
                ErrorCode = "1",
                ErrorMessage = "No extractable text. Support: .pdf, .txt, .md",
                Ingested = 0,
                Files = request.Files.Count
            };
        }

        await qdrantService.IngestAsync(chunks, cancellationToken);
        return new UpsertFileResponse
        {
            ErrorCode = "0",
            ErrorMessage = "Success",
            Ingested = chunks.Count,
            Files = request.Files.Count

        };
    }
}

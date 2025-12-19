using LinKit.Core.Cqrs;
using O24OpenAPI.AI.API.Application.Utils;
using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Framework.Models;
using Qdrant.Client;
using Qdrant.Client.Grpc;

namespace O24OpenAPI.AI.API.Application.Features
{
    public class UpsertPointCommand : BaseTransactionModel, ICommand<UpsertPointResponse>
    {
        public required string PointId { get; set; }
        public string TenantId { get; set; } = default!;
        public string DocType { get; set; } = "unknown";
        public new string Language { get; set; } = "en";
        public string DocId { get; set; } = default!;
        public string? Title { get; set; }
        public string Content { get; set; } = default!;
        public Dictionary<string, object>? Extra { get; set; }
    }

    public record UpsertPointResponse(
        string PointId,
        string Collection,
        int VectorSize,
        bool QdrantOk
    );

    [CqrsHandler]
    public class UpsertPointCommandHandler(QdrantClient qdrant)
        : ICommandHandler<UpsertPointCommand, UpsertPointResponse>
    {
        private const string Collection = "o24_static_knowledge_v1";
        private const int VectorSize = 1536;
        private readonly QdrantClient _qdrant = qdrant;

        public async Task<UpsertPointResponse> HandleAsync(
            UpsertPointCommand request,
            CancellationToken cancellationToken = default
        )
        {
            if (string.IsNullOrWhiteSpace(request.TenantId))
                throw new ArgumentException("TenantId is required.", nameof(request.TenantId));

            if (string.IsNullOrWhiteSpace(request.DocId))
                throw new ArgumentException("DocId is required.", nameof(request.DocId));

            if (string.IsNullOrWhiteSpace(request.Content))
                throw new ArgumentException("Content is required.", nameof(request.Content));

            var vector = Embedding.BuildFakeEmbedding(request.Content, VectorSize);

            if (vector.Length != VectorSize)
                throw new InvalidOperationException($"Embedding must be length {VectorSize}.");

            var pointId = string.IsNullOrWhiteSpace(request.PointId)
                ? Guid.NewGuid().ToString("N")
                : request.PointId.Trim();

            var payload = new Dictionary<string, Value>
            {
                ["tenant_id"] = request.TenantId,
                ["doc_id"] = request.DocId,
                ["doc_type"] = request.DocType,
                ["language"] = request.Language,
                ["title"] = request.Title ?? "",
                ["content"] = request.Content,
                ["source_system"] = "o24ai",
                ["tx_id"] = request.TransactionCode ?? "",
                ["user_code"] = request.CurrentUserCode ?? "",
            };

            UpdateResult? result = await _qdrant.UpsertAsync(
                collectionName: Collection,
                points:
                [
                    new PointStruct
                    {
                        Id = new PointId { Uuid = pointId },
                        Vectors = vector,
                        Payload = { payload },
                    },
                ],
                wait: true,
                cancellationToken: cancellationToken
            );
            if (result is null || result.Status != UpdateStatus.Completed)
            {
                throw new InvalidOperationException(
                    $"Qdrant upsert failed or not completed. Status={result?.Status}"
                );
            }
            return new UpsertPointResponse(
                PointId: pointId,
                Collection: Collection,
                VectorSize: vector.Length,
                QdrantOk: result?.Status == UpdateStatus.Completed
            );
        }
    }
}

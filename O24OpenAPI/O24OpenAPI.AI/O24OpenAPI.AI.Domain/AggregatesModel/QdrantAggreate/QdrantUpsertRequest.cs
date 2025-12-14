namespace O24OpenAPI.AI.Domain.AggregatesModel.QdrantAggreate
{
    public record QdrantUpsertRequest(
      string TenantId,
      string DocType,
      string Language,
      string DocId,
      string Title,
      int ChunkIndex,
      string Content,
      float[] Embedding,
      string SourceUri = null,
      string Version = null,
      string[] Tags = null,
      string[] AclRoles = null
);

    public record QdrantSearchRequest(
      string TenantId,
      float[] QueryEmbedding,
      int TopK = 5,
      string DocType = null,
      string Language = null,
      string[] RequireAnyRole = null
    );

}

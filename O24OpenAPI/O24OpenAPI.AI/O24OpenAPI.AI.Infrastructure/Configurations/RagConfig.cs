using O24OpenAPI.Core.Configuration;

namespace O24OpenAPI.AI.Infrastructure.Configurations
{
    public class RagConfig : IConfig
    {
        public int ChunkSize { get; set; } = 800;
        public int ChunkOverlap { get; set; } = 120;
        public int TopK { get; set; } = 5;
        public float MinScore { get; set; } = 0.25f; // gate cho cosine
    }
}

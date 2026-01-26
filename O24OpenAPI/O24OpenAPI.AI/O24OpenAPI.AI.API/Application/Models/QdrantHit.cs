namespace O24OpenAPI.AI.API.Application.Models
{

    public class QdrantPayload
    {
        public string Text { get; set; }
        public string Source { get; set; }
    }
    public class QdrantHit
    {
        public string Id { get; set; }
        public float Score { get; set; }
        public QdrantPayload Payload { get; set; }
    }
}

namespace O24OpenAPI.AI.API.Application.Models
{
    public class VecPoint
    {
        public string Id { get; set; }
        public float[] Vector { get; set; }
        public string Text { get; set; }
        public string? Source { get; set; }
    }
}

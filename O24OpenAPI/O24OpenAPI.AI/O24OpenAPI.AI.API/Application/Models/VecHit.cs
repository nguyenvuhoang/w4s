namespace O24OpenAPI.AI.API.Application.Models
{
    public class VecHit
    {
        public string Id { get; set; } = default!;
        public float Score { get; set; }
        public string Text { get; set; } = default!;
        public string Source { get; set; } = default!;
    }
}

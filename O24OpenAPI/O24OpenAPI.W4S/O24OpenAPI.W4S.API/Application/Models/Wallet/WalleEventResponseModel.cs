namespace O24OpenAPI.W4S.API.Application.Models.Wallet
{
    public class WalleEventResponseModel : BaseO24OpenAPIModel
    {
        public int Id { get; set; }
        public int WalletId { get; set; }

        // Identity / Display
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; }
        public string Location { get; set; }
        public string Color { get; set; }
        public string Icon { get; set; }

        // Calendar time
        public DateTime StartOnUtc { get; set; }
        public DateTime? EndOnUtc { get; set; }
        public bool IsAllDay { get; set; }

        // Type & Status
        public string EventType { get; set; }
        public string Status { get; set; }
    }
}

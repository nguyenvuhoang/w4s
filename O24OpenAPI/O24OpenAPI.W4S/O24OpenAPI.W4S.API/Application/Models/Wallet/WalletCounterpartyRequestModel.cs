namespace O24OpenAPI.W4S.API.Application.Models.Wallet
{
    public class WalletCounterpartyRequestModel
    {
        public int? Id { get; set; }
        public string DisplayName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string AvatarUrl { get; set; }
        public short? CounterpartyType { get; set; }
        public bool? IsFavorite { get; set; }
    }
}

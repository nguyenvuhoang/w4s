using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.NCH.Domain.AggregatesModel.ZaloAggregate
{
    public class ZaloOAToken : BaseEntity
    {
        public string OaId { get; set; } = default!;
        public string AppId { get; set; } = default!;

        public string AccessToken { get; set; } = default!;
        public string? RefreshToken { get; set; }

        public int ExpiresIn { get; set; }
        public DateTime ExpiresAtUtc { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime? LastUsedAtUtc { get; set; }
    }
}

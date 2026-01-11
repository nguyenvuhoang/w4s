using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.W4S.Domain.AggregatesModel.WalletMasterAggregate;

[Auditable]
public partial class WalletAvatar : BaseEntity
{
    public int WalletId { get; set; } = default!;
    public string? UserCode { get; set; }

    public string FileKey { get; set; } = default!;
    public string? FileUrl { get; set; }
    public string? ContentType { get; set; }
    public long? FileSize { get; set; }

    public bool IsCurrent { get; set; }
    public int SortOrder { get; set; }
    public string Status { get; set; } = "ACTIVE";

    public void SetCurrent() => IsCurrent = true;
    public void UnsetCurrent() => IsCurrent = false;

    public WalletAvatar() { }

}

using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.W4S.Domain.AggregatesModel.WalletMasterAggregate
{
    [Auditable]
    public partial class WalletLedgerEntry : BaseEntity
    {
        public string? TRANSACTIONID { get; set; }
        public string AccountNumber { get; set; } = default!;
        public string Currency { get; set; } = "VND";
        public int Group { get; set; }
        public int Index { get; set; }
        public DrCr DrCr { get; set; }
        public decimal Amount { get; set; }
        public DateTime PostingDateUtc { get; set; }
        public string? EntryType { get; set; }

        public WalletLedgerEntry() { }

    }
    public enum DrCr { D, C }
}

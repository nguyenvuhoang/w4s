using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Core.Domain;
using O24OpenAPI.W4S.Domain.Constants;

namespace O24OpenAPI.W4S.Domain.AggregatesModel.WalletMasterAggregate;

[Auditable]
public partial class WalletStatement : BaseEntity
{
    public int WalletId { get; set; }
    public string? AccountNumber { get; set; }

    // ===== Statement time =====
    public DateTime StatementOnUtc { get; set; } // posting time (ledger time)
    public DateTime? TransactionOnUtc { get; set; } // original tx time (bank/payment/manual)

    // ===== Ledger entry =====
    public string? EntryType { get; set; } // Debit/Credit/Adjustment
    public decimal Amount { get; set; }
    public string CurrencyCode { get; set; } = "VND";

    public decimal RunningBalance { get; set; }
    public decimal? OpeningBalance { get; set; }
    public decimal? ClosingBalance { get; set; }

    public string Description { get; set; } = string.Empty;

    // ===== Classification
    public int CategoryId { get; set; }
    public int EventId { get; set; } // link to wallet event/calendar item

    // ===== Source & References =====
    public string? Source { get; set; } // Manual/Payment/BankImport/Sync/Adjustment...
    public string? ReferenceType { get; set; } // e.g. "WalletTransaction", "Payment", "BankImportLine"
    public string? ReferenceId { get; set; } // reference key from other system/module
    public string? ExternalRef { get; set; } // bank trace id, gateway txn id, etc.

    // Reconciliation / matching
    public bool IsReconciled { get; set; }
    public DateTime? ReconciledOnUtc { get; set; }
    public string? ReconciledBy { get; set; }

    // ===== Status =====
    public string Status { get; set; } = WalletStatementStatus.POSTED;

    // ===== Audit / Integrity (Phase 4: blockchain/audit hash) =====
    public string? EntryHash { get; set; }
    public string? BlockchainTxHash { get; set; } // on-chain tx hash if anchored
    public DateTime? AnchoredOnUtc { get; set; }
}

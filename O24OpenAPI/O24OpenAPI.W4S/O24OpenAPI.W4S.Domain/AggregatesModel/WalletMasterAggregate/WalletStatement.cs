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
    public bool IsReconciled { get; set; } = true;
    public DateTime? ReconciledOnUtc { get; set; }
    public string? ReconciledBy { get; set; }

    // ===== Status =====
    public string Status { get; set; } = WalletStatementStatus.POSTED;

    // ===== Audit / Integrity (Phase 4: blockchain/audit hash) =====
    public string? EntryHash { get; set; }
    public string? BlockchainTxHash { get; set; } // on-chain tx hash if anchored
    public DateTime? AnchoredOnUtc { get; set; }


    public static WalletStatement Create(
         int walletId,
         string accountNumber,
         DateTime statementOnUtc,
         DrCr drCr,
         decimal amount,
         string currencyCode,
         decimal openingBalance,
         string description,
         int categoryId = 0,
         int eventId = 0,
         string? source = null,
         string? referenceType = null,
         string? referenceId = null,
         string? externalRef = null,
         DateTime? transactionOnUtc = null,
         string status = WalletStatementStatus.POSTED
     )
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(walletId);
        if (string.IsNullOrWhiteSpace(accountNumber)) throw new ArgumentException("AccountNumber is required", nameof(accountNumber));
        if (string.IsNullOrWhiteSpace(currencyCode)) throw new ArgumentException("CurrencyCode is required", nameof(currencyCode));
        if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be > 0");

        // Statement entry type: Debit/Credit/Adjustment (string)
        var entryType = drCr == DrCr.C ? "Credit" : "Debit";

        // Ledger rule: Credit => +, Debit => -
        var signed = drCr == DrCr.C ? amount : -amount;
        var closing = openingBalance + signed;

        return new WalletStatement
        {
            WalletId = walletId,
            AccountNumber = accountNumber,

            StatementOnUtc = statementOnUtc,
            TransactionOnUtc = transactionOnUtc,

            EntryType = entryType,
            Amount = amount,
            CurrencyCode = currencyCode,

            OpeningBalance = openingBalance,
            ClosingBalance = closing,
            RunningBalance = closing,

            Description = description ?? string.Empty,

            CategoryId = categoryId,
            EventId = eventId,

            Source = source,
            ReferenceType = referenceType,
            ReferenceId = referenceId,
            ExternalRef = externalRef,

            Status = status,
            IsReconciled = true
        };
    }


}

using O24OpenAPI.Core.Attributes;
using O24OpenAPI.Core.Domain;
using O24OpenAPI.W4S.Domain.Constants;

namespace O24OpenAPI.W4S.Domain.AggregatesModel.BudgetWalletAggregate;

[Auditable]
public partial class WalletEvent : BaseEntity
{
    public int WalletId { get; set; }

    // Identity / Display
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Location { get; set; }
    public string? Color { get; set; }
    public string? Icon { get; set; }

    // Calendar time
    public DateTime StartOnUtc { get; set; }
    public DateTime? EndOnUtc { get; set; }
    public bool IsAllDay { get; set; }

    // Type & Status
    public string EventType { get; set; } = WalletEventType.GENERAL;
    public string Status { get; set; } = WalletEventStatus.ACTIVE;

    // Optional: planning / budget link
    public decimal? PlannedAmount { get; set; }
    public string CurrencyCode { get; set; } = "VND";
    public int? CategoryId { get; set; } // planning category
    public int? BudgetId { get; set; } // link to budget plan if you have

    // Reminder
    public int? ReminderMinutes { get; set; } // e.g. 10, 30, 60, 1440
    public DateTime? ReminderOnUtc { get; set; } // computed schedule time (optional)

    // Recurrence (simple iCal-like or custom rule)
    public bool IsRecurring { get; set; }
    public string? RecurrenceRule { get; set; } // e.g. "FREQ=MONTHLY;INTERVAL=1;BYMONTHDAY=15"
    public string? RecurrenceGroupId { get; set; } // group instances

    // Link to statement/transaction later (optional)
    public string? ReferenceType { get; set; }
    public string? ReferenceId { get; set; }

    public WalletEvent() { }

    public static WalletEvent Create(
        int walletId,
        string title,
        DateTime startOnUtc,
        DateTime? endOnUtc = null,
        bool isAllDay = false,
        string? description = null,
        string? location = null,
        string? color = null,
        string? icon = null,
        string? eventType = null,
        int? reminderMinutes = null,
        bool isRecurring = false,
        string? recurrenceRule = null,
        string? referenceType = null,
        string? referenceId = null
    )
    {
        if (walletId <= 0)
            throw new ArgumentException("WalletId must be greater than zero.", nameof(walletId));

        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required.", nameof(title));

        if (endOnUtc.HasValue && endOnUtc < startOnUtc)
            throw new ArgumentException("End time cannot be earlier than start time.");

        var evt = new WalletEvent
        {
            WalletId = walletId,

            // Identity
            Title = title.Trim(),
            Description = description?.Trim(),
            Location = location?.Trim(),
            Color = color,
            Icon = icon,

            // Time
            StartOnUtc = DateTime.SpecifyKind(startOnUtc, DateTimeKind.Utc),
            EndOnUtc = endOnUtc.HasValue
                ? DateTime.SpecifyKind(endOnUtc.Value, DateTimeKind.Utc)
                : null,
            IsAllDay = isAllDay,

            // Type & Status
            EventType = eventType ?? WalletEventType.GENERAL,
            Status = WalletEventStatus.ACTIVE,

            // Reminder
            ReminderMinutes = reminderMinutes,
            ReminderOnUtc = reminderMinutes.HasValue
                ? DateTime.SpecifyKind(startOnUtc, DateTimeKind.Utc)
                    .AddMinutes(-reminderMinutes.Value)
                : null,

            // Recurrence
            IsRecurring = isRecurring,
            RecurrenceRule = isRecurring ? recurrenceRule : null,
            RecurrenceGroupId = isRecurring ? Guid.NewGuid().ToString("N") : null,

            // Reference
            ReferenceType = referenceType,
            ReferenceId = referenceId
        };

        return evt;
    }

}

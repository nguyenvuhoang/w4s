using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.CTH.Domain.AggregatesModel.CalendarAggregate;

public partial class BankWorkingCalendar : BaseEntity
{
    /// <summary>Bank code, e.g., "EMI".</summary>
    public string BankCode { get; set; } = null!;

    /// <summary>Branch code (nullable = applies to whole bank).</summary>
    public string? BranchCode { get; set; }

    /// <summary>The business date this row applies to (local date, no time).</summary>
    public DateTime WorkingDate { get; set; }
    public DateTime? BatchDate { get; set; }

    /// <summary>Is this date a working day</summary>
    public bool IsWorkingDay { get; set; }

    /// <summary>Is this date a holiday</summary>
    public bool IsHoliday { get; set; }

    /// <summary>Holiday code and name if holiday.</summary>
    public string? HolidayCode { get; set; }
    public string? HolidayName { get; set; }

    /// <summary>Business hours (local time, 24h).</summary>
    public TimeSpan OpenTime { get; set; }
    public TimeSpan CloseTime { get; set; }

    /// <summary>Optional lunch-break window.</summary>
    public TimeSpan LunchStartTime { get; set; }
    public TimeSpan LunchEndTime { get; set; }

    /// <summary>Operational cutoffs (settlement/transfer…)</summary>
    public TimeSpan CutoffPaymentTime { get; set; }
    public TimeSpan CutoffTransferTime { get; set; }

    /// <summary>Timezone IANA (e.g., "Asia/Ho_Chi_Minh").</summary>
    public string Timezone { get; set; } = "Asia/Ho_Chi_Minh";

    /// <summary>Calendar segmentation (e.g., "Bank", "FX", "Settlement").</summary>
    public string CalendarType { get; set; } = "Bank";

    /// <summary>Week mask, e.g. "Mon-Fri", "Mon-Sat".</summary>
    public string? WeekMask { get; set; }

    /// <summary>Convenience pointers for previous/next working date.</summary>
    public DateTime PreviousWorkingDate { get; set; }
    public DateTime NextWorkingDate { get; set; }

    /// <summary>Status: 1=Active, 0=Inactive (soft-switch rows).</summary>
    public bool Status { get; set; } = true;

    public string? Notes { get; set; }
}

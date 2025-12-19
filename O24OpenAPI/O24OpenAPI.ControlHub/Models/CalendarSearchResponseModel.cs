using O24OpenAPI.Core.Abstractions;

namespace O24OpenAPI.ControlHub.Models;

public class CalendarSearchResponseModel : BaseO24OpenAPIModel
{
    /// <summary>
    /// calendar id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// SqnDate
    /// </summary>
    public DateTime SqnDate { get; set; }

    /// <summary>
    /// IsCurrentDate
    /// </summary>
    public int IsCurrentDate { get; set; }

    /// <summary>
    /// IsHoliday
    /// </summary>
    public int IsHoliday { get; set; }

    /// <summary>
    /// IsEndOfWeek
    /// </summary>
    public int IsEndOfWeek { get; set; }

    /// <summary>
    /// IsEndOfMonth
    /// </summary>
    public int IsEndOfMonth { get; set; }

    /// <summary>
    /// IsEndOfQuater
    /// </summary>
    public int IsEndOfQuater { get; set; }

    /// <summary>
    /// IsEndOfHalfYear
    /// </summary>
    public int IsEndOfHalfYear { get; set; }

    /// <summary>
    /// IsEndOfYear
    /// </summary>
    public int IsEndOfYear { get; set; }

    /// <summary>
    /// IsBeginOfWeek
    /// </summary>
    public int IsBeginOfWeek { get; set; }

    /// <summary>
    /// IsBeginOfMonth
    /// </summary>
    public int IsBeginOfMonth { get; set; }

    /// <summary>
    /// IsBeginOfQuater
    /// </summary>
    public int IsBeginOfQuater { get; set; }

    /// <summary>
    /// IsBeginOfHalfYear
    /// </summary>
    public int IsBeginOfHalfYear { get; set; }

    /// <summary>
    /// IsBeginOfYear
    /// </summary>
    public int IsBeginOfYear { get; set; }

    /// <summary>
    /// IsFiscalEndOfWeek
    /// </summary>
    public int IsFiscalEndOfWeek { get; set; }

    /// <summary>
    /// IsFiscalEndOfMonth
    /// </summary>
    public int IsFiscalEndOfMonth { get; set; }

    /// <summary>
    /// IsFiscalEndOfQuater
    /// </summary>
    public int IsFiscalEndOfQuater { get; set; }

    /// <summary>
    /// IsFiscalEndOfHalfYear
    /// </summary>
    public int IsFiscalEndOfHalfYear { get; set; }

    /// <summary>
    /// IsFiscalEndOfYear
    /// </summary>
    public int IsFiscalEndOfYear { get; set; }

    /// <summary>
    /// IsFiscalBeginOfWeek
    /// </summary>
    public int IsFiscalBeginOfWeek { get; set; }

    /// <summary>
    /// IsFiscalBeginOfMonth
    /// </summary>
    public int IsFiscalBeginOfMonth { get; set; }

    /// <summary>
    /// IsFiscalBeginOfQuater
    /// </summary>
    public int IsFiscalBeginOfQuater { get; set; }

    /// <summary>
    /// IsFiscalBeginOfHalfYear
    /// </summary>
    public int IsFiscalBeginOfHalfYear { get; set; }

    /// <summary>
    /// IsFiscalBeginOfYear
    /// </summary>
    public int IsFiscalBeginOfYear { get; set; }

    /// <summary>
    /// Descs
    /// </summary>
    public string Descs { get; set; }

    /// <summary>
    /// CurrencyCode
    /// </summary>
    public string CurrencyCode { get; set; }
}

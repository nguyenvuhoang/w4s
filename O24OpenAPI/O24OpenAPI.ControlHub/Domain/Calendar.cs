using Newtonsoft.Json;
using O24OpenAPI.Core.Domain;

namespace O24OpenAPI.ControlHub.Domain;

/// <summary>
/// Calendar domain
/// </summary>
public partial class Calendar : BaseEntity
{
    /// <summary>
    /// Calendar constructor
    /// </summary>
    public Calendar() { }

    /// <summary>
    /// sqndt
    /// </summary>
    [JsonProperty("sqn_date")]
    public DateTime SqnDate { get; set; }

    /// <summary>
    /// isnow
    /// </summary>
    [JsonProperty("is_current_date")]
    public int IsCurrentDate { get; set; }

    /// <summary>
    /// isholiday
    /// </summary>
    [JsonProperty("is_holiday")]
    public int IsHoliday { get; set; }

    /// <summary>
    /// iseow
    /// </summary>
    [JsonProperty("is_end_of_week")]
    public int IsEndOfWeek { get; set; }

    /// <summary>
    /// iseom
    /// </summary>
    [JsonProperty("is_end_of_month")]
    public int IsEndOfMonth { get; set; }

    /// <summary>
    /// iseoq
    /// </summary>
    [JsonProperty("is_end_of_quater")]
    public int IsEndOfQuater { get; set; }

    /// <summary>
    /// iseoh
    /// </summary>
    [JsonProperty("is_end_of_half_year")]
    public int IsEndOfHalfYear { get; set; }

    /// <summary>
    /// iseoy
    /// </summary>
    [JsonProperty("is_end_of_year")]
    public int IsEndOfYear { get; set; }

    /// <summary>
    /// isbow
    /// </summary>
    [JsonProperty("is_begin_of_week")]
    public int IsBeginOfWeek { get; set; }

    /// <summary>
    /// isbom
    /// </summary>
    [JsonProperty("is_begin_of_month")]
    public int IsBeginOfMonth { get; set; }

    /// <summary>
    /// isboq
    /// </summary>
    [JsonProperty("is_begin_of_quater")]
    public int IsBeginOfQuater { get; set; }

    /// <summary>
    /// isboh
    /// </summary>
    [JsonProperty("is_begin_of_half_year")]
    public int IsBeginOfHalfYear { get; set; }

    /// <summary>
    /// isboy
    /// </summary>
    [JsonProperty("is_begin_of_year")]
    public int IsBeginOfYear { get; set; }

    /// <summary>
    /// isfeow
    /// </summary>
    [JsonProperty("is_fiscal_end_of_week")]
    public int IsFiscalEndOfWeek { get; set; }

    /// <summary>
    /// isfeom
    /// </summary>
    [JsonProperty("is_fiscal_end_of_month")]
    public int IsFiscalEndOfMonth { get; set; }

    /// <summary>
    /// isfeoq
    /// </summary>
    [JsonProperty("is_fiscal_end_of_quater")]
    public int IsFiscalEndOfQuater { get; set; }

    /// <summary>
    /// isfeoh
    /// </summary>
    [JsonProperty("is_fiscal_end_of_half_year")]
    public int IsFiscalEndOfHalfYear { get; set; }

    /// <summary>
    /// isfeoy
    /// </summary>
    [JsonProperty("is_fiscal_end_of_year")]
    public int IsFiscalEndOfYear { get; set; }

    /// <summary>
    /// isfbow
    /// </summary>
    [JsonProperty("is_fiscal_begin_of_week")]
    public int IsFiscalBeginOfWeek { get; set; }

    /// <summary>
    /// isfbom
    /// </summary>
    [JsonProperty("is_fiscal_begin_of_month")]
    public int IsFiscalBeginOfMonth { get; set; }

    /// <summary>
    /// isfboq
    /// </summary>
    [JsonProperty("is_fiscal_begin_of_quater")]
    public int IsFiscalBeginOfQuater { get; set; }

    /// <summary>
    /// isfboh
    /// </summary>
    [JsonProperty("is_fiscal_begin_of_half_year")]
    public int IsFiscalBeginOfHalfYear { get; set; }

    /// <summary>
    /// isfboy
    /// </summary>
    [JsonProperty("is_fiscal_begin_of_year")]
    public int IsFiscalBeginOfYear { get; set; }

    /// <summary>
    /// descs
    /// </summary>
    [JsonProperty("descs")]
    public string Descs { get; set; }

    /// <summary>
    /// ccrid
    /// </summary>
    [JsonProperty("currency_code")]
    public string CurrencyCode { get; set; }

    /// <summary>
    /// Last update date
    /// </summary>
    public DateTime? UpdatedOnUtc { get; set; }

    /// <summary>
    /// Last update date
    /// </summary>
    public DateTime? CreatedOnUtc { get; set; }
}

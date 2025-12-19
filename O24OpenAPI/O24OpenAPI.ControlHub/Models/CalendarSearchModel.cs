using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.ControlHub.Models;

public class CalendarSearchModel : BaseTransactionModel
{
    /// <summary>
    /// calendar search model constructor
    /// </summary>
    public CalendarSearchModel()
    {
        this.PageIndex = 0;
        this.PageSize = int.MaxValue;
    }

    /// <summary>
    /// PageIndex
    /// </summary>
    public int PageIndex { get; set; }

    /// <summary>
    /// PageSize
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Year
    /// </summary>
    public int? Year { get; set; }

    /// <summary>
    /// Month
    /// </summary>
    public int? Month { get; set; }
}

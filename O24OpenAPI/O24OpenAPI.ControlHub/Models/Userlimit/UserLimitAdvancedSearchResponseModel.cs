using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.ControlHub.Models.Userlimit;

public class UserLimitAdvancedSearchResponseModel : BaseO24OpenAPIModel
{
    /// <summary>
    /// user limit id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// RoleId
    /// </summary>
    public int? RoleId { get; set; }

    /// <summary>
    /// CommandId
    /// </summary>
    public string CommandId { get; set; }

    /// <summary>
    /// CurrencyCode
    /// </summary>
    public string CurrencyCode { get; set; }

    /// <summary>
    /// ULimit
    /// </summary>
    public decimal? ULimit { get; set; }

    /// <summary>
    /// CvTable
    /// </summary>
    public string CvTable { get; set; }

    /// <summary>
    /// LimitType
    /// </summary>
    public string LimitType { get; set; }

    /// <summary>
    /// Margin
    /// </summary>
    public int? Margin { get; set; }

    /// <summary>
    /// Module
    /// </summary>
    public string Module { get; set; }

    /// <summary>
    /// TranName
    /// </summary>
    public string TranName { get; set; }
}

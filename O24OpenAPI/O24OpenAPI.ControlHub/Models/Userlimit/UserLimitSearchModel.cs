using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.ControlHub.Models.Userlimit;

public class UserLimitSearchModel : BaseTransactionModel
{
    /// <summary>
    /// RoleId
    /// </summary>
    public int? RoleId { get; set; }

    /// <summary>
    /// CommandId
    /// </summary>
    public string CommandId { get; set; }

    /// <summary>
    /// LimitType
    /// </summary>
    /// <value></value>
    public string LimitType { get; set; }

    /// <summary>
    /// PageIndex
    /// </summary>
    public int PageIndex { get; set; } = 0;

    /// <summary>
    /// PageSize
    /// </summary>
    public int PageSize { get; set; } = int.MaxValue;
}

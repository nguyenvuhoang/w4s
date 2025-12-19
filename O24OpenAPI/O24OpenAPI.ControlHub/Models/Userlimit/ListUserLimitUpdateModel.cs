using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.ControlHub.Models.Userlimit;

public class ListUserLimitUpdateModel : BaseTransactionModel
{
    /// <summary>
    /// list user limit update model
    /// </summary>
    public ListUserLimitUpdateModel()
    {
        ListUserLimit = new List<UserLimitUpdateResponseModel>();
    }

    /// <summary>
    /// ListUserLimit
    /// </summary>
    /// <value></value>
    public List<UserLimitUpdateResponseModel> ListUserLimit { get; set; }
}

using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.ControlHub.Models;

public class DeleteUserModel : BaseTransactionModel
{
    public int Id { get; set; }
}

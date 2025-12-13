using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.ControlHub.Models;

public class LoadUserAgreementRequestModel : BaseTransactionModel
{
    public new string TransactionCode { get; set; }
}

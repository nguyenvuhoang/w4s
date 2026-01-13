using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.O24ACT.Models;

public class FundTransferModel : BaseTransactionModel
{
    public FundTransferModel()
    {
        this.TransferData = new List<FundTransferDetailModel>();
    }

    /// <summary>
    /// Account number (PDACC)
    /// </summary>
    public List<FundTransferDetailModel> TransferData { get; set; }
}

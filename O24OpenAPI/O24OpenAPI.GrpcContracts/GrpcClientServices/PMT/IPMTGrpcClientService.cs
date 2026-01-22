using O24OpenAPI.APIContracts.Models.PMT;

namespace O24OpenAPI.GrpcContracts.GrpcClientServices.PMT;

public interface IPMTGrpcClientService
{
    /// <summary>
    /// VNPay process pay
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="transactionDescription"></param>
    /// <param name="languageCode"></param>
    /// <param name="vNPayTransactionDate"></param>
    /// <returns></returns>
    Task<VNPayProcessPayResponseModel> VNPayProcessPay(string amount, string transactionDescription, string languageCode, string vNPayTransactionDate);
    /// <summary>
    ///    VNPay process return
    /// </summary>
    /// <param name="rawQuery"></param>
    /// <returns></returns>
    Task<VNPayProcessReturnModel> VNPayProcessReturn(string rawQuery);
}

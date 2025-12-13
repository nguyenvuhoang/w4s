using O24OpenAPI.APIContracts.Models.NCH;

namespace O24OpenAPI.GrpcContracts.GrpcClientServices.DTS;

public interface IDTSGrpcClientService
{
    Task<bool> VerifyUserAysnc(string contractNumber, string idCard);
    Task<string> GetContractNumberByAccountNumberAsync(string accountNumber);
    Task<bool> VerifyUserAnotherDeviceAsync(
        string contractNumber,
        DateTime dob,
        string licenseType,
        string licenseId,
        string language
    );

    /// <summary>
    /// Get SMS Loan Alert by type
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    Task<List<SMSLoanAlertModel>> GetSMSLoanAlertAsync(string type);
}


namespace O24OpenAPI.CMS.API.Application.Services.Interfaces;

public partial interface ISendMailService
{
    // /// <summary>
    // ///
    // /// </summary>
    // /// <returns></returns>
    // Task<CheckSentResponseModel> CheckSentBeforeRepay(DateTime dueDate, string accountNumber);
    // /// <summary>
    // ///
    // /// </summary>
    // /// <param name="model"></param>
    // /// <returns></returns>
    // SendMailRequestModel GetMailRequestBeforeRepay(RemindBeforeRepaymentModel model);
    // /// <summary>
    // ///
    // /// </summary>
    // /// <returns></returns>
    // Task<List<RemindBeforeRepaymentModel>> GetListFailBeforeRepay();
    // /// <summary>
    // ///
    // /// </summary>
    // /// <returns></returns>
    // Task<List<RemindBeforeRepaymentModel>> GetListFailAfterRepay();

    /// <summary>
    ///
    /// </summary>
    /// <param name="workflowId"></param>
    /// <param name="status"></param>
    /// <param name="accountNumber"></param>
    /// <param name="dueDate"></param>
    /// <returns></returns>
    Task<int> CountSentMail(
        string workflowId,
        string status,
        string accountNumber,
        DateTime? dueDate = null
    );

    /// <summary>
    ///
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    bool IsValidEmail(string email);
}

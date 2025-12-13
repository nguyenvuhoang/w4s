using LinqToDB;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using System.Text.RegularExpressions;

namespace O24OpenAPI.Web.CMS.Services.Services;

/// <summary>
/// VoucherLinkageService
/// </summary>
/// <remarks>
///
/// </remarks>
/// <param name="transactionRepository"></param>
public partial class SendMailService(IRepository<Transaction> transactionRepository)
    : ISendMailService
{
    #region Fields
    private readonly IRepository<Transaction> _transactionRepository = transactionRepository;

    #endregion

    /// <summary>
    ///
    /// </summary>
    /// <param name="workflowId"></param>
    /// <param name="status"></param>
    /// <param name="accountNumber"></param>
    /// <param name="dueDate"></param>
    /// <returns></returns>
    public async Task<int> CountSentMail(
        string workflowId,
        string status,
        string accountNumber,
        DateTime? dueDate = null
    )
    {
        var query = _transactionRepository.Table.Where(tran =>
            tran.TransactionCode == workflowId
            && tran.Status == status
            && tran.RequestBody.Contains(accountNumber)
        );

        if (dueDate.HasValue)
        {
            query = query.Where(tran =>
                tran.RequestBody.Contains(dueDate.Value.ToString("yyyy-MM-dd"))
            );
        }

        var count = await query.CountAsync();

        return count;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public bool IsValidEmail(string email)
    {
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        Regex regex = new Regex(pattern);

        return regex.IsMatch(email);
    }
}

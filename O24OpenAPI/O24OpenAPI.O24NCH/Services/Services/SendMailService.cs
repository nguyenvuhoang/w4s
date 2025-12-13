using O24OpenAPI.O24NCH.Services.Interfaces;
using System.Text.RegularExpressions;

namespace O24OpenAPI.Web.CMS.Services.Services;

/// <summary>
/// 
/// </summary>
/// <remarks>
///
/// </remarks>
/// <param name="transactionRepository"></param>
public partial class SendMailService()
    : ISendMailService
{


    /// <summary>
    ///
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public bool IsValidEmail(string email)
    {
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        Regex regex = new(pattern);

        return regex.IsMatch(email);
    }
}

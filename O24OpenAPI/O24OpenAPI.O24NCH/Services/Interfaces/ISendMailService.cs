namespace O24OpenAPI.O24NCH.Services.Interfaces;

public partial interface ISendMailService
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    bool IsValidEmail(string email);
}

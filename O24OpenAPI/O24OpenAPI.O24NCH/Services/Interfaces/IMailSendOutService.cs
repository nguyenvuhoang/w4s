using O24OpenAPI.O24NCH.Domain;

namespace O24OpenAPI.O24NCH.Services.Interfaces;

public interface IMailSendOutService
{
    /// <summary>
    /// Creates the email send out record.
    /// </summary>
    /// <param name="mailSendOut"></param>
    /// <returns></returns>
    Task<EmailSendOut> Create(EmailSendOut mailSendOut);
}

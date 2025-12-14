using O24OpenAPI.O24NCH.Domain;
using O24OpenAPI.O24NCH.Services.Interfaces;

namespace O24OpenAPI.O24NCH.Services.Services;

public class MailSendOutService(IRepository<EmailSendOut> mailSendOutRepository) : IMailSendOutService
{
    private readonly IRepository<EmailSendOut> _mailSendOutRepository = mailSendOutRepository;

    /// <summary>
    /// Creates the email send out record.
    /// </summary>
    /// <param name="mailSendOut"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public virtual async Task<EmailSendOut> Create(EmailSendOut mailSendOut)
    {
        await _mailSendOutRepository.InsertAsync(mailSendOut);
        return mailSendOut;
    }
}

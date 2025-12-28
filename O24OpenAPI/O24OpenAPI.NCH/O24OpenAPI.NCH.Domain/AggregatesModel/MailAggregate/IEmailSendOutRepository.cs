using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.NCH.Domain.AggregatesModel.MailAggregate;

public interface IEmailSendOutRepository : IRepository<EmailSendOut>
{
    Task<IReadOnlyList<EmailSendOut>> GetPendingAsync(int take);
    Task<EmailSendOut> Create(EmailSendOut mailSendOut);
}

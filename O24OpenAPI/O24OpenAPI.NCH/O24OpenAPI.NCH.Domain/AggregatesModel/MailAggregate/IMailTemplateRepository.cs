using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.NCH.Domain.AggregatesModel.MailAggregate;

public interface IMailTemplateRepository : IRepository<MailTemplate>
{
    Task<MailTemplate?> GetByCodeAsync(string code);
    Task<MailTemplate> GetByTemplateId(string templateId);
}

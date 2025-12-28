using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.NCH.Domain.AggregatesModel.MailAggregate;

public interface IMailConfigRepository : IRepository<MailConfig>
{
    Task<MailConfig?> GetActiveAsync();
    Task<MailConfig> GetByConfigId(string configId);
}

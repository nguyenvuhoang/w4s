using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.NCH.Domain.AggregatesModel.SmsAggregate;

public interface ISMSTemplateRepository : IRepository<SMSTemplate>
{
    Task<SMSTemplate?> GetByCodeAsync(string code);
    Task<IReadOnlyList<SMSTemplate>> GetActiveAsync();
}

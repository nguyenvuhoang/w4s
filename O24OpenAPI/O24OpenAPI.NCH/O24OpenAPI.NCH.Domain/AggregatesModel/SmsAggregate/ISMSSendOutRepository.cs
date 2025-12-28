using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.NCH.Domain.AggregatesModel.SmsAggregate;

public interface ISMSSendOutRepository : IRepository<SMSSendOut>
{
    Task<IReadOnlyList<SMSSendOut>> GetPendingAsync(int take);
}

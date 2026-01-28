using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.NCH.Domain.AggregatesModel.ZaloAggregate
{
    public interface IZaloZNSTemplateRepository : IRepository<ZaloZNSTemplate>
    {
        Task<ZaloZNSTemplate?> FindByTemplateIdAsync(string templateId);
    }
}

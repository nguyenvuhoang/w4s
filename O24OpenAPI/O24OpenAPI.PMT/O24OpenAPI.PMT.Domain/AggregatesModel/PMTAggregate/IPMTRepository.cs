using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.PMT.Domain.AggregatesModel.PMTAggregate;

public interface IPMTRepository : IRepository<PMTDomain>
{
    public void Add(PMTDomain entity);
}

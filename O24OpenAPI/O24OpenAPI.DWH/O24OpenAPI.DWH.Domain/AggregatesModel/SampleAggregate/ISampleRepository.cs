using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.DWH.Domain.AggregatesModel.SampleAggregate;

public interface ISampleRepository : IRepository<SampleDomain>
{
    public void Add(SampleDomain sample);
}

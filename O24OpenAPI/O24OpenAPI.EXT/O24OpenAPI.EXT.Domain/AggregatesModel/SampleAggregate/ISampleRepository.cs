using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.EXT.Domain.AggregatesModel.SampleAggregate;

public interface ISampleRepository : IRepository<SampleDomain>
{
    public void Add(SampleDomain sample);
}

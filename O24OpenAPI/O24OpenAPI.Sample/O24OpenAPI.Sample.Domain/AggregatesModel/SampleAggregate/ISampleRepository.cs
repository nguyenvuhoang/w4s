using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.Sample.Domain.AggregatesModel.SampleAggregate;

public interface ISampleRepository : IRepository<SampleDomain>
{
    public void Add(SampleDomain sample);
}

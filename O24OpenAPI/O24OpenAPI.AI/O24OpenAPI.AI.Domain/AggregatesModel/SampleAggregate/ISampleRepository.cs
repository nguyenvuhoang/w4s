using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.AI.Domain.AggregatesModel.SampleAggregate;

public interface ISampleRepository : IRepository<Sample>
{
    public void Add(Sample sample);
}

using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.CTH.Domain.AggregatesModel.SampleAggregate;

public interface ISampleRepository : IRepository<Sample>
{
    public void Add(Sample sample);
}

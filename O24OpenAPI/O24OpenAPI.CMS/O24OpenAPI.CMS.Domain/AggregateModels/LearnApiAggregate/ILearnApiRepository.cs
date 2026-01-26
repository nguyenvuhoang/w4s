using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.CMS.Domain.AggregateModels.LearnApiAggregate;

public interface ILearnApiRepository : IRepository<LearnApi>
{
    Task<LearnApi> GetByChannelAndLearnApiIdAsync(string channel, string learnApiId);
}

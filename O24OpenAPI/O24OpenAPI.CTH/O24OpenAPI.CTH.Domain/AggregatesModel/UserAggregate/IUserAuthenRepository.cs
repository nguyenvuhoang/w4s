using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate
{
    public interface IUserAuthenRepository : IRepository<UserAuthen>
    {
        Task<UserAuthen?> GetByUserCodeAsync(string userCode);
    }
}

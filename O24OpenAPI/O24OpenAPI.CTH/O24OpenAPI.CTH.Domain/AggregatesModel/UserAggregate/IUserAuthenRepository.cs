using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate
{
    public interface IUserAuthenRepository : IRepository<UserAuthen>
    {
        Task<UserAuthen?> GetByUserCodeAsync(string userCode);
        Task UpdateAsync(UserAuthen user);
        Task<UserAuthen> AddAsync(UserAuthen user);
        Task<UserAuthen?> GetByUserAuthenInfoAsync(
            string userCode,
            string authenType,
            string phoneNumber
        );
    }
}

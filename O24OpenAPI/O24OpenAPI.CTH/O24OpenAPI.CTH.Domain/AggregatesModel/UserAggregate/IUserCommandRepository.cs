using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

public interface IUserCommandRepository : IRepository<UserCommand>
{
    Task<List<string>> GetListCommandParentAsync(string applicationCode);
    Task<List<UserCommand>> LoadUserCommand(string applicationCode, string roleCommand);
    Task<List<UserCommand>> GetInfoFromFormCode(string applicationCode, string formCode);
}

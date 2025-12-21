using O24OpenAPI.Core.SeedWork;
using System;

namespace O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate
{
    public interface IUserCommandRepository : IRepository<UserCommand>
    {
        Task<List<string>> GetListCommandParentAsync(string applicationCode);
    }
}
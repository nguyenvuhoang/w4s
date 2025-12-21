//using LinKit.Core.Cqrs;
//using LinqToDB;
//using O24OpenAPI.Framework.Models;
//using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

//namespace O24OpenAPI.CTH.API.Application.Features.User
//{
//    public class GetUserInRolesByRoleIdCommnad: BaseTransactionModel, ICommand<List<UserInRole>>
//    {

//    }

//    public class GetUserInRolesByRoleIdHandler(IUserInRoleRepository userInRoleRepository) : ICommandHandler<GetUserInRolesByRoleIdCommnad, List<UserInRole>>
//    {
//        public async Task<List<UserInRole>> HandleAsync(GetUserInRolesByRoleIdCommnad request, CancellationToken cancellationToken = default)
//        {
//        return await userInRoleRepository.Table.Where(x => x.RoleId == roleId).ToListAsync();
//        }
//    }
//}

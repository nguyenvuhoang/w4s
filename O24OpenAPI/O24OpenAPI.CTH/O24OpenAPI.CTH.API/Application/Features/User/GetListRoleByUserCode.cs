//using LinKit.Core.Cqrs;
//using LinqToDB;
//using O24OpenAPI.Framework.Models;
//using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

//namespace O24OpenAPI.CTH.API.Application.Features.User
//{
//    public class GetListRoleByUserCodeCommnad: BaseTransactionModel, ICommand<List<UserInRole>>
//    {

//    }

//    public class GetListRoleByUserCodeHandler(IUserInRoleRepository userInRoleRepository) : ICommandHandler<GetListRoleByUserCodeCommnad, List<UserInRole>>
//    {
//        public async Task<List<UserInRole>> HandleAsync(GetListRoleByUserCodeCommnad request, CancellationToken cancellationToken = default)
//        {
//        var getRoleList = await userInRoleRepository
//                .Table.Where(s => s.UserCode.Equals(userCode))
//                .ToListAsync();
//            return getRoleList ?? [];
//        }
//    }
//}

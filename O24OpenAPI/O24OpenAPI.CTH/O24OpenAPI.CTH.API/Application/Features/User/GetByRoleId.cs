//using LinKit.Core.Cqrs;
//using LinqToDB;
//using O24OpenAPI.Framework.Models;
//using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

//namespace O24OpenAPI.CTH.API.Application.Features.User
//{
//    public class GetByRoleIdCommnad: BaseTransactionModel, ICommand<List<UserLimit>>
//    {

//    }

//    public class GetByRoleIdHandler(IUserLimitRepository userLimitRepository) : ICommandHandler<GetByRoleIdCommnad, List<UserLimit>>
//    {
//        public async Task<List<UserLimit>> HandleAsync(GetByRoleIdCommnad request, CancellationToken cancellationToken = default)
//        {
//        return await userLimitRepository.Table.Where(uL => uL.RoleId == roleId).ToListAsync();
//        }
//    }
//}

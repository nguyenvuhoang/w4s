//using LinKit.Core.Cqrs;
//using LinqToDB;
//using O24OpenAPI.Framework.Models;
//using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

//namespace O24OpenAPI.CTH.API.Application.Features.User
//{
//    public class GetByRoleIdAndCommandIdCommnad: BaseTransactionModel, ICommand<UserRight>
//    {

//    }

//    public class GetByRoleIdAndCommandIdHandler(IUserRightRepository userRightRepository) : ICommandHandler<GetByRoleIdAndCommandIdCommnad, UserRight>
//    {
//        public async Task<UserRight> HandleAsync(GetByRoleIdAndCommandIdCommnad request, CancellationToken cancellationToken = default)
//        {
//        return await userRightRepository
//                .Table.Where(s => s.RoleId == roleId && s.CommandId == commandId)
//                .FirstOrDefaultAsync();
//        }
//    }
//}

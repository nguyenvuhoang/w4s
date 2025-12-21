//using LinKit.Core.Cqrs;
//using O24OpenAPI.Framework.Models;
//using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

//namespace O24OpenAPI.CTH.API.Application.Features.User
//{
//    public class BulkInsertCommnad: BaseTransactionModel, ICommand<List<UserInRole>>
//    {

//    }

//    public class BulkInsertHandler(IUserInRoleRepository userInRoleRepository) : ICommandHandler<BulkInsertCommnad, List<UserInRole>>
//    {
//        public async Task<List<UserInRole>> HandleAsync(BulkInsertCommnad request, CancellationToken cancellationToken = default)
//        {
//        await userInRoleRepository.BulkInsert(userInRole);
//            return userInRole;
//        }
//    }
//}

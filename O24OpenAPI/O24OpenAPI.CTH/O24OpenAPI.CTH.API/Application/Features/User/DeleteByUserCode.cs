//using LinKit.Core.Cqrs;
//using LinqToDB;
//using O24OpenAPI.Framework.Models;
//using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

//namespace O24OpenAPI.CTH.API.Application.Features.User
//{
//    public class DeleteByUserCodeCommnad: BaseTransactionModel, ICommand<bool>
//    {

//    }

//    public class DeleteByUserCodeHandler(IUserInRoleRepository userInRoleRepository) : ICommandHandler<DeleteByUserCodeCommnad, bool>
//    {
//        public async Task<bool> HandleAsync(DeleteByUserCodeCommnad request, CancellationToken cancellationToken = default)
//        {
//        var roles = await userInRoleRepository.Table
//                .Where(x => x.UserCode == userCode)
//                .ToListAsync();

//            foreach (var role in roles)
//            {
//                await userInRoleRepository.Delete(role);
//            }
//        }
//    }
//}

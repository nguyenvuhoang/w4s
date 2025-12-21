//using LinKit.Core.Cqrs;
//using LinqToDB;
//using O24OpenAPI.Framework.Models;
//using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

//namespace O24OpenAPI.CTH.API.Application.Features.User
//{
//    public class GetNextRoleIdCommnad: BaseTransactionModel, ICommand<int>
//    {

//    }

//    public class GetNextRoleIdHandler(IUserRoleRepository userRoleRepository) : ICommandHandler<GetNextRoleIdCommnad, int>
//    {
//        public async Task<int> HandleAsync(GetNextRoleIdCommnad request, CancellationToken cancellationToken = default)
//        {
//        var maxId = await userRoleRepository.Table
//                .Select(x => (int?)x.RoleId)
//                .MaxAsync();

//            return (maxId ?? 0) + 1;
//        }
//    }
//}

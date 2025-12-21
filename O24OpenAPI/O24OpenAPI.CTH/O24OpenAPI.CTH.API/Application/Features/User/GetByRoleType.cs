//using LinKit.Core.Cqrs;
//using LinqToDB;
//using O24OpenAPI.Framework.Models;
//using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

//namespace O24OpenAPI.CTH.API.Application.Features.User
//{
//    public class GetByRoleTypeCommnad: BaseTransactionModel, ICommand<List<int>>
//    {

//    }

//    public class GetByRoleTypeHandler(IUserRoleRepository userRoleRepository) : ICommandHandler<GetByRoleTypeCommnad, List<int>>
//    {
//        public async Task<List<int>> HandleAsync(GetByRoleTypeCommnad request, CancellationToken cancellationToken = default)
//        {
//        return await userRoleRepository.Table
//                .Where(c => c.RoleType == roletype)
//                .Select(x => x.RoleId)
//                .Distinct()
//                .ToListAsync();
//        }
//    }
//}

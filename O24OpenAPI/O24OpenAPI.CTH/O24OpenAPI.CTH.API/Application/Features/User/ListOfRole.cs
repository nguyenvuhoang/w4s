//using LinKit.Core.Cqrs;
//using LinqToDB;
//using O24OpenAPI.Framework.Models;
//using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

//namespace O24OpenAPI.CTH.API.Application.Features.User
//{
//    public class ListOfRoleCommnad: BaseTransactionModel, ICommand<List<UserInRole>>
//    {

//    }

//    public class ListOfRoleHandler(IUserInRoleRepository userInRoleRepository) : ICommandHandler<ListOfRoleCommnad, List<UserInRole>>
//    {
//        public async Task<List<UserInRole>> HandleAsync(ListOfRoleCommnad request, CancellationToken cancellationToken = default)
//        {
//        var listOfRole = await userInRoleRepository.Table.Where(s => s.UserCode == usercode).ToListAsync();
//            return listOfRole;
//        }
//    }
//}

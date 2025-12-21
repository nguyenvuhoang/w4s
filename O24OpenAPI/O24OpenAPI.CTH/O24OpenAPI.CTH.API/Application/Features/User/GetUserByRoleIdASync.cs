//using LinKit.Core.Cqrs;
//using LinqToDB;
//using O24OpenAPI.CTH.Constant;
//using O24OpenAPI.Framework.Models;
//using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
//using O24OpenAPI.CTH.API.Application.Constants;

//namespace O24OpenAPI.CTH.API.Application.Features.User
//{
//    public class GetUserByRoleIdASyncCommnad: BaseTransactionModel, ICommand<IPagedList<UserAccount>>
//    {
//        public int RoleId { get; set; }
//    }

//    public class GetUserByRoleIdASyncHandler(IUserAccountRepository userAccountRepository, IUserInRoleRepository userInRoleRepository) : ICommandHandler<GetUserByRoleIdASyncCommnad, IPagedList<UserAccount>>
//    {
//        public async Task<IPagedList<UserAccount>> HandleAsync(GetUserByRoleIdASyncCommnad request, CancellationToken cancellationToken = default)
//        {
//        var userList = await userInRoleRepository.GetUserInRolesByRoleIdAsync(request.RoleId);
//            if (userList == null || userList.Count == 0)
//            {
//                throw await O24Exception.CreateAsync(
//                    O24CTHResourceCode.Validation.UserNotFoundByRoleId,
//                    request.Language,
//                    [request.RoleId.ToString()]
//                );
//            }

//            var userCodes = userList.Select(u => u.UserCode).ToList();

//            var users = await userAccountRepository
//                .Table.Where(s => userCodes.Contains(s.UserCode))
//                .ToListAsync();

//            var pageList = await users.AsQueryable().ToPagedList(request.PageIndex, request.PageSize);
//            return pageList;
//        }
//    }
//}

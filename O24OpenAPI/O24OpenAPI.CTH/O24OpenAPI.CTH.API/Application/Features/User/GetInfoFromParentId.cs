//using LinKit.Core.Cqrs;
//using LinqToDB;
//using O24OpenAPI.Framework.Models;
//using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

//namespace O24OpenAPI.CTH.API.Application.Features.User
//{
//    public class GetInfoFromParentIdCommnad: BaseTransactionModel, ICommand<virtual Task<List<UserCommandResponseModel>>>
//    {

//    }

//    public class GetInfoFromParentIdHandler(IUserCommandRepository userCommandRepository, IUserRoleRepository userRoleRepository, IUserRightRepository userRightRepository) : ICommandHandler<GetInfoFromParentIdCommnad, virtual Task<List<UserCommandResponseModel>>>
//    {
//        public async Task<virtual Task<List<UserCommandResponseModel>>> HandleAsync(GetInfoFromParentIdCommnad request, CancellationToken cancellationToken = default)
//        {
//        var listLeftJoin = await (
//                from userCommand in userCommandRepository.Table.Where(s =>
//                    s.ApplicationCode == applicationCode && s.ParentId == parentId && s.Enabled
//                )
//                from userRole in userRoleRepository.Table.DefaultIfEmpty()
//                from userRight in userRightRepository
//                    .Table.Where(s => s.CommandId == userCommand.CommandId && s.RoleId == userRole.Id)
//                    .DefaultIfEmpty()

//                select new UserCommandResponseModel()
//                {
//                    ParentId = userCommand.ParentId,
//                    CommandId = userCommand.CommandId,
//                    CommandNameLanguage = userCommand.CommandName,
//                    Icon = userCommand.GroupMenuIcon,
//                    GroupMenuVisible = userCommand.GroupMenuVisible,
//                    GroupMenuId = userCommand.GroupMenuId,
//                }
//            ).OrderBy(s => s.DisplayOrder).ThenBy(s => s.ParentId).ThenBy(s => s.CommandId).ToListAsync();

//            return listLeftJoin;
//        }
//    }
//}

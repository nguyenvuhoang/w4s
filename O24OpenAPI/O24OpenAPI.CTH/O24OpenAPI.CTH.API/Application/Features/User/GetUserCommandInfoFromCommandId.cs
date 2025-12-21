//using LinKit.Core.Cqrs;
//using LinqToDB;
//using O24OpenAPI.Framework.Models;
//using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

//namespace O24OpenAPI.CTH.API.Application.Features.User
//{
//    public class GetUserCommandInfoFromCommandIdCommnad: BaseTransactionModel, ICommand<virtual Task<List<CommandIdInfoModel>>>
//    {

//    }

//    public class GetUserCommandInfoFromCommandIdHandler(IUserCommandRepository userCommandRepository, IUserRoleRepository userRoleRepository, IUserRightRepository userRightRepository) : ICommandHandler<GetUserCommandInfoFromCommandIdCommnad, virtual Task<List<CommandIdInfoModel>>>
//    {
//        public async Task<virtual Task<List<CommandIdInfoModel>>> HandleAsync(GetUserCommandInfoFromCommandIdCommnad request, CancellationToken cancellationToken = default)
//        {
//        var listLeftJoin = await (
//                from userCommand in userCommandRepository.Table.Where(s =>
//                    s.ApplicationCode == applicationCode && s.CommandId == commandId && s.Enabled
//                )
//                from userRole in userRoleRepository.Table.DefaultIfEmpty()
//                from userRight in userRightRepository
//                    .Table.Where(s => s.CommandId == commandId && s.RoleId == userRole.RoleId)
//                    .DefaultIfEmpty()

//                select new CommandIdInfoModel()
//                {
//                    ParentId = userCommand.ParentId,
//                    CommandId = userCommand.CommandId,
//                    CommandName = userCommand.CommandName,
//                    ApplicationCode = userCommand.ApplicationCode,
//                    CommandType = userCommand.CommandType,
//                    RoleId = userRole.RoleId,
//                    RoleName = userRole.RoleName,
//                    CommandIdDetail =
//                        (
//                            userCommand.CommandId == userRight.CommandId
//                            && userRole.RoleId == userRight.RoleId
//                        )
//                            ? userRight.CommandIdDetail
//                            : null,
//                    Invoke =
//                        (
//                            userCommand.CommandId == userRight.CommandId
//                            && userRole.RoleId == userRight.RoleId
//                        )
//                            ? userRight.Invoke
//                            : 0,
//                    Approve =
//                        (
//                            userCommand.CommandId == userRight.CommandId
//                            && userRole.RoleId == userRight.RoleId
//                        )
//                            ? userRight.Approve
//                            : 0,
//                    GroupMenuIcon = userCommand.GroupMenuIcon,
//                    GroupMenuVisible = userCommand.GroupMenuVisible,
//                    GroupMenuId = userCommand.GroupMenuId,
//                    GroupMenuListAuthorizeForm = userCommand.GroupMenuListAuthorizeForm,
//                }
//            ).OrderBy(s => s.RoleId).ThenBy(s => s.ParentId).ThenBy(s => s.CommandId).ToListAsync();

//            return listLeftJoin;
//        }
//    }
//}

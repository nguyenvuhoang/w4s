//using LinKit.Core.Cqrs;
//using LinqToDB;
//using O24OpenAPI.Framework.Models;
//using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

//namespace O24OpenAPI.CTH.API.Application.Features.User
//{
//    public class GetInfoFromCommandIdCommnad: BaseTransactionModel, ICommand<virtual Task<List<UserCommandResponseModel>>>
//    {

//    }

//    public class GetInfoFromCommandIdHandler(IUserCommandRepository userCommandRepository, IUserRoleRepository userRoleRepository, IUserRightRepository userRightRepository) : ICommandHandler<GetInfoFromCommandIdCommnad, virtual Task<List<UserCommandResponseModel>>>
//    {
//        public async Task<virtual Task<List<UserCommandResponseModel>>> HandleAsync(GetInfoFromCommandIdCommnad request, CancellationToken cancellationToken = default)
//        {
//        var listLeftJoin = await (
//                from userCommand in userCommandRepository.Table
//                join userRight in userRightRepository.Table
//                    on userCommand.CommandId equals userRight.CommandId
//                join userRole in userRoleRepository.Table on userRight.RoleId equals userRole.RoleId
//                where
//                    userCommand.ApplicationCode == applicationCode
//                    && userCommand.CommandId == commandId
//                    && userCommand.Enabled
//                select new UserCommandResponseModel
//                {
//                    ParentId = userCommand.ParentId,
//                    CommandId = userCommand.CommandId,
//                    CommandNameLanguage = userCommand.CommandName,
//                    Icon = userCommand.GroupMenuIcon,
//                    GroupMenuVisible = userCommand.GroupMenuVisible,
//                    GroupMenuId = userCommand.GroupMenuId,
//                }
//            ).ToListAsync();

//            return listLeftJoin;
//        }
//    }
//}

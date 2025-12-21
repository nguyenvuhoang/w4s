//using LinKit.Core.Cqrs;
//using LinqToDB;
//using O24OpenAPI.Framework.Models;
//using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

//namespace O24OpenAPI.CTH.API.Application.Features.User
//{
//    public class GetMenuInfoFromRoleIdCommnad: BaseTransactionModel, ICommand<List<CommandHierarchyModel>>
//    {

//    }

//    public class GetMenuInfoFromRoleIdHandler(IUserCommandRepository userCommandRepository, IUserRoleRepository userRoleRepository, IUserRightRepository userRightRepository) : ICommandHandler<GetMenuInfoFromRoleIdCommnad, List<CommandHierarchyModel>>
//    {
//        public async Task<List<CommandHierarchyModel>> HandleAsync(GetMenuInfoFromRoleIdCommnad request, CancellationToken cancellationToken = default)
//        {
//        var userRights = await userRightRepository
//                .Table.Where(ur => ur.RoleId == roleId && ur.Invoke == 1)
//                .ToListAsync();

//            var commandIds = userRights.Select(ur => ur.CommandId).Distinct().ToList();

//            var userCommands = await userCommandRepository
//                .Table.Where(uc =>
//                    commandIds.Contains(uc.CommandId)
//                    && uc.ApplicationCode == applicationCode
//                    && uc.Enabled
//                )
//                .OrderBy(uc => uc.DisplayOrder)
//                .ToListAsync();

//            var userRole = await userRoleRepository
//                .Table.Where(r => r.RoleId == roleId)
//                .FirstOrDefaultAsync();

//            var result = (
//                from uc in userCommands
//                join ur in userRights on uc.CommandId equals ur.CommandId
//                select new CommandHierarchyModel
//                {
//                    ParentId = uc.ParentId,
//                    CommandId = uc.CommandId,
//                    Label = Utils.StringExtensions.TryGetLabelFromJson(uc.CommandNameLanguage, lang),
//                    CommandType = uc.CommandType,
//                    CommandUri = uc.CommandURI,
//                    RoleId = roleId,
//                    RoleName = userRole?.RoleName,
//                    Invoke = ur.Invoke == 1,
//                    Approve = ur.Approve == 1,
//                    Icon = uc.GroupMenuIcon,
//                    GroupMenuVisible = uc.GroupMenuVisible,
//                    GroupMenuId = uc.GroupMenuId,
//                    GroupMenuListAuthorizeForm = uc.GroupMenuListAuthorizeForm,
//                }
//            ).ToList();

//            foreach (var item in result)
//            {
//                item.IsAgreement = await IsUserAgreement(item.CommandId);
//            }

//            return result;
//        }
//    }
//}

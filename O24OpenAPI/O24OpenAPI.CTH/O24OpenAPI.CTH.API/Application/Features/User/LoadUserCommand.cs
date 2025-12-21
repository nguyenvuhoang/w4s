//using LinKit.Core.Cqrs;
//using LinqToDB;
//using O24OpenAPI.Framework.Models;
//using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

//namespace O24OpenAPI.CTH.API.Application.Features.User
//{
//    public class LoadUserCommandCommnad: BaseTransactionModel, ICommand<List<UserCommand>>
//    {

//    }

//    public class LoadUserCommandHandler(IUserCommandRepository userCommandRepository) : ICommandHandler<LoadUserCommandCommnad, List<UserCommand>>
//    {
//        public async Task<List<UserCommand>> HandleAsync(LoadUserCommandCommnad request, CancellationToken cancellationToken = default)
//        {
//        var commandListHashSet = roleCommand.JsonConvertToModel<HashSet<string>>();

//            return await userCommandRepository
//                .Table.Where(s =>
//                    s.ApplicationCode == applicationCode
//                    && roleCommand.Contains(s.CommandId)
//                    && s.IsVisible
//                )
//                .Select(s => new UserCommand
//                {
//                    ApplicationCode = s.ApplicationCode,
//                    ParentId = s.ParentId,
//                    CommandId = s.CommandId,
//                    CommandName = s.CommandName,
//                    CommandNameLanguage = s.CommandNameLanguage,
//                    CommandType = s.CommandType,
//                    CommandURI = s.CommandURI,
//                    Enabled = s.Enabled,
//                    DisplayOrder = s.DisplayOrder,
//                    GroupMenuIcon = s.GroupMenuIcon,
//                    GroupMenuVisible = s.GroupMenuVisible,
//                    GroupMenuId = s.GroupMenuId,
//                })
//                .ToListAsync();
//        }
//    }
//}

//using LinKit.Core.Cqrs;
//using LinqToDB;
//using O24OpenAPI.Framework.Models;
//using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

//namespace O24OpenAPI.CTH.API.Application.Features.User
//{
//    public class LoadFullUserCommandsCommnad: BaseTransactionModel, ICommand<List<CTHUserCommandModel>>
//    {

//    }

//    public class LoadFullUserCommandsHandler(IUserCommandRepository userCommandRepository) : ICommandHandler<LoadFullUserCommandsCommnad, List<CTHUserCommandModel>>
//    {
//        public async Task<List<CTHUserCommandModel>> HandleAsync(LoadFullUserCommandsCommnad request, CancellationToken cancellationToken = default)
//        {
//        try
//            {
//                var listUserCommandDomain = await userCommandRepository
//                    .Table.OrderBy(x => x.ApplicationCode)
//                    .ThenBy(x => x.CommandId)
//                    .ThenBy(x => x.DisplayOrder)
//                    .ToListAsync();

//                var listUserCommand = listUserCommandDomain
//                    .Select(x => new CTHUserCommandModel
//                    {
//                        ApplicationCode = x.ApplicationCode,
//                        CommandId = x.CommandId,
//                        ParentId = x.ParentId,
//                        CommandName = x.CommandName,
//                        CommandNameLanguage = x.CommandNameLanguage,
//                        CommandType = x.CommandType,
//                        CommandURI = x.CommandURI ?? "",
//                        Enabled = x.Enabled,
//                        IsVisible = x.IsVisible,
//                        DisplayOrder = x.DisplayOrder,
//                        GroupMenuIcon = x.GroupMenuIcon,
//                        GroupMenuVisible = x.GroupMenuVisible,
//                        GroupMenuId = x.GroupMenuId ?? "",
//                        GroupMenuListAuthorizeForm = x.GroupMenuListAuthorizeForm ?? "",
//                    })
//                    .ToList();

//                return listUserCommand;
//            }
//            catch (Exception ex)
//            {
//                await ex.LogErrorAsync();
//                BusinessLogHelper.Error(ex, "Error while loading full user commands");
//                return [];
//            }
//        }
//    }
//}

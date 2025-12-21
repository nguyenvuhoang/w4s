//using LinKit.Core.Cqrs;
//using LinqToDB;
//using O24OpenAPI.Framework.Models;
//using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

//namespace O24OpenAPI.CTH.API.Application.Features.User
//{
//    public class GetInfoFromFormCodeCommnad: BaseTransactionModel, ICommand<virtual Task<List<UserCommand>>>
//    {

//    }

//    public class GetInfoFromFormCodeHandler(IUserCommandRepository userCommandRepository) : ICommandHandler<GetInfoFromFormCodeCommnad, virtual Task<List<UserCommand>>>
//    {
//        public async Task<virtual Task<List<UserCommand>>> HandleAsync(GetInfoFromFormCodeCommnad request, CancellationToken cancellationToken = default)
//        {
//        var result = await userCommandRepository
//                .Table.Where(s =>
//                    s.ApplicationCode == applicationCode
//                    && (s.GroupMenuId == formCode)
//                    && s.Enabled == true
//                )
//                .ToListAsync();
//            return result;
//        }
//    }
//}

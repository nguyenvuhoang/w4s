//using LinKit.Core.Cqrs;
//using LinqToDB;
//using O24OpenAPI.Framework.Models;
//using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

//namespace O24OpenAPI.CTH.API.Application.Features.User
//{
//    public class GetActiveByLoginNameCommnad: BaseTransactionModel, ICommand<UserSession>
//    {

//    }

//    public class GetActiveByLoginNameHandler(IUserSessionRepository userSessionRepository) : ICommandHandler<GetActiveByLoginNameCommnad, UserSession>
//    {
//        public async Task<UserSession> HandleAsync(GetActiveByLoginNameCommnad request, CancellationToken cancellationToken = default)
//        {
//        return await userSessionRepository
//                .Table.Where(x =>
//                    x.LoginName == loginName && !x.IsRevoked && x.ExpiresAt > DateTime.UtcNow
//                )
//                .FirstOrDefaultAsync();
//        }
//    }
//}

//using LinKit.Core.Cqrs;
//using LinqToDB;
//using O24OpenAPI.Framework.Models;
//using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

//namespace O24OpenAPI.CTH.API.Application.Features.User
//{
//    public class GetByRefreshTokenCommnad: BaseTransactionModel, ICommand<UserSession>
//    {

//    }

//    public class GetByRefreshTokenHandler(IUserSessionRepository userSessionRepository) : ICommandHandler<GetByRefreshTokenCommnad, UserSession>
//    {
//        public async Task<UserSession> HandleAsync(GetByRefreshTokenCommnad request, CancellationToken cancellationToken = default)
//        {
//        var hashed = token.Hash();
//            return await userSessionRepository
//                .Table.Where(s => s.RefreshToken == hashed)
//                .FirstOrDefaultAsync();
//        }
//    }
//}

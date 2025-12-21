//using LinKit.Core.Cqrs;
//using LinqToDB;
//using O24OpenAPI.Framework.Models;
//using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

//namespace O24OpenAPI.CTH.API.Application.Features.User
//{
//    public class GetByTokenCommnad: BaseTransactionModel, ICommand<virtual Task<UserSession>>
//    {

//    }

//    public class GetByTokenHandler(IUserSessionRepository userSessionRepository) : ICommandHandler<GetByTokenCommnad, virtual Task<UserSession>>
//    {
//        public async Task<virtual Task<UserSession>> HandleAsync(GetByTokenCommnad request, CancellationToken cancellationToken = default)
//        {
//        var cacheKey = new CacheKey(token);
//            var session = await _staticCacheManager.Get<UserSession>(cacheKey);

//            if (session == null || session.Id == 0)
//            {
//                var hashedToken = token.Hash();
//                var query = userSessionRepository.Table.Where(s => s.Token == hashedToken);

//                if (activeOnly)
//                {
//                    query = query.Where(s => !s.IsRevoked && s.ExpiresAt > DateTime.UtcNow);
//                }

//                session = await query.FirstOrDefaultAsync();

//                if (session != null)
//                {
//                    await _staticCacheManager.Set(cacheKey, session);
//                }
//            }

//            return session;
//        }
//    }
//}

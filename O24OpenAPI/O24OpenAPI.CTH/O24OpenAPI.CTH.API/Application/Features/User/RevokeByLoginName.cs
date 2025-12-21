//using LinKit.Core.Cqrs;
//using LinqToDB;
//using O24OpenAPI.Framework.Models;
//using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

//namespace O24OpenAPI.CTH.API.Application.Features.User
//{
//    public class RevokeByLoginNameCommnad: BaseTransactionModel, ICommand<bool>
//    {

//    }

//    public class RevokeByLoginNameHandler(IUserSessionRepository userSessionRepository) : ICommandHandler<RevokeByLoginNameCommnad, bool>
//    {
//        public async Task<bool> HandleAsync(RevokeByLoginNameCommnad request, CancellationToken cancellationToken = default)
//        {
//        var sessions = await userSessionRepository
//                .Table.Where(x => x.LoginName == loginName && !x.IsRevoked)
//                .ToListAsync();

//            foreach (var session in sessions)
//            {
//                session.IsRevoked = true;
//                session.ExpiresAt = DateTime.UtcNow;
//                await userSessionRepository.Update(session);
//                await _staticCacheManager.Remove(new CacheKey(session.Token));
//            }
//        }
//    }
//}

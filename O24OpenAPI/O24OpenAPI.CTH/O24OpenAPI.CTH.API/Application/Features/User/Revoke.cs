//using LinqToDB;
//using O24OpenAPI.Framework.Models;
//using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

//namespace O24OpenAPI.CTH.API.Application.Features.User
//{
//    public class RevokeCommnad: BaseTransactionModel, ICommand<virtual Task>
//    {

//    }

//    public class RevokeHandler(IUserSessionRepository userSessionRepository) : ICommandHandler<RevokeCommnad, virtual Task>
//    {
//        public async Task<virtual Task> HandleAsync(RevokeCommnad request, CancellationToken cancellationToken = default)
//        {
//        var userSession =
//                await GetByToken(token) ?? throw new O24OpenAPIException("Invalid session.");

//            userSession.Revoke();
//            await _staticCacheManager.Remove(new CacheKey(token));

//            await userSessionRepository.Update(userSession);
//        }
//    }
//}

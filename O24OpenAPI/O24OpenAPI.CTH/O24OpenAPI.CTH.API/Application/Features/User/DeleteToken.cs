//using LinqToDB;
//using O24OpenAPI.Framework.Models;
//using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

//namespace O24OpenAPI.CTH.API.Application.Features.User
//{
//    public class DeleteTokenCommnad: BaseTransactionModel, ICommand<virtual Task>
//    {

//    }

//    public class DeleteTokenHandler(IUserSessionRepository userSessionRepository) : ICommandHandler<DeleteTokenCommnad, virtual Task>
//    {
//        public async Task<virtual Task> HandleAsync(DeleteTokenCommnad request, CancellationToken cancellationToken = default)
//        {
//        var userSession = await GetByToken(token);

//            if (userSession == null)
//            {
//                throw new ArgumentNullException(nameof(userSession));
//            }

//            await _staticCacheManager.Remove(new CacheKey(token));

//            await userSessionRepository.Delete(userSession);
//        }
//    }
//}

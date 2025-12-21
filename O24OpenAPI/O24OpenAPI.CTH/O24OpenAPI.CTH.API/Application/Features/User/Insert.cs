//using LinqToDB;
//using O24OpenAPI.Framework.Models;
//using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

//namespace O24OpenAPI.CTH.API.Application.Features.User
//{
//    public class InsertCommnad: BaseTransactionModel, ICommand<virtual Task>
//    {

//    }

//    public class InsertHandler(IUserSessionRepository userSessionRepository) : ICommandHandler<InsertCommnad, virtual Task>
//    {
//        public async Task<virtual Task> HandleAsync(InsertCommnad request, CancellationToken cancellationToken = default)
//        {
//        ArgumentNullException.ThrowIfNull(userSession);
//            await userSessionRepository.Insert(userSession);
//            // var sessionModel = userSession.ToModel<UserSessionModel>();
//            await _staticCacheManager.Set(CachingKey.SessionKey(userSession.Token), userSession);
//        }
//    }
//}

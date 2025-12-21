//using LinKit.Core.Cqrs;
//using LinqToDB;
//using O24OpenAPI.Framework.Models;
//using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

//namespace O24OpenAPI.CTH.API.Application.Features.User
//{
//    public class GetByUserAuthenInfoCommnad: BaseTransactionModel, ICommand<UserAuthen>
//    {

//    }

//    public class GetByUserAuthenInfoHandler(IUserAuthenRepository userAuthenRepository) : ICommandHandler<GetByUserAuthenInfoCommnad, UserAuthen>
//    {
//        public async Task<UserAuthen> HandleAsync(GetByUserAuthenInfoCommnad request, CancellationToken cancellationToken = default)
//        {
//        return await userAuthenRepository.Table.Where(
//                s => s.UserCode == userCode
//                && s.AuthenType == authenType
//                && s.Phone == phoneNumber).
//                FirstOrDefaultAsync();
//        }
//    }
//}

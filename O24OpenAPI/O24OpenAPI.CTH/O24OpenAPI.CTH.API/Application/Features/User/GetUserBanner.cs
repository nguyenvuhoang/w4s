//using LinKit.Core.Cqrs;
//using LinqToDB;
//using O24OpenAPI.Framework.Models;
//using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

//namespace O24OpenAPI.CTH.API.Application.Features.User
//{
//    public class GetUserBannerCommnad: BaseTransactionModel, ICommand<string>
//    {

//    }

//    public class GetUserBannerHandler(IUserBannerRepository userBannerRepository) : ICommandHandler<GetUserBannerCommnad, string>
//    {
//        public async Task<string> HandleAsync(GetUserBannerCommnad request, CancellationToken cancellationToken = default)
//        {
//        try
//            {
//                return userBannerRepository
//                        .Table.Where(x => x.UserCode == usercode)
//                        .Select(x => x.BannerSource)
//                        .FirstOrDefault() ?? "default";
//            }
//            catch (Exception ex)
//            {
//                await ex.LogErrorAsync();
//                return string.Empty;
//            }
//        }
//    }
//}

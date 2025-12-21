//using LinKit.Core.Cqrs;
//using LinqToDB;
//using O24OpenAPI.Framework.Models;
//using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

//namespace O24OpenAPI.CTH.API.Application.Features.User
//{
//    public class GetUserLimitToUpdateCommnad: BaseTransactionModel, ICommand<virtual Task<UserLimit>>
//    {
//        public int RoleId { get; set; }
//        public string CommandId { get; set; }
//        public string CurrencyCode { get; set; }
//        public decimal? ULimit { get; set; }
//        public string LimitType { get; set; }
//    }

//    public class GetUserLimitToUpdateHandler(IUserLimitRepository userLimitRepository) : ICommandHandler<GetUserLimitToUpdateCommnad, virtual Task<UserLimit>>
//    {
//        public async Task<virtual Task<UserLimit>> HandleAsync(GetUserLimitToUpdateCommnad request, CancellationToken cancellationToken = default)
//        {
//        var uLimit = await userLimitRepository
//                .Table.Where(s =>
//                    s.RoleId == request.RoleId
//                    && s.CommandId == request.CommandId
//                    && s.CurrencyCode == request.CurrencyCode
//                    && s.LimitType == request.LimitType
//                )
//                .FirstOrDefaultAsync();
//            return uLimit;
//        }
//    }
//}

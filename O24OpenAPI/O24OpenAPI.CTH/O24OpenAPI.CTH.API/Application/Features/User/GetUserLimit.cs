//using LinKit.Core.Cqrs;
//using LinqToDB;
//using O24OpenAPI.Framework.Models;
//using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

//namespace O24OpenAPI.CTH.API.Application.Features.User
//{
//    public class GetUserLimitCommnad: BaseTransactionModel, ICommand<decimal>
//    {

//    }

//    public class GetUserLimitHandler(IUserLimitRepository userLimitRepository, IUserInRoleRepository userInRoleRepository) : ICommandHandler<GetUserLimitCommnad, decimal>
//    {
//        public async Task<decimal> HandleAsync(GetUserLimitCommnad request, CancellationToken cancellationToken = default)
//        {
//        var query =
//                from l in userLimitRepository.Table
//                join r in userInRoleRepository.Table on l.RoleId equals r.RoleId
//                where
//                    r.UserCode == user.UserCode && l.CommandId == commandId && l.LimitType == limitType
//                select l;
//            var limits = await query.ToListAsync();
//            if (limits.Any())
//            {
//                //Add condition
//                if (limits.All(s => s.ULimit == null))
//                {
//                    throw new Exception("Unauthorized");
//                }

//                var limit = limits.Max(l => l.ULimit) ?? 0;
//                return limit;
//            }
//            return 0;
//        }
//    }
//}

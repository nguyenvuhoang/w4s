//using LinKit.Core.Cqrs;
//using LinqToDB;
//using O24OpenAPI.Framework.Models;
//using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

//namespace O24OpenAPI.CTH.API.Application.Features.User
//{
//    public class IsUserAgreementCommnad: BaseTransactionModel, ICommand<bool>
//    {

//    }

//    public class IsUserAgreementHandler(IUserAgreementRepository userAgreementRepository) : ICommandHandler<IsUserAgreementCommnad, bool>
//    {
//        public async Task<bool> HandleAsync(IsUserAgreementCommnad request, CancellationToken cancellationToken = default)
//        {
//        var isAgreement = await userAgreementRepository
//                .Table.Where(ua => ua.TransactionCode.Contains(commandid) && ua.IsActive)
//                .FirstOrDefaultAsync();
//            if (isAgreement != null)
//            {
//                return true;
//            }
//            return false;
//        }
//    }
//}

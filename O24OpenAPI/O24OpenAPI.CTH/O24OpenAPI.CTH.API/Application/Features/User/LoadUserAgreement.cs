//using LinKit.Core.Cqrs;
//using LinqToDB;
//using O24OpenAPI.Framework.Models;
//using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

//namespace O24OpenAPI.CTH.API.Application.Features.User
//{
//    public class LoadUserAgreementCommnad: BaseTransactionModel, ICommand<UserAgreement>
//    {
//        public new string TransactionCode { get; set; }
//    }

//    public class LoadUserAgreementHandler(IUserAgreementRepository userAgreementRepository) : ICommandHandler<LoadUserAgreementCommnad, UserAgreement>
//    {
//        public async Task<UserAgreement> HandleAsync(LoadUserAgreementCommnad request, CancellationToken cancellationToken = default)
//        {
//        return await userAgreementRepository.Table
//                .Where(s => s.IsActive && s.TransactionCode == request.TransactionCode)
//                .FirstOrDefaultAsync();
//        }
//    }
//}

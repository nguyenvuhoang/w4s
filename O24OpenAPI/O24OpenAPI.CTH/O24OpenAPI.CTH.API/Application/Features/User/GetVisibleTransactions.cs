//using LinKit.Core.Cqrs;
//using LinqToDB;
//using O24OpenAPI.Framework.Models;
//using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

//namespace O24OpenAPI.CTH.API.Application.Features.User
//{
//    public class GetVisibleTransactionsCommnad: BaseTransactionModel, ICommand<List<VisibleTransactionResponse>>
//    {

//    }

//    public class GetVisibleTransactionsHandler(IUserCommandRepository userCommandRepository) : ICommandHandler<GetVisibleTransactionsCommnad, List<VisibleTransactionResponse>>
//    {
//        public async Task<List<VisibleTransactionResponse>> HandleAsync(GetVisibleTransactionsCommnad request, CancellationToken cancellationToken = default)
//        {
//        var q =
//                from userCommand in userCommandRepository.Table.Where(s =>
//                    s.ApplicationCode == channelId
//                    && s.Enabled
//                    && s.IsVisible
//                    && s.CommandType == "T"
//                    && s.ParentId != "0"
//                )
//                select new VisibleTransactionResponse
//                {
//                    TransactionCode = userCommand.CommandId,
//                    TransactionName = userCommand.CommandName,
//                    TransactionNameLanguage = userCommand.CommandNameLanguage,
//                    ModuleCode = userCommand.ParentId,
//                };
//            return await q.ToListAsync();
//        }
//    }
//}

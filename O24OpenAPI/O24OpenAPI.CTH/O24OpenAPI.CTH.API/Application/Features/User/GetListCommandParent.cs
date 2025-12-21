//using LinKit.Core.Cqrs;
//using LinqToDB;
//using O24OpenAPI.Framework.Models;
//using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

//namespace O24OpenAPI.CTH.API.Application.Features.User
//{
//    public class GetListCommandParentCommnad: BaseTransactionModel, ICommand<List<string>>
//    {

//    }

//    public class GetListCommandParentHandler(IUserCommandRepository userCommandRepository) : ICommandHandler<GetListCommandParentCommnad, List<string>>
//    {
//        public async Task<List<string>> HandleAsync(GetListCommandParentCommnad request, CancellationToken cancellationToken = default)
//        {
//        return await userCommandRepository
//                .Table.Where(s =>
//                    s.ApplicationCode == applicationCode
//                    && s.CommandType == "M"
//                    && s.Enabled
//                    && s.ParentId == "0"
//                )
//                .Select(s => s.CommandId)
//                .Distinct()
//                .ToListAsync();
//        }
//    }
//}

//using LinKit.Core.Cqrs;
//using LinqToDB;
//using O24OpenAPI.Framework.Models;
//using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

//namespace O24OpenAPI.CTH.API.Application.Features.User
//{
//    public class GetCommandMenuByChannelCommnad: BaseTransactionModel, ICommand<List<UserCommandResponseModel>>
//    {

//    }

//    public class GetCommandMenuByChannelHandler(IUserCommandRepository userCommandRepository) : ICommandHandler<GetCommandMenuByChannelCommnad, List<UserCommandResponseModel>>
//    {
//        public async Task<List<UserCommandResponseModel>> HandleAsync(GetCommandMenuByChannelCommnad request, CancellationToken cancellationToken = default)
//        {
//        return await userCommandRepository
//                .Table.Where(s => s.ApplicationCode == channelId && s.CommandType == "M" && s.Enabled)
//                .Select(s => new UserCommandResponseModel(s))
//                .ToListAsync();
//        }
//    }
//}

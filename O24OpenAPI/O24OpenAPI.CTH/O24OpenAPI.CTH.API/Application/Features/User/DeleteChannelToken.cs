//using LinqToDB;
//using O24OpenAPI.Framework.Models;
//using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

//namespace O24OpenAPI.CTH.API.Application.Features.User
//{
//    public class DeleteChannelTokenCommnad: BaseTransactionModel, ICommand<virtual Task>
//    {

//    }

//    public class DeleteChannelTokenHandler(IUserSessionRepository userSessionRepository) : ICommandHandler<DeleteChannelTokenCommnad, virtual Task>
//    {
//        public async Task<virtual Task> HandleAsync(DeleteChannelTokenCommnad request, CancellationToken cancellationToken = default)
//        {
//        Dictionary<string, string> searchInput = new() { { "ChannelId", channelId } };

//            await userSessionRepository.FilterAndDelete(searchInput);
//        }
//    }
//}

//using LinKit.Core.Cqrs;
//using LinqToDB;
//using O24OpenAPI.Framework.Models;
//using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

//namespace O24OpenAPI.CTH.API.Application.Features.User
//{
//    public class AddUserRightCommnad: BaseTransactionModel, ICommand<UserRight>
//    {

//    }

//    public class AddUserRightHandler(IUserRightRepository userRightRepository) : ICommandHandler<AddUserRightCommnad, UserRight>
//    {
//        public async Task<UserRight> HandleAsync(AddUserRightCommnad request, CancellationToken cancellationToken = default)
//        {
//        return await userRightRepository.InsertAsync(entity);
//        }
//    }
//}

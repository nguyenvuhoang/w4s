//using LinKit.Core.Cqrs;
//using O24OpenAPI.Framework.Models;
//using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

//namespace O24OpenAPI.CTH.API.Application.Features.User
//{
//    public class DeleteBulkCommnad: BaseTransactionModel, ICommand<bool>
//    {

//    }

//    public class DeleteBulkHandler(IUserInRoleRepository userInRoleRepository) : ICommandHandler<DeleteBulkCommnad, bool>
//    {
//        public async Task<bool> HandleAsync(DeleteBulkCommnad request, CancellationToken cancellationToken = default)
//        {
//        await userInRoleRepository.BulkDelete(listUserInRole);
//            return true;
//        }
//    }
//}

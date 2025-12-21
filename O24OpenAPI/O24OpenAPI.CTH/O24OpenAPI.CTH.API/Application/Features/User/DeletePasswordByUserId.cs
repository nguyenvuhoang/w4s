//using LinKit.Core.Cqrs;
//using LinqToDB;
//using O24OpenAPI.Framework.Models;
//using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

//namespace O24OpenAPI.CTH.API.Application.Features.User
//{
//    public class DeletePasswordByUserIdCommnad: BaseTransactionModel, ICommand<bool>
//    {

//    }

//    public class DeletePasswordByUserIdHandler(IUserPasswordRepository userPasswordRepository) : ICommandHandler<DeletePasswordByUserIdCommnad, bool>
//    {
//        public async Task<bool> HandleAsync(DeletePasswordByUserIdCommnad request, CancellationToken cancellationToken = default)
//        {
//        var entity = await userPasswordRepository.Table
//            .FirstOrDefaultAsync(x => x.UserId == userId);

//            if (entity != null)
//            {
//                await userPasswordRepository.Delete(entity);
//            }
//        }
//    }
//}

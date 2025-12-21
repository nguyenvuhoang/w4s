//using LinKit.Core.Cqrs;
//using LinqToDB;
//using O24OpenAPI.CTH.Constant;
//using O24OpenAPI.Framework.Models;
//using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
//using O24OpenAPI.CTH.API.Application.Constants;

//namespace O24OpenAPI.CTH.API.Application.Features.User
//{
//    public class DeleteUserByIdCommnad: BaseTransactionModel, ICommand<bool>
//    {
//        public int Id { get; set; }
//    }

//    public class DeleteUserByIdHandler(IUserAccountRepository userAccountRepository, IUserInRoleRepository userInRoleRepository, IUserPasswordRepository userPasswordRepository) : ICommandHandler<DeleteUserByIdCommnad, bool>
//    {
//        public async Task<bool> HandleAsync(DeleteUserByIdCommnad request, CancellationToken cancellationToken = default)
//        {
//        var entity = await userAccountRepository.Table.FirstOrDefaultAsync(x => x.Id == request.Id);

//            if (entity != null)
//            {
//                if (entity.LoginName == request.CurrentLoginname)
//                {
//                    throw await O24Exception.CreateAsync(
//                        O24CTHResourceCode.Operation.DeleteOwnUserError,
//                        request.Language
//                    );
//                }
//                await userInRoleRepository.DeleteByUserCodeAsync(entity.UserId);
//                await userPasswordRepository.DeletePasswordByUserIdAsync(entity.UserId);
//                await DeleteUserByUserIdAsync(entity.UserId);
//                return true;
//            }
//            return false;
//        }
//    }
//}

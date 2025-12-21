//using LinKit.Core.Cqrs;
//using O24OpenAPI.Framework.Models;
//using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

//namespace O24OpenAPI.CTH.API.Application.Features.User
//{
//    public class GetByIdCommnad: BaseTransactionModel, ICommand<virtual Task<UserDevice>>
//    {

//    }

//    public class GetByIdHandler(IUserDeviceRepository userDeviceRepository) : ICommandHandler<GetByIdCommnad, virtual Task<UserDevice>>
//    {
//        public async Task<virtual Task<UserDevice>> HandleAsync(GetByIdCommnad request, CancellationToken cancellationToken = default)
//        {
//        return await userDeviceRepository.GetById(id, cache => null);
//        }
//    }
//}

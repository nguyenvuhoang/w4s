//using LinKit.Core.Cqrs;
//using LinqToDB;
//using O24OpenAPI.Framework.Models;
//using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
//using O24OpenAPI.Core;

//namespace O24OpenAPI.CTH.API.Application.Features.User
//{
//    public class SimpleSearchCommnad: BaseTransactionModel, ICommand<IPagedList<UserDevice>>
//    {

//    }

//    public class SimpleSearchHandler(IUserDeviceRepository userDeviceRepository) : ICommandHandler<SimpleSearchCommnad, IPagedList<UserDevice>>
//    {
//        public async Task<IPagedList<UserDevice>> HandleAsync(SimpleSearchCommnad request, CancellationToken cancellationToken = default)
//        {
//        var query =
//                from d in userDeviceRepository.Table
//                where
//                    (!string.IsNullOrEmpty(model.SearchText) && d.UserCode.Contains(model.SearchText))
//                    || true
//                select d;
//            return await query.ToPagedList(model.PageIndex, model.PageSize);
//        }
//    }
//}

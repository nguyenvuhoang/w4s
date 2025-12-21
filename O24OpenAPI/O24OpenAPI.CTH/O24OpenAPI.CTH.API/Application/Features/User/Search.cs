//using LinKit.Core.Cqrs;
//using LinqToDB;
//using O24OpenAPI.Framework.Models;
//using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

//namespace O24OpenAPI.CTH.API.Application.Features.User
//{
//    public class SearchCommnad: BaseTransactionModel, ICommand<virtual Task<IPagedList<UserLimit>>>
//    {

//    }

//    public class SearchHandler(IUserLimitRepository userLimitRepository) : ICommandHandler<SearchCommnad, virtual Task<IPagedList<UserLimit>>>
//    {
//        public async Task<virtual Task<IPagedList<UserLimit>>> HandleAsync(SearchCommnad request, CancellationToken cancellationToken = default)
//        {
//        model.PageSize = model.PageSize == 0 ? int.MaxValue : model.PageSize;
//            if (model.SearchText == null)
//            {
//                model.SearchText = String.Empty;
//            }

//            var userLimits = await userLimitRepository.GetAllPaged(
//                query =>
//                {
//                    query = query.Where(c =>
//                        c.RoleId.ToString().Contains(model.SearchText, ICIC)
//                        || c.CommandId.Contains(model.SearchText, ICIC)
//                    );
//                    query = query.OrderByDescending(a => a.Id);
//                    return query;
//                },
//                model.PageIndex,
//                model.PageSize
//            );
//            return userLimits;
//        }
//    }
//}

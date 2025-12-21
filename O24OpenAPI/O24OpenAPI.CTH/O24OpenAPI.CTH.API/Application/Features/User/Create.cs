//using LinKit.Core.Cqrs;
//using LinqToDB;
//using System;
//using System.Collections.Generic;
//using System.Threading;
//using System.Threading.Tasks;
//using O24OpenAPI.CTH.Constant;
//using O24OpenAPI.CTH.Domain;
//using O24OpenAPI.Framework.Models;
//using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;

//namespace O24OpenAPI.CTH.API.Features.User
//{
//    public class CreateCommnad: BaseTransactionModel, ICommand<virtual Task>
//    {

//    }

//    public class CreateHandler(IUserLimitRepository userLimitRepository) : ICommandHandler<CreateCommnad, virtual Task>
//    {
//        public async Task<virtual Task> HandleAsync(CreateCommnad request, CancellationToken cancellationToken = default)
//        {
//        await userLimitRepository.BulkInsert(userLimits);
//        }
//    }
//}

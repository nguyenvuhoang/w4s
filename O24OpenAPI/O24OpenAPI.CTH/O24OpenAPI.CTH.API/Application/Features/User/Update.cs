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
//    public class UpdateCommnad: BaseTransactionModel, ICommand<bool>
//    {

//    }

//    public class UpdateHandler(IUserAuthenRepository userAuthenRepository) : ICommandHandler<UpdateCommnad, bool>
//    {
//        public async Task<bool> HandleAsync(UpdateCommnad request, CancellationToken cancellationToken = default)
//        {
//        await userAuthenRepository.Update(user);
//        }
//    }
//}

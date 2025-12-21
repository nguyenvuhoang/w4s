using LinKit.Core.Cqrs;
using O24OpenAPI.CTH.API.Application.Models.Userlimit;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Features.User;

public class UpdateListUserLimitCommnad : BaseTransactionModel, ICommand<List<UserLimitUpdateResponseModel>>
{
    public List<UserLimitUpdateResponseModel> ListUserLimit { get; set; }
}


public class UpdateListUserLimitHandler(IUserLimitRepository userLimitRepository) : ICommandHandler<UpdateListUserLimitCommnad, List<UserLimitUpdateResponseModel>>
{
    public async Task<List<UserLimitUpdateResponseModel>> HandleAsync(UpdateListUserLimitCommnad request, CancellationToken cancellationToken = default)
    {
        UserLimit checkUserLimit;
        var respone = new List<UserLimitUpdateResponseModel>();

        foreach (var item in request.ListUserLimit)
        {
            checkUserLimit = await userLimitRepository.GetUserLimitToUpdate(item.RoleId, item.CommandId, item.CurrencyCode, item.LimitType);

            if (checkUserLimit != null)
            {
                checkUserLimit.ULimit = item.ULimit;
                await userLimitRepository.Update(checkUserLimit);
            }
            else
            {
                checkUserLimit = new UserLimit()
                {
                    RoleId = item.RoleId,
                    CommandId = item.CommandId,
                    CurrencyCode = item.CurrencyCode,
                    ULimit = item.ULimit,
                    CvTable = string.Empty,
                    LimitType = item.LimitType,
                    Margin = 0,
                };
                await userLimitRepository.InsertAsync(checkUserLimit);
            }
            respone.Add(
                new UserLimitUpdateResponseModel()
                {
                    RoleId = checkUserLimit.RoleId,
                    CommandId = checkUserLimit.CommandId,
                    CurrencyCode = checkUserLimit.CurrencyCode,
                    ULimit = checkUserLimit.ULimit,
                    LimitType = checkUserLimit.LimitType,
                }
            );
        }
        return respone;
    }

    
}

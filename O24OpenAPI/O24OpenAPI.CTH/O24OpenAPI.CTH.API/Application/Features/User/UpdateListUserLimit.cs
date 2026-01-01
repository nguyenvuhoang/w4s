using System.Text.Json.Serialization;
using LinKit.Core.Cqrs;
using O24OpenAPI.CTH.API.Application.Models.Userlimit;
using O24OpenAPI.CTH.Domain.AggregatesModel.UserAggregate;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Features.User;

public class UpdateListUserLimitCommnad
    : BaseTransactionModel,
        ICommand<List<UserLimitUpdateResponseModel>>
{
    /// <summary>
    /// roleid
    /// </summary>
    [JsonPropertyName("role_id")]
    public int RoleId { get; set; }

    /// <summary>
    /// cmdid
    /// </summary>
    [JsonPropertyName("command_id")]
    public string CommandId { get; set; }

    /// <summary>
    /// ccrid
    /// </summary>
    [JsonPropertyName("currency_code")]
    public string CurrencyCode { get; set; }

    /// <summary>
    /// ulimit
    /// </summary>
    [JsonPropertyName("u_limit")]
    public decimal? ULimit { get; set; }

    /// <summary>
    /// limittype
    /// </summary>
    [JsonPropertyName("limit_type")]
    public string LimitType { get; set; }
}

public class UpdateListUserLimitHandler(IUserLimitRepository userLimitRepository)
    : ICommandHandler<UpdateListUserLimitCommnad, List<UserLimitUpdateResponseModel>>
{
    public async Task<List<UserLimitUpdateResponseModel>> HandleAsync(
        UpdateListUserLimitCommnad request,
        CancellationToken cancellationToken = default
    )
    {
        var respone = new List<UserLimitUpdateResponseModel>();

        var checkUserLimit = await userLimitRepository.GetUserLimitToUpdate(
            request.RoleId,
            request.CommandId,
            request.CurrencyCode,
            request.LimitType
        );

        if (checkUserLimit != null)
        {
            checkUserLimit.ULimit = request.ULimit;
            await userLimitRepository.Update(checkUserLimit);
        }
        else
        {
            checkUserLimit = new UserLimit()
            {
                RoleId = request.RoleId,
                CommandId = request.CommandId,
                CurrencyCode = request.CurrencyCode,
                ULimit = request.ULimit,
                CvTable = string.Empty,
                LimitType = request.LimitType,
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

        return respone;
    }
}

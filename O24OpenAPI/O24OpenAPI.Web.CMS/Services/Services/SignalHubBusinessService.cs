using Microsoft.AspNetCore.SignalR;
using O24OpenAPI.Core.Logging.Helpers;
using O24OpenAPI.Web.CMS.Models.Request;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using System.Text.Json;

namespace O24OpenAPI.Web.CMS.Services.Services;

public class SignalHubBusinessService(IHubContext<SignalHubService> hubContext) : ISignalHubBusinessService
{
    private readonly IHubContext<SignalHubService> _hubContext = hubContext;

    public async Task<bool> Send(SignalRSendModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        var transactionCode = model.TransactionCode;
        switch (transactionCode)
        {
            case "SYNCBALANCE":
                await SyncBalanceToUser(model);
                break;
            case "LOGOUTUSER":
                await LogoutUserAsync(model);
                break;
            default:
                throw new NotImplementedException($"Transaction code '{transactionCode}' is not implemented.");
        }
        return true;
    }

    /// <summary>
    /// Sync balance to user
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task SyncBalanceToUser(SignalRSendModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        var userId = model.UserId;
        var deviceId = model.DeviceId;
        var balanceObj = JsonSerializer.Deserialize<object>(JsonSerializer.Serialize(model.Data))!;
        BusinessLogHelper.Info($"Sync balance to user {userId} with DeviceID: {deviceId} and BalanceInfo: {balanceObj}");
        await SignalHubService.SendUpdateBalanceAsync(_hubContext, userId, deviceId, balanceObj);

    }
    /// <summary>
    /// Logout user
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task LogoutUserAsync(SignalRSendModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        var userId = model.UserId;
        var deviceId = model.DeviceId;
        var userInfo = JsonSerializer.Deserialize<object>(JsonSerializer.Serialize(model.Data))!;
        BusinessLogHelper.Info($"Logging out user {userId} with DeviceID: {deviceId} and UserInfo: {userInfo}");
        await SignalHubService.SendUserLogOut(_hubContext, userId, deviceId, userInfo);
    }
}

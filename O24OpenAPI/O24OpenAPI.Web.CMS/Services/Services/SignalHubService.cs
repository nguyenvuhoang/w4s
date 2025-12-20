using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using O24OpenAPI.Framework.Services;
using O24OpenAPI.Framework.Utils;
using O24OpenAPI.Logging.Helpers;

namespace O24OpenAPI.Web.CMS.Services.Services;

public class SignalHubService : Hub
{
    private static readonly ConcurrentDictionary<string, HashSet<string>> UserConnections = new();

    public SignalHubService() { }

    public async Task Init(string token = null)
    {
        var connectionId = Context.ConnectionId;
        string connectionKey;
        string userId = null;
        string deviceId = null;

        BusinessLogHelper.Info(
            "SignalR Init called: ConnectionId={0}, Token={1}",
            connectionId,
            token
        );
        if (!string.IsNullOrEmpty(token))
        {
            var jwtTokenService = EngineContext.Current.Resolve<IJwtTokenService>();
            userId = jwtTokenService.GetUserCodeFromToken(token);
            deviceId = jwtTokenService.GetDeviceIdFromToken(token);
            BusinessLogHelper.Info(
                $"SignalR Init: UserId={userId}, DeviceId={deviceId}, connectionId={connectionId}"
            );
        }

        if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(deviceId))
        {
            connectionKey = $"{userId}:{deviceId}";
        }
        else
        {
            connectionKey = $"guest:{connectionId}";
            userId = "guest";
        }

        if (!UserConnections.TryGetValue(connectionKey, out var connections))
        {
            connections = [];
            UserConnections[connectionKey] = connections;
        }

        connections.Add(connectionId);

        await Clients
            .Client(connectionId)
            .SendAsync(
                "Init",
                new
                {
                    Status = "Connected",
                    UseType = token != null ? "Authenticated" : "Guest",
                    User = userId,
                    ConnectionKey = connectionKey,
                }
            );
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        var connectionId = Context.ConnectionId;
        foreach (var key in UserConnections.Keys)
        {
            if (UserConnections[key].Contains(connectionId))
            {
                UserConnections[key].Remove(connectionId);
                if (!UserConnections[key].Any())
                {
                    UserConnections.TryRemove(key, out _);
                }

                break;
            }
        }
        return base.OnDisconnectedAsync(exception);
    }

    public static async Task SignalSendToUser(
        IHubContext<SignalHubService> hubContext,
        string chanel,
        object message,
        string userId,
        string deviceId
    )
    {
        ConsoleUtil.WriteInfo(
            $"Sending message to Channel={chanel} UserId={userId}, DeviceId={deviceId}, Message={message}"
        );
        BusinessLogHelper.Info(
            $"Sending message to Channel={chanel} UserId={userId}, DeviceId={deviceId}, Message={message}"
        );
        if (UserConnections.TryGetValue($"{userId}:{deviceId}", out var connections))
        {
            foreach (var connectionId in connections)
            {
                await hubContext.Clients.Client(connectionId).SendAsync(chanel, message);
            }
        }
    }

    /// <summary>
    /// Send banner update notification to user
    /// </summary>
    /// <param name="hubContext"></param>
    /// <param name="userId"></param>
    /// <param name="deviceId"></param>
    /// <param name="languageCode"></param>
    /// <returns></returns>
    public static async Task SendUpdateBannerAsync(
        IHubContext<SignalHubService> hubContext,
        object banner
    )
    {
        await hubContext.Clients.All.SendAsync("BannerUpdated", banner);
    }

    /// <summary>
    /// Send balance update notification to user
    /// </summary>
    /// <param name="hubContext"></param>
    /// <param name="userId"></param>
    /// <param name="deviceId"></param>
    /// <param name="balanceInfo"></param>
    /// <returns></returns>
    public static async Task SendUpdateBalanceAsync(
        IHubContext<SignalHubService> hubContext,
        string userId,
        string deviceId,
        object balanceInfo
    )
    {
        await SignalSendToUser(hubContext, "BalanceUpdated", balanceInfo, userId, deviceId);
    }

    /// <summary>
    /// Send user log out notification to user
    /// </summary>
    /// <param name="hubContext"></param>
    /// <param name="userId"></param>
    /// <param name="deviceId"></param>
    /// <param name="balanceInfo"></param>
    /// <returns></returns>
    public static async Task SendUserLogOut(
        IHubContext<SignalHubService> hubContext,
        string userId,
        string deviceId,
        object userInfo
    )
    {
        await SignalSendToUser(hubContext, "UserLogOut", userInfo, userId, deviceId);
    }
}

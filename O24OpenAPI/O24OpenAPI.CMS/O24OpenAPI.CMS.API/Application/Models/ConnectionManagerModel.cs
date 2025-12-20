#region Assembly Jits.Neptune.Web.Framework, Version=1.0.2.10, Culture=neutral, PublicKeyToken=null
// Jits.Neptune.Web.Framework.dll
#endregion

using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace O24OpenAPI.CMS.API.Application.Models;

/// <summary>
///
/// </summary>
public class ConnectionManager
{
    private readonly ConcurrentDictionary<string, WebSocket> _sockets =
        new ConcurrentDictionary<string, WebSocket>();

    /// <summary>
    ///
    /// </summary>
    public ConnectionManager()
    {
        // _sockets ;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public WebSocket GetSocketById(string id)
    {
        return _sockets.FirstOrDefault(p => p.Key == id).Value;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public int Count()
    {
        return _sockets.Count;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public ConcurrentDictionary<string, WebSocket> GetAll()
    {
        return _sockets;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <param name="socket"></param>
    public void AddSocket(string id, WebSocket socket)
    {
        _sockets.TryAdd(id, socket);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task RemoveSocket(string id)
    {
        WebSocket websocketRemove;

        _sockets.TryRemove(id, out websocketRemove);

        await websocketRemove.CloseAsync(
            closeStatus: WebSocketCloseStatus.NormalClosure,
            statusDescription: "Closed by the ConnectionManager",
            cancellationToken: CancellationToken.None
        );
    }
}

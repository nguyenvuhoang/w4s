using Microsoft.AspNetCore.Http;

namespace O24OpenAPI.Core.Utils;

/// <summary>
/// The http utils class
/// </summary>
public class HttpUtils
{
    /// <summary>
    /// Gets the response using the specified http response message
    /// </summary>
    /// <typeparam name="T">The </typeparam>
    /// <param name="httpResponseMessage">The http response message</param>
    /// <exception cref="Exception">Error: {httpResponseMessage.StatusCode}</exception>
    /// <returns>A task containing the</returns>
    public static async Task<T> GetResponseAsync<T>(HttpResponseMessage httpResponseMessage)
    {
        if (httpResponseMessage.IsSuccessStatusCode)
        {
            var jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();
            var result = System.Text.Json.JsonSerializer.Deserialize<T>(jsonResponse);
            return result;
        }
        else
        {
            throw new Exception($"Error: {httpResponseMessage.StatusCode}");
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    public static Dictionary<string, string> GetHeaders(HttpContext httpContext)
    {
        Dictionary<string, string> requestHeaders = [];
        foreach (var header in httpContext.Request.Headers)
        {
            requestHeaders.Add(header.Key.ToLower(), header.Value);
        }

        return requestHeaders;
    }
}

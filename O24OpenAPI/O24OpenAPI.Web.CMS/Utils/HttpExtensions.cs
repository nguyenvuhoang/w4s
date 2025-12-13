using Newtonsoft.Json;

namespace O24OpenAPI.Web.CMS.Utils;

public static class HttpExtensions
{
    public static async Task<T> GetResponseAsync<T>(
        this HttpResponseMessage httpResponseMessage
    )
    {
        if (httpResponseMessage.IsSuccessStatusCode)
        {
            var jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<T>(jsonResponse);
            return result;
        }
        else
        {
            throw new Exception($"Error: {httpResponseMessage.StatusCode}");
        }
    }
}

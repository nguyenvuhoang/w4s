using Newtonsoft.Json.Linq;

namespace O24OpenAPI.CTH.API.Application.Extensions
{
    public static class ErrorExtensions
    {

        public static JObject BuildErrorDataFromResponse(string response)
        {
            if (string.IsNullOrWhiteSpace(response))
                return [];

            JObject result;
            try
            {
                result = JObject.Parse(response);
            }
            catch
            {
                return new JObject
                {
                    ["data"] = new JObject
                    {
                        ["error_message"] = response
                    }
                };
            }

            if (result.TryGetValue("error_message", out JToken? messageToken))
            {
                var msg = messageToken?.ToString();
                if (!string.IsNullOrEmpty(msg))
                {
                    result["data"] = new JObject
                    {
                        ["error_message"] = messageToken,
                        ["next_action"] = result["error_next_action"],
                        ["error_code"] = result["error_code"],
                    };
                }
            }

            return result;
        }

    }
}

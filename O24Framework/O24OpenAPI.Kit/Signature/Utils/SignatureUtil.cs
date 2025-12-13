using O24OpenAPI.Kit.Utils;
using System.Text.Json;

namespace O24OpenAPI.Kit.Signature.Utils;

internal static class SignatureUtil
{
    public const string SignatureKeyTemplate = "{0}|{1}";
    public const string SignatureDataTemplate = "{0}_{1}_{2}";

    internal static string NormalizeJson(this object requestObject)
    {
        if (requestObject is string stringRequest)
        {
            return stringRequest;
        }
        var options = new JsonSerializerOptions
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = false,
        };

        string tempJson = JsonSerializer.Serialize(requestObject, options);

        var sortedDictionary = JsonSerializer.Deserialize<
            SortedDictionary<string, JsonElement>
        >(tempJson);
        if (sortedDictionary == null)
        {
            if (requestObject == null)
            {
                return "{}";
            }

            throw new ArgumentException(
                "requestObject cannot be deserialized into a key-value structure for sorting."
            );
        }

        string normalizedJson = JsonSerializer.Serialize(sortedDictionary, options);

        return normalizedJson;
    }

    internal static (string signature, string timestamp, string nounce) GenSignature(this object data, string privateKey)
    {
        string normalizedJsonString = data.NormalizeJson();
        string timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        string nounce = GenerateNounce();

        string rawSignatureData = string.Format(SignatureDataTemplate, normalizedJsonString, timestamp, nounce);
        rawSignatureData = string.Format(SignatureKeyTemplate, privateKey, rawSignatureData);

        string signature = rawSignatureData.Hash(HashAlgorithmOption.SHA512);
        return (signature, timestamp, nounce);
    }

    private static string GenerateNounce()
    {
        return Guid.NewGuid().ToString("N");
    }
}

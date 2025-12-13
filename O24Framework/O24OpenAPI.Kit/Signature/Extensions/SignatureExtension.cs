using O24OpenAPI.Kit.Signature.Utils;

namespace O24OpenAPI.Kit.Signature.Extensions;

public static class SignatureExtension
{
    public static (string signature, string timestamp, string nounce) GetSignature(this object data, string privateKey)
    {
        return data.GenSignature(privateKey);
    }
}

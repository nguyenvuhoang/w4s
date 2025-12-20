using O24OpenAPI.Client.Infisical;
using O24OpenAPI.Kit.Models;

namespace O24OpenAPI.Kit.Keyvault.Extensions;

public static class KeyvaultExtension
{
    private static readonly string key = "O24OpenAPI.SecretKey";

    public static string GetSecretKey()
    {
        var json = InfisicalManager.GetSecretByKey<SecretVaultModel>(key);
        string vaultKey = json.SecretKey;
        return vaultKey;
    }
}

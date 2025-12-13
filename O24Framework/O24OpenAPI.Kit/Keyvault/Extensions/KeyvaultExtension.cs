using O24OpenAPI.Kit.Models;
using O24OpenAPI.O24OpenAPIClient.Infisical;

namespace O24OpenAPI.Kit.Keyvault.Extensions;

public static class KeyvaultExtension
{
    private readonly static string key = "O24OpenAPI.SecretKey";
    public static string GetSecretKey()
    {
        var json = InfisicalManager.GetSecretByKey<SecretVaultModel>(key);
        string vaultKey = json.SecretKey;
        return vaultKey;
    }

}

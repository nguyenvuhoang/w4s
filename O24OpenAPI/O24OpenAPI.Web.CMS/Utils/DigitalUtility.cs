using System.Security.Cryptography;
using System.Text;
using Jint;
using QRCoder;

namespace O24OpenAPI.Web.CMS.Utils.Common;

public static class DigitalUtility
{
    private const string PrivateKey =
        "-----BEGIN RSA PRIVATE KEY-----\nMIIEogIBAAKCAQEAt05W/MyMnT8oR/NLspUQoHqC3lcXiM9crTd5CL5EDKAUGK5b\nZMvb8zWgtxg60aR+JyCqpVQmPKP8IqcY5Ll/QO1pkeufHEVKIo2CUPBCQwiU2yUj\nbSQhiPXliwlTc9CwCA4MT7SUmI6x2k5dtVx5RPyAVviYJsr0lV0epztpiiEH7EGu\n1nOHLgS1R6xikNCaqLWEembY2F09lvAiDbXzjE9A1jQ+9/FEDbxDnZXZv8sD7OmP\nwEjjj9kqTe7Nhq+fMXzFkNo1CZB2euGAn3VQWtUcM8se9/Y9JBlK45ceL9JM+m0z\nXX41u9ZN84BMtVtGknF2FEtIvnjyacT977qMaQIDAQABAoIBAHxt8kCCfYUUZTyK\nG71q0LGO2B0jk3csA6YVfNSBi2HinCGw4ZtHgtPSeHkxOmQqu/PFifDWcWpCipnf\nQdepT4d8YWhLQGOJaWyREH/ux5wHTDyCNk0U9Kqq+JlryDla1NNy7cqBM0lnu7UL\n8zJ0RmGc4WWoNGhI5sQpRPCSNO8snIqPioDDdAVIm3lREoTcA/plCRdI80AQavzE\nEwfl6HExVgk3EZISCyNiyw80Oc0DHDRo5wm/xT6rzEjRVqgE39GjLrNYf97aezOm\nYVN9gytp3xn4bVVFcc7or91EMujpod8n+RpiOSnBIyIpgIha5/Ly7UGBOkEp/08o\n5HqJLDECgYEA4R2Vk9mm02bq4FZ3ue+09U3UVxU7s6virURTQhKWUMKDB5F8D8VV\nHlFZvECNfjzhl2rA6TDmj+uHoR5pbApVlowULgO1n9iRZooV+eyhs/wKcK7CVaGV\nHH7P3wJL7Vv7PXh/jsCRChKpi158uQSErGilONp1Lut2G5jDEZsO7TUCgYEA0HRW\ne+ciW+zqcenjeyboh5uQ1JEpT+x8KE8qXhRJzv/bUQCmJTCZHRQir5ZZ+qf7lXLZ\n6kT5UCV3XbxV503qaLtN7HqT6GwcNVx+IljDliZtHEwM4cXQ/o3UIntXTwVKgiZS\nzjLaIDQqxQmbmFRoOrVogEw7DCqmiM1J+LHWbOUCgYAb/ca8lKK2xKRN8tJ+Tz0M\nB3pwvZYn2CaebtF/dLo2HomWZlOrzliwJWNoEgrF+KIAZujH1inFGX9K3HI1We+c\ngZx8wD9UOeSSgHcNFYxhyeQucLx/U1EsMuR8EVgJkpdh5FrQEEe4netxBfWyN8qb\neYOZ1ygdBnAwLEWNKd+oRQKBgByogeNS6YRd0/WJRD8AIGPUXVr8AgP87SzVhWud\num/8+Pfv8OeYANTgmcwuDXzKb2oiehcdGHGOVRTL+bttGNcBOTtTttXzVwV3C9hI\n/8q6ybFkOzkm6w60DQAYmO5COfYesq+qihZ3VZH1OjD9Gb9IhW9uC4gmrPKN1qec\nO3CJAoGALrYbwyqmMpkJ63YaEQ/bJGeayMVCk/pWbCCYFw9rL1ctFfX5PlDBagKA\naO7n368TQqPJ5GWtOM5X7/y7bC/o3Xzu7hKM6Ks+YRaJOSUtXRJolEnAVkDSh9eu\nC4p6kC4z2V0YQ9E90s/ClHdjNTCBNnQQdhIFW170yZAM5QGhQqI=\n-----END RSA PRIVATE KEY-----";
    private const string PublicKey =
        "-----BEGIN PUBLIC KEY-----\nMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAt05W/MyMnT8oR/NLspUQ\noHqC3lcXiM9crTd5CL5EDKAUGK5bZMvb8zWgtxg60aR+JyCqpVQmPKP8IqcY5Ll/\nQO1pkeufHEVKIo2CUPBCQwiU2yUjbSQhiPXliwlTc9CwCA4MT7SUmI6x2k5dtVx5\nRPyAVviYJsr0lV0epztpiiEH7EGu1nOHLgS1R6xikNCaqLWEembY2F09lvAiDbXz\njE9A1jQ+9/FEDbxDnZXZv8sD7OmPwEjjj9kqTe7Nhq+fMXzFkNo1CZB2euGAn3VQ\nWtUcM8se9/Y9JBlK45ceL9JM+m0zXX41u9ZN84BMtVtGknF2FEtIvnjyacT977qM\naQIDAQAB\n-----END PUBLIC KEY-----";

    public static T TotpExecute<T>(string functionName, params object[] arguments)
    {
        var currentDirectory = Directory.GetCurrentDirectory();

        var totpScript = File.ReadAllText(currentDirectory + "/JavaScript/TOTP.js");
        var script = File.ReadAllText(currentDirectory + "/JavaScript/Script.js");
        var combinedScript = totpScript + script;
        var engine = new Engine();
        engine.Execute(combinedScript);
        var result = engine.Invoke(functionName, arguments);

        return (T)Convert.ChangeType(result.ToObject(), typeof(T));
    }

    public static string GenerateQrCodeBase64(string data)
    {
        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
        Base64QRCode qrCode = new Base64QRCode(qrCodeData);
        string qrCodeImageAsBase64 = qrCode.GetGraphic(20);
        return qrCodeImageAsBase64;
    }

    public static string EncryptWithRSA(string jsonData)
    {
        using var rsa = new RSACryptoServiceProvider(2048);
        try
        {
            rsa.ImportFromPem(PublicKey);
            var dataBytes = Encoding.UTF8.GetBytes(jsonData);
            var encryptedBytes = rsa.Encrypt(dataBytes, false);
            return Convert.ToBase64String(encryptedBytes);
        }
        finally
        {
            rsa.PersistKeyInCsp = false;
        }
    }

    public static string DecryptWithRSA(string encryptedData)
    {
        using var rsa = new RSACryptoServiceProvider(2048);
        try
        {
            rsa.ImportFromPem(PrivateKey);
            var base64String = Convert.FromBase64String(encryptedData);
            var decryptedBytes = rsa.Decrypt(base64String, false);

            return Encoding.UTF8.GetString(decryptedBytes);
        }
        finally
        {
            rsa.PersistKeyInCsp = false;
        }
    }

    public static bool TryDecryptWithRSA(string encryptedData, ref string outData)
    {
        using var rsa = new RSACryptoServiceProvider(2048);
        try
        {
            rsa.ImportFromPem(PrivateKey);
            var base64String = Convert.FromBase64String(encryptedData);
            var decryptedBytes = rsa.Decrypt(base64String, false);
            outData = Encoding.UTF8.GetString(decryptedBytes);
            return true;
        }
        finally
        {
            rsa.PersistKeyInCsp = false;
        }
    }
}

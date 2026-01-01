using O24OpenAPI.Framework.Extensions;
using System.Text.Json;
using System.Xml;

namespace O24OpenAPI.NCH.Common;

public class O24SMSResponseParser
{
    /// <summary>
    /// Phân tích SOAP XML để lấy msg_id / trans_id và xác định thành công hay thất bại.
    /// </summary>
    /// <param name="soapXml">SOAP response string</param>
    /// <returns>(MessageId, IsSuccess)</returns>
    public static (
        string MsgId,
        bool IsSuccess,
        int resultCode,
        string resultDesc,
        string ProviderKey,
        string RawCode
    ) ExtractMsgIdAndStatusFromSOAP(
        string soapXml,
        Dictionary<string, HashSet<string>> successCodesPerProvider
    )
    {
        try
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(soapXml);

            // ======== UNITEL (descr chứa JSON, error_code rời) ========
            var errorCodeNode = xmlDoc
                .GetElementsByTagName("error_code")
                .Cast<XmlNode>()
                .FirstOrDefault();
            var descrNode = xmlDoc.GetElementsByTagName("descr").Cast<XmlNode>().FirstOrDefault();

            if (errorCodeNode != null && int.TryParse(errorCodeNode.InnerText, out int errorCode))
            {
                string msgId = null;
                string msgdescr = null;
                if (descrNode != null && !string.IsNullOrWhiteSpace(descrNode.InnerText))
                {
                    var jsonString = System.Net.WebUtility.HtmlDecode(descrNode.InnerText);
                    try
                    {
                        var jsonDoc = JsonDocument.Parse(jsonString);
                        if (jsonDoc.RootElement.TryGetProperty("msg_id", out var msgIdProp))
                        {
                            msgId = msgIdProp.GetString();
                        }
                        if (jsonDoc.RootElement.TryGetProperty("descr", out var msgdescrProp))
                        {
                            msgdescr = msgdescrProp.GetString();
                        }
                    }
                    catch { }
                }

                string rawCode = errorCode.ToString();
                bool isSuccess =
                    successCodesPerProvider.TryGetValue("UNITEL", out var successCodes)
                    && successCodes.Contains(rawCode);

                return (msgId, isSuccess, errorCode, msgdescr, "UNITEL", rawCode);
            }

            // ======== LTC (resultCode + trans_id) ========
            var resultCodeNode = xmlDoc
                .GetElementsByTagName("resultCode")
                .Cast<XmlNode>()
                .FirstOrDefault();
            var transIdNode = xmlDoc
                .GetElementsByTagName("trans_id")
                .Cast<XmlNode>()
                .FirstOrDefault();

            if (resultCodeNode != null)
            {
                string rawCode = resultCodeNode.InnerText?.Trim();
                int.TryParse(rawCode, out int resultCode);

                var resultDescNode = xmlDoc
                    .GetElementsByTagName("resultDesc")
                    .Cast<XmlNode>()
                    .FirstOrDefault();

                bool isSuccess =
                    successCodesPerProvider.TryGetValue("LTC", out var successCodes)
                    && successCodes.Contains(rawCode);

                return (
                    MsgId: transIdNode?.InnerText,
                    IsSuccess: isSuccess,
                    resultCode,
                    resultDesc: resultDescNode?.InnerText,
                    ProviderKey: "LTC",
                    RawCode: rawCode
                );
            }

            // ======== ETL (ax23:apiResponseCode + ax23:messageID) ========
            var apiResponseCodeNode = xmlDoc
                .GetElementsByTagName("ax23:apiResponseCode")
                .Cast<XmlNode>()
                .FirstOrDefault();
            var messageIdNode = xmlDoc
                .GetElementsByTagName("ax23:messageID")
                .Cast<XmlNode>()
                .FirstOrDefault();
            var tranIdNode = xmlDoc
                .GetElementsByTagName("ax23:tranID")
                .Cast<XmlNode>()
                .FirstOrDefault();

            if (
                apiResponseCodeNode != null
                && int.TryParse(apiResponseCodeNode.InnerText, out int apiResponseCode)
            )
            {
                string msgId = messageIdNode?.InnerText ?? tranIdNode?.InnerText;
                string rawCode = apiResponseCode.ToString();

                bool isSuccess =
                    successCodesPerProvider.TryGetValue("ETL", out var successCodes)
                    && successCodes.Contains(rawCode);

                return (msgId, isSuccess, apiResponseCode, "ETL response", "ETL", rawCode);
            }
        }
        catch (Exception ex)
        {
            _ = ex.LogErrorAsync();
        }

        return (null, false, 0, null, null, null);
    }
}

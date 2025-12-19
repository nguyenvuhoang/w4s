using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace O24OpenAPI.Framework.Extensions;

public static class HttpContextExtensions
{
    extension(HttpContext httpContext)
    {
        public Dictionary<string, string> GetHeaders()
        {
            Dictionary<string, string> requestHeaders = [];

            foreach (KeyValuePair<string, StringValues> header in httpContext.Request.Headers)
            {
                requestHeaders[header.Key.ToLowerInvariant()] = header.Value.ToString();
            }

            return requestHeaders;
        }

        public Dictionary<string, string> GetCookies()
        {
            Dictionary<string, string> requestCookies = [];
            foreach (KeyValuePair<string, string> header in httpContext.Request.Cookies)
            {
                requestCookies.Add(header.Key.ToLower(), header.Value);
            }

            return requestCookies;
        }

        public string GetClientIPAddress()
        {
            return httpContext.Connection.RemoteIpAddress.ToString();
        }

        public string GetClientOs()
        {
            string rs_ = "";

            string ua = httpContext.Request.Headers["User-Agent"].ToString();

            if (ua.Contains("Android"))
            {
                return string.Format("Android {0}", GetMobileVersion(ua, "Android"));
            }

            if (ua.Contains("iPad"))
            {
                return string.Format("iPad OS {0}", GetMobileVersion(ua, "OS"));
            }

            if (ua.Contains("iPhone"))
            {
                return string.Format("iPhone OS {0}", GetMobileVersion(ua, "OS"));
            }

            if (ua.Contains("Linux") && ua.Contains("KFAPWI"))
            {
                return "Kindle Fire";
            }

            if (ua.Contains("RIM Tablet") || (ua.Contains("BB") && ua.Contains("Mobile")))
            {
                return "Black Berry";
            }

            if (ua.Contains("Windows Phone"))
            {
                return string.Format("Windows Phone {0}", GetMobileVersion(ua, "Windows Phone"));
            }

            if (ua.Contains("Mac OS"))
            {
                return "Mac OS";
            }

            if (ua.Contains("Windows NT 5.1") || ua.Contains("Windows NT 5.2"))
            {
                return "Windows XP";
            }

            if (ua.Contains("Windows NT 6.0"))
            {
                return "Windows Vista";
            }

            if (ua.Contains("Windows NT 6.1"))
            {
                return "Windows 7";
            }

            if (ua.Contains("Windows NT 6.2"))
            {
                return "Windows 8";
            }

            if (ua.Contains("Windows NT 6.3"))
            {
                return "Windows 8.1";
            }

            if (ua.Contains("Windows NT 10"))
            {
                return "Windows 10";
            }

            return rs_;
        }

        public string GetClientBrowser()
        {
            string browser = "";
            string browserDetails = httpContext.Request.Headers["User-Agent"].ToString();
            string user = browserDetails.ToLower();
            if (user.Contains("msie"))
            {
                string Substring = browserDetails.Substring(browserDetails.IndexOf("MSIE")).Split(";")[
                    0
                ];
                browser = Substring.Split(" ")[0].Replace("MSIE", "IE") + "-" + Substring.Split(" ")[1];
            }
            else if (user.Contains("safari") && user.Contains("version"))
            {
                browser =
                    browserDetails.Substring(browserDetails.IndexOf("Safari")).Split(" ")[0].Split("/")[
                        0
                    ]
                    + "-"
                    + browserDetails
                        .Substring(browserDetails.IndexOf("Version"))
                        .Split(" ")[0]
                        .Split("/")[1];
            }
            else if (user.Contains("opr") || user.Contains("opera"))
            {
                if (user.Contains("opera"))
                {
                    browser =
                        browserDetails
                            .Substring(browserDetails.IndexOf("Opera"))
                            .Split(" ")[0]
                            .Split("/")[0]
                        + "-"
                        + browserDetails
                            .Substring(browserDetails.IndexOf("Version"))
                            .Split(" ")[0]
                            .Split("/")[1];
                }
                else if (user.Contains("opr"))
                {
                    browser = browserDetails
                        .Substring(browserDetails.IndexOf("OPR"))
                        .Split(" ")[0]
                        .Replace("/", "-")
                        .Replace("OPR", "Opera");
                }
            }
            else if (
                (user.IndexOf("mozilla/7.0") > -1)
                || (user.IndexOf("netscape6") != -1)
                || (user.IndexOf("mozilla/4.7") != -1)
                || (user.IndexOf("mozilla/4.78") != -1)
                || (user.IndexOf("mozilla/4.08") != -1)
                || (user.IndexOf("mozilla/3") != -1)
            )
            {
                // browser=(userAgent.Substring(userAgent.IndexOf("MSIE")).Split("
                // ")[0]).Replace("/", "-");
                browser = "Netscape-?";
            }
            else if (user.Contains("edg"))
            {
                browser = browserDetails
                    .Substring(browserDetails.IndexOf("Edg"))
                    .Split(" ")[0]
                    .Replace("/", "-");
            }
            else if (user.Contains("firefox"))
            {
                browser = browserDetails
                    .Substring(browserDetails.IndexOf("Firefox"))
                    .Split(" ")[0]
                    .Replace("/", "-");
            }
            else if (user.Contains("chrome"))
            {
                browser = browserDetails
                    .Substring(browserDetails.IndexOf("Chrome"))
                    .Split(" ")[0]
                    .Replace("/", "-");
            }
            else if (user.Contains("rv"))
            {
                browser = "IE";
            }
            else
            {
                browser = "UnKnown, More-Info: " + browserDetails;
                //browser = "UnKnown, More-Info: " + browserDetails.Substring(0, 30);
            }

            return browser;
        }
    }

    private static string GetMobileVersion(string userAgent, string device)
    {
        string temp = userAgent[(userAgent.IndexOf(device) + device.Length)..].TrimStart();
        string version = string.Empty;

        foreach (char character in temp)
        {
            bool validCharacter = false;
            int test = 0;

            if (int.TryParse(character.ToString(), out test))
            {
                version += character;
                validCharacter = true;
            }

            if (character == '.' || character == '_')
            {
                version += '.';
                validCharacter = true;
            }

            if (validCharacter == false)
            {
                break;
            }
        }

        return version;
    }
}

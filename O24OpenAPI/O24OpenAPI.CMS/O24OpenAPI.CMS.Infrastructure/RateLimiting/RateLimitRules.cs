namespace O24OpenAPI.CMS.Infrastructure.RateLimiting;

public static class RateLimitRules
{
    private static readonly Dictionary<string, int> Limits = new()
    {
        ["/api/auth/loginopenapi"] = 5,
        ["/api/health/ping"] = 20,
        ["default"] = 500,
    };

    public static int GetLimit(string path)
    {
        foreach (KeyValuePair<string, int> rule in Limits)
        {
            if (rule.Key != "default" && path.StartsWith(rule.Key))
                return rule.Value;
        }

        return Limits["default"];
    }
}

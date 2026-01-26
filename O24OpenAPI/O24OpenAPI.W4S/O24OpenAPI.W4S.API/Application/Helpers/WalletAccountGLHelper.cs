namespace O24OpenAPI.W4S.API.Application.Helpers
{
    public static class WalletAccountGLHelper
    {
        /// <summary>
        /// Resolves the account alias by replacing placeholders in the template with corresponding values from the dictionary.
        /// </summary>
        /// <param name="template"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string ResolveAccountAlias(
            string template,
            IDictionary<string, string> values)
        {
            foreach (var kv in values)
                template = template.Replace($"{{{kv.Key}}}", kv.Value);

            return template;
        }
    }
}

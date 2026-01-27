using O24OpenAPI.Core.Configuration;

namespace O24OpenAPI.CMS.Infrastructure.Configurations
{
    public class ZaloConfiguration : IConfig
    {
        public string AppId { get; set; } = default!;
        public string RedirectUri { get; set; } = default!;
        public string SecretKey { get; set; } = default!;
    }
}

using O24OpenAPI.Core.Configuration;

namespace O24OpenAPI.W4S.Infrastructure.Configurations
{
    public class W4SSetting : ISettings
    {
        public string BaseCurrency { get; set; } = "VND";
    }
}

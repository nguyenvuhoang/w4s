using O24OpenAPI.Core.Configuration;

namespace O24OpenAPI.EXT.Infrastructure.Configurations;

public class EXTSetting : ISettings
{
    public string VcbUrl { get; set; } = "https://portal.vietcombank.com.vn/Usercontrols/TVPortal.TyGia/pXML.aspx";
}

using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.O24ACT.Models;

public class MultiValueName : BaseO24OpenAPIModel
{
    /// <summary>
    /// AccountChartUpdateModel
    /// </summary>
    public MultiValueName()
    {
        this.LaosName = string.Empty;
        this.ThaiName = string.Empty;
        this.KhmerName = string.Empty;
        this.VietnameseName = string.Empty;
    }
    /// <summary>
    ///
    /// </summary>
    public string LaosName { get; set; }
    /// <summary>
    ///
    /// </summary>
    public string ThaiName { get; set; }
    /// <summary>
    ///
    /// </summary>
    public string KhmerName { get; set; }
    /// <summary>
    ///
    /// </summary>
    public string VietnameseName { get; set; }
}

using O24OpenAPI.Web.CMS.Models.O9;

namespace Jits.Neptune.Web.CMS.LogicOptimal9.Common;
/// <summary>
///
/// </summary>
public class BackOfficeSetting
{
    /// <summary>
    ///
    /// </summary>
    public string TableName { get; set; }
    /// <summary>
    ///
    /// </summary>
    public string ViewName { get; set; }
    /// <summary>
    ///
    /// </summary>
    public string GetProcedure { get; set; }
    /// <summary>
    ///
    /// </summary>
    public string AddCode { get; set; }
    /// <summary>
    ///
    /// </summary>
    public string UpdateCode { get; set; }
    /// <summary>
    ///
    /// </summary>
    public string DeleteCode { get; set; }
    /// <summary>
    ///
    /// </summary>
    public string ApproveCode { get; set; }
    /// <summary>
    ///
    /// </summary>
    public string RejectCode { get; set; }
    /// <summary>
    ///
    /// </summary>
    public string ProcessCode { get; set; }
    /// <summary>
    ///
    /// </summary>
    public string InsertFields { get; set; }
    /// <summary>
    ///
    /// </summary>
    public string UpdateFields { get; set; }
    /// <summary>
    ///
    /// </summary>
    public List<JsonSearchFtag> Fields { get; set; }
    /// <summary>
    ///
    /// </summary>
    public string IDField { get; set; }
    /// <summary>
    ///
    /// </summary>
    public List<string> DateConvertToLongFields { get; set; }
}

using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.ControlHub.Models;

public partial class CommandIdInfoModel : BaseO24OpenAPIModel
{
    /// <summary>
    ///
    /// </summary>
    public CommandIdInfoModel() { }

    /// <summary>
    /// role id
    /// </summary>
    public int RoleId { get; set; }

    /// <summary>
    /// rolename
    /// </summary>
    public string RoleName { get; set; }

    /// <summary>
    /// Parent command Id
    /// </summary>
    public string ParentId { get; set; }

    /// <summary>
    /// /// Command Id
    /// </summary>
    public string CommandId { get; set; }

    /// <summary>
    /// Command Name
    /// </summary>
    public string CommandName { get; set; }

    /// <summary>
    /// Command Id Detail
    /// </summary>
    public string CommandIdDetail { get; set; }

    /// <summary>
    /// Invoke
    /// </summary>
    public int Invoke { get; set; }

    /// <summary>
    /// Approve
    /// </summary>
    public int Approve { get; set; }

    /// <summary>
    /// ApplicationCode
    /// </summary>
    public string ApplicationCode { get; set; }

    /// <summary>
    /// Command Type
    /// </summary>
    /// <value></value>
    public string CommandType { get; set; }

    /// <summary>
    /// GroupMenuIcon
    /// </summary>
    public string GroupMenuIcon { get; set; }

    /// <summary>
    /// GroupMenuVisible
    /// </summary>
    public string GroupMenuVisible { get; set; }

    /// <summary>
    /// GroupMenuId
    /// </summary>
    public string GroupMenuId { get; set; }

    /// <summary>
    /// GroupMenuListAuthorizeForm
    /// </summary>
    public string GroupMenuListAuthorizeForm { get; set; }
}

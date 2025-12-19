using Newtonsoft.Json;
using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.O24ACT.Models;

public partial class BranchModel : BaseO24OpenAPIModel
{
    /// <summary>
    /// Contructor
    /// </summary>
    public BranchModel() { }

    /// <summary>
    /// BranchCode
    /// </summary>
    public string BranchCode { get; set; }

    /// <summary>
    /// BranchName
    /// </summary>
    public string BranchName { get; set; }

    /// <summary>
    /// BranchCode
    /// </summary>
    public int Id { get; set; }
}

public class ListBranchModel : BaseO24OpenAPIModel
{
    public ListBranchModel()
    {
        Branchs = [];
    }

    /// <summary>
    /// Id
    /// </summary>
    [JsonProperty("branchs")]
    public List<BranchModel> Branchs { get; set; }
}

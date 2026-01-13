using O24OpenAPI.Framework.Domain;

namespace O24OpenAPI.O24ACT.Models;

public class BaseValidatorModel
{
    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string MethodName { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string NameSpace { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string Parameter { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string Workflowid { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public string Condition { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public List<GLEntries> Postings { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    public Dictionary<string, object> Fields { get; set; } = [];
}

using O24OpenAPI.Core.Configuration;

namespace O24OpenAPI.Web.CMS.Models.O9;

public class Optimal9Settings
{
    /// <summary>
    ///
    /// </summary>
    public string Memcached { get; set; }

    /// <summary>
    ///
    /// </summary>
    public int PoolConnection { get; set; }

    /// <summary>
    ///
    /// </summary>
    public CoreConfig Configure { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string LbName { get; set; } = "API";

    /// <summary>
    ///
    /// </summary>
    public string CoreMode { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string O9UserName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string O9Password { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool O9Encrypt { get; set; }
}

public class CommonConfig : IConfig
{
    /// <summary>
    ///
    /// </summary>
    public string Memcached { get; set; }

    /// <summary>
    ///
    /// </summary>
    public int PoolConnection { get; set; }

    /// <summary>
    ///
    /// </summary>
    public CoreConfig Configure { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string LbName { get; set; } = "API";

    /// <summary>
    ///
    /// </summary>
    public string CoreMode { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string O9UserName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string O9Password { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool O9Encrypt { get; set; }
}

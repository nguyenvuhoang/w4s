namespace Jits.Neptune.Web.CMS.LogicOptimal9.Common;

/// <summary>
/// Codetypes
/// </summary>
public enum EnmSendTypeOption
{
    /// <summary>
    ///
    /// </summary>
    Synchronize,
    /// <summary>
    ///
    /// </summary>
    AsSynchronize,
    /// <summary>
    ///
    /// </summary>
    Notifications
}

/// <summary>
/// Codetypes
/// </summary>
public enum EnmOrderTime
{
    /// <summary>
    /// None
    /// </summary>
    None = -1,
    /// <summary>
    /// InQuery
    /// </summary>
    InQuery = 0,
    /// <summary>
    /// InDataGrid
    /// </summary>
    InDataGrid = 1,
}

/// <summary>
/// Codetypes
/// </summary>
public enum EnmCacheAction
{
    /// <summary>
    ///
    /// </summary>
    NoCached = 0,
    /// <summary>
    ///
    /// </summary>
    Cached = 1,
    /// <summary>
    ///
    /// </summary>
    ClearCached = 2
}

/// <summary>
/// Codetypes
/// </summary>
public enum EnmJsonResponse
{
    /// <summary>
    /// success
    /// </summary>
    O = 0,
    /// <summary>
    /// error
    /// </summary>
    E = 1,
    /// <summary>
    /// warning
    /// </summary>
    W = 2
}

/// <summary>
/// Codetypes
/// </summary>
public enum EnmResultResponse
{
    /// <summary>
    /// Transaction error
    /// </summary>
    NOT_SUCCESS = -1,

    /// <summary>
    /// Transaction successful
    /// </summary>
    SUCCESS = 0,

    /// <summary>
    /// over limit
    /// </summary>
    SYS_OVER_LIMIT = 1,

    /// <summary>
    /// Need approved status dormant
    /// </summary>
    SYS_APR_STATUS = 2,

    /// <summary>
    ///
    /// </summary>
    SYS_APR_AMT = 3,

    /// <summary>
    ///
    /// </summary>
    STK_CHANGE_BRANCH = 4,

    /// <summary>
    ///
    /// </summary>
    SYS_APR_WD = 5,

    /// <summary>
    ///
    /// </summary>
    SYS_APPROVAL_REQUIRED = 6,

    /// <summary>
    ///
    /// </summary>
    SYS_APR_RELEASE = 7,

    /// <summary>
    ///
    /// </summary>
    SYS_APR_CWR = 8,

    /// <summary>
    ///
    /// </summary>
    CIF_CHECK_APPROVAL = 9,

    /// <summary>
    ///
    /// </summary>
    SYS_APR_CRD = 10,

    /// <summary>
    ///
    /// </summary>
    MULTI_TRANS_ERR = 11,

    /// <summary>
    ///
    /// </summary>
    PMT_APR_MULTI = 12,

    /// <summary>
    ///
    /// </summary>
    SYS_APR_DPT_OPN = 13,

    /// <summary>
    ///
    /// </summary>
    SYS_WARNING = 14
}

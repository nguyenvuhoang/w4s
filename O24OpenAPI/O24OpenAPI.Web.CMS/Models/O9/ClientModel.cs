using Jits.Neptune.Web.CMS.LogicOptimal9.Common;
using Newtonsoft.Json.Linq;

namespace O24OpenAPI.Web.CMS.Models.O9;

/// <summary>
///
/// </summary>
public class BackOfficeModel
{
    /// <summary>
    ///
    /// </summary>
    public BackOfficeModel() { }

    /// <summary>
    ///
    /// </summary>
    public BackOfficeModel(UserSessions userSessions)
    {
        UserId = userSessions.Usrid;
        UserBranchId = userSessions.UsrBranchid;
        SessionId = userSessions.Ssesionid;
        TransactionDate = userSessions.Txdt.ToString("dd/MM/yyyy");
    }

    /// <summary>
    ///
    /// </summary>
    public BackOfficeModel(WorkflowRequestModel workflow, List<JsonData> transactionBody)
    {
        var userSessions = workflow.user_sessions;
        UserId = userSessions.Usrid;
        UserBranchId = userSessions.UsrBranchid;
        SessionId = userSessions.Ssesionid;
        TransactionDate = userSessions.Txdt.ToString("dd/MM/yyyy");

        FunctionId = workflow.WorkflowFunc;
        TransactionBody = transactionBody;
    }

    /// <summary>
    ///
    /// </summary>
    public string TransactionCode { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string TransactionDate { get; set; }

    /// <summary>
    ///
    /// </summary>
    public List<JsonData> TransactionBody { get; set; } = new List<JsonData>();

    /// <summary>
    ///
    /// </summary>
    public string FunctionId { get; set; } = "";

    /// <summary>
    ///
    /// </summary>
    public EnmCacheAction EnmCacheAction { get; set; } = EnmCacheAction.NoCached;

    /// <summary>
    ///
    /// </summary>
    public int UserBranchId { get; set; }

    /// <summary>
    ///
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string Lang { get; set; } = "en";

    /// <summary>
    ///
    /// </summary>
    public string Status { get; set; } = "C";

    /// <summary>
    ///
    /// </summary>
    public string TransactionRefId { get; set; } = null;

    /// <summary>
    ///
    /// </summary>
    public string ValueDate { get; set; } = null;

    /// <summary>
    ///
    /// </summary>
    public int? ApproveUserId { get; set; } = null;

    /// <summary>
    ///
    /// </summary>
    public string ApproveUserIp { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string ApproveDate { get; set; } = "";

    /// <summary>
    ///
    /// </summary>
    public string IsReverse { get; set; } = "N";

    /// <summary>
    ///
    /// </summary>
    public string UserWS { get; set; } = "";

    /// <summary>
    ///
    /// </summary>
    public string ApproveUserWS { get; set; } = "";

    /// <summary>
    ///
    /// </summary>
    public int HBranchId { get; set; } = 0;

    /// <summary>
    ///
    /// </summary>
    public int RBranchId { get; set; } = 0;

    /// <summary>
    ///
    /// </summary>
    public string ApproveReason { get; set; } = null;

    /// <summary>
    ///
    /// </summary>
    public string PRN { get; set; } = "";

    /// <summary>
    ///
    /// </summary>
    public bool HasArrayValue { get; set; } = false;

    /// <summary>
    ///
    /// </summary>
    public string SessionId { get; set; }
}

/// <summary>
///
/// </summary>
public class FrontOfficeModel
{
    /// <summary>
    ///
    /// </summary>
    public FrontOfficeModel() { }

    /// <summary>
    ///
    /// </summary>
    /// <param name="userSessions"></param>
    public FrontOfficeModel(UserSessions userSessions)
    {
        UserId = userSessions.Usrid;
        UserBranchId = userSessions.UsrBranchid;
        SessionId = userSessions.Ssesionid;
        TransactionDate = userSessions.Txdt.ToString("dd/MM/yyyy");
    }

    /// <summary>
    ///
    /// </summary>
    public FrontOfficeModel(
        WorkflowRequestModel workflow,
        string status,
        JObject transactionBody,
        JObject ifcFee
    )
    {
        var userSessions = workflow.user_sessions;
        UserId = userSessions.Usrid;
        UserBranchId = userSessions.UsrBranchid;
        SessionId = userSessions.Ssesionid;
        TransactionDate = userSessions.Txdt.ToString("dd/MM/yyyy");
        ValueDate = workflow.ValueDate?.ToString("dd/MM/yyyy") ?? workflow.StringWorkingDate;

        ApproveUserId = workflow.user_approve.Usrid != 0 ? workflow.user_approve.Usrid : null;
        ApproveDate =
            workflow.user_approve.Usrid != 0
                ? (workflow.ValueDate?.ToString("dd/MM/yyyy") ?? workflow.StringWorkingDate)
                : null;

        IfcFee = ifcFee;
        Status = status;
        TransactionCode = workflow.WorkflowFunc;
        TransactionBody = transactionBody;
    }

    /// <summary>
    ///
    /// </summary>
    public string SessionId { get; set; }

    /// <summary>
    ///
    /// </summary>
    public int UserBranchId { get; set; }

    /// <summary>
    ///
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string Lang { get; set; } = "en";

    /// <summary>
    ///
    /// </summary>
    public string TransactionCode { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string TransactionDate { get; set; }

    /// <summary>
    ///
    /// </summary>
    public JObject TransactionBody { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string FunctionId { get; set; } = "";

    /// <summary>
    ///
    /// </summary>
    public EnmCacheAction EnmCacheAction { get; set; } = EnmCacheAction.NoCached;

    /// <summary>
    ///
    /// </summary>
    public string Status { get; set; } = "C";

    /// <summary>
    ///
    /// </summary>
    public string TransactionRefId { get; set; } = null;

    /// <summary>
    ///
    /// </summary>
    public string ValueDate { get; set; } = null;

    /// <summary>
    ///
    /// </summary>
    public JObject IfcFee { get; set; } = null;

    /// <summary>
    ///
    /// </summary>
    public string UserWS { get; set; } = "";

    /// <summary>
    ///
    /// </summary>
    public int? ApproveUserId { get; set; } = null;

    /// <summary>
    ///
    /// </summary>
    public string ApproveUserIp { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string ApproveUserWS { get; set; } = "";

    /// <summary>
    ///
    /// </summary>
    public string ApproveDate { get; set; } = "";

    /// <summary>
    ///
    /// </summary>
    public string IsReverse { get; set; } = "N";

    /// <summary>
    ///
    /// </summary>
    public int? HBranchId { get; set; } = null;

    /// <summary>
    ///
    /// </summary>
    public int? RBranchId { get; set; } = null;

    /// <summary>
    ///
    /// </summary>
    public string ApproveReason { get; set; } = null;

    /// <summary>
    ///
    /// </summary>
    public JsonPosting PostingData { get; set; } = null;

    /// <summary>
    ///
    /// </summary>
    public string PRN { get; set; } = "";

    /// <summary>
    ///
    /// </summary>
    public string Id { get; set; } = null;

    /// <summary>
    ///
    /// </summary>
    public bool HasJsonFrontOfficeMapping { get; set; } = true;
}

/// <summary>
///
/// </summary>
public static class BackOfficeExtension
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public static JsonBackOffice ToJsonBackOffice(this BackOfficeModel model)
    {
        if (string.IsNullOrEmpty(model.FunctionId))
        {
            model.FunctionId = GlobalVariable.UTIL_CALL_PROC;
        }

        JsonBackOffice clsJsonBackOffice = new()
        {
            TXCODE = model.TransactionCode, // Field 1
            TXDT = model.TransactionDate, // Field 2
            BRANCHID = model.UserBranchId, // Field 5
            USRID = model.UserId, // Field 7
            LANG = model.Lang, // Field 8
            STATUS = model.Status, // Field 14
            TXREFID = model.TransactionRefId, // Field 3
            VALUEDT = model.ValueDate, // Field 4
            USRWS = string.IsNullOrEmpty(model.UserWS)
                ? GlobalVariable.O9_GLOBAL_COMPUTER_NAME
                : model.UserWS, // Field 6
            APUSER = model.ApproveUserId, // Field 9
            APUSRIP = model.ApproveUserIp, // Field 10
            APUSRWS = model.ApproveUserWS, // Field 11
            APDT = model.ApproveDate, // Field 12
            ISREVERSE = model.IsReverse, // Field 14
            HBRANCHID = model.HBranchId, // Field 15
            RBRANCHID = model.RBranchId, // Field 16
            TXBODY = model.TransactionBody, // Field 17
            APREASON = model.ApproveReason, // Field 18
            PRN = model.PRN, // Field 19
            ID = Guid.NewGuid()
                .ToString() // Field 20
            ,
        };
        return clsJsonBackOffice;
    }
}

/// <summary>
///
/// </summary>
public static class FrontOfficeExtension
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public static JsonFrontOffice ToJsonFrontOffice(this FrontOfficeModel model)
    {
        if (string.IsNullOrEmpty(model.FunctionId))
        {
            model.FunctionId = GlobalVariable.UTIL_CALL_PROC;
        }

        if (string.IsNullOrEmpty(model.PRN))
        {
            model.PRN = "";
        }

        JsonFrontOffice clsJsonFrontOffice = new()
        {
            TXCODE = model.TransactionCode, // Field 1
            TXDT = model.TransactionDate, // Field 2
            BRANCHID = model.UserBranchId, // Field 5
            USRID = model.UserId, // Field 7
            LANG = model.Lang, // Field 8
            STATUS = model.Status, // Field 14
            TXREFID = model.TransactionRefId, // Field 3
            VALUEDT = !string.IsNullOrEmpty(model.ValueDate)
                ? model.ValueDate
                : model.TransactionDate, // Field 4
            USRWS = model.UserWS, // Field 6
            APUSER = model.ApproveUserId, // Field 9
            APUSRIP = model.ApproveUserIp, // Field 10
            APUSRWS = model.ApproveUserWS, // Field 11
            APDT = model.ApproveDate, // Field 12
            ISREVERSE = model.IsReverse, // Field 14
            HBRANCHID = model.HBranchId, // Field 15
            RBRANCHID = model.RBranchId, // Field 16
            TXBODY = model.TransactionBody, // Field 17
            APREASON = model.ApproveReason, // Field 18
            IFCFEE = model.IfcFee,
            PRN = model.PRN, // Field 19
            ID = string.IsNullOrEmpty(model.Id) ? Guid.NewGuid().ToString() : model.Id,
        };
        return clsJsonFrontOffice;
    }
}

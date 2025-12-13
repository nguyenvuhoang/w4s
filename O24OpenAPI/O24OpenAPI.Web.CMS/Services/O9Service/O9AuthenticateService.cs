using Apache.NMS;
using Jits.Neptune.Web.CMS.LogicOptimal9.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using O24OpenAPI.Core.Domain.Users;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.GrpcContracts.GrpcClientServices.CTH;
using O24OpenAPI.Web.CMS.Constant;
using O24OpenAPI.Web.CMS.Models;
using O24OpenAPI.Web.CMS.Models.ContextModels;
using O24OpenAPI.Web.CMS.Models.O9;
using O24OpenAPI.Web.CMS.Models.Request;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using O24OpenAPI.Web.Framework.Services;

namespace O24OpenAPI.Web.CMS.Services.O9Service;

public class O9AuthenticateService(
    IO9ClientService o9clientService,
    JWebUIObjectContextModel context,
    IJwtTokenService jwtTokenService,
    IUserSessionsService userSessionsService,
    ICTHGrpcClientService cthService
) : ICoreAuthenticateService
{
    private readonly IO9ClientService _o9clientService = o9clientService;
    private JWebUIObjectContextModel _context = context;
    private readonly IJwtTokenService _jwtTokenService = jwtTokenService;
    private readonly IUserSessionsService _userSessionsService = userSessionsService;
    private readonly ICTHGrpcClientService _cthService = cthService;

    public async Task<LoginCoreResponse> Login(AuthenJWTModel model)
    {
        JsonLogin loginRequest = new()
        {
            L = model.LoginName,
            P = model.IsEncrypted ? model.PassWord : O9Encrypt.SHA256Encrypt(model.PassWord),
            A = false,
        };

        string strResult = await _o9clientService.GenJsonBodyRequestAsync(
            objJsonBody: loginRequest,
            functionId: GlobalVariable.UMG_LOGIN,
            sessionId: "",
            isResultCaching: EnmCacheAction.NoCached,
            sendType: EnmSendTypeOption.Synchronize,
            usrId: "-1",
            priority: MsgPriority.Normal
        );

        if (string.IsNullOrEmpty(strResult))
        {
            throw new O24OpenAPIException($"User not found!");
        }

        LoginO9ResponseModel responseModel =
            JsonConvert.DeserializeObject<LoginO9ResponseModel>(strResult)
            ?? throw new O24OpenAPIException("Invalid username or password!");

        if (responseModel.e.HasValue())
        {
            throw await ErrorUtils.CreateException($"SYS.{responseModel.e}");
        }
        var expireTime = GlobalVariable.ExpireTime();
        var token = _jwtTokenService.GetNewJwtToken(
            new User
            {
                Id = responseModel.usrid,
                Username = responseModel.usrname,
                UserCode = responseModel.usrcd,
                BranchCode = responseModel.branchcd,
                LoginName = responseModel.lgname,
                DeviceId = model.DeviceId,
            },
            expireTime.ToUnixTimeSeconds()
        );
        var hashedToken = token.Hash();
        var refreshToken = JwtTokenService.GenerateRefreshToken();
        var hashedRefreshToken = refreshToken.Hash();
        responseModel.token = token;
        responseModel.RefreshToken = refreshToken;
        var commandSet = new HashSet<string>(responseModel.menuarc);
        commandSet.UnionWith(responseModel.txdef);
        var strSql =
            @"SELECT su.POSITION, sb.BRANCHCD, su.LGNAME, su.USRCD, su.USRNAME, sb.BRNAME, su.STATUS, sb.ISONLINE
                            FROM o9cbs.S_USRAC su INNER JOIN o9cbs.S_BRANCH sb ON su.BRANCHID = sb.BRANCHID
                            WHERE su.USRID  = {0}";
        strSql = string.Format(strSql, responseModel.usrid);
        var result = await _o9clientService.SearchAsync(strSql, 0);
        var dataSearch =
            (JObject)result.SelectToken("data")?.FirstOrDefault()
            ?? throw new InvalidOperationException("User profile not found");
        var userProfile = dataSearch.JsonConvertToModel<LoginInfoModel>();
        responseModel.BranchStatus = userProfile.BranchStatus;

        var setChannelRoles = await _cthService.GetChannelRolesAsync(6);
        var contextUserLogin = _context.InfoUser.GetUserLogin();
        UserSessions userSessions = new()
        {
            Usrid = responseModel.usrid,
            UserCode = responseModel.usrcd,
            Ssesionid = responseModel.uuid,
            UsrBranch = responseModel.branchcd,
            UsrBranchid = responseModel.branchid,
            Usrname = responseModel.usrname,
            Lang = responseModel.lang,
            Txdt = DateTime.ParseExact(responseModel.busdate, "dd/MM/yyyy", null),
            Exptime = expireTime.DateTime,
            Token = hashedToken,
            Acttype = SessionStatus.Login,
            Wsip = _context.InfoRequest.GetIp(),
            CommandList = commandSet.ToSerialize(),
            ResetPassword = responseModel.pwdreset.HasValue(),
            LoginName = model.LoginName,
            ApplicationCode = _context.InfoApp.GetApp(),
            SessionDevice = contextUserLogin?.MyDevice.ToSerialize(),
            Wsname = contextUserLogin?.GetDeviceName(),
            HashedRefreshToken = hashedRefreshToken,
            ChannelRoles = setChannelRoles.ToSerializeSystemText(),
        };

        await _userSessionsService.Insert(userSessions);
        if (!model.IsDigital)
        {
            _context.Bo.AddPackFo("rolecommand", commandSet);
        }
        else
        {
            O9Client.CoreBankingSession = JsonConvert.DeserializeObject<JsonLoginResponse>(
                strResult
            );
        }
        return new LoginCoreResponse(responseModel);
    }

    public async Task<string> Logout()
    {
        var userSessions = SessionUtils.GetUserSession(_context);
        string proc = "UMG_LOGOUT";
        string strResult = await _o9clientService.GenJsonBodyRequestAsync(
            null,
            proc,
            userSessions.Ssesionid,
            EnmCacheAction.NoCached,
            EnmSendTypeOption.Synchronize,
            userSessions.Usrid.ToString()
        );
        await _userSessionsService.UpdateActtype(userSessions.Token, SessionStatus.Logout);
        return strResult;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<JObject> GetById(int id)
    {
        JObject jsResult = null;
        JsonGetDataById jsRequest = new() { I = id, M = false };

        string strJsonResult = await _o9clientService.GenJsonDataByIdRequest(
            jsRequest,
            "ADM_GET_USER"
        );
        if (!string.IsNullOrEmpty(strJsonResult))
        {
            jsResult = JObject.Parse(strJsonResult);
        }

        return jsResult;
    }

    /// <summary>
    /// /// UMG_CHANGE_PASSWORD
    /// </summary>
    /// <param name="workflow"></param>
    /// <returns></returns>
    /// <exception cref="NeptuneException"></exception>
    public async Task<JToken> ChangePassword(ChangePasswordModel model)
    {
        JObject response = new();
        try
        {
            var userSession = SessionUtils.GetUserSession(_context);

            string oldPwd = model.OldPassword;
            string newPwd = model.NewPassword;
            string lgName = userSession.LoginName;
            JObject jsRequest = new()
            {
                { "O", O9Encrypt.SHA256Encrypt(oldPwd) },
                { "N", O9Encrypt.SHA256Encrypt(newPwd) },
                { "NO", newPwd },
                { "LG", lgName },
            };
            string strJsonResult = await _o9clientService.GenJsonBodyRequestAsync(
                jsRequest,
                "UMG_CHANGE_PASSWORD",
                userSession.Ssesionid,
                EnmCacheAction.NoCached,
                EnmSendTypeOption.Synchronize,
                userSession.Usrid.ToString(),
                MsgPriority.Normal
            );
            if (strJsonResult.HasValue())
            {
                JObject jsResult = JObject.Parse(strJsonResult);
                string errDesc = string.Empty;
                if (jsResult.ToLowerKey().Value<string>("e").HasValue())
                {
                    throw new Exception(jsResult.ToLowerKey().Value<string>("e"));
                }

                if (jsResult.ToLowerKey().Value<bool>("t"))
                {
                    response.Add("update", "Y");
                }
            }
            return response.BuildWorkflowResponseSuccess(false);
        }
        catch (Exception ex)
        {
            throw await ErrorUtils.CreateException(ex.Message, ex);
        }
    }
}

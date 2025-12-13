namespace O24OpenAPI.Core.Domain.Users;

public class UserContext
{
    public string UserId { get; private set; }
    public string UserCode { get; private set; }
    public string UserName { get; private set; }
    public string UserChannel { get; private set; }
    public string LoginName { get; private set; }

    public void SetUserContext(UserContext userContext)
    {
        if (userContext == null)
        {
            return;
        }

        UserId = userContext.UserId;
        UserCode = userContext.UserCode;
        UserName = userContext.UserName;
        UserChannel = userContext.UserChannel;
        LoginName = userContext.LoginName;
    }

    public void SetUserContext(UserContextTemplate userContext)
    {
        if (userContext == null)
        {
            return;
        }

        UserId = userContext.UserId;
        UserCode = userContext.UserCode;
        UserName = userContext.UserName;
        UserChannel = userContext.UserChannel;
        LoginName = userContext.LoginName;
    }

    public void SetUserId(string userId)
    {
        UserId = userId;
    }

    public void SetUserCode(string userCode)
    {
        UserCode = userCode;
    }

    public void SetUserName(string userName)
    {
        UserName = userName;
    }

    public void SetUserChannel(string userChannel)
    {
        UserChannel = userChannel;
    }

    public void SetLoginName(string loginName)
    {
        LoginName = loginName;
    }
}

public class UserContextTemplate
{
    public string UserId { get; set; }
    public string UserCode { get; set; }
    public string UserName { get; set; }
    public string UserChannel { get; set; }
    public string LoginName { get; set; }
}

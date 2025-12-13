USE o24cth
GO
IF OBJECT_ID('dbo.__GetMenuInfoFromRoleId', 'P ') IS NOT NULL DROP PROCEDURE dbo.__GetMenuInfoFromRoleId;
GO
CREATE PROCEDURE [dbo].[__GetMenuInfoFromRoleId]
    @ApplicationCode NVARCHAR(50),
    @RoleId NVARCHAR(10),
    @Lang VARCHAR(2),
    @UserCommand NVARCHAR(MAX) OUTPUT
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Recursive CTE to fetch Parent-Child hierarchy
    WITH CommandHierarchy AS (
        -- Select all commands (both parents and children)
        SELECT 
            A.ParentId,
            A.CommandId,
            JSON_VALUE(A.CommandNameLanguage, CONCAT('$.', @Lang)) AS label,
            A.CommandType,
						A.CommandURI,
            C.RoleId,
            C.RoleName,
            B.Invoke,
            B.Approve,
            A.GroupMenuIcon AS icon,
            A.GroupMenuVisible,
            A.GroupMenuId,
            A.GroupMenuListAuthorizeForm
        FROM 
            dbo.UserCommand A 
        INNER JOIN dbo.UserRight B ON B.CommandId = A.CommandId
        INNER JOIN dbo.UserRole C ON C.RoleID = B.RoleID
        WHERE 
            A.ApplicationCode = @ApplicationCode
            AND A.Enabled = 1
            AND C.RoleID = @RoleId
    )

    -- Generate JSON output for parent commands with children only under children key
    SELECT @UserCommand = (
        SELECT 
            Parent.ParentId as parentid,
            Parent.CommandId as commandid ,
            Parent.label as label,
            Parent.CommandType as commandtype ,
			Parent.CommandURI AS href,
            Parent.RoleId as roleid,
            Parent.RoleName as rolename,
            Parent.Invoke as invoke,
            Parent.Approve as approve,
            Parent.icon as icon,
            Parent.GroupMenuVisible as groupmenuvisible ,
            Parent.GroupMenuId as groupmenuid ,
            Parent.GroupMenuListAuthorizeForm as groupmenulistauthorizeform ,
            -- Subquery for children, excluding any children from the top-level command list
            (
                SELECT 
                    Child.CommandId as commandid ,
                    Child.label as label,
                    Child.CommandType as commandtype,
					Child.CommandURI AS href,
                    Child.RoleId as roleid ,
                    Child.RoleName as rolename,
                    Child.Invoke as invoke,
                    Child.Approve as approve,
                    Child.icon as icon,
                    Child.GroupMenuVisible as groupmenuvisible,
                    Child.GroupMenuId AS prefix,
                    Child.GroupMenuListAuthorizeForm as groupmenulistauthorizeform
                FROM CommandHierarchy AS Child
                WHERE Child.ParentId = Parent.CommandId
                FOR JSON PATH
            ) AS children
        FROM CommandHierarchy AS Parent
        WHERE Parent.ParentId = '0' -- Only select top-level parent commands
        FOR JSON PATH
    );

END
GO
USE O24CMS
GO
IF OBJECT_ID('dbo.__GetCurrentUserName', 'FN') IS NOT NULL DROP FUNCTION dbo.__GetCurrentUserName;
GO
CREATE FUNCTION dbo.__GetCurrentUserName(
    @WorkflowScheme nvarchar(max)
)
RETURNS NVARCHAR(150)
BEGIN
    RETURN JSON_VALUE(
        @WorkflowScheme,
        '$.Request.RequestHeader.TxContext.current_username'
    );
END
GO
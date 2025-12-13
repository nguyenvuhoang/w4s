USE o24cth
GO
IF OBJECT_ID('dbo.__GetCurrentLoginName', 'FN') IS NOT NULL DROP FUNCTION dbo.__GetCurrentLoginName;
GO
CREATE FUNCTION dbo.__GetCurrentLoginName(
    @WorkflowScheme nvarchar(max)
)
RETURNS NVARCHAR(150)
BEGIN
    RETURN JSON_VALUE(
        @WorkflowScheme,
        '$.Request.RequestHeader.TxContext.current_loginname'
    );
END
GO
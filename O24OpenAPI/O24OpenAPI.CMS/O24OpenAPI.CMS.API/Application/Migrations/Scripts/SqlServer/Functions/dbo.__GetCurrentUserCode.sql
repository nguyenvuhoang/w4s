USE O24CMS
GO
IF OBJECT_ID('dbo.__GetCurrentUserCode', 'FN') IS NOT NULL DROP FUNCTION dbo.__GetCurrentUserCode;
GO
CREATE FUNCTION dbo.__GetCurrentUserCode(
    @WorkflowScheme nvarchar(max)
)
RETURNS NVARCHAR(2000)
BEGIN
    RETURN JSON_VALUE(
        @WorkflowScheme,
        '$.request.request_header.tx_context.current_user_code'
    );
END
GO
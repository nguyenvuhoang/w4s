USE O24CMS
GO
IF OBJECT_ID('dbo.__GetCurrentBranchCode', 'FN') IS NOT NULL DROP FUNCTION dbo.__GetCurrentBranchCode;
GO
CREATE FUNCTION dbo.__GetCurrentBranchCode(
    @WorkflowScheme nvarchar(max)
)
RETURNS NVARCHAR(10)
BEGIN
    RETURN JSON_VALUE(
        @WorkflowScheme,
        '$.request.request_header.tx_context.current_branch_code'
    );
END
GO
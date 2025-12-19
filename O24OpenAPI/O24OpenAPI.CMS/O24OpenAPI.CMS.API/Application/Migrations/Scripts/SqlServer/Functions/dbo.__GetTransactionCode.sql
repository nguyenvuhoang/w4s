USE O24CMS
GO
IF OBJECT_ID('dbo.__GetTransactionCode', 'FN') IS NOT NULL DROP FUNCTION dbo.__GetTransactionCode;
GO
CREATE FUNCTION dbo.__GetTransactionCode(
    @WorkflowScheme nvarchar(max)
)
RETURNS NVARCHAR(500)
BEGIN
    RETURN JSON_VALUE(
        @WorkflowScheme,
        '$.Request.RequestHeader.TxContext.transaction_code'
    );
END
GO
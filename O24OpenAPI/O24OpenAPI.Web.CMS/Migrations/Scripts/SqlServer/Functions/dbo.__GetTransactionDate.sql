USE O24CMS
GO
IF OBJECT_ID('dbo.__GetTransactionDate', 'FN') IS NOT NULL DROP FUNCTION dbo.__GetTransactionDate;
GO
CREATE FUNCTION dbo.__GetTransactionDate(
    @WorkflowScheme nvarchar(max)
)
RETURNS DATETIME2
BEGIN
    RETURN JSON_VALUE(
        @WorkflowScheme,
        '$.Request.RequestHeader.TxContext.transaction_date'
    );
END
GO
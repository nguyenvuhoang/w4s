USE o24cth
GO
IF OBJECT_ID('dbo.__GetTransactionNumber', 'FN') IS NOT NULL DROP FUNCTION dbo.__GetTransactionNumber;
GO
CREATE FUNCTION dbo.__GetTransactionNumber(
    @WorkflowScheme nvarchar(max)
)
RETURNS NVARCHAR(100)
BEGIN
    RETURN JSON_VALUE(
        @WorkflowScheme,
        '$.Request.RequestHeader.TxContext.transaction_number'
    );
END
GO
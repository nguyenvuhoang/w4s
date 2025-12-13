USE o24cth
GO
IF OBJECT_ID('dbo.__GetDescription', 'FN') IS NOT NULL DROP FUNCTION dbo.__GetDescription;
GO
CREATE FUNCTION dbo.__GetDescription(
    @WorkflowScheme nvarchar(max)
)
RETURNS NVARCHAR(2000)
BEGIN
    RETURN JSON_VALUE(
        @WorkflowScheme,
        '$.Request.RequestHeader.TxContext.description'
    );
END
GO
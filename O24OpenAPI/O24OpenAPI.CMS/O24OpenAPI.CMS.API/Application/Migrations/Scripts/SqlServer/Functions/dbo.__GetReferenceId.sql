USE O24CMS
GO
IF OBJECT_ID('dbo.__GetReferenceId', 'FN') IS NOT NULL DROP FUNCTION dbo.__GetReferenceId;
GO
CREATE FUNCTION dbo.__GetReferenceId(
    @WorkflowScheme nvarchar(max)
)
RETURNS NVARCHAR(2000)
BEGIN
    RETURN JSON_VALUE(
        @WorkflowScheme,
        '$.Request.RequestHeader.TxContext.reference_id'
    );
END
GO
USE O24CMS
GO
IF OBJECT_ID('dbo.__GetRefId', 'FN') IS NOT NULL DROP FUNCTION dbo.__GetRefId;
GO
CREATE FUNCTION dbo.__GetRefId(
    @WorkflowScheme nvarchar(max)
)
RETURNS NVARCHAR(100)
BEGIN
    RETURN JSON_VALUE(
        @WorkflowScheme,
        '$.Request.RequestHeader.TxContext.ref_id'
    );
END
GO
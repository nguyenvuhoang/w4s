USE O24CMS
GO
IF OBJECT_ID('dbo.__GetSubCode', 'FN') IS NOT NULL DROP FUNCTION dbo.__GetSubCode;
GO
CREATE FUNCTION dbo.__GetSubCode(
    @WorkflowScheme nvarchar(max)
)
RETURNS NVARCHAR(200)
BEGIN
    RETURN JSON_VALUE(
        @WorkflowScheme,
        '$.Request.RequestHeader.TxContext.sub_code'
    );
END
GO
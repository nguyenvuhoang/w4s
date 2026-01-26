USE O24CMS
GO
IF OBJECT_ID('dbo.__GetLanguage', 'FN') IS NOT NULL DROP FUNCTION dbo.__GetLanguage;
GO
CREATE FUNCTION dbo.__GetLanguage(
    @WorkflowScheme nvarchar(max)
)
RETURNS nvarchar(10)
BEGIN
    RETURN JSON_VALUE(
        @WorkflowScheme,
        '$.request.request_body.workflow_input.lang'
    );
END
GO
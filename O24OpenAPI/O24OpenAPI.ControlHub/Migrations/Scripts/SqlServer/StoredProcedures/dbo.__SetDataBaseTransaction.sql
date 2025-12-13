USE o24cth
GO
IF OBJECT_ID('dbo.__SetDataBaseTransaction', 'P ') IS NOT NULL DROP PROCEDURE dbo.__SetDataBaseTransaction;
GO
CREATE PROCEDURE [dbo].[__SetDataBaseTransaction]
    @WorkflowScheme NVARCHAR(MAX),
    @OutputWorkflowScheme NVARCHAR(MAX) OUTPUT
AS
BEGIN
    SELECT
        @OutputWorkflowScheme = JSON_MODIFY(@OutputWorkflowScheme, CONCAT(N'$.Response.Data.', [Key]), value)
    FROM
    OPENJSON(JSON_QUERY(@WorkflowScheme, '$.Request.RequestHeader.TxContext'))
    
END
GO
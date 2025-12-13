USE o24cth
GO
IF OBJECT_ID('dbo.__GetValueDate', 'FN') IS NOT NULL DROP FUNCTION dbo.__GetValueDate;
GO
CREATE FUNCTION [dbo].[__GetValueDate](
    @WorkflowScheme nvarchar(max)
)
RETURNS DATETIME2
BEGIN
    RETURN JSON_VALUE(
        @WorkflowScheme,
        '$.request.request_header.tx_context.value_date'
    );
END
GO
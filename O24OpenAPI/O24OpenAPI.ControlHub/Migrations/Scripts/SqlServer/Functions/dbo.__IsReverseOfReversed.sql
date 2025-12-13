USE o24cth
GO
IF OBJECT_ID('dbo.__IsReverseOfReversed', 'FN') IS NOT NULL DROP FUNCTION dbo.__IsReverseOfReversed;
GO
CREATE FUNCTION [dbo].[__IsReverseOfReversed](
    @WorkflowScheme nvarchar(max)
)
RETURNS BIT
BEGIN

    DECLARE @ReversalExecutionId NVARCHAR(100) = JSON_VALUE(@WorkflowScheme,'$.request.request_header.reversal_execution_id');

    DECLARE @IsCompensated NVARCHAR(100) = JSON_VALUE(@WorkflowScheme,'$.request.request_header.is_compensated');

	IF dbo.__HasValue(@ReversalExecutionId) = 1 AND @IsCompensated = 'Y'
	BEGIN
		RETURN 1;
	END;

    RETURN 0;
END
GO
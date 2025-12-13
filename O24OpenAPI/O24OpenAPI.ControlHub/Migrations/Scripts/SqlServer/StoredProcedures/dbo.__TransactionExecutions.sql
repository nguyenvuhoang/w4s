USE o24cth
GO
IF OBJECT_ID('dbo.__TransactionExecutions', 'P ') IS NOT NULL DROP PROCEDURE dbo.__TransactionExecutions;
GO
CREATE PROCEDURE [dbo].[__TransactionExecutions](
    @WorkflowScheme nvarchar(max), 
    @IsReverse nvarchar(1), -- R: Reverse |N: Normal
    @OutputWorkflowScheme nvarchar(max) OUTPUT
)
AS
BEGIN    
    -- Lấy thông tin giao dịch
    DECLARE @TransactionCode varchar(50) = dbo.__GetTransactionCode(@WorkflowScheme);
	
    DECLARE @PathData varchar(1000) = '$.request.request_body.data';
    DECLARE @OutputJsonParam NVARCHAR(MAX) = JSON_QUERY(@WorkflowScheme, @PathData);
    -- Thực hiện truy vấn và thực hiện các quy tắc
    DECLARE @RuleName NVARCHAR(1000);
    
    DECLARE @Sql NVARCHAR(MAX);
    DECLARE @Params NVARCHAR(MAX);
    DECLARE @StoreProcedure NVARCHAR(MAX);
    DECLARE @Parameter NVARCHAR(MAX);
    DECLARE @JsonParam NVARCHAR(MAX);

	DECLARE myCursor CURSOR
	FOR
	SELECT StoreProcedure, JSON_QUERY(Parameter) Parameter
	FROM dbo.TransactionExecutions
	WHERE WorkflowId = @TransactionCode AND IsEnable = 1 AND dbo.__CheckCondition(@WorkflowScheme, JSON_QUERY(Condition)) = 1 ORDER BY ExecOrder

	OPEN myCursor;

	FETCH NEXT FROM myCursor
	INTO @StoreProcedure, @Parameter;

	WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @JsonParam = JSON_QUERY(dbo.__MappingParameter(@OutputJsonParam, JSON_QUERY(@Parameter)));
		
		SET @Sql = 'EXEC dbo.' +@StoreProcedure+ ' @WorkflowScheme, @IsReverse, @JsonParam, @OutputJsonParam OUTPUT, @OutputWorkflowScheme OUTPUT';

		-- Thực hiện truy vấn
		SET @Params = N'@WorkflowScheme NVARCHAR(MAX), @IsReverse NVARCHAR(1), @JsonParam NVARCHAR(MAX), @OutputJsonParam nvarchar(max) OUTPUT, @OutputWorkflowScheme nvarchar(max) OUTPUT';
		
		EXEC sp_executesql @Sql,
			@Params,
			@WorkflowScheme,
			@IsReverse,
			@JsonParam,
			@OutputJsonParam OUTPUT,
			@OutputWorkflowScheme OUTPUT;

		FETCH NEXT FROM myCursor
		INTO @StoreProcedure, @Parameter;
	END;
END
GO
USE O24CMS
GO
IF OBJECT_ID('dbo.DPT_TRANSACTION_EXEC', 'P ') IS NOT NULL DROP PROCEDURE dbo.DPT_TRANSACTION_EXEC;
GO
CREATE PROCEDURE [dbo].[DPT_TRANSACTION_EXEC]
    @WorkflowScheme nvarchar(max), 
    @IsReverse nvarchar(1), -- R: Reverse |N: Normal
    @OutputWorkflowScheme nvarchar(max) OUTPUT
AS
BEGIN TRY
BEGIN TRANSACTION

    -- Start default store not change
    EXEC dbo.__ValidateBaseInfo @WorkflowScheme = @WorkflowScheme
    SET @OutputWorkflowScheme = @WorkflowScheme;

    DECLARE @StartTime DATETIME = GETUTCDATE();
	DECLARE @Duration INT = 0;
	DECLARE @JsonResponse nvarchar(max) = JSON_QUERY(@WorkflowScheme, '$.request.request_body.data');
	
	-- Start Validate Transactions, define in table TransactionRules
    EXEC dbo.__ValidateTransactionRule 
        @WorkflowScheme = @WorkflowScheme, 
        @IsReverse = @IsReverse
	-- End Validate Transactions

	IF @IsReverse = 'R'
	BEGIN
	
		DECLARE @RefId VARCHAR(50) = dbo.__GetRefId(@WorkflowScheme);

		SELECT @JsonResponse = JSON_QUERY(ResponseBody) from o9deposit.dbo.[Transaction] where RefId = @RefId;

		SET @OutputWorkflowScheme = JSON_MODIFY(@OutputWorkflowScheme, '$.response.data', JSON_QUERY(@JsonResponse));

		EXEC dbo.__SetDataBaseTransaction
			@WorkflowScheme = @WorkflowScheme,
			@OutputWorkflowScheme = @OutputWorkflowScheme OUTPUT

		EXEC dbo.__ReverseTransaction @WorkflowScheme = @WorkflowScheme;

		EXEC dbo.__CreateTransaction 
					@WorkflowScheme = @OutputWorkflowScheme, 
					@StartTime = @StartTime,
					@IsReverse = @IsReverse,
					@OutputWorkflowScheme = @OutputWorkflowScheme OUTPUT;

		COMMIT TRANSACTION;
		RETURN
	END;

	SET @OutputWorkflowScheme = JSON_MODIFY(@OutputWorkflowScheme, '$.response.data', JSON_QUERY(@JsonResponse));

	EXEC dbo.__SetDataBaseTransaction
		@WorkflowScheme = @WorkflowScheme,
		@OutputWorkflowScheme = @OutputWorkflowScheme OUTPUT

	--Start Xử lý chính của giao dịch, define in table TransactionExecutions

	EXEC dbo.__TransactionExecutions 
				@WorkflowScheme = @WorkflowScheme,
				@IsReverse = @IsReverse,
				@OutputWorkflowScheme = @OutputWorkflowScheme OUTPUT;

	--End Xử lý chính của giao dịch
    
	EXEC dbo.__CreateTransaction 
				@WorkflowScheme = @OutputWorkflowScheme, 
				@StartTime = @StartTime,
				@IsReverse = @IsReverse,
				@OutputWorkflowScheme = @OutputWorkflowScheme OUTPUT;
	-- End default store
COMMIT TRANSACTION
END TRY
BEGIN CATCH
    ROLLBACK;
    -- Ghi thông tin vào log
    DECLARE @Error NVARCHAR(MAX) = ERROR_MESSAGE();
    DECLARE @FullError NVARCHAR(MAX) = 'ErrorMessage: ' + ERROR_MESSAGE() + ' - ErrorStore: '+ ERROR_PROCEDURE() +' - ErrorLine: ' +CAST(ERROR_LINE() AS varchar(10));

    EXEC dbo.__Log 
        @WorkflowScheme = @WorkflowScheme, 
        @ShortMessage = @Error, 
        @FullMessage = @FullError;

	EXEC dbo.__CreateTransaction 
				@WorkflowScheme = @OutputWorkflowScheme, 
				@StartTime = @StartTime,
				@IsReverse = @IsReverse,
				@IsError = 1,
				@ErrorMessage = @Error,
				@OutputWorkflowScheme = @OutputWorkflowScheme OUTPUT;

    THROW;
END CATCH;
GO
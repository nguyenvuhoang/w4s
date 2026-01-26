USE O24CMS
GO
IF OBJECT_ID('dbo.IB_VERIFY_ACCOUNT', 'P ') IS NOT NULL DROP PROCEDURE dbo.IB_VERIFY_ACCOUNT;
GO

CREATE PROCEDURE [dbo].[IB_VERIFY_ACCOUNT] @WorkflowScheme NVARCHAR(max)
	,@IsReverse NVARCHAR(1)
	,-- R: Reverse |N: Normal
	@OutputWorkflowScheme NVARCHAR(max) OUTPUT
AS
BEGIN TRY
	BEGIN TRANSACTION

	-- Thêm thời gian bắt đầu thực hiện transaction
	DECLARE @StartTime DATETIME = GETUTCDATE();

	SET @OutputWorkflowScheme = @WorkflowScheme;

	DECLARE @Lang VARCHAR(2) = dbo.__GetLanguage(@WorkflowScheme);
	DECLARE @CurrentBranchCode VARCHAR(5) = dbo.__GetCurrentBranchCode(@WorkflowScheme);
	DECLARE @WorkingDate DATETIME = dbo.__GetValueDate(@WorkflowScheme);
	DECLARE @PathData VARCHAR(1000) = '$.Request.RequestBody.Data';
	DECLARE @jsonResponse NVARCHAR(max) = JSON_QUERY(@WorkflowScheme, @PathData);
	-- KHAI BAO DANH SACH FIELD CHO GIAO DICH
	DECLARE @ApplicationCode NVARCHAR(50) = JSON_VALUE(@WorkflowScheme, @PathData + '.applicationcode');
	DECLARE @Accountnumber NVARCHAR(50) = JSON_VALUE(@WorkflowScheme, @PathData + '.accountnumber');
	DECLARE @Bankcode NVARCHAR(50) = JSON_VALUE(@WorkflowScheme, @PathData + '.bankcode');
	
	DECLARE @ErrorMessage NVARCHAR(3000) = '';
	DECLARE @ContractNumber NVARCHAR(100);

	-- Check data input REQUIRED
	IF @ApplicationCode IS NULL
	BEGIN
		SET @ErrorMessage = dbo.__GetError('Common.Value.Required', @Lang, 'Application Code');

		THROW 50001
			,@ErrorMessage
			,1;
	END

	IF @ApplicationCode !='DIGITAL'
	BEGIN
		SET @ErrorMessage = dbo.__GetError('Common.Value.Required', @Lang, 'Application Code');

		THROW 50001
			,@ErrorMessage
			,1;
	END

	IF (ISNULL(@Accountnumber,'') ='' OR @Accountnumber='')
	BEGIN
		SET @ErrorMessage = dbo.__GetError('Common.Value.Required', @Lang, 'accountnumber');

		THROW 50001
			,@ErrorMessage
			,1;
	END

	
	IF (ISNULL(@Bankcode,'') ='' OR @Bankcode='')
	BEGIN
		SET @ErrorMessage = dbo.__GetError('Common.Value.Required', @Lang, 'bankcode');

		THROW 50001
			,@ErrorMessage
			,1;
	END


	DECLARE @DataJsonResponseBank NVARCHAR(MAX);

	-- Gán kết quả JSON vào biến @DataJsonResponse
	

		IF NOT EXISTS(
		SELECT 1 FROM dbo.D_BANK WHERE IsSender=1 AND  Bin=@Bankcode)
		BEGIN
			BEGIN
			SET @ErrorMessage = dbo.__GetError('Digital.NotFound.BankCodeInternal', @Lang, @Bankcode);

			THROW 50001
				,@ErrorMessage
				,1;
			 END
		END
		ELSE
		BEGIN 
		    SELECT @DataJsonResponseBank = (SELECT b.AccountNumber as accountnumber, b.CurrencyCode as currencycode, c.FullName as  receivername
			FROM dbo.D_CONTRACT a inner join dbo.D_CONTRACTACCOUNT b ON a.ContractNumber=b.ContractNumber  and b.AccountNumber= @Accountnumber INNER JOIN dbo.D_DIGITALBANKINGUSER c ON b.ContractNumber=c.ContractNumber
			FOR JSON PATH,WITHOUT_ARRAY_WRAPPER, INCLUDE_NULL_VALUES
			)
		END
    
		IF ISNULL(@DataJsonResponseBank, '') = '' OR @DataJsonResponseBank = ''
		BEGIN
			SET @ErrorMessage = dbo.__GetError('Common.Value.NotExist', @Lang, @Accountnumber);

			THROW 50001
				,@ErrorMessage
				,1;
	
		END
		

	-- Set giá trị trả về 
	SET @jsonResponse = JSON_MODIFY(@jsonResponse, '$.accountinfor',  JSON_QUERY(@DataJsonResponseBank));

	SET @OutputWorkflowScheme = JSON_MODIFY(@OutputWorkflowScheme, '$.Response.Data', JSON_QUERY(@jsonResponse));

	EXEC dbo.__SetDataBaseTransaction @WorkflowScheme = @WorkflowScheme
		,@OutputWorkflowScheme = @OutputWorkflowScheme OUTPUT

	-- Thêm thông tin transaction
	DECLARE @Duration INT = DATEDIFF(MILLISECOND, @StartTime, GETUTCDATE());

	SET @OutputWorkflowScheme = JSON_MODIFY(@OutputWorkflowScheme, '$.Request.RequestHeader.TxContext.start_time', CONVERT(NVARCHAR, @StartTime, 127));
	SET @OutputWorkflowScheme = JSON_MODIFY(@OutputWorkflowScheme, '$.Request.RequestHeader.TxContext.duration', @Duration);

	EXEC dbo.__CreateTransaction @WorkflowScheme = @OutputWorkflowScheme;

	COMMIT TRANSACTION
END TRY

BEGIN CATCH
	ROLLBACK;

	-- Ghi thông tin vào log
	DECLARE @Error NVARCHAR(MAX) = ERROR_MESSAGE();
	DECLARE @FullError NVARCHAR(MAX) = 'ErrorMessage: ' + ISNULL(ERROR_MESSAGE(), '') + ' - ErrorStore: ' + ISNULL(ERROR_PROCEDURE(), '') + ' - ErrorLine: ' + CAST(ISNULL(ERROR_LINE(), - 1) AS VARCHAR(10));

	-- Cập nhật dữ liệu error bảng Transaction
	EXEC dbo.__CreateTransaction @WorkflowScheme = @OutputWorkflowScheme
		,@IsError = 1
		,@ErrorMessage = @FullError;

	EXEC dbo.__Log @WorkflowScheme = @WorkflowScheme
		,@ShortMessage = @Error
		,@FullMessage = @FullError;

	throw;
END CATCH;

GO
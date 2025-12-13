USE O24CMS
GO
IF OBJECT_ID('dbo.D_TEMPLATETRANSFER_INFO', 'P ') IS NOT NULL DROP PROCEDURE dbo.D_TEMPLATETRANSFER_INFO;
GO

CREATE PROCEDURE [dbo].[D_TEMPLATETRANSFER_INFO] @WorkflowScheme NVARCHAR(max)
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
	DECLARE @ApplicationCode VARCHAR(500) = JSON_VALUE(@WorkflowScheme, @PathData + '.applicationcode');
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

	DECLARE @DataJsonResponseBank NVARCHAR(MAX);

	-- Gán kết quả JSON vào biến @DataJsonResponse
	SELECT @DataJsonResponseBank = (
			SELECT    
			   a.Id AS id,   
			   a.ApplicationCode AS applicationcode, 
			    a.TemplateCode AS templatecode,  
			    a.TemplateName AS templatename,  
			    a.Description AS description,
			    a.SenderAccount AS senderaccount,
			    a.ReceiverAccount AS receiveraccount,  
			    a.Amount AS amount,  
			    a.CurrencyCode AS currencycode,  
			    a.TransactionCode AS transactioncode,
			    a.UserCode AS usercode,
			    a.ExecuteNow AS executenow,  
			    a.ExecuteDate AS executedate,  
			    a.ChargeFee AS chargefee,  
			    a.CityCode AS citycode,
			    a.CountryCode AS countrycode,
			    a.IdentifyNo AS identifyno,  
			    a.IssuePlace AS issueplace,  
			    a.ReceiverName AS receivername,
			    a.BankCode AS bankcode,
			    a.SenderName AS sendername,  
			    a.ReceiverAdd AS receiveradd,  
			    a.Branch AS branch,  
			    a.BranchDesc AS branchdesc,
			    a.ReceiverID AS receiverid
				,bankinfor.Logo as logo
				,bankinfor.LookupSupported as lookupsupported
				,bankinfor.Name as name
				,bankinfor.ShortName as shortname
				,bankinfor.Bin AS bankcode 
			FROM dbo.D_TEMPLATETRANSFER a
			INNER JOIN dbo.D_BANK bankinfor ON a.BankCode = bankinfor.Bin
			FOR JSON AUTO
				,INCLUDE_NULL_VALUES
			);

	-- Set giá trị trả về 
	IF ISNULL(@DataJsonResponseBank, '') <> ''
		AND @DataJsonResponseBank != ''
	BEGIN
		SET @JsonResponse = JSON_MODIFY(@JsonResponse, '$.listtemplate', JSON_QUERY(@DataJsonResponseBank));
	END
	ELSE
	BEGIN
		SET @JsonResponse = JSON_MODIFY(@JsonResponse, '$.listtemplate', '[]');
	END

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
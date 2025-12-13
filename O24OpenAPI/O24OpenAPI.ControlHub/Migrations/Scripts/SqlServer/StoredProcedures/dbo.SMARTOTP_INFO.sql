USE o24cth
GO
IF OBJECT_ID('dbo.SMARTOTP_INFO', 'P ') IS NOT NULL DROP PROCEDURE dbo.SMARTOTP_INFO;
GO
CREATE PROCEDURE [dbo].[SMARTOTP_INFO]
    @WorkflowScheme nvarchar(max),
    @IsReverse nvarchar(1), -- R: Reverse |N: Normal
    @OutputWorkflowScheme nvarchar(max) OUTPUT
AS
BEGIN TRY
    BEGIN TRANSACTION
    -- Thêm thời gian bắt đầu thực hiện transaction
    DECLARE @StartTime DATETIME = GETUTCDATE();

    SET @OutputWorkflowScheme = @WorkflowScheme;

    DECLARE @Lang VARCHAR(2) = dbo.__GetLanguage(@WorkflowScheme);
    DECLARE @CurrentBranchCode VARCHAR(5) = dbo.__GetCurrentBranchCode(@WorkflowScheme);
    DECLARE @WorkingDate DATETIME = dbo.__GetValueDate(@WorkflowScheme);

    DECLARE @PathData varchar(1000) = '$.Request.RequestBody.Data';

    DECLARE @jsonResponse nvarchar(max) = JSON_QUERY(@WorkflowScheme, @PathData);
		
    -- KHAI BAO DANH SACH FIELD CHO GIAO DICH

	
    DECLARE @UserCode 							VARCHAR(50) = JSON_VALUE(@WorkflowScheme, @PathData + '.info.usercode'); 
    DECLARE @deviceid 				VARCHAR(500) = JSON_VALUE(@WorkflowScheme, @PathData + '.deviceid');
		
    DECLARE @ErrorMessage			NVARCHAR(3000) = '';
		DECLARE @IsActive						BIT = 0; 


    -- Check data input REQUIRED
    IF @usercode IS NULL
    BEGIN
        SET @ErrorMessage = dbo.__GetError('Common.Value.Required', @Lang, 'User Code');
        THROW 50001, @ErrorMessage, 1;
    END

    IF @deviceid IS NULL
    BEGIN
        SET @ErrorMessage = dbo.__GetError('Common.Value.Required', @Lang, 'Device Id');
        THROW 50001, @ErrorMessage, 1;
    END   

    -- Process Check User Login
		
		IF NOT EXISTS (
					SELECT 1 
					FROM [dbo].[D_DIGITALBANKINGUSER]
					WHERE UserCode = @usercode
				)
		BEGIN
					SET @ErrorMessage = dbo.__GetError('Common.Value.UserCodeIsNotExist', @Lang, @usercode);
					THROW 50001, @ErrorMessage, 1;
		END
		

	
	  SELECT TOP 1 
    @IsActive = CASE 
                    WHEN status = 'A' THEN 1 
                    ELSE 0 
                END
		FROM 
			 [dbo].[SmartOTPInfo]
		WHERE 
			UserCode = @usercode and
			DeviceId = @deviceid and 
      Status = 'A'

		 
		 
    -- Them du lieu tra ve
    SET @jsonResponse = JSON_MODIFY(@jsonResponse, '$.usercode', @usercode);
	  SET @jsonResponse = JSON_MODIFY(@jsonResponse, '$.isactive', IIF(@IsActive = 1, 'true', 'false'));


		SET @OutputWorkflowScheme = JSON_MODIFY(@OutputWorkflowScheme, '$.Response.Data', JSON_QUERY(@jsonResponse));

    EXEC dbo.__SetDataBaseTransaction @WorkflowScheme = @WorkflowScheme,
                                      @OutputWorkflowScheme = @OutputWorkflowScheme OUTPUT

    -- Thêm thông tin transaction
    DECLARE @Duration INT = DATEDIFF(MILLISECOND, @StartTime, GETUTCDATE());
    SET @OutputWorkflowScheme
        = JSON_MODIFY(
                         @OutputWorkflowScheme,
                         '$.Request.RequestHeader.TxContext.start_time',
                         CONVERT(NVARCHAR, @StartTime, 127)
                     );
    SET @OutputWorkflowScheme
        = JSON_MODIFY(
                         @OutputWorkflowScheme,
                         '$.Request.RequestHeader.TxContext.duration',
                         @Duration
                     );
		
    EXEC dbo.__CreateTransaction @WorkflowScheme = @OutputWorkflowScheme;
		

    COMMIT TRANSACTION
END TRY
BEGIN CATCH
    ROLLBACK;
    -- Ghi thông tin vào log
    DECLARE @Error NVARCHAR(MAX) = ERROR_MESSAGE();
    DECLARE @FullError NVARCHAR(MAX)
        = 'ErrorMessage: ' + ISNULL(ERROR_MESSAGE(), '') + ' - ErrorStore: ' + ISNULL(ERROR_PROCEDURE(), '')
          + ' - ErrorLine: ' + CAST(ISNULL(ERROR_LINE(), -1) AS varchar(10));

    -- Cập nhật dữ liệu error bảng Transaction
    EXEC dbo.__CreateTransaction @WorkflowScheme = @OutputWorkflowScheme,
                                 @IsError = 1,
                                 @ErrorMessage = @FullError;

    EXEC dbo.__Log @WorkflowScheme = @WorkflowScheme,
                   @ShortMessage = @Error,
                   @FullMessage = @FullError;

    throw;
END CATCH;
GO
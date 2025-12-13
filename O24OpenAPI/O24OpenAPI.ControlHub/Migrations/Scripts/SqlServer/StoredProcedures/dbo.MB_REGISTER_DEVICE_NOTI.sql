USE o24cth
GO
IF OBJECT_ID('dbo.MB_REGISTER_DEVICE_NOTI', 'P ') IS NOT NULL DROP PROCEDURE dbo.MB_REGISTER_DEVICE_NOTI;
GO
CREATE PROCEDURE [dbo].[MB_REGISTER_DEVICE_NOTI]
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
    DECLARE @AppType varchar(6) = JSON_VALUE(@WorkflowScheme, @PathData + '.AppType'); -- Gia tri nay dung se out ra ngoai
    DECLARE @UserCode varchar(100) = JSON_VALUE(@WorkflowScheme, @PathData + '.UserCode');
		DECLARE @DeviceId varchar(100) = JSON_VALUE(@WorkflowScheme, @PathData + '.DeviceId');
		DECLARE @DeviceType varchar(20) 			 = JSON_VALUE(@WorkflowScheme, @PathData + '.DeviceType');
		DECLARE @PushId varchar(100) 			 = JSON_VALUE(@WorkflowScheme, @PathData + '.PushID');
		DECLARE @Body nvarchar(Max) 			 = JSON_QUERY(@WorkflowScheme, @PathData );
    DECLARE @ErrorMessage			NVARCHAR(3000) = '';
		

    -- Check data input REQUIRED
    IF @UserCode IS NULL
    BEGIN
        SET @ErrorMessage = dbo.__GetError('Common.Value.Required', @Lang, 'UserCode');
        THROW 50001, @ErrorMessage, 1;
    END

    IF @DeviceId IS NULL
    BEGIN
        SET @ErrorMessage = dbo.__GetError('Common.Value.Required', @Lang, 'DeviceId');
        THROW 50001, @ErrorMessage, 1;
    END   
		
		IF @PushId IS NULL
    BEGIN
        SET @ErrorMessage = dbo.__GetError('Common.Value.Required', @Lang, 'PushId');
        THROW 50001, @ErrorMessage, 1;
    END  
		
		IF @AppType IS NULL
    BEGIN
        SET @ErrorMessage = dbo.__GetError('Common.Value.Required', @Lang, 'AppType');
        THROW 50001, @ErrorMessage, 1;
    END 

		
		UPDATE dbo.[D_Device]
			SET Status = 'D'
		WHERE UserCode = @UserCode AND AppType = 'OTP';
		

		EXEC dbo.__InsertEntity @WorkflowScheme = @WorkflowScheme,
                            @EntityJson = @Body,
														@Entity = 'D_Device';

	
		
    -- Them du lieu tra ve
    SET @jsonResponse = JSON_MODIFY(@jsonResponse, '$.success', 'true');
	
		
		-- CAP NHAT THONG TIN DANG NHAP THANH CONG
		
	

		SET @OutputWorkflowScheme = JSON_MODIFY(@OutputWorkflowScheme, '$.Response.Data', JSON_QUERY(@jsonResponse));

    EXEC dbo.__SetDataBaseTransaction @WorkflowScheme = @WorkflowScheme,
                                      @OutputWorkflowScheme = @OutputWorkflowScheme OUTPUT;

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
USE O24CMS
GO
IF OBJECT_ID('dbo.BO_CREATE_CONTRACT', 'P ') IS NOT NULL DROP PROCEDURE dbo.BO_CREATE_CONTRACT;
GO
CREATE PROCEDURE [dbo].[BO_CREATE_CONTRACT]
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

    DECLARE @ApplicationCode 					VARCHAR(500) = JSON_VALUE(@WorkflowScheme, @PathData + '.applicationcode');

    DECLARE @ErrorMessage							NVARCHAR(3000) = '';
		DECLARE @ContractNumber 					NVARCHAR(100)	= JSON_VALUE(@WorkflowScheme, @PathData + '.contractnumber');
		DECLARE @description 							NVARCHAR(500)	= JSON_VALUE(@WorkflowScheme, @PathData + '.description');
		DECLARE @TransactionID						NVARCHAR(50)	= ''
		DECLARE @CustomerCode							NVARCHAR(50);
		
    -- Check data input REQUIRED
    IF @ApplicationCode IS NULL
    BEGIN
        SET @ErrorMessage = dbo.__GetError('Common.Value.Required', @Lang, 'Application Code');
        THROW 50001, @ErrorMessage, 1;
    END   
		
		
		WHILE 1 = 1
		BEGIN
				-- Tạo chuỗi ngẫu nhiên 16 ký tự
				SET @TransactionID = CONVERT(NVARCHAR(50), NEWID()) + CONVERT(NVARCHAR(50), NEWID());
				-- Cắt chuỗi thành 16 ký tự
				SET @TransactionID = LEFT(@TransactionID, 16);
				
				-- Kiểm tra xem TransactionID có tồn tại trong bảng D_CONTRACT không
				IF NOT EXISTS (SELECT 1 FROM [dbo].[D_CONTRACT] WHERE [TransactionID] = @TransactionID)
				BEGIN
						-- Nếu không tồn tại, thoát vòng lặp và sử dụng giá trị này
						BREAK;
				END
		END
		
		SET @WorkflowScheme = JSON_MODIFY(@WorkflowScheme,'$.Request.RequestHeader.TxContext.reference_id', @TransactionID)
		SET @WorkflowScheme = JSON_MODIFY(@WorkflowScheme,'$.Request.RequestHeader.TxContext.ref_id', @TransactionID)
		SET @WorkflowScheme = JSON_MODIFY(@WorkflowScheme,'$.Request.RequestHeader.TxContext.description', @description)
		SET @WorkflowScheme = JSON_MODIFY(@WorkflowScheme,'$.Request.RequestHeader.TxContext.transaction_code', 'BO_CREATE_CONTRACT')


    -- Process Check User Is Exist
		IF EXISTS (
					SELECT 1 
					FROM [dbo].[D_CONTRACT]
					WHERE ContractNumber = @ContractNumber
				)
		BEGIN
					SET @ErrorMessage = dbo.__GetError('Common.Value.ContractNumberIsExist', @Lang, @ContractNumber);
					THROW 50001, @ErrorMessage, 1;
		END
		
		BEGIN
			EXEC dbo.__CreateCustomer	@WorkflowScheme = @WorkflowScheme, 
																@IsReverse = @IsReverse, 
																@OutputWorkflowScheme = @OutputWorkflowScheme OUTPUT,
																@CustomerCode = @CustomerCode OUTPUT

		END
			
		BEGIN
			EXEC dbo.__CreateContract	@WorkflowScheme = @WorkflowScheme, 
																@IsReverse = @IsReverse, 
																@CustomerCode	= @CustomerCode,
																@OutputWorkflowScheme = @OutputWorkflowScheme
		END
		
	
		BEGIN
			EXEC dbo.__CreateContractAccount	@WorkflowScheme = @WorkflowScheme, 
																				@IsReverse = @IsReverse, 
																				@OutputWorkflowScheme = @OutputWorkflowScheme
		END
		


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
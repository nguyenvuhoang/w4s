USE o24cth
GO
IF OBJECT_ID('dbo.GENERATE_CONTRACT_NUMBER', 'P ') IS NOT NULL DROP PROCEDURE dbo.GENERATE_CONTRACT_NUMBER;
GO
CREATE 
PROCEDURE [dbo].[GENERATE_CONTRACT_NUMBER]
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
		
    DECLARE @ErrorMessage			NVARCHAR(3000) = '';
		DECLARE @IsActive						BIT = 0; 
		DECLARE @CC INT = 0;
    DECLARE 
        @ContractNo VARCHAR(50),
        @ProductID VARCHAR(50),
        @ContractLevel INT,
        @KYCID INT,
        @BRANCHID VARCHAR(50);
    -- Fetch default values from the system variables
    SET @ProductID = (SELECT [value] FROM ParaServer WHERE Code = 'PRODUCTDEFAULT');
    SET @ContractLevel = (SELECT [value] FROM ParaServer WHERE Code = 'CONTRACTLEVELDEFAULT');
    SET @KYCID = (SELECT [value] FROM ParaServer WHERE Code = 'KYCID');
    SET @BRANCHID = (SELECT [value]  FROM ParaServer WHERE Code = 'BRANCHHO');

		
    -- Check data input REQUIRED
			
		SET @ContractNo = SUBSTRING(REPLACE(NEWID(), '-', ''), 1, 16);

        IF EXISTS (SELECT 1 FROM D_CONTRACT WHERE ContractNumber = @ContractNo)
        BEGIN
            DECLARE @Number INT = 1;

            WHILE (@Number <= 10 AND EXISTS (SELECT 1 FROM D_CONTRACT WHERE ContractNumber = @ContractNo))
            BEGIN
                SET @ContractNo = SUBSTRING(REPLACE(NEWID(), '-', ''), 1, 16);
                SET @Number = @Number + 1;
                PRINT @Number;
            END

            IF EXISTS (SELECT 1 FROM D_CONTRACT WHERE ContractNumber = @ContractNo)
            BEGIN
								SET @ErrorMessage = dbo.__GetError('PORTAL.GENERATE_CONTRACT_FAILED', @Lang, '');
								THROW 50001, @ErrorMessage, 1;
            END
        END
				
		 
    -- Them du lieu tra ve
    SET @jsonResponse = JSON_MODIFY(@jsonResponse, '$.contractnumber', @ContractNo);

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
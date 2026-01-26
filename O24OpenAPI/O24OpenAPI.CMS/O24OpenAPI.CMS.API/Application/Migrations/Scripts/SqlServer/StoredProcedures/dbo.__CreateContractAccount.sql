USE O24CMS
GO
IF OBJECT_ID('dbo.__CreateContractAccount', 'P ') IS NOT NULL DROP PROCEDURE dbo.__CreateContractAccount;
GO
CREATE PROCEDURE [dbo].[__CreateContractAccount]
(
    @WorkflowScheme NVARCHAR(MAX),
    @IsReverse NVARCHAR(2),
		@OutputWorkflowScheme NVARCHAR(max) OUTPUT
)
AS
BEGIN TRY
    BEGIN TRANSACTION


    -- Thêm thời gian bắt đầu thực hiện transaction
    DECLARE @StartTime DATETIME = GETUTCDATE();
		
    -- Gán giá trị trả về
    SET @OutputWorkflowScheme = @WorkflowScheme;

    DECLARE @PathData VARCHAR(1000) = '$.Request.RequestBody.Data';
    DECLARE @jsonResponse nvarchar(max) = JSON_QUERY(@WorkflowScheme, @PathData);

    DECLARE @Description VARCHAR(2000) = dbo.__GetDescription(@WorkflowScheme);
    DECLARE @ErrorMessage NVARCHAR(3000) = '';
		

    --- KHOI TAO GIA TRI MAC DINH CHO D_CONTRACT
		DECLARE @Contractnumber VARCHAR(50) = JSON_VALUE(@WorkflowScheme, @PathData + '.contractnumber');
		DECLARE @DepositAccount	NVARCHAR(MAX) = JSON_QUERY(@WorkflowScheme, @PathData + '.depositaccount');
		
		BEGIN
        THROW 50001, @DepositAccount, 1;
    END   
	
    --- D_CONTRACT ACCOUNT-----
    BEGIN
        INSERT INTO [dbo].[D_CONTRACTACCOUNT] 
					([ContractNumber], [AccountNumber], [AccountType], [CurrencyCode], [Status], [BranchID], [BankAccountType], [BankId], [IsPrimary])
				SELECT 
						@Contractnumber,
						JSON_VALUE(jsonData.value, '$.accountnumber'),
						JSON_VALUE(jsonData.value, '$.accounttype'),
						JSON_VALUE(jsonData.value, '$.currencycode'),
						JSON_VALUE(jsonData.value, '$.status'),
						JSON_VALUE(jsonData.value, '$.branchid'),
						JSON_VALUE(jsonData.value, '$.bankaccounttype'),
						JSON_VALUE(jsonData.value, '$.bankid'),
						CAST(JSON_VALUE(jsonData.value, '$.isprimary') AS BIT)
				FROM OPENJSON(@DepositAccount) 
				WITH (
						value NVARCHAR(MAX) '$' 
				) AS jsonData;


    END
		
    COMMIT TRANSACTION
END TRY
BEGIN CATCH
    ROLLBACK;
    -- Ghi thông tin vào log
    DECLARE @Error NVARCHAR(MAX) = ERROR_MESSAGE();

    DECLARE @FullError NVARCHAR(MAX)
        = 'ErrorMessage: ' + ISNULL(ERROR_MESSAGE(), '') + ' - ErrorStore: ' + ISNULL(ERROR_PROCEDURE(), '')
          + ' - ErrorLine: ' + CAST(ISNULL(ERROR_LINE(), -1) AS varchar(10));

    EXEC dbo.__Log @WorkflowScheme = @WorkflowScheme,
                   @ShortMessage = @Error,
                   @FullMessage = @FullError;

    throw;
END CATCH;
GO
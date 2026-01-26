USE O24CMS
GO
IF OBJECT_ID('dbo.__UpdateHistory', 'P ') IS NOT NULL DROP PROCEDURE dbo.__UpdateHistory;
GO
CREATE 
PROCEDURE [dbo].[__UpdateHistory] (
    @WorkflowScheme NVARCHAR(MAX), 
    @Contractnumber NVARCHAR(50), 
    @Amount DECIMAL(24, 5), 
    @DebitOrCredit NVARCHAR(3)
)
AS
BEGIN
    -- Check base info
    EXEC dbo.__ValidateBaseInfo @WorkflowScheme = @WorkflowScheme
    
    -- Lấy thông tin giao dịch
    DECLARE @ValueDate DATE = dbo.__GetValueDate(@WorkflowScheme);
    DECLARE @RefId VARCHAR(50) = dbo.__GetRefId(@WorkflowScheme);
    DECLARE @TransactionDate DATE = dbo.__GetTransactionDate(@WorkflowScheme);
    DECLARE @TransactionCode VARCHAR(50) = dbo.__GetTransactionCode(@WorkflowScheme);
    DECLARE @Description NVARCHAR(2000) = dbo.__GetDescription(@WorkflowScheme);
    DECLARE @CurrentUserCode NVARCHAR(2000) = dbo.__GetCurrentUserCode(@WorkflowScheme);
    DECLARE @ChannelId NVARCHAR(2000) = dbo.__GetChannelId(@WorkflowScheme);
    DECLARE @CreatedOnUtc DATETIME = GETUTCDATE();
    DECLARE @UpdatedOnUtc DATETIME = GETUTCDATE();
		DECLARE @TransactionNumber varchar(50) = dbo.__GetTransactionNumber(@WorkflowScheme);

    -- Chèn dữ liệu vào bảng DepositHistory từ biến JSON

		INSERT INTO [dbo].[D_CONTRACT_HISTORY]
           ([ContractNumber]
           ,[TransactionDate]
           ,[TransactionRefId]
           ,[ValueDate]
           ,[TransactionCode]
           ,[Amount]
           ,[Dorc]
           ,[Description]
           ,[UserCreated]
           ,[Status]
           ,[TransactionNumber]
           ,[CreatedOnUtc]
           ,[UpdatedOnUtc]
           ,[UserApprove]
           ,[ChannelId])
     VALUES
           (@Contractnumber
           ,@TransactionDate
           ,@RefId
           ,@ValueDate
           ,@TransactionCode
           ,@Amount
           ,@DebitOrCredit
           ,@Description
           ,@CurrentUserCode
           ,'N'
           ,@TransactionNumber
           ,@CreatedOnUtc
           ,@UpdatedOnUtc
           ,NULL
           ,@ChannelId)

END
GO
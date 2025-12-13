USE o24cth
GO
IF OBJECT_ID('dbo.__CreateContract', 'P ') IS NOT NULL DROP PROCEDURE dbo.__CreateContract;
GO
CREATE 
PROCEDURE [dbo].[__CreateContract]
(
    @WorkflowScheme NVARCHAR(MAX),
    @IsReverse NVARCHAR(2),
		@CustomerCode		NVARCHAR(max),
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
		DECLARE @Contractnumber VARCHAR(50) = JSON_VALUE(@WorkflowScheme, @PathData + '.contractnumber');

    --- KHOI TAO GIA TRI MAC DINH CHO D_CONTRACT
    DECLARE @ContractCode		 	NVARCHAR(100) = JSON_VALUE(@WorkflowScheme, @PathData + '.contractnumber');
    DECLARE @ContractType 		NVARCHAR(3) 	= 'IND';
    DECLARE @UserType 				NVARCHAR(10)	= JSON_VALUE(@WorkflowScheme, @PathData + '.usertypesub');
    DECLARE @ProductID 				NVARCHAR(10)	= 'IND';
    DECLARE @BranchID 				NVARCHAR(10)	= JSON_VALUE(@WorkflowScheme, @PathData + '.branch');
    DECLARE @CreateDate			 	DATETIME			= JSON_VALUE(@WorkflowScheme, @PathData + '.startdate'); 
    DECLARE @EndDate					DATETIME			= JSON_VALUE(@WorkflowScheme, @PathData + '.expiredate');
    DECLARE @LastModify 			DATETIME			= JSON_VALUE(@WorkflowScheme, @PathData + '.startdate'); 
    DECLARE @UserCreate 			NVARCHAR(10)	=	JSON_VALUE(@WorkflowScheme, @PathData + '.info.usercode'); 
    DECLARE @UserLastModify 	DATETIME			= NULL;			
    DECLARE @UserApprove 			NVARCHAR(10)	= 'BEN';
    DECLARE @ApproveDate 			DATETIME			= NULL;
    DECLARE @Status 					NVARCHAR(5)		= 'A';
    DECLARE @IsSpecialMan 		NVARCHAR(1)		= NULL;
    DECLARE @IsReceiverList 	NVARCHAR(50)	= 'N';
    DECLARE @IsAutoRenew 			NVARCHAR(1)		= 'Y';
    DECLARE @ContractLevelId 	NUMERIC(5)		= '1'		
		DECLARE @Mer_Code					NVARCHAR(10)	= NULL;
		DECLARE @ShopName					NVARCHAR(250)	= NULL;
		DECLARE @LocalShopName		NVARCHAR(250)	= NULL;
		DECLARE @ParentContract		NVARCHAR(50)	= NULL;
		DECLARE @ControlType			NVARCHAR(50)	= 'MATRIX';
		DECLARE @TransactionID		NVARCHAR(50)	= JSON_VALUE(@WorkflowScheme, '$.Request.RequestHeader.TxContext.reference_id'); 
		
		
    --- D_CONTRACT ACCOUNT-----
    BEGIN
        INSERT INTO [dbo].[D_CONTRACT] 
				(
						[ContractNumber], 
						[ContractCode], 
						[CustomerCode], 
						[ContractType], 
						[UserType], 
						[ProductID], 
						[BranchID], 
						[CreateDate], 
						[EndDate], 
						[LastModify], 
						[UserCreate], 
						[UserLastModify], 
						[UserApprove], 
						[ApproveDate], 
						[Status], 
						[IsSpecialMan], 
						[IsReceiverList], 
						[IsAutoRenew], 
						[Description], 
						[ContractLevelId], 
						[Mer_Code], 
						[ShopName], 
						[LocalShopName], 
						[ParentContract], 
						[ControlType], 
						[TransactionID]
				) 
				VALUES 
				(
						@ContractNumber, 
						@ContractCode, 
						@CustomerCode, 
						@ContractType, 
						@UserType, 
						@ProductID, 
						@BranchID, 
						@CreateDate, 
						@EndDate, 
						@LastModify, 
						@UserCreate, 
						@UserLastModify, 
						@UserApprove, 
						@ApproveDate, 
						@Status, 
						@IsSpecialMan, 
						@IsReceiverList, 
						@IsAutoRenew, 
						@Description, 
						@ContractLevelId, 
						@Mer_Code, 
						@ShopName, 
						@LocalShopName, 
						@ParentContract, 
						@ControlType, 
						@TransactionID
				)


    END

    DECLARE @ContractId INT = 0;
    SELECT @ContractId = Id
    FROM dbo.[D_CONTRACT]
    WHERE ContractNumber = @Contractnumber;

    BEGIN
        EXEC dbo.__UpTransactionDetailTypeInsert @WorkflowScheme = @WorkflowScheme,
                                                 @IsReverse = @IsReverse,
                                                 @TableName = 'D_CONTRACT',
                                                 @IdEntity = @ContractId;
    END


    -- 3. Update CreditHistory
		BEGIN
			EXEC dbo.__UpdateHistory @WorkflowScheme = @WorkflowScheme,
															 @Contractnumber = @Contractnumber,
															 @Amount = 0,
															 @DebitOrCredit = 'C'
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
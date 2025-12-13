USE o24cth
GO
IF OBJECT_ID('dbo.__CreateTransaction', 'P ') IS NOT NULL DROP PROCEDURE dbo.__CreateTransaction;
GO
CREATE PROCEDURE [dbo].[__CreateTransaction]
    @WorkflowScheme NVARCHAR(MAX),
    @IsError BIT = 0,
    @ErrorMessage NVARCHAR(MAX) = NULL
AS
BEGIN
    
    DECLARE @TransactionCode VARCHAR(50) = dbo.__GetTransactionCode(@WorkflowScheme);
    DECLARE @SubCode VARCHAR(50) = dbo.__GetSubCode(@WorkflowScheme);
    DECLARE @TransactionDate DATETIME = dbo.__GetTransactionDate(@WorkflowScheme);
    DECLARE @ValueDate DATETIME = dbo.__GetValueDate(@WorkflowScheme);
    DECLARE @ReferenceId VARCHAR(50) = dbo.__GetReferenceId(@WorkflowScheme);
    DECLARE @RefId VARCHAR(50) = dbo.__GetRefId(@WorkflowScheme);
    DECLARE @TransactionNumber VARCHAR(50) = dbo.__GetTransactionNumber(@WorkflowScheme);
    DECLARE @RequestBody NVARCHAR(MAX) = JSON_QUERY(@WorkflowScheme, '$.Request.RequestBody');
    DECLARE @ResponseBody NVARCHAR(MAX) = JSON_QUERY(@WorkflowScheme, '$.Response.Data');
    DECLARE @Description NVARCHAR(2000) = dbo.__GetDescription(@WorkflowScheme);
    DECLARE @StartTime BIGINT = DATEDIFF_BIG(ms, DATEFROMPARTS(1970, 1, 1), JSON_VALUE(@WorkflowScheme, '$.Request.RequestHeader.TxContext.start_time'));
    DECLARE @Duration INT = JSON_VALUE(@WorkflowScheme, '$.Request.RequestHeader.TxContext.duration');
    DECLARE @ChannelId VARCHAR(2000) = dbo.__GetChannelId(@WorkflowScheme);
    DECLARE @UserCode VARCHAR(2000) = dbo.__GetCurrentUserCode(@WorkflowScheme);
    DECLARE @UserName VARCHAR(2000) = dbo.__GetCurrentUserName(@WorkflowScheme);
    DECLARE @LoginName VARCHAR(2000) = dbo.__GetCurrentLoginName(@WorkflowScheme);
    DECLARE @BranchCode VARCHAR(2000) =  dbo.__GetCurrentBranchCode(@WorkflowScheme);

    -- Insert vào bảng Transaction
    IF NOT EXISTS (SELECT 1 FROM dbo.[Transaction] WHERE RefId = @RefId)
        INSERT INTO dbo.[Transaction] (
            [TransactionCode],
            [SubCode],
            [TransactionDate],
            [ValueDate],
            [ReferenceId],
            [RefId],
            [TransactionNumber],
            [Status],
            [IsReverse],
            [Amount1],
            [RequestBody],
            [ResponseBody],
            [Description],
            [StartTime],
            [Duration],
            [ChannelId],
            [UserCode],
            [UserName],
            [LoginName],
            [BranchCode]
        )
        VALUES (
            @TransactionCode,
            @SubCode,
            @TransactionDate,
            @ValueDate,
            @ReferenceId,
            @RefId,
            @TransactionNumber,
            CASE WHEN @IsError = 1 THEN 'E' ELSE 'C' END,
            0,
            0,
            @RequestBody,
            @ResponseBody,
            CASE WHEN @IsError = 1 THEN @ErrorMessage ELSE @Description END,
            ISNULL(@StartTime,0),
            ISNULL(@Duration,0),
            @ChannelId,
            @UserCode,
            @UserName,
            @LoginName,
            @BranchCode
        );
    ELSE
        UPDATE
            dbo.[Transaction]
        SET
            Status = CASE
                WHEN @IsError = 1 THEN 'E'
                ELSE 'C'
            END,
            Description = CASE
                WHEN @IsError = 1 THEN @ErrorMessage
                ELSE @Description
            END
        WHERE
            RefId = @RefId
END;
GO
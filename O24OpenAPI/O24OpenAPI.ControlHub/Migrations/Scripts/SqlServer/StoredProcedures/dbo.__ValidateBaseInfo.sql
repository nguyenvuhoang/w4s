USE o24cth
GO
IF OBJECT_ID('dbo.__ValidateBaseInfo', 'P ') IS NOT NULL DROP PROCEDURE dbo.__ValidateBaseInfo;
GO
CREATE PROCEDURE dbo.__ValidateBaseInfo
    @WorkflowScheme nvarchar(max)
AS
BEGIN

    -- Trích xuất các giá trị cần kiểm tra từ chuỗi JSON
    DECLARE @ValueDate date = dbo.__GetValueDate(@WorkflowScheme);
    DECLARE @TransactionDate date = dbo.__GetTransactionDate(@WorkflowScheme);
    DECLARE @TransactionCode varchar(2000) = dbo.__GetTransactionCode(@WorkflowScheme);
    DECLARE @TransactionNumber varchar(50) = dbo.__GetTransactionNumber(@WorkflowScheme);
    DECLARE @RefId varchar(50) = dbo.__GetRefId(@WorkflowScheme);
    DECLARE @CurrentBranchCode varchar(2000) = dbo.__GetCurrentBranchCode(@WorkflowScheme);
    DECLARE @Description varchar(2000) = dbo.__GetDescription(@WorkflowScheme);
    DECLARE @Language varchar(2000) = dbo.__GetLanguage(@WorkflowScheme);
    DECLARE @ReferenceId varchar(2000) = dbo.__GetReferenceId(@WorkflowScheme);
    DECLARE @CurrentUserCode varchar(2000) = dbo.__GetCurrentUserCode(@WorkflowScheme);
    
    -- Kiểm tra giá trị NULL và nếu có, gán thông báo lỗi và ném ra ngoại lệ
    IF @ValueDate IS NULL OR @TransactionDate IS NULL OR @TransactionNumber IS NULL 
        OR @RefId IS NULL OR @CurrentBranchCode IS NULL OR @Description IS NULL 
        OR @Language IS NULL OR @ReferenceId IS NULL OR @CurrentUserCode IS NULL 
        OR @TransactionCode IS NULL
    BEGIN
        DECLARE @ErrorMessage nvarchar(3000);
        SET @ErrorMessage = dbo.__GetError('Common.DataIsNulll', @Language, 
                                         CASE
                                            WHEN @ValueDate IS NULL THEN 'ValueDate'
                                            WHEN @TransactionDate IS NULL THEN 'TransactionDate'
                                            WHEN @TransactionNumber IS NULL THEN 'TransactionNumber'
                                            WHEN @RefId IS NULL THEN 'RefId'
                                            WHEN @CurrentBranchCode IS NULL THEN 'CurrentBranchCode'
                                            WHEN @Description IS NULL THEN 'Description'
                                            WHEN @Language IS NULL THEN 'Language'
                                            WHEN @ReferenceId IS NULL THEN 'ReferenceId'
                                            WHEN @CurrentUserCode IS NULL THEN 'CurrentUserCode'
                                            WHEN @TransactionCode IS NULL THEN 'TransactionCode'
                                         END);
        THROW 50001, @ErrorMessage, 1;
    END
END
GO
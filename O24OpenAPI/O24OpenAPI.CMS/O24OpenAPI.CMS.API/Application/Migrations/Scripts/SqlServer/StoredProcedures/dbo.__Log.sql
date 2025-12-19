USE O24CMS
GO
IF OBJECT_ID('dbo.__Log', 'P ') IS NOT NULL DROP PROCEDURE dbo.__Log;
GO
CREATE PROCEDURE [dbo].[__Log] (
    @WorkflowScheme NVARCHAR(MAX),
    @ShortMessage NVARCHAR(MAX),
    @FullMessage NVARCHAR(MAX) = ''
)

AS
BEGIN TRY
    -- Bắt đầu transaction lưu log
    BEGIN TRANSACTION;

        -- Lấy thông tin giao dịch từ chuỗi JSON
    
        DECLARE @RefId varchar(50) = dbo.__GetRefId(@WorkflowScheme);
        DECLARE @TransactionCode varchar(2000) = dbo.__GetTransactionCode(@WorkflowScheme);
        
        INSERT
            INTO
            dbo.Log
                (
                ShortMessage,
                LogLevelId,
                FullMessage,
                PageUrl,
                ReferredUrl,
                CreatedOnUtc
            )
            VALUES(
                ISNULL(
                    @ShortMessage,
                    ''
                ),
                20,
                ISNULL(
                    @FullMessage,
                    ''
                ),
                ISNULL(
                    @TransactionCode,
                    ''
                ),
                ISNULL(
                    @RefId,
                    ''
                ),
                GETUTCDATE()
            );
    
    -- Kết thúc transaction lưu log
    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    DECLARE @FullError NVARCHAR(MAX) = 'ErrorMessage: ' + ISNULL(ERROR_MESSAGE(), '') + ' - ErrorStore: '+ ISNULL(ERROR_PROCEDURE(), '') +' - ErrorLine: ' + CAST(ISNULL(ERROR_LINE(), -1) AS varchar(10));
    THROW 50000, @FullError, 1;
END CATCH
GO
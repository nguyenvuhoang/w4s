USE O24CMS
GO
IF OBJECT_ID('dbo.__GET_TRANSACTION_DETAIL_BY_FIELD', 'FN') IS NOT NULL DROP FUNCTION dbo.__GET_TRANSACTION_DETAIL_BY_FIELD;
GO
CREATE 
FUNCTION [dbo].[__GET_TRANSACTION_DETAIL_BY_FIELD]
(
    @Entity 		NVARCHAR(250),
		@EntityId 	int,
		@FieldName  NVARCHAR(250)
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
    DECLARE @Value NVARCHAR(MAX);

    SELECT @Value = NewValue
		FROM dbo.D_TRANSACTIONDETAILS
		WHERE Entity = @Entity AND EntityId = @EntityId AND FieldName = @FieldName

    -- Nếu không tìm thấy Caption, trả về Code
    IF @Value IS NULL
        SET @Value = '';

    RETURN @Value;
END;
GO
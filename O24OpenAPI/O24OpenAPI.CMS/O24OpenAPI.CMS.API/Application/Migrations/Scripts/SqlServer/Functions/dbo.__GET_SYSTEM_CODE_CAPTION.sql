USE O24CMS
GO
IF OBJECT_ID('dbo.__GET_SYSTEM_CODE_CAPTION', 'FN') IS NOT NULL DROP FUNCTION dbo.__GET_SYSTEM_CODE_CAPTION;
GO
CREATE 
FUNCTION [dbo].[__GET_SYSTEM_CODE_CAPTION]
(
    @CodeId NVARCHAR(50),
		@CodeName NVARCHAR(50),
    @CodeGroup NVARCHAR(50)
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
    DECLARE @Caption NVARCHAR(MAX);

    SELECT @Caption = Caption
    FROM dbo.C_CODELIST WITH (NOLOCK)
    WHERE CodeGroup = @CodeGroup AND CodeName = @CodeName AND CodeId = @CodeId;

    -- Nếu không tìm thấy Caption, trả về Code
    IF @Caption IS NULL
        SET @Caption = @CodeId;

    RETURN @Caption;
END;
GO
USE o24cth
GO
IF OBJECT_ID('dbo.__GetError', 'FN') IS NOT NULL DROP FUNCTION dbo.__GetError;
GO
CREATE FUNCTION dbo.__GetError(
    @ErrorName varchar(100), -- Mã lỗi
    @Lang varchar(5),  -- Ngôn ngữ được sử dụng để trả về message | ex: en, vi
    @Params varchar(MAX) = NULL --  @Param = NULL -> Không thay thế param vào error message
)
RETURNS nvarchar(3000)
AS
BEGIN
	-- Biến lưu trữ giá trị lỗi cuối cùng
	DECLARE @Error 				NVARCHAR(MAX);
	DECLARE @Result 			NVARCHAR(MAX) = '{}';
	DECLARE @ResourceCode NVARCHAR(MAX);

	-- Lấy giá trị lỗi từ bảng LocaleStringResource dựa trên ResourceName và Language
	-- Sử dụng COALESCE để chọn giá trị đầu tiên không null từ ResourceValue và ErrorName
	SELECT TOP 1 @Error = COALESCE(NULLIF(r.ResourceValue, ''), @ErrorName), @ResourceCode = COALESCE(NULLIF(r.ResourceCode, 'SYS_00_01'), @ResourceCode)
	FROM dbo.LocaleStringResource r
	WHERE r.ResourceName = @ErrorName
		AND [Language] = ISNULL(@Lang, 'en');

	DECLARE @Param1 NVARCHAR(MAX), @Param2 NVARCHAR(MAX), @Param3 NVARCHAR(MAX), @Param4 NVARCHAR(MAX);

	-- Chia chuỗi param thành các phần tử
	WITH SplitValues
	AS (
		SELECT ItemNumber = ROW_NUMBER() OVER (
				ORDER BY (
						SELECT NULL
						)
				), Value = LTRIM(RTRIM(value))
		FROM STRING_SPLIT(@Params, '|')
		)
	-- Gán giá trị vào các biến param tương ứng
	SELECT @Param1 = MAX(CASE 
				WHEN ItemNumber = 1
					THEN Value
				END), @Param2 = MAX(CASE 
				WHEN ItemNumber = 2
					THEN Value
				END), @Param3 = MAX(CASE 
				WHEN ItemNumber = 3
					THEN Value
				END), @Param4 = MAX(CASE 
				WHEN ItemNumber = 4
					THEN Value
				END)
	FROM SplitValues;

	-- Thực hiện thay thế trong chuỗi định dạng
	SET @Error = REPLACE(@Error, '{0}', ISNULL(@Param1, ''));
	SET @Error = REPLACE(@Error, '{1}', ISNULL(@Param2, ''));
	SET @Error = REPLACE(@Error, '{2}', ISNULL(@Param3, ''));
	SET @Error = REPLACE(@Error, '{3}', ISNULL(@Param4, ''));

	-- Trả về giá trị lỗi cuối cùng
	-- Tìm vị trí của dấu chấm cuối cùng
	IF (CHARINDEX('.', REVERSE(@ErrorName)) <> 0)
	BEGIN
		DECLARE @dotIndex INT = LEN(@ErrorName) - CHARINDEX('.', REVERSE(@ErrorName)) + 1;
		-- Lấy chuỗi sau dấu chấm cuối cùng nếu có
		DECLARE @ErrorCode NVARCHAR(MAX) = NULL;

		IF @dotIndex > 0
			SET @ErrorCode = SUBSTRING(@ErrorName, @dotIndex + 1, LEN(@ErrorName) - @dotIndex);
	END
	
	SET @Result = 
        N'{"errorcode": "' + ISNULL(@ResourceCode, '') + 
        N'", "errormessage": "' + ISNULL(@Error, '') + N'"}';

	RETURN @Result;
END
GO
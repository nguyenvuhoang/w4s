USE O24CMS
GO
IF OBJECT_ID('dbo.__HasValue', 'FN') IS NOT NULL DROP FUNCTION dbo.__HasValue;
GO
CREATE FUNCTION dbo.__HasValue (
    @values nvarchar(max)
)
RETURNS bit
AS
BEGIN
	DECLARE @checkValue int = 0;
	IF ISNULL(@values, '') = ''
	BEGIN
		RETURN 0
	END

	IF ISJSON(@values) = 1 
	BEGIN
		SELECT @checkValue = COUNT(*) FROM OPENJSON(@values);
		IF @checkValue = 0
		BEGIN
			RETURN 0;
		END
	END;
	
    RETURN 1;
END
GO
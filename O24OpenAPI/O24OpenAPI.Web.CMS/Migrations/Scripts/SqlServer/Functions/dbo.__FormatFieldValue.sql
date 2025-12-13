USE O24CMS
GO
IF OBJECT_ID('dbo.__FormatFieldValue', 'FN') IS NOT NULL DROP FUNCTION dbo.__FormatFieldValue;
GO
CREATE FUNCTION dbo.__FormatFieldValue
(
    @FieldValue NVARCHAR(MAX),
    @FormatType NVARCHAR(100)
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
    DECLARE @FormattedValue NVARCHAR(MAX);

    IF @FieldValue IS NOT NULL
    BEGIN
        -- Apply conversion based on FormatType
				IF @FormatType = 'STRING'
					 SET @FormattedValue = @FieldValue; 
        ELSE IF @FormatType = 'DATE'
        BEGIN
            -- Convert to date format, for example, 'YYYY-MM-DD'
            SET @FormattedValue = CONVERT(NVARCHAR(10), CAST(@FieldValue AS DATE), 120); 
        END
        ELSE IF @FormatType = 'NUMBER'
        BEGIN
            -- Convert to number format with commas (1,000.00)
            SET @FormattedValue = FORMAT(CAST(@FieldValue AS DECIMAL(18, 2)), 'N2');
        END
        ELSE IF @FormatType = 'BOOLEAN'
        BEGIN
            -- Convert True/False to Yes/No
            SET @FormattedValue = CASE 
                                    WHEN @FieldValue = '1' THEN CAST(1 AS BIT)
                                    WHEN @FieldValue = '0' THEN CAST(0 AS BIT)
                                  END;
        END
        ELSE
        BEGIN
            -- If no FormatType, return original value 
            SET @FormattedValue = @FieldValue;
        END
    END
    ELSE
    BEGIN
        -- Return NULL if FieldValue is NULL
        SET @FormattedValue = NULL;
    END

    RETURN @FormattedValue;
END
GO
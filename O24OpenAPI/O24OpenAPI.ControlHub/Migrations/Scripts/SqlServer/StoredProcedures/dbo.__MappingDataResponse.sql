USE o24cth
GO
IF OBJECT_ID('dbo.__MappingDataResponse', 'P ') IS NOT NULL DROP PROCEDURE dbo.__MappingDataResponse;
GO
CREATE PROCEDURE [dbo].[__MappingDataResponse]
    @TranCode 					NVARCHAR(100),
    @KeyFieldValue 			NVARCHAR(50),  -- Dynamic parameter for key field (e.g., ContractNumber),
		@DataResponse 			NVARCHAR(MAX)  OUTPUT
AS
BEGIN
    DECLARE @vreturnresponse NVARCHAR(MAX);
    DECLARE @ErrorMessage NVARCHAR(MAX);
    DECLARE @SQL NVARCHAR(MAX)    
    DECLARE @FieldNo INT
    DECLARE @FieldDesc NVARCHAR(100)
    DECLARE @FieldStyle NVARCHAR(50)
    DECLARE @FieldName NVARCHAR(1000)
    DECLARE @ValueStyle NVARCHAR(10)
    DECLARE @ValueName NVARCHAR(1000)
    DECLARE @DataTable NVARCHAR(1000)
    DECLARE @FieldKey NVARCHAR(1000)  -- FieldKey (e.g., ContractNumber or another key)
    DECLARE @JsonResult NVARCHAR(MAX) = '{}';  -- Initialize as empty JSON object
    DECLARE @FieldValue NVARCHAR(MAX) -- Variable to store field value dynamically
		DECLARE @FormatType NVARCHAR(1000)
    
    -- Check if TranCode is defined in the table
    IF NOT EXISTS (
        SELECT 1 
        FROM [dbo].[APIOUTPUTDATADEFINE]
        WHERE TranCode = @TranCode
    )
    BEGIN
        SET @ErrorMessage = 'TRANCODENOTDEFINED';
        THROW 50001, @ErrorMessage, 1;
    END
    
    -- Cursor to loop through the configuration table
    DECLARE ConfigCursor CURSOR FOR
        SELECT FieldNo, FieldDesc, FieldStyle, FieldName, ValueStyle, ValueName, FormatType, DataTable, FieldKey
        FROM [dbo].[APIOUTPUTDATADEFINE]
        WHERE TranCode = @TranCode

    OPEN ConfigCursor
    FETCH NEXT FROM ConfigCursor INTO @FieldNo, @FieldDesc, @FieldStyle, @FieldName, @ValueStyle, @ValueName,@FormatType, @DataTable, @FieldKey

    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Dynamically build SQL to get the value
        SET @SQL = N'SELECT ' + @FieldName + ' AS FieldValue ' +
                   'FROM ' + @DataTable + ' ' +
                   'WHERE ' + @FieldKey + ' = @KeyFieldValue AND Status = ''A'''

        -- Declare a table to hold the result of dynamic SQL
        DECLARE @ResultTable TABLE (FieldValue NVARCHAR(MAX));

        -- Execute the dynamic SQL and insert result into @ResultTable
        INSERT INTO @ResultTable(FieldValue)
        EXEC sp_executesql @SQL, N'@KeyFieldValue NVARCHAR(50)', @KeyFieldValue;

        -- Fetch the FieldValue from @ResultTable
        SELECT @FieldValue = FieldValue FROM @ResultTable;
				
				-- Apply the conversion using the new function dbo.fn_FormatValue
        SET @FieldValue = [dbo].[__FormatFieldValue](@FieldValue, @FormatType);

        -- Add to JSON result dynamically by merging FieldName and FieldValue into a single JSON object
        IF @FieldValue IS NOT NULL
        BEGIN
            SET @JsonResult = JSON_MODIFY(@JsonResult, '$.' + @FieldName, @FieldValue);
        END

        FETCH NEXT FROM ConfigCursor INTO @FieldNo, @FieldDesc, @FieldStyle, @FieldName, @ValueStyle, @ValueName,@FormatType, @DataTable, @FieldKey
    END

    CLOSE ConfigCursor
    DEALLOCATE ConfigCursor

    -- Return the final JSON object
    SET @DataResponse =  @JsonResult;
END
GO
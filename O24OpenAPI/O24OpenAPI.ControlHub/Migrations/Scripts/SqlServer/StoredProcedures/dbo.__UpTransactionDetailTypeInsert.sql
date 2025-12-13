USE o24cth
GO
IF OBJECT_ID('dbo.__UpTransactionDetailTypeInsert', 'P ') IS NOT NULL DROP PROCEDURE dbo.__UpTransactionDetailTypeInsert;
GO
create 
PROCEDURE dbo.__UpTransactionDetailTypeInsert
(
    @WorkflowScheme nvarchar(max),
    @IsReverse NVARCHAR(1), -- R: Reverse |N: Normal
    @TableName NVARCHAR(max),
    @IdEntity INT           -- Id in table

)
AS
BEGIN
    -- Check base info
    EXEC dbo.__ValidateBaseInfo @WorkflowScheme = @WorkflowScheme

    -- Lấy thông tin giao dịch

    DECLARE @RefId VARCHAR(50) = dbo.__GetRefId(@WorkflowScheme);
    DECLARE @TransactionNumber VARCHAR(50) = dbo.__GetTransactionNumber(@WorkflowScheme);
    DECLARE @OldValue NVARCHAR(max);
    DECLARE @StrQuery nvarchar(max) = ''

    --	DECLARE @TableName NVARCHAR(255) = 'AccountLinkage'; -- Replace with your table name
    DECLARE @ColumnName NVARCHAR(255);
    DECLARE @DynamicSQL NVARCHAR(MAX);

    -- Create a cursor to loop through columns
    DECLARE ColumnCursor CURSOR FOR
    SELECT COLUMN_NAME
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = @TableName;

    OPEN ColumnCursor;

    -- Fetch the first column
    FETCH NEXT FROM ColumnCursor
    INTO @ColumnName;

    -- Loop through columns
    WHILE @@FETCH_STATUS = 0
    BEGIN
        IF @ColumnName <> 'Id'
        BEGIN
            -- Build and execute dynamic SQL statement
            SET @DynamicSQL
                = 'INSERT INTO dbo.TransactionDetails
    		(RefId, Entity, FieldName, Status, UpdateType, EntityId, NewValue)' + 'SELECT  ''' + @RefId + ''','''
                  + @TableName + ''',''' + @ColumnName + ''',''N'',''I'',' + CAST(@IdEntity AS NVARCHAR(30)) + ','
                  + 'CAST(' + QUOTENAME(@ColumnName) + ' AS NVARCHAR(MAX)) ' + 'FROM ' + @TableName + ' WHERE Id = '
                  + CAST(@IdEntity AS NVARCHAR(30)) + ';'

            EXEC sp_executesql @DynamicSQL;
        END
        -- Fetch the next column
        FETCH NEXT FROM ColumnCursor
        INTO @ColumnName;
    END

    -- Close and deallocate the cursor
    CLOSE ColumnCursor;
    DEALLOCATE ColumnCursor;

END
GO
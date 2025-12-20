USE O24CMS
GO
IF OBJECT_ID('dbo.__UpdateEntity', 'P ') IS NOT NULL DROP PROCEDURE dbo.__UpdateEntity;
GO
CREATE PROCEDURE [dbo].[__UpdateEntity]
    @WorkflowScheme nvarchar(max),
    @EntityJson nvarchar(max),
	@TableName	nvarchar(100),
	@EntityId int,
	@Action nvarchar(1)
AS
BEGIN

	DECLARE @RefId VARCHAR(50) = dbo.__GetRefId(@WorkflowScheme);
	DECLARE @ValueDate DATE = dbo.__GetValueDate(@WorkflowScheme);
    DECLARE @Lang varchar(2000) = dbo.__GetLanguage(@WorkflowScheme)
	
	DECLARE @columnName nvarchar(500);
	DECLARE @columnType nvarchar(500);
	DECLARE @maxCharLength int;
	DECLARE @maxIntLength int;
	DECLARE @numericScale int;
	
    DECLARE @ErrorMessage nvarchar(3000) = '';
	
	DECLARE @sqlExec nvarchar(max) = '';
	
	IF @Action = 'U'
	BEGIN

		SET @sqlExec = @sqlExec + (SELECT 'UPDATE dbo.' + @TableName + ' SET ' + STRING_AGG(a.[key] + ' = ' + CASE WHEN DATA_TYPE = 'nvarchar' THEN '''' + a.[value] + '''' ELSE 'CAST(''' + a.[value] + ''' as ' +  CASE WHEN DATA_TYPE = 'decimal' THEN 'decimal(' + CAST(NUMERIC_PRECISION AS VARCHAR(max)) + ', ' + CAST(NUMERIC_SCALE AS VARCHAR(max)) + ')' ELSE DATA_TYPE COLLATE DATABASE_DEFAULT  END +')' END, ',') + ', UpdatedOnUtc = GETUTCDATE() WHERE Id = ' + CAST(@EntityId AS VARCHAR(max)) + '; '
									FROM openjson(@EntityJson) a
									INNER JOIN (
										SELECT COLUMN_NAME
											,DATA_TYPE
											,CHARACTER_MAXIMUM_LENGTH
											,NUMERIC_PRECISION
											,NUMERIC_SCALE
											,TABLE_NAME
										FROM INFORMATION_SCHEMA.COLUMNS
										) b ON a.[key] = b.COLUMN_NAME COLLATE DATABASE_DEFAULT
									WHERE b.TABLE_NAME = @TableName COLLATE DATABASE_DEFAULT
									GROUP BY b.TABLE_NAME);
					
		EXEC sp_executesql @sqlExec;
	END
	ELSE IF @Action = 'C'
	BEGIN
		
		SET @sqlExec = @sqlExec + (SELECT 'UPDATE dbo.' + @TableName + ' SET ' + STRING_AGG(a.[key] + ' = ' + a.[key] + ' + CAST(''' + a.[value] + ''' as decimal(30,5))',',') + ', UpdatedOnUtc = GETUTCDATE() WHERE Id = ' + CAST(@EntityId AS VARCHAR) + '; '
									FROM openjson(@EntityJson) a
									INNER JOIN (
										SELECT COLUMN_NAME
											,DATA_TYPE
											,CHARACTER_MAXIMUM_LENGTH
											,NUMERIC_PRECISION
											,NUMERIC_SCALE
											,TABLE_NAME
										FROM INFORMATION_SCHEMA.COLUMNS
										) b ON a.[key] = b.COLUMN_NAME COLLATE DATABASE_DEFAULT
									WHERE b.TABLE_NAME = @TableName COLLATE DATABASE_DEFAULT);
			

		EXEC sp_executesql @sqlExec;
	END
	ELSE IF @Action = 'D'
	BEGIN
		SET @sqlExec = @sqlExec + (SELECT 'UPDATE dbo.' + @TableName + ' SET ' + STRING_AGG(a.[key] + ' = ' + a.[key] + ' - CAST(''' + a.[value] + ''' as decimal(30,5))',',') + ', UpdatedOnUtc = GETUTCDATE() WHERE Id = ' + CAST(@EntityId AS VARCHAR(max)) + '; '
									FROM openjson(@EntityJson) a
									INNER JOIN (
										SELECT COLUMN_NAME
											,DATA_TYPE
											,CHARACTER_MAXIMUM_LENGTH
											,NUMERIC_PRECISION
											,NUMERIC_SCALE
											,TABLE_NAME
										FROM INFORMATION_SCHEMA.COLUMNS
										) b ON a.[key] = b.COLUMN_NAME COLLATE DATABASE_DEFAULT
									WHERE b.TABLE_NAME = @TableName COLLATE DATABASE_DEFAULT);
						
	
		EXEC sp_executesql @sqlExec;
	END;
END;
GO
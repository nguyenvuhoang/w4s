USE o24cth
GO
IF OBJECT_ID('dbo.__InsertEntity', 'P ') IS NOT NULL DROP PROCEDURE dbo.__InsertEntity;
GO
CREATE PROCEDURE [dbo].[__InsertEntity]
    @WorkflowScheme nvarchar(max),
    @EntityJson nvarchar(max),
	@Entity	nvarchar(100)
AS
BEGIN

	DECLARE @RefId VARCHAR(50) = dbo.__GetRefId(@WorkflowScheme);
	DECLARE @ValueDate DATE = dbo.__GetValueDate(@WorkflowScheme);
    DECLARE @Lang varchar(2000) = dbo.__GetLanguage(@WorkflowScheme)
	
    DECLARE @ErrorMessage nvarchar(3000) = '';
	
	DECLARE @sqlExec nvarchar(max) = '';
	DECLARE @SqlColumnOnUtc nvarchar(max) = '';
	DECLARE @SqlUpdatedOnUtc nvarchar(max) = '';
	DECLARE @OutPutInsertValue nvarchar(max) = '';
	DECLARE @OutPutInsertColumn nvarchar(max) = '';
	
	DECLARE @effectColumn nvarchar(max);
	DECLARE @defineColumn nvarchar(max);

	DECLARE @IsJsonArray nvarchar(2) = LEFT(@EntityJson, 1) + RIGHT(@EntityJson, 1); 
	DECLARE @JsonColunm nvarchar(max) = '';

	IF @IsJsonArray != '[]'
	BEGIN
		SET @EntityJson =  JSON_QUERY('['+@EntityJson+']');
	END;

	SELECT 
		@effectColumn = STRING_AGG(field_name, ','),
		@defineColumn = STRING_AGG(field_name + ' ' + CASE DATA_TYPE WHEN 'decimal' THEN 'decimal(' + CAST(NUMERIC_PRECISION as nvarchar(max)) + ','+ CAST(NUMERIC_SCALE as nvarchar(max)) +')' 
												WHEN 'nvarchar' THEN 'nvarchar(' + CASE WHEN CHARACTER_MAXIMUM_LENGTH = -1 THEN 'max' ELSE CAST(CHARACTER_MAXIMUM_LENGTH as nvarchar(max)) END + ')'
												ELSE DATA_TYPE COLLATE DATABASE_DEFAULT END ,',')
	FROM (
		SELECT [key] field_name
			,[value] field_value
		FROM openjson(@EntityJson, '$[0]')
		) a
	INNER JOIN (
		SELECT COLUMN_NAME
			,DATA_TYPE
			,CHARACTER_MAXIMUM_LENGTH
			,NUMERIC_PRECISION
			,NUMERIC_SCALE
			,TABLE_NAME
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = @Entity AND COLUMN_NAME NOT IN ('Id', 'CreatedOnUtc','UpdatedOnUtc')
		) b ON a.field_name = b.COLUMN_NAME COLLATE DATABASE_DEFAULT 

	SELECT @OutPutInsertColumn = STRING_AGG('inserted.'+COLUMN_NAME, ','),
			@OutPutInsertValue = STRING_AGG(COLUMN_NAME, ',')
	FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @Entity;

	SELECT @SqlColumnOnUtc = ',' + STRING_AGG(COLUMN_NAME, ','), @SqlUpdatedOnUtc = ',' + STRING_AGG('GETUTCDATE() ' +COLUMN_NAME, ',') FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @Entity AND COLUMN_NAME IN ('CreatedOnUtc','UpdatedOnUtc')

	

	SET @sqlExec = @sqlExec + 'INSERT INTO dbo.'+@Entity+' (' +@effectColumn + ' ' + ISNULL(@SqlColumnOnUtc,'')+')'; --, CreatedOnUtc, UpdatedOnUtc

	SET @sqlExec = @sqlExec + 'SELECT '+@effectColumn+' ' + ISNULL(@SqlUpdatedOnUtc,'') +' '; --, GETUTCDATE() CreatedOnUtc, GETUTCDATE() UpdatedOnUtc
	SET @sqlExec = @sqlExec + 'FROM openjson('''+@EntityJson+''') WITH ('+@defineColumn+'); ';

	
	IF dbo.__HasValue(@sqlExec) = 1
	BEGIN
		EXEC sp_executesql @sqlExec;
	END;
END;
GO
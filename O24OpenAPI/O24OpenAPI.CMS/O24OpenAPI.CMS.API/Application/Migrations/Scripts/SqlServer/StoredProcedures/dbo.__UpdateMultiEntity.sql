USE O24CMS
GO
IF OBJECT_ID('dbo.__UpdateMultiEntity', 'P ') IS NOT NULL DROP PROCEDURE dbo.__UpdateMultiEntity;
GO
CREATE PROCEDURE [dbo].[__UpdateMultiEntity]
    @JsonData nvarchar(max),
	@RefId nvarchar(100)
AS
BEGIN
	DECLARE @columnName nvarchar(500);
	DECLARE @columnType nvarchar(500);
	DECLARE @maxCharLength int;
	DECLARE @maxIntLength int;
	DECLARE @numericScale int;

    DECLARE @ErrorMessage nvarchar(3000) = '';
	
	DECLARE @effectColumn nvarchar(max) = '';
	DECLARE @defineColumn nvarchar(max) = '';
	DECLARE @sqlExec nvarchar(max) = '';
	DECLARE @sqlDetails nvarchar(max) = '';
	
	DECLARE @index int;
	DECLARE @Entity nvarchar(100);
	DECLARE @EntityId int;
	DECLARE @IsAudit bit;
	DECLARE @EntityData nvarchar(max);

	DECLARE curLoop CURSOR FOR
	SELECT 
		[key],
		JSON_VALUE(item.value, '$.Entity') Entity,
		JSON_VALUE(item.value, '$.EntityId') EntityId,
		ISNULL(JSON_VALUE(item.value, '$.IsAudit'), 0) IsAudit,
		JSON_Query(item.value, '$.Data') [Data]
	FROM OPENJSON(@JsonData) item

	OPEN curLoop;
	FETCH NEXT FROM curLoop INTO @index, @Entity, @EntityId, @IsAudit, @EntityData

	WHILE @@FETCH_STATUS = 0
	BEGIN
		DECLARE @debit nvarchar(max) = JSON_QUERY(@EntityData, '$.debit');
		DECLARE @credit nvarchar(max) = JSON_QUERY(@EntityData, '$.credit');
		DECLARE @update nvarchar(max) = JSON_QUERY(@EntityData, '$.update');
		DECLARE @insert nvarchar(max) = JSON_QUERY(@EntityData, '$.insert');

		IF dbo.__HasValue(@debit) = 1
		BEGIN
			SET @sqlExec = @sqlExec + (SELECT 'UPDATE dbo.' + @Entity + ' SET ' + STRING_AGG(a.[key] + ' = ' + a.[key] + ' - ' + a.[value] + '',',') + ', UpdatedOnUtc = GETUTCDATE() WHERE Id = ' + CAST(@EntityId AS VARCHAR(max)) + '; '
										FROM openjson(@debit) a);
			IF @IsAudit = 1
			BEGIN			
				SET @sqlExec = @sqlExec + 'DECLARE @DataAfterDebit_'+CAST(@index as nvarchar(max))+' NVARCHAR(MAX); SET @DataAfterDebit_'+CAST(@index as nvarchar(max))+' = (SELECT * FROM dbo.'+@Entity+' WHERE Id= '+CAST(@EntityId as varchar(max))+' FOR JSON PATH,WITHOUT_ARRAY_WRAPPER,INCLUDE_NULL_VALUES);INSERT INTO dbo.TransactionDetails(RefId,Entity,FieldName,STATUS,UpdateType,EntityId,OldValue,NewValue,Description) SELECT '''+@RefId+''','''+@Entity+''',a.[key],''N'',''D'','+CAST(@EntityId as varchar(max))+',CAST(b.[value] as decimal(30,5)) + CAST(a.[value] as decimal(30,5)),b.[value],'''' FROM openjson('''+@debit+''') a inner join openjson(@DataAfterDebit_'+CAST(@index as nvarchar(max))+') b on a.[key] = b.[key]; '
			END;
		END;

		IF dbo.__HasValue(@credit) = 1
		BEGIN
			SET @sqlExec = @sqlExec + (SELECT 'UPDATE dbo.' + @Entity + ' SET ' + STRING_AGG(a.[key] + ' = ' + a.[key] + ' + ' + a.[value] + '',',') + ', UpdatedOnUtc = GETUTCDATE() WHERE Id = ' + CAST(@EntityId AS VARCHAR(max)) + '; '
										FROM openjson(@credit) a);
			IF @IsAudit = 1
			BEGIN
				SET @sqlExec = @sqlExec + 'DECLARE @DataAfterCredit_'+CAST(@index as nvarchar(max))+' NVARCHAR(MAX); SET @DataAfterCredit_'+CAST(@index as nvarchar(max))+' = (SELECT * FROM dbo.'+@Entity+' WHERE Id= '+CAST(@EntityId as varchar(max))+' FOR JSON PATH,WITHOUT_ARRAY_WRAPPER,INCLUDE_NULL_VALUES);INSERT INTO dbo.TransactionDetails(RefId,Entity,FieldName,STATUS,UpdateType,EntityId,OldValue,NewValue,Description) SELECT '''+@RefId+''','''+@Entity+''',a.[key],''N'',''C'','+CAST(@EntityId as varchar(max))+',CAST(b.[value] as decimal(30,5)) - CAST(a.[value] as decimal(30,5)),b.[value],'''' FROM openjson('''+@credit+''') a inner join openjson(@DataAfterCredit_'+CAST(@index as nvarchar(max))+') b on a.[key] = b.[key]; '
			END;
		END;

		IF dbo.__HasValue(@update) = 1
		BEGIN
			IF @IsAudit = 1
			BEGIN
				SET @sqlExec = @sqlExec + 'DECLARE @DataBeforeUpdate_'+CAST(@index as nvarchar(max))+' NVARCHAR(MAX); SET @DataBeforeUpdate_'+CAST(@index as nvarchar(max))+' = (SELECT * FROM dbo.'+@Entity+' WHERE Id= '+CAST(@EntityId as varchar(max))+' FOR JSON PATH,WITHOUT_ARRAY_WRAPPER,INCLUDE_NULL_VALUES);INSERT INTO dbo.TransactionDetails(RefId,Entity,FieldName,STATUS,UpdateType,EntityId,OldValue,NewValue,Description) SELECT '''+@RefId+''','''+@Entity+''',a.[key],''N'',''U'','+CAST(@EntityId as varchar(max))+',b.[value],a.[value],'''' FROM openjson('''+@update+''') a inner join openjson(@DataBeforeUpdate_'+CAST(@index as nvarchar(max))+') b on a.[key] = b.[key]; '
			END;

			SET @sqlExec = @sqlExec + (SELECT 'UPDATE dbo.' + @Entity + ' SET ' + STRING_AGG(a.[key] + ' = ' + CASE WHEN DATA_TYPE = 'nvarchar' THEN '''' + a.[value] + '''' ELSE 'CAST(''' + a.[value] + ''' as ' +  CASE WHEN DATA_TYPE = 'decimal' THEN 'decimal(' + CAST(NUMERIC_PRECISION AS VARCHAR(max)) + ', ' + CAST(NUMERIC_SCALE AS VARCHAR) + ')' ELSE DATA_TYPE COLLATE DATABASE_DEFAULT  END +')' END, ',') + ', UpdatedOnUtc = GETUTCDATE() WHERE Id = ' + CAST(@EntityId AS VARCHAR(max)) + '; '
										FROM openjson(@update) a
										INNER JOIN (
											SELECT COLUMN_NAME
												,DATA_TYPE
												,CHARACTER_MAXIMUM_LENGTH
												,NUMERIC_PRECISION
												,NUMERIC_SCALE
												,TABLE_NAME
											FROM INFORMATION_SCHEMA.COLUMNS
											) b ON a.[key] = b.COLUMN_NAME COLLATE DATABASE_DEFAULT
										WHERE b.TABLE_NAME = @Entity COLLATE DATABASE_DEFAULT
										GROUP BY b.TABLE_NAME);
		END;
		
		IF dbo.__HasValue(@insert) = 1
		BEGIN
			SET @effectColumn = '';
			SET @defineColumn = '';

			DECLARE @SqlColumnOnUtc nvarchar(max) = '';
			DECLARE @OutPutInsertValue nvarchar(max) = '';
			DECLARE @OutPutInsertColumn nvarchar(max) = '';
			DECLARE @SqlUpdatedOnUtc nvarchar(max) = '';

			DECLARE @IsJsonArray nvarchar(2) = LEFT(@insert, 1) + RIGHT(@insert, 1); 
			DECLARE @JsonColunm nvarchar(max) = '';

			IF @IsJsonArray != '[]'
			BEGIN
				SET @insert =  JSON_QUERY('['+@insert+']');
			END;

			SELECT 
				@effectColumn = STRING_AGG(field_name, ','),
				@defineColumn = STRING_AGG(field_name + ' ' + CASE DATA_TYPE WHEN 'decimal' THEN 'decimal(' + CAST(NUMERIC_PRECISION as nvarchar(max)) + ','+ CAST(NUMERIC_SCALE as nvarchar(max)) +')' 
														WHEN 'nvarchar' THEN 'nvarchar(' + CASE WHEN CHARACTER_MAXIMUM_LENGTH = -1 THEN 'max' ELSE CAST(CHARACTER_MAXIMUM_LENGTH as nvarchar(max)) END + ')'
														ELSE DATA_TYPE COLLATE DATABASE_DEFAULT END ,',')
			FROM (
				SELECT [key] field_name
					,[value] field_value
				FROM openjson(@insert, '$[0]')
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
				) b ON a.field_name = b.COLUMN_NAME COLLATE DATABASE_DEFAULT;

			SELECT @OutPutInsertColumn = STRING_AGG('inserted.'+COLUMN_NAME, ','),
					@OutPutInsertValue = STRING_AGG(COLUMN_NAME, ',')
			FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @Entity;

			SELECT  @SqlColumnOnUtc = ',' + STRING_AGG(COLUMN_NAME, ','), @SqlUpdatedOnUtc = ',' + STRING_AGG('GETUTCDATE() ' +COLUMN_NAME, ',') FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @Entity AND COLUMN_NAME IN ('CreatedOnUtc','UpdatedOnUtc')

			IF @IsAudit = 1
			BEGIN
				SET @sqlExec = @sqlExec + 'SELECT * INTO #'+@Entity+'_'+CAST(@index as nvarchar(max))+' FROM dbo.'+@Entity+' WHERE 1=0; ';

				SET @sqlExec = @sqlExec + 'ALTER TABLE #'+@Entity+'_'+CAST(@index as nvarchar(max))+' DROP COLUMN Id; ';
				SET @sqlExec = @sqlExec + 'ALTER TABLE #'+@Entity+'_'+CAST(@index as nvarchar(max))+' ADD Id int not null; ';
			END;

			SET @sqlExec = @sqlExec + 'INSERT INTO dbo.'+@Entity+' (' +@effectColumn + ' ' + ISNULL(@SqlColumnOnUtc,'')+')'; --, CreatedOnUtc, UpdatedOnUtc

			IF @IsAudit = 1
			BEGIN
				SET @sqlExec = @sqlExec + 'OUTPUT '+ISNULL(@OutPutInsertColumn,'')+ ' '
				SET @sqlExec = @sqlExec + 'INTO #'+@Entity+'_'+CAST(@index as nvarchar(max))+'('+@OutPutInsertValue+') ';
			END;

			SET @sqlExec = @sqlExec + 'SELECT '+@effectColumn+' ' + ISNULL(@SqlUpdatedOnUtc,'') +' '; --, GETUTCDATE() CreatedOnUtc, GETUTCDATE() UpdatedOnUtc
			SET @sqlExec = @sqlExec + 'FROM openjson('''+@insert+''') WITH ('+@defineColumn+'); ';

			IF @IsAudit = 1
			BEGIN
				SET @sqlExec = @sqlExec + 'DECLARE @NewData_'+CAST(@index as nvarchar(max))+' NVARCHAR(MAX); ';
				SET @sqlExec = @sqlExec + 'SET @NewData_'+CAST(@index as nvarchar(max))+' = ( ';
				SET @sqlExec = @sqlExec + '		SELECT a.* ';
				SET @sqlExec = @sqlExec + '		FROM #'+@Entity+'_'+CAST(@index as nvarchar(max))+' a ';
				SET @sqlExec = @sqlExec + '		FOR JSON PATH, INCLUDE_NULL_VALUES); ';
				SET @sqlExec = @sqlExec + '	INSERT INTO dbo.TransactionDetails ( ';
				SET @sqlExec = @sqlExec + '		RefId, Entity,FieldName,STATUS,UpdateType,EntityId,OldValue,NewValue,Description) ';
				SET @sqlExec = @sqlExec + '	SELECT '''+@RefId+''','''+@Entity+''',col.[key],''N'',''I'',JSON_VALUE(item.[value], ''$.Id''),'''',JSON_VALUE(item.[value], ''$.'' + col.[key]),'''' ';
				SET @sqlExec = @sqlExec + '	FROM (SELECT * FROM OPENJSON(@NewData_'+CAST(@index as nvarchar(max))+')) item, (SELECT * FROM OPENJSON(@NewData_'+CAST(@index as nvarchar(max))+', ''$[0]'')) col; ';
				SET @sqlExec = @sqlExec + '	DROP TABLE #'+@Entity+'_'+CAST(@index as nvarchar(max))+'; ';
			END;
		END;
				
		FETCH NEXT FROM curLoop INTO @index, @Entity, @EntityId, @IsAudit, @EntityData;
	END;

	IF dbo.__HasValue(@sqlExec) = 1
	BEGIN
		EXEC sp_executesql @sqlExec;
	END;

	CLOSE curLoop;
	DEALLOCATE curLoop;
END
GO
USE o24cth
GO
IF OBJECT_ID('dbo.__CheckCondition', 'FN') IS NOT NULL DROP FUNCTION dbo.__CheckCondition;
GO
CREATE FUNCTION [dbo].[__CheckCondition] (
    @WorkflowScheme NVARCHAR(MAX)
    ,@JsonCondition NVARCHAR(MAX))
RETURNS BIT
BEGIN
    -- Nếu không phải là JSON, trả về 1
    IF dbo.__HasValue(@JsonCondition) = 0
    BEGIN
        RETURN 1;
    END;

	DECLARE @IsJsonArray nvarchar(2) = LEFT(@JsonCondition, 1) + RIGHT(@JsonCondition, 1); 
	DECLARE @JsonColunm nvarchar(max) = '';

	IF @IsJsonArray != '[]'
	BEGIN
		SET @JsonCondition =  JSON_QUERY('['+@JsonCondition+']');
	END;

	DECLARE Cur CURSOR FOR
	SELECT 
		 JSON_VALUE([value], '$.func') func, JSON_VALUE([value], '$.type') [type], JSON_QUERY([value], '$.paras') paras 
	FROM OPENJSON(@JsonCondition) 

	-- Mở cursor
	OPEN Cur;

	DECLARE @paras NVARCHAR(MAX);
	DECLARE @query nvarchar(max) = '';
	DECLARE @func NVARCHAR(100);
	DECLARE @type NVARCHAR(100);

	DECLARE @IsResult bit = 1;

	-- Fetch dòng đầu tiên vào các biến
	FETCH NEXT FROM Cur INTO @func, @type, @paras;

	-- Bắt đầu vòng lặp với cursor
	WHILE @@FETCH_STATUS = 0
	BEGIN
		DECLARE @paras1 nvarchar(max);
		DECLARE @paras2 nvarchar(max);
		DECLARE @paras_type1 nvarchar(100);
		DECLARE @paras_type2 nvarchar(100);

		IF UPPER(@func) IN ('AND', 'OR')
		BEGIN
			DECLARE @result1 bit;
			DECLARE @result2 bit;

			SELECT @paras1 = [value], @paras_type1 = [type] from openjson(@paras) where [key] = 0;
			SELECT @paras2 = [value], @paras_type2 = [type] from openjson(@paras) where [key] = 1;

			SET @result1 = dbo.__CheckCondition(@WorkflowScheme, JSON_QUERY(@paras1));
			SET @result2 = dbo.__CheckCondition(@WorkflowScheme, JSON_QUERY(@paras2));

			SET @IsResult = CASE UPPER(@func) 
								WHEN 'AND' THEN CASE WHEN @result1 = 1 AND @result2 = 1 THEN 1 ELSE 0 END
								ELSE CASE WHEN @result1 = 1 OR @result2 = 1 THEN 1 ELSE 0 END
							END;
		END
		ELSE IF UPPER(@func) = 'HASVALUE'
		BEGIN
			SELECT @paras1 = [value], @paras_type1 = [type] from openjson(@paras) where [key] = 0;

			SET @IsResult = dbo.__HasValue(JSON_VALUE(@WorkflowScheme, @paras1)); 
		END
		ELSE
		BEGIN

			SELECT @paras1 = [value], @paras_type1 = [type] from openjson(@paras) where [key] = 0;
			SELECT @paras2 = [value], @paras_type2 = [type] from openjson(@paras) where [key] = 1;

			IF UPPER(@type) = 'NUMBER'
			BEGIN
				DECLARE @num1 decimal(35,5) = 0;
				DECLARE @num2 decimal(35,5) = 0;

				SELECT 
					@num1 = SUM(CAST(CASE WHEN LEFT([value],2) = '$.' THEN JSON_VALUE(@WorkflowScheme, [value])
							WHEN LEFT([value],3) = '+$.' THEN JSON_VALUE(@WorkflowScheme, REPLACE([value], '+',''))
							WHEN LEFT([value],3) = '-$.' THEN REPLACE(LEFT([value],1) +  JSON_VALUE(@WorkflowScheme, REPLACE([value],'-','')),'--','')
							ELSE [value]
					END as decimal(35,5)))
				FROM string_split(REPLACE(REPLACE(REPLACE(@paras1,' ', '') ,'+', '|+'), '-','|-'),'|');

				SELECT 
					@num2 = SUM(CAST(CASE WHEN LEFT([value],2) = '$.' THEN JSON_VALUE(@WorkflowScheme, [value])
							WHEN LEFT([value],3) = '+$.' THEN JSON_VALUE(@WorkflowScheme, REPLACE([value], '+',''))
							WHEN LEFT([value],3) = '-$.' THEN REPLACE(LEFT([value],1) +  JSON_VALUE(@WorkflowScheme, REPLACE([value],'-','')),'--','')
							ELSE [value]
					END as decimal(35,5)))
				FROM string_split(REPLACE(REPLACE(REPLACE(@paras2,' ', '') ,'+', '|+'), '-','|-'),'|');

				SET @IsResult = CASE @func 
									WHEN '>' THEN CASE WHEN @num1 > @num2 THEN 1 ELSE 0 END 
									WHEN '>=' THEN CASE WHEN @num1 >= @num2 THEN 1 ELSE 0 END 
									WHEN '<' THEN CASE WHEN @num1 < @num2 THEN 1 ELSE 0 END 
									WHEN '<=' THEN CASE WHEN @num1 <= @num2 THEN 1 ELSE 0 END 
									WHEN '=' THEN CASE WHEN @num1 = @num2 THEN 1 ELSE 0 END 
									WHEN '!=' THEN CASE WHEN @num1 != @num2 THEN 1 ELSE 0 END 
									ELSE 0
								END; 
			END
			ELSE IF UPPER(@type) = 'STRING'
			BEGIN
				DECLARE @string1 nvarchar(max);
				DECLARE @string2 nvarchar(max);
			
				IF LEFT(@paras1,2) = '$.'
				BEGIN
					SET @string1 = JSON_VALUE(@WorkflowScheme, @paras1);
				END
				ELSE
				BEGIN
					SET @string1 = @paras1;
				END;
			
				IF LEFT(@paras2,2) = '$.'
				BEGIN
					SET @string2 = JSON_VALUE(@WorkflowScheme, @paras2);
				END
				ELSE
				BEGIN
					SET @string2 = @paras2;
				END;

				SET @IsResult = CASE @func 
									WHEN '=' THEN CASE WHEN @string1 = @string2 THEN 1 ELSE 0 END 
									WHEN '!=' THEN CASE WHEN @string1 != @string2 THEN 1 ELSE 0 END 
									ELSE 0
								END; 
			END
			ELSE IF UPPER(@type) = 'BOOL'
			BEGIN
				DECLARE @bit1 bit;
				DECLARE @bit2 bit;
			
				IF LEFT(@paras1,2) = '$.'
				BEGIN
					SET @bit1 = JSON_VALUE(@WorkflowScheme, @paras1);
				END
				ELSE
				BEGIN
					SET @bit1 = CAST(@paras1 as bit);
				END;
			
				IF LEFT(@paras2,2) = '$.'
				BEGIN
					SET @bit2 = JSON_VALUE(@WorkflowScheme, @paras2);
				END
				ELSE
				BEGIN
					SET @bit2 = CAST(@paras2 as bit);
				END;

				SET @IsResult = CASE @func 
									WHEN '=' THEN CASE WHEN @bit1 = @bit2 THEN 1 ELSE 0 END 
									WHEN '!=' THEN CASE WHEN @bit1 != @bit2 THEN 1 ELSE 0 END 
									ELSE 0
								END; 
			END;
		END;

		FETCH NEXT FROM Cur INTO @func, @type, @paras;
	END;

	CLOSE Cur;
	DEALLOCATE Cur;

	RETURN @IsResult;
END;
GO
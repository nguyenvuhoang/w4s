USE O24CMS
GO
IF OBJECT_ID('dbo.__MappingParameter', 'FN') IS NOT NULL DROP FUNCTION dbo.__MappingParameter;
GO
CREATE FUNCTION [dbo].[__MappingParameter]
(
	@JsonInput nvarchar(max),
	@JsonParam nvarchar(max)
)
RETURNS nvarchar(max)
AS
BEGIN
	DECLARE @Result nvarchar(max) = '{}';

	IF dbo.__HasValue(@JsonParam) = 0
	BEGIN
		RETURN '{}';
	END;

	DECLARE @key nvarchar(500);
	DECLARE @value nvarchar(max);
	DECLARE @type int;

	DECLARE Cur CURSOR FOR
	SELECT 
		[key], [value], [type] 
	FROM openjson(@JsonParam)
	-- Mở cursor
	OPEN Cur;
	-- Fetch dòng đầu tiên vào các biến
	FETCH NEXT FROM Cur INTO @key, @value, @type;

	-- Bắt đầu vòng lặp với cursor
	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF @type = 2
		BEGIN
			SET @Result = JSON_MODIFY(@Result, '$.' + @key, @value);
		END
		ELSE IF @type = 1
		BEGIN
			IF CHARINDEX('+', @value) > 0 OR CHARINDEX('-', @value) > 0 
			BEGIN
				DECLARE @sum decimal(35,5) = 0;
				SELECT 
					@sum = SUM(CAST(CASE WHEN LEFT([value],2) = '$.' THEN JSON_VALUE(@JsonInput, [value])
						 WHEN LEFT([value],3) = '+$.' THEN JSON_VALUE(@JsonInput, REPLACE([value], '+',''))
						 WHEN LEFT([value],3) = '-$.' THEN REPLACE(LEFT([value],1) +  JSON_VALUE(@JsonInput, REPLACE([value],'-','')),'--','')
						 ELSE [value]
					END as decimal(35,5)))
				from string_split(REPLACE(REPLACE(REPLACE(@value,' ', '') ,'+', '|+'), '-','|-'),'|');
			END
			ELSE
			BEGIN
				IF left(@value, 2) = '$.'
				BEGIN
					DECLARE @json nvarchar(max) = ISNULL(JSON_VALUE(@JsonInput,@value),JSON_QUERY(@JsonInput,@value));

					IF ISJSON(@json) = 1
					BEGIN
						SET @Result = JSON_MODIFY(@Result, '$.' + @key, JSON_QUERY(@json));
					END
					ELSE
					BEGIN
						SET @Result = JSON_MODIFY(@Result, '$.' + @key, @json);
					END
				END
				ELSE
				BEGIN
					SET @Result = JSON_MODIFY(@Result, '$.' + @key, @value);
				END;
			END;
		END
		ELSE IF @type = 3
		BEGIN
			SET @Result = JSON_MODIFY(@Result, '$.' + @key, CAST(@value as bit));
		END
		ELSE IF @type =  4
		BEGIN
			SET @Result = JSON_MODIFY(@Result, '$.' + @key, JSON_QUERY(@value));

			--SET @Result = JSON_MODIFY(@Result, @JsonModifed + @key, JSON_QUERY(@value));
		END
		ELSE IF @type =  5
		BEGIN
			SET @Result = JSON_MODIFY(@Result, '$.' + @key, JSON_QUERY(dbo.__MappingParameter(@JsonInput,JSON_QUERY(@value))));

			--SET @Result = JSON_MODIFY(@Result, @JsonModifed + @key, JSON_QUERY(@value));
		END;

		-- Fetch dòng tiếp theo vào các biến
		FETCH NEXT FROM Cur INTO @key, @value, @type;
	END

	-- Đóng cursor sau khi sử dụng
	CLOSE Cur;
	DEALLOCATE Cur;

    -- Kết quả sẽ có dạng: Json object
    RETURN @Result

END;
GO
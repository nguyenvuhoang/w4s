USE o24cth
GO
IF OBJECT_ID('dbo.INFO_FAVORITEFEATURE_MERGE_USER_CHOOSEN', 'P ') IS NOT NULL DROP PROCEDURE dbo.INFO_FAVORITEFEATURE_MERGE_USER_CHOOSEN;
GO

CREATE PROCEDURE [dbo].[INFO_FAVORITEFEATURE_MERGE_USER_CHOOSEN] @WorkflowScheme NVARCHAR(max)
	,@IsReverse NVARCHAR(1)
	,-- R: Reverse |N: Normal
	@OutputWorkflowScheme NVARCHAR(max) OUTPUT
AS
BEGIN TRY
	BEGIN TRANSACTION

	-- Thêm thời gian bắt đầu thực hiện transaction
	DECLARE @StartTime DATETIME = GETUTCDATE();

	SET @OutputWorkflowScheme = @WorkflowScheme;

	DECLARE @Lang VARCHAR(2) = dbo.__GetLanguage(@WorkflowScheme);
	DECLARE @CurrentBranchCode VARCHAR(5) = dbo.__GetCurrentBranchCode(@WorkflowScheme);
	DECLARE @WorkingDate DATETIME = dbo.__GetValueDate(@WorkflowScheme);
	DECLARE @PathData VARCHAR(1000) = '$.Request.RequestBody.Data';
	DECLARE @jsonResponse NVARCHAR(max) = JSON_QUERY(@WorkflowScheme, @PathData);
	-- KHAI BAO DANH SACH FIELD CHO GIAO DICH
    DECLARE @UserCode 							VARCHAR(50) = JSON_VALUE(@WorkflowScheme, @PathData + '.info.usercode'); 
	DECLARE @ApplicationCode VARCHAR(500) = JSON_VALUE(@WorkflowScheme, @PathData + '.applicationcode');
	DECLARE @ErrorMessage NVARCHAR(3000) = '';
	DECLARE @ContractNumber NVARCHAR(100);

	-- Check data input REQUIRED
	IF @ApplicationCode IS NULL
	BEGIN
		SET @ErrorMessage = dbo.__GetError('Common.Value.Required', @Lang, 'Application Code');

		THROW 50001
			,@ErrorMessage
			,1;
	END

	IF @ApplicationCode !='DIGITAL'
	BEGIN
		SET @ErrorMessage = dbo.__GetError('Common.Value.Required', @Lang, 'Application Code');

		THROW 50001
			,@ErrorMessage
			,1;
	END

	DECLARE @ListDataQuery NVARCHAR(MAX);

	-- Gán kết quả JSON vào biến @DataJsonResponse
  CREATE TABLE #Temp
(
   
    SubItemCode NVARCHAR(100)
);
INSERT INTO #Temp
SELECT b.SubItemCode FROM dbo.USERFAVORITEFEATURE a inner join dbo.FAVORITEFEATURE b   on a.FavoriteFeatureID=b.Id AND a.UserCode = @UserCode
			inner join [dbo].[FAVORITEFEATURESUBITEM] c on b.SubItemCode = c.SubItemCode 	

SELECT * FROM #Temp


	
    
  SELECT @ListDataQuery = (
			
SELECT distinct(a.FavoriteFeatureCode)itemcode,a.FavoriteFeatureName itemname, subitem.Id id,subitem.SubItemCode as subitemcode ,subitem.Icon as icon ,subitem.Label as label,
subitem.Url as url,
CASE  WHEN  #Temp.SubItemCode IS NOT NULL  THEN CAST(1as bit)  ELSE CAST(0 as bit)  END as favorite
FROM dbo.FAVORITEFEATURE a INNER JOIN dbo.FAVORITEFEATURESUBITEM subitem ON a.SubItemCode=subitem.SubItemCode
LEFT JOIN #Temp on subitem.SubItemCode=#Temp.SubItemCode FOR  JSON AUTO, INCLUDE_NULL_VALUES
			);		
		
	-- Set giá trị trả về 
	IF ISNULL(@ListDataQuery, '') <> ''AND @ListDataQuery != ''
	BEGIN
		SET @JsonResponse = JSON_MODIFY(@JsonResponse, '$.favoriteItems', JSON_QUERY(@ListDataQuery));
	END
	ELSE
	BEGIN
		SET @JsonResponse = JSON_MODIFY(@JsonResponse, '$.favoriteItems', '[]');
	END

	SET @OutputWorkflowScheme = JSON_MODIFY(@OutputWorkflowScheme, '$.Response.Data', JSON_QUERY(@jsonResponse));

	EXEC dbo.__SetDataBaseTransaction @WorkflowScheme = @WorkflowScheme
		,@OutputWorkflowScheme = @OutputWorkflowScheme OUTPUT

	-- Thêm thông tin transaction
	DECLARE @Duration INT = DATEDIFF(MILLISECOND, @StartTime, GETUTCDATE());

	SET @OutputWorkflowScheme = JSON_MODIFY(@OutputWorkflowScheme, '$.Request.RequestHeader.TxContext.start_time', CONVERT(NVARCHAR, @StartTime, 127));
	SET @OutputWorkflowScheme = JSON_MODIFY(@OutputWorkflowScheme, '$.Request.RequestHeader.TxContext.duration', @Duration);

	EXEC dbo.__CreateTransaction @WorkflowScheme = @OutputWorkflowScheme;

	COMMIT TRANSACTION
END TRY

BEGIN CATCH
	ROLLBACK;

	-- Ghi thông tin vào log
	DECLARE @Error NVARCHAR(MAX) = ERROR_MESSAGE();
	DECLARE @FullError NVARCHAR(MAX) = 'ErrorMessage: ' + ISNULL(ERROR_MESSAGE(), '') + ' - ErrorStore: ' + ISNULL(ERROR_PROCEDURE(), '') + ' - ErrorLine: ' + CAST(ISNULL(ERROR_LINE(), - 1) AS VARCHAR(10));

	-- Cập nhật dữ liệu error bảng Transaction
	EXEC dbo.__CreateTransaction @WorkflowScheme = @OutputWorkflowScheme
		,@IsError = 1
		,@ErrorMessage = @FullError;

	EXEC dbo.__Log @WorkflowScheme = @WorkflowScheme
		,@ShortMessage = @Error
		,@FullMessage = @FullError;

	throw;
END CATCH;

GO
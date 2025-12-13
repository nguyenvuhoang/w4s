USE o24cth
GO
IF OBJECT_ID('dbo.INSERT_USERFAVORITEFEATURE', 'P ') IS NOT NULL DROP PROCEDURE dbo.INSERT_USERFAVORITEFEATURE;
GO
CREATE PROCEDURE [dbo].[INSERT_USERFAVORITEFEATURE] @WorkflowScheme NVARCHAR(max)
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
	
	
	
    DECLARE @UserCode VARCHAR(50) = JSON_VALUE(@WorkflowScheme, @PathData + '.info.usercode'); 
	DECLARE @ApplicationCode VARCHAR(500) = JSON_VALUE(@WorkflowScheme, @PathData + '.applicationcode');
	DECLARE @FavoriteFeatureID INT = JSON_VALUE(@WorkflowScheme, @PathData + '.favoriteFeatureid');
	DECLARE @Favorite BIT = ISNULL(JSON_VALUE(@WorkflowScheme, @PathData + '.favorite'),0);
	DECLARE @Description NVARCHAR(500) = JSON_VALUE(@WorkflowScheme, @PathData + '.description');
	DECLARE @ErrorMessage NVARCHAR(3000) = '';
	

	-- Check data input REQUIRED
	IF @ApplicationCode IS NULL
	BEGIN
		SET @ErrorMessage = dbo.__GetError('Common.Value.Required', @Lang, 'Application Code');

		THROW 50001
			,@ErrorMessage
			,1;
	END

	
	IF @UserCode IS NULL OR @UserCode=''
	BEGIN
		SET @ErrorMessage = dbo.__GetError('Common.Value.Required', @Lang, 'UserCode');

		THROW 50001
			,@ErrorMessage
			,1;
	END

	IF @FavoriteFeatureID = 0 
	BEGIN
		SET @ErrorMessage = dbo.__GetError('Common.Value.Required', @Lang, 'FavoriteFeatureID');

		THROW 50001
			,@ErrorMessage
			,1;
	END

	-- Check User Code Tồn tại chưa
    IF NOT EXISTS (
		SELECT 1  FROM dbo.S_USERACCOUNT WHERE UserCode=@UserCode
	)
	BEGIN
	 SET @ErrorMessage = dbo.__GetError('Common.Value.NotExist', @Lang, @UserCode);

		THROW 50001
			,@ErrorMessage
			,1;

	END

	-- Check FavoriteFeatureID Tồn tại chưa
	IF NOT EXISTS (
		SELECT 1  FROM dbo.FAVORITEFEATURE WHERE ID=@FavoriteFeatureID
	)
	BEGIN
	 SET @ErrorMessage = dbo.__GetError('Common.Value.NotExist', @Lang, @FavoriteFeatureID);

		THROW 50001
			,@ErrorMessage
			,1;

	END

	-- Check Unique trước khi inser
	IF NOT EXISTS (
		SELECT 1  FROM dbo.USERFAVORITEFEATURE WHERE UserCode=@UserCode AND FavoriteFeatureID=@FavoriteFeatureID
	)
	BEGIN
	-- Insert
	     INSERT INTO o24cms.dbo.USERFAVORITEFEATURE
			( UserCode, FavoriteFeatureID, Favorite, Description)
			VALUES(@UserCode, @FavoriteFeatureID, 1, '');

	END 
	ELSE 
	BEGIN
	   SET @ErrorMessage = dbo.__GetError('Common.Value.Unique', @Lang, @FavoriteFeatureID);

		THROW 50001
			,@ErrorMessage
			,1;
	END


	-- Set giá trị trả về 
	
	
	SET @OutputWorkflowScheme = JSON_MODIFY(@OutputWorkflowScheme, '$.Response.Data', JSON_QUERY(@jsonResponse));

	EXEC dbo.__SetDataBaseTransaction @WorkflowScheme = @WorkflowScheme
		,@OutputWorkflowScheme = @OutputWorkflowScheme OUTPUT

	
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
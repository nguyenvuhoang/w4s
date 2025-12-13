USE o24cth
GO
IF OBJECT_ID('dbo.INSERT_USERFAVORITEFEATURES_CHOSE', 'P ') IS NOT NULL DROP PROCEDURE dbo.INSERT_USERFAVORITEFEATURES_CHOSE;
GO

CREATE PROCEDURE [dbo].[INSERT_USERFAVORITEFEATURES_CHOSE] @WorkflowScheme NVARCHAR(max)
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
	DECLARE @favoriteitems NVARCHAR(MAX) = JSON_QUERY(@WorkflowScheme, @PathData + '.favoriteitems');
	DECLARE @ErrorMessage NVARCHAR(3000) = '';

	-- Tạo bảng tạm để lưu kết quả
	CREATE TABLE #TempTable (
		FavoriteFeatureCode NVARCHAR(50)
		,SubItemCode NVARCHAR(50)
		,Favorite BIT
		);

	-- Duyệt qua JSON và lấy itemcode và subitemcode để chèn vào bảng tạm
	INSERT INTO #TempTable (
		FavoriteFeatureCode
		,SubItemCode
		,Favorite
		)
	SELECT item.ItemCode
		,subitem.SubItemCode
		,subitem.Favorite
	FROM OPENJSON(@favoriteitems) WITH (
			ItemCode NVARCHAR(50) '$.itemcode'
			,SubItem NVARCHAR(MAX) '$.subitem' AS JSON
			) AS item
	CROSS APPLY OPENJSON(item.SubItem) WITH (
			SubItemCode NVARCHAR(50) '$.subitemcode'
			,Favorite BIT '$.favorite'
			) AS subitem;

	-- Xem kết quả
	SELECT *
	FROM #TempTable;

	CREATE TABLE #TempIDFavoriteFeature (IDFavoriteFeature INT);

	INSERT INTO #TempIDFavoriteFeature
	SELECT b.ID
	FROM #TempTable a
	INNER JOIN dbo.FavoriteFeature b ON a.FavoriteFeatureCode = b.FavoriteFeatureCode
		AND a.SubItemCode = b.SubItemCode
	WHERE a.Favorite = 1

	-- Biến để lưu giá trị từ con trỏ
	DECLARE @IDFavoriteFeatureCur INT
	DECLARE @SubItemCode NVARCHAR(50);

	-- Tạo con trỏ để duyệt qua bảng tạm
	DECLARE cursor_temp CURSOR
	FOR
	SELECT IDFavoriteFeature
	FROM #TempIDFavoriteFeature;

	-- Mở con trỏ
	OPEN cursor_temp;

	-- Lấy dữ liệu từ con trỏ và gán vào các biến
	FETCH NEXT
	FROM cursor_temp
	INTO @IDFavoriteFeatureCur;

	-- Bắt đầu vòng lặp để chèn dữ liệu từ bảng tạm vào bảng đích
	WHILE @@FETCH_STATUS = 0
	BEGIN
		-- Chèn từng hàng vào bảng đích
		IF NOT EXISTS (
				SELECT 1
				FROM dbo.UserFavoriteFeature
				WHERE UserCode = @UserCode
					AND FavoriteFeatureID = @IDFavoriteFeatureCur
				)
		BEGIN
			INSERT INTO o24cms.dbo.UserFavoriteFeature (
				UserCode
				,FavoriteFeatureID
				,Favorite
				,Description
				,CreatedOnUtc
				,UpdatedOnUtc
				)
			VALUES (
				@UserCode
				,@IDFavoriteFeatureCur
				,1
				,N''
				,GETUTCDATE()
				,GETUTCDATE()
				);
		END

		-- Lấy hàng tiếp theo từ con trỏ
		FETCH NEXT
		FROM cursor_temp
		INTO @IDFavoriteFeatureCur;
	END

	-- Đóng và xóa con trỏ
	CLOSE cursor_temp;

	DEALLOCATE cursor_temp;

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
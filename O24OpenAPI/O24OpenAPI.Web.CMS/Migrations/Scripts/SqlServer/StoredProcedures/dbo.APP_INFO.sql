USE O24CMS
GO
IF OBJECT_ID('dbo.APP_INFO', 'P ') IS NOT NULL DROP PROCEDURE dbo.APP_INFO;
GO
CREATE PROCEDURE [dbo].[APP_INFO]
    @WorkflowScheme nvarchar(max),
    @IsReverse nvarchar(1), -- R: Reverse |N: Normal
    @OutputWorkflowScheme nvarchar(max) OUTPUT
AS
BEGIN TRY
    BEGIN TRANSACTION
    -- Thêm thời gian bắt đầu thực hiện transaction
    DECLARE @StartTime DATETIME = GETUTCDATE();

    SET @OutputWorkflowScheme = @WorkflowScheme;

    DECLARE @Lang VARCHAR(2) = dbo.__GetLanguage(@WorkflowScheme);
    DECLARE @CurrentBranchCode VARCHAR(5) = dbo.__GetCurrentBranchCode(@WorkflowScheme);
    DECLARE @WorkingDate DATETIME = dbo.__GetValueDate(@WorkflowScheme);

    DECLARE @PathData varchar(1000) = '$.request.request_body.data';

    DECLARE @jsonResponse nvarchar(max) = JSON_QUERY(@WorkflowScheme, @PathData);
		
    -- KHAI BAO DANH SACH FIELD CHO GIAO DICH
		
	
    DECLARE @UserCode 							VARCHAR(50) = 'sems'; 
    DECLARE @ApplicationCode 				VARCHAR(500) = JSON_VALUE(@WorkflowScheme, @PathData + '.applicationcode');

    DECLARE @ErrorMessage			NVARCHAR(3000) = '';
    DECLARE @Avatar						NVARCHAR(max);
		DECLARE @UserCommand			NVARCHAR(max);
		DECLARE @RoleId NVARCHAR(10);
		DECLARE @CommandId NVARCHAR(50);
		
		DECLARE @IsActive						BIT = 0; 
	
    -- Check data input REQUIRED
    IF @usercode IS NULL
    BEGIN
        SET @ErrorMessage = dbo.__GetError('Common.Value.Required', @Lang, 'User Code');
        THROW 50001, @ErrorMessage, 1;
    END

    IF @ApplicationCode IS NULL
    BEGIN
        SET @ErrorMessage = dbo.__GetError('Common.Value.Required', @Lang, 'Application Code');
        THROW 50001, @ErrorMessage, 1;
    END   
		

    -- Process Check User Login
		IF (@ApplicationCode = 'DIGITAL')
			BEGIN 
			IF NOT EXISTS (
						SELECT 1 
						FROM [dbo].[D_DIGITALBANKINGUSER]
						WHERE UserCode = @usercode
					)
			BEGIN
						SET @ErrorMessage = dbo.__GetError('Common.Value.UserCodeIsNotExist', @Lang, @usercode);
						THROW 50001, @ErrorMessage, 1;
			END
			ELSE
			BEGIN
					-- GET INFO TO RETURN
					SELECT  @Avatar = dibauseravatar.IMAGEBIN
					
					FROM [dbo].[D_DIGITALBANKINGUSER] dibauser
					INNER JOIN  [dbo].[D_DIGITALBANKINGUSER_AVATAR] dibauseravatar ON dibauseravatar.UserId =  dibauser.UserCode
					WHERE dibauser.ApplicationCode = @ApplicationCode
					AND dibauser.UserCode= @UserCode
			END
		END
		ELSE IF (@ApplicationCode = 'BO')
		BEGIN
			IF NOT EXISTS (
						SELECT 1 
						FROM [dbo].[S_USERPORTAL]
						WHERE UserCode = @usercode
					)
			BEGIN
						SET @ErrorMessage = dbo.__GetError('Common.Value.UserCodeIsNotExist', @Lang, @usercode);
						THROW 50001, @ErrorMessage, 1;
			END
			ELSE
			BEGIN
					SELECT  @Avatar = portaluseravatar.IMAGEBIN
					FROM [dbo].[S_USERPORTAL] portaluser
					INNER JOIN  [dbo].[S_USERPORTAL_AVATAR] portaluseravatar ON portaluseravatar.UserId =  portaluser.UserCode
					WHERE portaluser.UserCode= @UserCode
			END
		END
				
		SELECT @RoleId = RoleID
		FROM dbo.UserInRole
		WHERE UserCode = @UserCode
		
		-- Execute the procedure for each RoleId - RoleID Default is Guest 1
		EXEC dbo.__GetMenuInfoFromRoleId 
							@ApplicationCode = @ApplicationCode,
							@RoleId = @RoleId,
							@Lang = @Lang ,
							@UserCommand = @UserCommand OUTPUT;
		
		
    -- Them du lieu tra ve
    SET @jsonResponse = JSON_MODIFY(@jsonResponse, '$.avatar', @Avatar);
		SET @jsonResponse = JSON_MODIFY(@jsonResponse, '$.usercommand', JSON_QUERY(@UserCommand));
	

		SET @OutputWorkflowScheme = JSON_MODIFY(@OutputWorkflowScheme, '$.Response.Data', JSON_QUERY(@jsonResponse));

    EXEC dbo.__SetDataBaseTransaction @WorkflowScheme = @WorkflowScheme,
                                      @OutputWorkflowScheme = @OutputWorkflowScheme OUTPUT

    -- Thêm thông tin transaction
    DECLARE @Duration INT = DATEDIFF(MILLISECOND, @StartTime, GETUTCDATE());
    SET @OutputWorkflowScheme
        = JSON_MODIFY(
                         @OutputWorkflowScheme,
                         '$.request.request_header.tx_context.start_time',
                         CONVERT(NVARCHAR, @StartTime, 127)
                     );
    SET @OutputWorkflowScheme
        = JSON_MODIFY(
                         @OutputWorkflowScheme,
                         '$.request.request_header.tx_context.duration',
                         @Duration
                     );
		
    EXEC dbo.__CreateTransaction @WorkflowScheme = @OutputWorkflowScheme;
		

    COMMIT TRANSACTION
END TRY
BEGIN CATCH
    ROLLBACK;
    -- Ghi thông tin vào log
    DECLARE @Error NVARCHAR(MAX) = ERROR_MESSAGE();
    DECLARE @FullError NVARCHAR(MAX)
        = 'ErrorMessage: ' + ISNULL(ERROR_MESSAGE(), '') + ' - ErrorStore: ' + ISNULL(ERROR_PROCEDURE(), '')
          + ' - ErrorLine: ' + CAST(ISNULL(ERROR_LINE(), -1) AS varchar(10));

    -- Cập nhật dữ liệu error bảng Transaction
    EXEC dbo.__CreateTransaction @WorkflowScheme = @OutputWorkflowScheme,
                                 @IsError = 1,
                                 @ErrorMessage = @FullError;

    EXEC dbo.__Log @WorkflowScheme = @WorkflowScheme,
                   @ShortMessage = @Error,
                   @FullMessage = @FullError;

    throw;
END CATCH;
GO
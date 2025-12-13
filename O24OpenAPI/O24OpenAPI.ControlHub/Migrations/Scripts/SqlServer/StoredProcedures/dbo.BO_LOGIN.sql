USE o24cth
GO
IF OBJECT_ID('dbo.BO_LOGIN', 'P ') IS NOT NULL DROP PROCEDURE dbo.BO_LOGIN;
GO
CREATE PROCEDURE [dbo].[BO_LOGIN]
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
    DECLARE @WorkingDate DATETIME = dbo.__GetValueDate(@WorkflowScheme);

    DECLARE @PathData varchar(1000) = '$.Request.RequestBody.Data';

    DECLARE @jsonResponse nvarchar(max) = JSON_QUERY(@WorkflowScheme, @PathData);

    -- KHAI BAO DANH SACH FIELD CHO GIAO DICH
    DECLARE @UserCode varchar(25) = JSON_VALUE(@WorkflowScheme, @PathData + '.lgname');
    DECLARE @PassWord varchar(500) = JSON_VALUE(@WorkflowScheme, @PathData + '.password');

    DECLARE @ErrorMessage			NVARCHAR(3000) = '';
		DECLARE @Fullname					NVARCHAR(200) = '';
    DECLARE @Avatar						NVARCHAR(max);
		DECLARE @UserCommand			NVARCHAR(max);
		DECLARE @Email						NVARCHAR(100) = '';
		DECLARE @ApplicationCode 	NVARCHAR(50) = 'O24PORTAL';
		DECLARE @RoleId NVARCHAR(10);
		DECLARE @MergedUserCommand NVARCHAR(MAX) = N'[]'; -- Initialize as an empty JSON array
		DECLARE @CommandId NVARCHAR(50);
        DECLARE @BranchCode				NVARCHAR(100) = '';
		
		
	
    -- Check data input REQUIRED
    IF @UserCode IS NULL
    BEGIN
        SET @ErrorMessage = dbo.__GetError('Common.Value.Required', @Lang, 'UserCode');
        THROW 50001, @ErrorMessage, 1;
    END

    IF @PassWord IS NULL
    BEGIN
        SET @ErrorMessage = dbo.__GetError('Common.Value.Required', @Lang, 'Password');
        THROW 50001, @ErrorMessage, 1;
    END   

    -- Process Check User Login
		
		SELECT @Fullname = ISNULL(userportal.FirstName,'') + ' ' +  ISNULL(userportal.MiddleName,'') + ' ' + ISNULL(userportal.LastName,''),
		@Email = userportal.Email ,@BranchCode = userportal.BranchID
		FROM [dbo].[S_USERPORTAL] userportal
		INNER JOIN [dbo].[S_USERPASSWORD_PORTAL] userpassword ON userportal.UserCode=userpassword.UserCode
		WHERE userportal.status= 1
		AND userportal.UserCode= @UserCode 
		AND userpassword.password= @Password
		
		IF NOT EXISTS (
		SELECT 1
		FROM [dbo].[S_USERPORTAL] userportal
		INNER JOIN [dbo].[S_USERPASSWORD_PORTAL] userpassword ON userportal.UserCode=userpassword.UserCode
		WHERE userportal.status= 1
		AND userportal.UserCode= @UserCode 
		AND userpassword.password= @Password 
		)
		BEGIN
			SET @ErrorMessage = dbo.__GetError('Common.Value.UsernamePasswordIncorrect', @Lang,'');
					THROW 50001, @ErrorMessage, 1;
			
		END
		
		
		
		SELECT @Fullname = ISNULL(userportal.FirstName,'') + ' ' +  ISNULL(userportal.MiddleName,'') + ' ' + ISNULL(userportal.LastName,''),
		@Email = userportal.Email 
		FROM [dbo].[S_USERPORTAL] userportal
		INNER JOIN [dbo].[S_USERPASSWORD_PORTAL] userpassword ON userportal.UserCode=userpassword.UserCode
		WHERE userportal.status= 1
		AND userportal.UserCode= @UserCode 
		AND userpassword.password= @Password
		IF (@Fullname = '') 
		BEGIN
			SET @ErrorMessage = dbo.__GetError('Common.Value.UsernamePasswordIncorrect', @Lang,'');
					THROW 50001, @ErrorMessage, 1;
		END


	-- GET MENU INFO FROM CURRENT USER 
		DECLARE RoleCursor CURSOR FOR
			SELECT b.RoleId
			FROM [dbo].[S_USERACCOUNT] a
			INNER JOIN dbo.UserInRole b ON a.UserCode = b.UserCode
			WHERE a.UserCode = @UserCode;

		OPEN RoleCursor;
		FETCH NEXT FROM RoleCursor INTO @RoleId;

		WHILE @@FETCH_STATUS = 0
		BEGIN
		-- Execute the procedure for each RoleId
		EXEC dbo.__GetMenuInfoFromRoleId 
			@ApplicationCode = @ApplicationCode,
			@RoleId = @RoleId,
			@Lang = @Lang ,
			@UserCommand = @UserCommand OUTPUT;

		-- Check if the UserCommand is valid JSON before processing
		IF @UserCommand IS NOT NULL AND ISJSON(@UserCommand) = 1
		BEGIN
			-- Use OPENJSON to extract each object from the @UserCommand array
			DECLARE @TempUserCommand TABLE (jsonObject NVARCHAR(MAX));
			INSERT INTO @TempUserCommand (jsonObject)
			SELECT value 
			FROM OPENJSON(@UserCommand); -- This extracts objects from the array

			 -- Loop through the extracted objects and append them one by one
			DECLARE @jsonObject NVARCHAR(MAX);
			DECLARE ObjectCursor CURSOR FOR
				SELECT jsonObject FROM @TempUserCommand;

			OPEN ObjectCursor;
			FETCH NEXT FROM ObjectCursor INTO @jsonObject;

			WHILE @@FETCH_STATUS = 0
			BEGIN
				-- Extract the CommandId from the current object to check for duplicates
				SET @CommandId = JSON_VALUE(@jsonObject, '$.CommandId');

				-- Check if CommandId already exists in the merged result
				IF NOT EXISTS (
					SELECT 1 
					FROM OPENJSON(@MergedUserCommand) 
					WHERE JSON_VALUE(value, '$.CommandId') = @CommandId
				)
				BEGIN
					-- Append the current jsonObject to the MergedUserCommand
					SET @MergedUserCommand = JSON_MODIFY(@MergedUserCommand, 'append $', JSON_QUERY(@jsonObject));
				END
				ELSE
				BEGIN
					PRINT 'CommandId already exists: ' + @CommandId;
				END

				FETCH NEXT FROM ObjectCursor INTO @jsonObject;
			END

			-- Close the object cursor
			CLOSE ObjectCursor;
			DEALLOCATE ObjectCursor;
		END
		ELSE
		BEGIN
			PRINT 'Invalid or NULL UserCommand for RoleId: ' + @RoleId;
		END

    -- Fetch the next RoleId
    FETCH NEXT FROM RoleCursor INTO @RoleId;
	END

	-- Close and deallocate the cursor
	CLOSE RoleCursor;
	DEALLOCATE RoleCursor;

    -- Them du lieu tra ve
  SET @jsonResponse = JSON_MODIFY(@jsonResponse,'$.name', @Fullname);
	SET @jsonResponse = JSON_MODIFY(@jsonResponse,'$.email', @Email);
	SET @jsonResponse = JSON_MODIFY(@jsonResponse, '$.usercode', @UserCode);
	SET @jsonResponse = JSON_MODIFY(@jsonResponse, '$.branchcode', @BranchCode);

	SET @jsonResponse = JSON_MODIFY(@jsonResponse, '$.usercommand', JSON_QUERY(@MergedUserCommand));
	

	SET @OutputWorkflowScheme = JSON_MODIFY(@OutputWorkflowScheme, '$.Response.Data', JSON_QUERY(@jsonResponse));

    EXEC dbo.__SetDataBaseTransaction @WorkflowScheme = @WorkflowScheme,
                                      @OutputWorkflowScheme = @OutputWorkflowScheme OUTPUT

    -- Thêm thông tin transaction
    DECLARE @Duration INT = DATEDIFF(MILLISECOND, @StartTime, GETUTCDATE());
    SET @OutputWorkflowScheme
        = JSON_MODIFY(
                         @OutputWorkflowScheme,
                         '$.Request.RequestHeader.TxContext.start_time',
                         CONVERT(NVARCHAR, @StartTime, 127)
                     );
    SET @OutputWorkflowScheme
        = JSON_MODIFY(
                         @OutputWorkflowScheme,
                         '$.Request.RequestHeader.TxContext.duration',
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
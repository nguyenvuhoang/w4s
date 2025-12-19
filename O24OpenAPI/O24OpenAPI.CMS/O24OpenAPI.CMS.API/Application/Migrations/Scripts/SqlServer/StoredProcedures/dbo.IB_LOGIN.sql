USE O24CMS
GO
IF OBJECT_ID('dbo.IB_LOGIN', 'P ') IS NOT NULL DROP PROCEDURE dbo.IB_LOGIN;
GO
CREATE PROCEDURE [dbo].[IB_LOGIN]
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
	
    DECLARE @PathData varchar(1000) = '$.Request.RequestBody.Data';

    DECLARE @jsonResponse nvarchar(max) = JSON_QUERY(@WorkflowScheme, @PathData);

    -- KHAI BAO DANH SACH FIELD CHO GIAO DICH
    DECLARE @Loginname varchar(25) = JSON_VALUE(@WorkflowScheme, @PathData + '.lgname'); -- Gia tri nay dung se out ra ngoai
    DECLARE @PassWord varchar(500) = JSON_VALUE(@WorkflowScheme, @PathData + '.password');
		DECLARE @OS varchar(10) 			 = JSON_VALUE(@WorkflowScheme, @PathData + '.os');

    DECLARE @ErrorMessage			NVARCHAR(3000) = '';
		DECLARE @Fullname					NVARCHAR(200) = '';
    DECLARE @Avatar						NVARCHAR(max);
		DECLARE @UserCommand			NVARCHAR(max);
		DECLARE @Email						NVARCHAR(100) = '';
		DECLARE @ApplicationCode 	NVARCHAR(50) = 'O24DIBA';
		DECLARE @RoleId NVARCHAR(10);
		DECLARE @CommandId NVARCHAR(50);
		DECLARE @UserCode 				NVARCHAR(100) = '';
		DECLARE @BranchCode				NVARCHAR(100) = '';
		

    -- Check data input REQUIRED
    IF @Loginname IS NULL
    BEGIN
        SET @ErrorMessage = dbo.__GetError('Common.Value.Required', @Lang, 'Login name');
        THROW 50001, @ErrorMessage, 1;
    END

    IF @PassWord IS NULL
    BEGIN
        SET @ErrorMessage = dbo.__GetError('Common.Value.Required', @Lang, 'Password');
        THROW 50001, @ErrorMessage, 1;
    END   

    -- Process Check User Login
		
		SELECT  @Fullname = dibauser.FullName, @Email = dibauser.Email, @UserCode = dibauser.UserCode, @BranchCode = dibauser.BranchCode
		
		FROM [dbo].[D_DIGITALBANKINGUSER] dibauser
		INNER JOIN  [dbo].[S_USERACCOUNT] useraccount ON dibauser.UserCode =  useraccount.UserCode
		INNER JOIN [dbo].[S_USERPASSWORD] userpassword ON userpassword.UserCode=dibauser.UserCode
		WHERE dibauser.ApplicationCode ='DIGITAL' 
		AND useraccount.UserAccountStatus='A'
		AND useraccount.UserName= @Loginname 
		AND userpassword.password=@PassWord ;
		
	
		
		IF (@UserCode = '') 
		BEGIN
			SET @ErrorMessage = dbo.__GetError('Common.Value.UsernamePasswordIncorrect', @Lang,'');
					THROW 50001, @ErrorMessage, 1;
		END
		
		
    -- Them du lieu tra ve
    SET @jsonResponse = JSON_MODIFY(@jsonResponse, '$.name', @Fullname);
		SET @jsonResponse = JSON_MODIFY(@jsonResponse, '$.email', @Email);
		SET @jsonResponse = JSON_MODIFY(@jsonResponse, '$.usercode', @UserCode);
		SET @jsonResponse = JSON_MODIFY(@jsonResponse, '$.branchcode', @BranchCode);
		
		-- CAP NHAT THONG TIN DANG NHAP THANH CONG
		EXEC [dbo].[__UpdateLogin] @OS = @OS
	

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
USE O24CMS
GO
IF OBJECT_ID('dbo.__CreateCustomer', 'P ') IS NOT NULL DROP PROCEDURE dbo.__CreateCustomer;
GO
CREATE 
PROCEDURE [dbo].[__CreateCustomer]
(
    @WorkflowScheme NVARCHAR(MAX),
    @IsReverse NVARCHAR(2),
		@OutputWorkflowScheme NVARCHAR(max) OUTPUT,
		@CustomerCode		NVARCHAR(50) OUTPUT
)
AS
BEGIN TRY
    BEGIN TRANSACTION


    -- Thêm thời gian bắt đầu thực hiện transaction
    DECLARE @StartTime DATETIME = GETUTCDATE();
		
    -- Gán giá trị trả về
    SET @OutputWorkflowScheme = @WorkflowScheme;

    DECLARE @PathData VARCHAR(1000) = '$.Request.RequestBody.Data';
    DECLARE @jsonResponse nvarchar(max) = JSON_QUERY(@WorkflowScheme, @PathData);

    DECLARE @Description VARCHAR(2000) = dbo.__GetDescription(@WorkflowScheme);
    DECLARE @ErrorMessage NVARCHAR(3000) = '';
		
    --- KHOI TAO GIA TRI MAC DINH CHO D_CONTRACT
    DECLARE @CUSTID 				NVARCHAR(50);
		DECLARE @FULLNAME 			NVARCHAR(255)		= JSON_VALUE(@WorkflowScheme, @PathData + '.fullname');
		DECLARE @SHORTNAME 			NVARCHAR(255) 	= JSON_VALUE(@WorkflowScheme, @PathData + '.shortname');
		DECLARE @DOB 						DATETIME				= JSON_VALUE(@WorkflowScheme, @PathData + '.birthday');
		DECLARE @ADDRRESIDENT 	NVARCHAR(2000)	= JSON_VALUE(@WorkflowScheme, @PathData + '.residentaddress');
		DECLARE @ADDRTEMP 			NVARCHAR(2000)	= '';
		DECLARE @SEX 						CHAR(1)					= JSON_VALUE(@WorkflowScheme, @PathData + '.gender');			
		DECLARE @NATION 				NVARCHAR(50)		= JSON_VALUE(@WorkflowScheme, @PathData + '.nation');			
		DECLARE @TEL 						NVARCHAR(50)		= JSON_VALUE(@WorkflowScheme, @PathData + '.phone');			
		DECLARE @FAX 						NVARCHAR(30)		= '';
		DECLARE @Email 					NVARCHAR(100)		= JSON_VALUE(@WorkflowScheme, @PathData + '.email');			
		DECLARE @LICENSETYPE 		NVARCHAR(10)		= 'NRIC';			
		DECLARE @LICENSEID 			NVARCHAR(50)		= JSON_VALUE(@WorkflowScheme, @PathData + '.licenseid');			
		DECLARE @ISSUEDATE 			DATETIME				= JSON_VALUE(@WorkflowScheme, @PathData + '.issuedate');
		DECLARE @ISSUEPLACE 		NVARCHAR(100)		= JSON_VALUE(@WorkflowScheme, @PathData + '.issueplace');
		DECLARE @JOB 						NVARCHAR(100)		= '';
		DECLARE @OFFICEADDR 		NVARCHAR(2000)	= '';
		DECLARE @OFFICEPHONE 		NVARCHAR(30)		= '';
		DECLARE @CFTYPE 				CHAR(10)				= JSON_VALUE(@WorkflowScheme, @PathData + '.customertype');		
		DECLARE @BRANCHID 			CHAR(10)				= JSON_VALUE(@WorkflowScheme, @PathData + '.branch');		
		DECLARE @STATUS 				NVARCHAR(50)		= 'P';
		DECLARE @CUSTCODE 			NVARCHAR(50)		= JSON_VALUE(@WorkflowScheme, @PathData + '.corebankingcifnumber');
		DECLARE @CFCODE 				NVARCHAR(50)		= JSON_VALUE(@WorkflowScheme, @PathData + '.corebankingcifnumber');
		DECLARE @CTYPE 					NVARCHAR(1)			= 'B';
		DECLARE @PhoneCountryCode NVARCHAR(50)  = '+856';
		DECLARE @LinkedUserID 	NVARCHAR(50)		= '';
		DECLARE @FirstName 			NVARCHAR(250)		= '';
		DECLARE @MiddleName 		NVARCHAR(250)		= '';
		DECLARE @LastName 			NVARCHAR(250)		= '';
		DECLARE @ExpiryDate 		DATETIME				= NULL;
		DECLARE @KycID 					NVARCHAR(20)		= '1';
		DECLARE @UserCreated 		NVARCHAR(50)		= '';
		DECLARE @DateCreated 		DATETIME				= JSON_VALUE(@WorkflowScheme, @PathData + '.startdate'); 
		DECLARE @UserModified 	NVARCHAR(50)		= NULL;
		DECLARE @LastModified 	DATETIME				= JSON_VALUE(@WorkflowScheme, @PathData + '.startdate'); 	
		DECLARE @UserApproved 	NVARCHAR(50)		= NULL;
		DECLARE @DateApproved 	DATETIME				= NULL;
		DECLARE @Township 			NVARCHAR(50)		= JSON_VALUE(@WorkflowScheme, @PathData + '.townshipname'); 	 
		DECLARE @Region 				NVARCHAR(50)		= JSON_VALUE(@WorkflowScheme, @PathData + '.region'); 	 
		DECLARE @FULLNAMEMM 		NVARCHAR(1000)	= JSON_VALUE(@WorkflowScheme, @PathData + '.localfullname'); 	 
		DECLARE @ADDRMM 				NVARCHAR(1000)	= JSON_VALUE(@WorkflowScheme, @PathData + '.addresslaos'); 	
		DECLARE @PHONEMM 				NVARCHAR(255)		= JSON_VALUE(@WorkflowScheme, @PathData + '.phonelao'); 
		DECLARE @LATITUDE 			NVARCHAR(50)		= NULL;
		DECLARE @LONGITUDE 			NVARCHAR(50)		= NULL;
		DECLARE @AGENTLOCATION 	NVARCHAR(MAX)		= NULL;
		
		
		-- Tạo UUID mới
		WHILE 1 = 1
		BEGIN
				-- Tạo UUID theo định dạng GUID
				SET @CUSTID = NEWID();
				
				-- Kiểm tra xem UUID có tồn tại trong bảng D_CUSTOMER không
				IF NOT EXISTS (SELECT 1 FROM [dbo].[D_CUSTOMER] WHERE [CUSTID] = @CUSTID)
				BEGIN
						-- Nếu không tồn tại, thoát vòng lặp và sử dụng giá trị này
						BREAK;
				END
		END



    --- D_CONTRACT ACCOUNT-----
    BEGIN
        INSERT INTO [dbo].[D_CUSTOMER] 
				(
						[CUSTID], 
						[FULLNAME], 
						[SHORTNAME], 
						[DOB], 
						[ADDRRESIDENT], 
						[ADDRTEMP], 
						[SEX], 
						[NATION], 
						[TEL], 
						[FAX], 
						[Email], 
						[LICENSETYPE], 
						[LICENSEID], 
						[ISSUEDATE], 
						[ISSUEPLACE], 
						[DESCRIPTION], 
						[JOB], 
						[OFFICEADDR], 
						[OFFICEPHONE], 
						[CFTYPE], 
						[BRANCHID], 
						[STATUS], 
						[CUSTCODE], 
						[CFCODE], 
						[CTYPE], 
						[PhoneCountryCode], 
						[LinkedUserID], 
						[FirstName], 
						[MiddleName], 
						[LastName], 
						[ExpiryDate], 
						[KycID], 
						[UserCreated], 
						[DateCreated], 
						[UserModified], 
						[LastModified], 
						[UserApproved], 
						[DateApproved], 
						[Township], 
						[Region], 
						[FULLNAMEMM], 
						[ADDRMM], 
						[PHONEMM], 
						[LATITUDE], 
						[LONGITUDE], 
						[AGENTLOCATION]
				) 
				VALUES 
				(
						@CUSTID, 
						@FULLNAME, 
						@SHORTNAME, 
						@DOB, 
						@ADDRRESIDENT, 
						@ADDRTEMP, 
						@SEX, 
						@NATION, 
						@TEL, 
						@FAX, 
						@Email, 
						@LICENSETYPE, 
						@LICENSEID, 
						@ISSUEDATE, 
						@ISSUEPLACE, 
						@DESCRIPTION, 
						@JOB, 
						@OFFICEADDR, 
						@OFFICEPHONE, 
						@CFTYPE, 
						@BRANCHID, 
						@STATUS, 
						@CUSTCODE, 
						@CFCODE, 
						@CTYPE, 
						@PhoneCountryCode, 
						@LinkedUserID, 
						@FirstName, 
						@MiddleName, 
						@LastName, 
						@ExpiryDate, 
						@KycID, 
						@UserCreated, 
						@DateCreated, 
						@UserModified, 
						@LastModified, 
						@UserApproved, 
						@DateApproved, 
						@Township, 
						@Region, 
						@FULLNAMEMM, 
						@ADDRMM, 
						@PHONEMM, 
						@LATITUDE, 
						@LONGITUDE, 
						@AGENTLOCATION
				)

    END

    DECLARE @CustomerId INT = 0;
    SELECT @CustomerId = Id
    FROM dbo.[D_CUSTOMER]
    WHERE CUSTID = @CUSTID;

    BEGIN
        EXEC dbo.__UpTransactionDetailTypeInsert @WorkflowScheme = @WorkflowScheme,
                                                 @IsReverse = @IsReverse,
                                                 @TableName = 'D_CUSTOMER',
                                                 @IdEntity = @CustomerId;
    END
		
		SET @CustomerCode = @CUSTID;
		
		COMMIT TRANSACTION
		
END TRY
BEGIN CATCH
    ROLLBACK;
    -- Ghi thông tin vào log
    DECLARE @Error NVARCHAR(MAX) = ERROR_MESSAGE();

    DECLARE @FullError NVARCHAR(MAX)
        = 'ErrorMessage: ' + ISNULL(ERROR_MESSAGE(), '') + ' - ErrorStore: ' + ISNULL(ERROR_PROCEDURE(), '')
          + ' - ErrorLine: ' + CAST(ISNULL(ERROR_LINE(), -1) AS varchar(10));

    EXEC dbo.__Log @WorkflowScheme = @WorkflowScheme,
                   @ShortMessage = @Error,
                   @FullMessage = @FullError;
									 
	  BEGIN
        THROW 50001, @FullError, 1;
    END   

END CATCH;
GO
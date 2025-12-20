USE O24CMS
GO
IF OBJECT_ID('dbo.__ReverseTransaction', 'P ') IS NOT NULL DROP PROCEDURE dbo.__ReverseTransaction;
GO
CREATE PROCEDURE dbo.__ReverseTransaction (
    @WorkflowScheme NVARCHAR(MAX)
)
AS
BEGIN
    DECLARE @RefId NVARCHAR(100) = dbo.__GetRefId(@WorkflowScheme);
    DECLARE @TransactionNumber NVARCHAR(100) = dbo.__GetTransactionNumber(@WorkflowScheme);
    DECLARE @IsReverseOfReversed bit = dbo.__IsReverseOfReversed(@WorkflowScheme);

	DECLARE @SqlReverse nvarchar(max) = '';

	IF @IsReverseOfReversed = 0
	BEGIN
		-- Reverse thông tin dựa vào bảng transaction details
		--Reverse for case Update Entity
		SET @SqlReverse += ISNULL((SELECT STRING_AGG(sqlexec, ' ') FROM (select 'UPDATE dbo.' + Entity + ' SET ' + STRING_AGG(FieldName + ' = ' + CASE 
														WHEN DATA_TYPE = 'nvarchar'
															THEN '''' + OldValue + ''''
														ELSE 'CAST(''' + OldValue + ''' as ' + CASE DATA_TYPE
																WHEN 'nvarchar'
																	THEN 'nvarchar(' + CAST(CHARACTER_MAXIMUM_LENGTH AS VARCHAR(max)) + ')'
																WHEN 'decimal'
																	THEN 'decimal(' + CAST(NUMERIC_PRECISION AS VARCHAR(max)) + ', ' + CAST(NUMERIC_SCALE AS VARCHAR(max)) + ')'
																ELSE DATA_TYPE
																END + ')'
														END, ',') + ' WHERE Id = ' + CAST(EntityId AS VARCHAR(max)) + '; ' sqlexec
								from dbo.[TransactionDetails] a
									inner join (select RefId, min(Id) Id 
												from dbo.[TransactionDetails] 
												where UpdateType = 'U'
												GROUP BY RefId, Entity, FieldName, EntityId) b on a.Id = b.Id and a.RefId = b.RefId
									INNER JOIN (
												SELECT COLUMN_NAME
													,DATA_TYPE
													,CHARACTER_MAXIMUM_LENGTH
													,NUMERIC_PRECISION
													,NUMERIC_SCALE
													,TABLE_NAME
												FROM INFORMATION_SCHEMA.COLUMNS
												) c ON a.FieldName = c.COLUMN_NAME COLLATE DATABASE_DEFAULT
												AND a.Entity = c.TABLE_NAME COLLATE DATABASE_DEFAULT
								where a.RefId = @RefId and UpdateType = 'U' AND Status = 'N'
								GROUP BY Entity, EntityId, a.RefId) a),'');

		SET @SqlReverse += ISNULL((SELECT STRING_AGG(sqlexec, ' ') FROM (select ' UPDATE dbo.' + Entity + ' SET ' + STRING_AGG(FieldName + ' = ' + FieldName + ' - ' +CAST(Amount AS nvarchar(max)), ',') + ' WHERE Id = ' + CAST(EntityId AS VARCHAR(max)) + '; ' sqlexec
								from (select RefId, Entity, FieldName, EntityId, SUM(CAST(NewValue as decimal(35,5)) - CAST(OldValue as decimal(35,5))) Amount 
									from dbo.[TransactionDetails] where RefId = @RefId
									AND UpdateType IN ('C','D') AND Status = 'N'
									GROUP BY RefId, Entity, FieldName, EntityId) a
								GROUP BY RefId, Entity, EntityId) a),'');

		--Reverse for case Insert Entity
		SET @SqlReverse += ISNULL((SELECT STRING_AGG(sqlexec, ' ') FROM (select 'DELETE dbo.' + Entity + ' WHERE Id = '+CAST(EntityId as varchar(max))+'; ' sqlexec
							from dbo.TransactionDetails a
								INNER JOIN (
											SELECT COLUMN_NAME
												,DATA_TYPE
												,CHARACTER_MAXIMUM_LENGTH
												,NUMERIC_PRECISION
												,NUMERIC_SCALE
												,TABLE_NAME
											FROM INFORMATION_SCHEMA.COLUMNS
											) b
								ON a.FieldName = b.COLUMN_NAME COLLATE DATABASE_DEFAULT AND a.Entity = b.TABLE_NAME  COLLATE DATABASE_DEFAULT
							where RefId = @RefId AND UpdateType = 'I' AND Status = 'N'
							GROUP BY Entity, EntityId, RefId) a),'');

		IF dbo.__HasValue(@SqlReverse) = 1
		BEGIN
			EXEC sp_executesql @SqlReverse;
		END;
    
		-- Chuyển trạng thái audit -> Reverse
		UPDATE dbo.[Transaction]
			SET IsReverse = 1
		WHERE RefId = @RefId;
    
		UPDATE dbo.[DepositHistory]
			SET DepositHistoryStatus = 'R'
		WHERE RefId = @RefId;

		UPDATE dbo.TransactionDetails
			SET Status = 'R'
		WHERE RefId = @RefId;

		UPDATE dbo.DepositAccountTrans
			SET TransactionStatus = 'R'
		WHERE TransactionNumber = @TransactionNumber;

		UPDATE dbo.DepositIFCBalanceTrans
			SET TransactionStatus = 'R'
		WHERE TransactionNumber = @TransactionNumber;

		UPDATE dbo.ODContractIFCBalanceTrans
			SET TransactionStatus = 'R'
		WHERE TransactionNumber = @TransactionNumber;

		UPDATE dbo.OverdraftContractTrans
			SET TransactionStatus = 'R'
		WHERE TransactionNumber = @TransactionNumber;

		UPDATE dbo.ProvisioningTrans
			SET TransactionStatus = 'R'
		WHERE TransactionNumber = @TransactionNumber;

		UPDATE dbo.DepositStatement
			SET StatementStatus = 'R'
		WHERE TransactionNumber = @TransactionNumber;

		UPDATE dbo.GLEntries
			SET TransactionStatus = 'R'
		WHERE TransactionNumber = @TransactionNumber;
	END
	ELSE
	BEGIN
		
		DECLARE @EntityJson nvarchar(max);

		SET @EntityJson = (SELECT 
								'[' + STRING_AGG(ReverseData, ',') +']'
							FROM (
							SELECT 
								'{"Entity":"'+Entity+'", "EntityId":'+CAST(EntityId as varchar)+',"Data": {"insert": [' +ISNULL(STRING_AGG(CASE WHEN UpdateType = 'I' THEN EntityData END, ','),'')+ '],"update": ' +ISNULL(STRING_AGG(CASE WHEN UpdateType = 'U' THEN EntityData END, ','),'{}')+ ',"debit": ' +ISNULL(STRING_AGG(CASE WHEN UpdateType = 'D' THEN EntityData END, ','),'{}')+ ',"credit": ' +ISNULL(STRING_AGG(CASE WHEN UpdateType = 'C' THEN EntityData END, ','),'{}')+ '}}' ReverseData
							FROM (
							SELECT 
								Entity,
								UpdateType,
								CASE WHEN UpdateType = 'I' THEN 0 ELSE EntityId END EntityId,
								'{' + STRING_AGG('"' +FieldName +'": "' +CASE WHEN UpdateType = 'I' THEN NewValue 
																				WHEN UpdateType = 'C' THEN CAST(CAST(NewValue as decimal(30,5)) - CAST(OldValue as decimal(30,5)) as nvarchar)
																				WHEN UpdateType = 'D' THEN  CAST(CAST(OldValue as decimal(30,5)) - CAST(NewValue as decimal(30,5)) as nvarchar) 
																				ELSE OldValue 
																				END+'"',',') + '}' EntityData
							from dbo.TransactionDetails 
							where RefId = @Refid AND Status = 'R'
							GROUP BY Entity,UpdateType, EntityId, RefId ) a
							GROUP BY Entity,EntityId) a);

		DELETE dbo.TransactionDetails WHERE RefId =  @RefId AND Status = 'R';

		EXEC [dbo].[__UpdateMultiEntity]
						@JsonData = @EntityJson,
						@RefId = @RefId; 
    
		UPDATE dbo.[Transaction]
			SET IsReverse = 0
		WHERE RefId = @RefId;
    
		UPDATE dbo.[DepositHistory]
			SET DepositHistoryStatus = 'N'
		WHERE RefId = @RefId;

		UPDATE dbo.DepositAccountTrans
			SET TransactionStatus = 'N'
		WHERE TransactionNumber = @TransactionNumber;

		UPDATE dbo.DepositIFCBalanceTrans
			SET TransactionStatus = 'N'
		WHERE TransactionNumber = @TransactionNumber;

		UPDATE dbo.ODContractIFCBalanceTrans
			SET TransactionStatus = 'N'
		WHERE TransactionNumber = @TransactionNumber;

		UPDATE dbo.OverdraftContractTrans
			SET TransactionStatus = 'N'
		WHERE TransactionNumber = @TransactionNumber;

		UPDATE dbo.ProvisioningTrans
			SET TransactionStatus = 'N'
		WHERE TransactionNumber = @TransactionNumber;

		UPDATE dbo.DepositStatement
			SET StatementStatus = 'N'
		WHERE TransactionNumber = @TransactionNumber;

		UPDATE dbo.GLEntries
			SET TransactionStatus = 'N'
		WHERE TransactionNumber = @TransactionNumber;
	END;
END
GO
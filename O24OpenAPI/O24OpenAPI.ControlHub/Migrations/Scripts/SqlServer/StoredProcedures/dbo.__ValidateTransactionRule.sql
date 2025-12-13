USE o24cth
GO
IF OBJECT_ID('dbo.__ValidateTransactionRule', 'P ') IS NOT NULL DROP PROCEDURE dbo.__ValidateTransactionRule;
GO
CREATE PROCEDURE [dbo].[__ValidateTransactionRule](
    @WorkflowScheme nvarchar(max), 
    @IsReverse nvarchar(1) -- R: Reverse |N: Normal
)
AS
BEGIN    
    -- Lấy thông tin giao dịch
    DECLARE @TransactionCode varchar(50) = dbo.__GetTransactionCode(@WorkflowScheme);
    DECLARE @PathData nvarchar(1000) = '$.request.request_body.data';
    
    DECLARE @Sql NVARCHAR(MAX) = '';
    DECLARE @Params NVARCHAR(MAX);

    SELECT
        @Sql = STRING_AGG(
            'EXEC dbo.' + JSON_VALUE(FullClassName, '$.StoredProcedureName') + ' @WorkflowScheme, @IsReverse, @JsonParam =''' + [Parameter] + ''' ',
            ' '
        )
    FROM
        dbo.TransactionRules a
		inner join 
		dbo.RuleDefinition b on a.RuleName = b.RuleName
    WHERE
        WorkflowId = @TransactionCode
		AND JSON_PATH_EXISTS(b.FullClassName, '$.StoredProcedureName') = 1
        AND IsEnable = 1 
		AND dbo.__CheckCondition(@WorkflowScheme, JSON_QUERY(Condition)) = 1;
		
    -- Thực hiện truy vấn
    SET @Params = '@WorkflowScheme NVARCHAR(MAX), @IsReverse NVARCHAR(MAX)';
    
    EXEC sp_executesql @Sql,
        @Params,
        @WorkflowScheme,
        @IsReverse;
END
GO
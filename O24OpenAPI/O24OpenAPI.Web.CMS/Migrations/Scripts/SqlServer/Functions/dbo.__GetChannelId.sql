USE O24CMS
GO
IF OBJECT_ID('dbo.__GetChannelId', 'FN') IS NOT NULL DROP FUNCTION dbo.__GetChannelId;
GO
CREATE FUNCTION dbo.__GetChannelId(
    @WorkflowScheme nvarchar(max)
)
RETURNS nvarchar(10)
BEGIN
    RETURN JSON_VALUE(
        @WorkflowScheme,
        '$.Request.RequestHeader.channel_id'
    );
END
GO
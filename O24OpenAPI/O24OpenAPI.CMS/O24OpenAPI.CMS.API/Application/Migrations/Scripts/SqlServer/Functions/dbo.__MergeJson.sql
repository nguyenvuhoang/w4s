USE O24CMS
GO
IF OBJECT_ID('dbo.__MergeJson', 'FN') IS NOT NULL DROP FUNCTION dbo.__MergeJson;
GO
CREATE function [dbo].[__MergeJson]
(
    @a nvarchar(max),
    @b nvarchar(max)
)
returns nvarchar(max)
as
begin
    if left(@a, 1) = '{' and left(@b, 1) = '{' begin
        select @a =
            case
                when d.[type] in (4,5) then
                    json_modify(@a, concat('$.',d.[key]), json_query(d.[value]))
                when d.[type] in (3) then
                    json_modify(@a, concat('$.',d.[key]), cast(d.[value] as bit))
                when d.[type] in (2) and try_cast(d.[value] as int) = 1 then
                    json_modify(@a, concat('$.',d.[key]), cast(d.[value] as int))
                when d.[type] in (0) then
                    json_modify(json_modify(@a, concat('lax $.',d.[key]), 'null'), concat('strict $.',d.[key]), null)
                else
                    json_modify(@a, concat('$.',d.[key]), d.[value])
            end
        from openjson(@b) as d
    end else if left(@a, 1) = '[' and left(@b, 1) = '{' begin
        select @a = json_modify(@a, 'append $', json_query(@b))
    end else begin
        select @a = concat('[', @a, ',', right(@b, len(@b) - 1))
    end

    return @a
end
GO
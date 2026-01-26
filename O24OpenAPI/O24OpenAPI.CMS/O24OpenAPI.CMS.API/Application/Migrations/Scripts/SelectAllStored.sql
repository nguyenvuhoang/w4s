SELECT 
    SCHEMA_NAME(o.schema_id) COLLATE DATABASE_DEFAULT + '.' + o.[name] COLLATE DATABASE_DEFAULT AS [ObjectName],
    CASE o.[type]
        WHEN 'P' THEN 'Stored Procedure'
        WHEN 'FN' THEN 'Scalar Function'
        WHEN 'TF' THEN 'Table Function'
        WHEN 'IF' THEN 'Inline Table Function'
    END AS [ObjectType],
    'USE O24CMS'  + CHAR(13) + CHAR(10) +
    'GO' + CHAR(13) + CHAR(10) +
    'IF OBJECT_ID(''' + SCHEMA_NAME(o.schema_id) COLLATE DATABASE_DEFAULT + '.' + 
    o.[name] COLLATE DATABASE_DEFAULT + ''', ''' + o.[type] COLLATE DATABASE_DEFAULT + ''') IS NOT NULL ' +
    'DROP ' +
    CASE o.[type]
        WHEN 'P' THEN 'PROCEDURE '
        WHEN 'FN' THEN 'FUNCTION '
        WHEN 'TF' THEN 'FUNCTION '
        WHEN 'IF' THEN 'FUNCTION '
    END +
    SCHEMA_NAME(o.schema_id) COLLATE DATABASE_DEFAULT + '.' + 
    o.[name] COLLATE DATABASE_DEFAULT + ';' + CHAR(13) + CHAR(10) +
    'GO' + CHAR(13) + CHAR(10) +
    m.[definition] COLLATE DATABASE_DEFAULT + CHAR(13) + CHAR(10) +
    'GO' AS [Script]
FROM 
    sys.sql_modules m
INNER JOIN 
    sys.objects o ON m.object_id = o.object_id
WHERE 
    o.[type] IN ('P', 'FN', 'TF', 'IF')
ORDER BY 
    o.[type], o.[name];
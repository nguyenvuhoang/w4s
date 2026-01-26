-- o24cms
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'o24cms')
BEGIN
    CREATE DATABASE [o24cms] CONTAINMENT = NONE;
END
GO

USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.sql_logins WHERE name = 'o24cms')
BEGIN
    CREATE LOGIN [o24cms] WITH PASSWORD = N'o24cms', 
        DEFAULT_DATABASE = [o24cms], 
        DEFAULT_LANGUAGE = [us_english], 
        CHECK_EXPIRATION = OFF, 
        CHECK_POLICY = OFF;
END
GO

USE o24cms;
GO

IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = 'o24cms')
BEGIN
    CREATE USER [o24cms] FOR LOGIN [o24cms];
END
GO

USE master;
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.server_permissions p
    JOIN sys.server_principals sp ON p.grantee_principal_id = sp.principal_id
    WHERE sp.name = 'o24cms' AND p.permission_name = 'CONTROL SERVER'
)
BEGIN
    GRANT CONTROL SERVER TO [o24cms];
END
GO
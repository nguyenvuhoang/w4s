-- o24ext
USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'o24ext')
BEGIN
    CREATE DATABASE [o24ext] CONTAINMENT = NONE;
END
GO

USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.sql_logins WHERE name = 'o24ext')
BEGIN
    CREATE LOGIN [o24ext] WITH PASSWORD = N'o24ext', 
        DEFAULT_DATABASE = [o24ext], 
        DEFAULT_LANGUAGE = [us_english], 
        CHECK_EXPIRATION = OFF, 
        CHECK_POLICY = OFF;
END
GO

USE o24ext;
GO

IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = 'o24ext')
BEGIN
    CREATE USER [o24ext] FOR LOGIN [o24ext];
END
GO

USE master;
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.server_permissions p
    JOIN sys.server_principals sp ON p.grantee_principal_id = sp.principal_id
    WHERE sp.name = 'o24ext' AND p.permission_name = 'CONTROL SERVER'
)
BEGIN
    GRANT CONTROL SERVER TO [o24ext];
END
GO

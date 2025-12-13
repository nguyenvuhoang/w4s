-- o24ai
USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'o24ai')
BEGIN
    CREATE DATABASE [o24ai] CONTAINMENT = NONE;
END
GO

USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.sql_logins WHERE name = 'o24ai')
BEGIN
    CREATE LOGIN [o24ai] WITH PASSWORD = N'o24ai', 
        DEFAULT_DATABASE = [o24ai], 
        DEFAULT_LANGUAGE = [us_english], 
        CHECK_EXPIRATION = OFF, 
        CHECK_POLICY = OFF;
END
GO

USE o24ai;
GO

IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = 'o24ai')
BEGIN
    CREATE USER [o24ai] FOR LOGIN [o24ai];
END
GO

USE master;
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.server_permissions p
    JOIN sys.server_principals sp ON p.grantee_principal_id = sp.principal_id
    WHERE sp.name = 'o24ai' AND p.permission_name = 'CONTROL SERVER'
)
BEGIN
    GRANT CONTROL SERVER TO [o24ai];
END
GO

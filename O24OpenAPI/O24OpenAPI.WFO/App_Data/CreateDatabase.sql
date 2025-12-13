-- o24wfo
USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'o24wfo')
BEGIN
    CREATE DATABASE [o24wfo] CONTAINMENT = NONE;
END
GO

USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.sql_logins WHERE name = 'o24wfo')
BEGIN
    CREATE LOGIN [o24wfo] WITH PASSWORD = N'o24wfo', 
        DEFAULT_DATABASE = [o24wfo], 
        DEFAULT_LANGUAGE = [us_english], 
        CHECK_EXPIRATION = OFF, 
        CHECK_POLICY = OFF;
END
GO

USE o24wfo;
GO

IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = 'o24wfo')
BEGIN
    CREATE USER [o24wfo] FOR LOGIN [o24wfo];
END
GO

USE master;
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.server_permissions p
    JOIN sys.server_principals sp ON p.grantee_principal_id = sp.principal_id
    WHERE sp.name = 'o24wfo' AND p.permission_name = 'CONTROL SERVER'
)
BEGIN
    GRANT CONTROL SERVER TO [o24wfo];
END
GO

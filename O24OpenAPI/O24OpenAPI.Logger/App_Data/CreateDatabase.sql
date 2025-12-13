-- o24logger
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'o24logger')
BEGIN
    CREATE DATABASE [o24logger] CONTAINMENT = NONE;
END
GO

USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.sql_logins WHERE name = 'o24logger')
BEGIN
    CREATE LOGIN [o24logger] WITH PASSWORD = N'o24logger', 
        DEFAULT_DATABASE = [o24logger], 
        DEFAULT_LANGUAGE = [us_english], 
        CHECK_EXPIRATION = OFF, 
        CHECK_POLICY = OFF;
END
GO

USE o24logger;
GO

IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = 'o24logger')
BEGIN
    CREATE USER [o24logger] FOR LOGIN [o24logger];
END
GO

USE master;
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.server_permissions p
    JOIN sys.server_principals sp ON p.grantee_principal_id = sp.principal_id
    WHERE sp.name = 'o24logger' AND p.permission_name = 'CONTROL SERVER'
)
BEGIN
    GRANT CONTROL SERVER TO [o24logger];
END
GO
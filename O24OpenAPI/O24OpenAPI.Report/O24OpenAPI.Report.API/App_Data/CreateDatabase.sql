-- o24report
USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'o24report')
BEGIN
    CREATE DATABASE o24report CONTAINMENT = NONE;
END
GO

USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.sql_logins WHERE name = 'o24report')
BEGIN
    CREATE LOGIN o24report WITH PASSWORD = N'o24report', 
        DEFAULT_DATABASE = o24report, 
        DEFAULT_LANGUAGE = [us_english], 
        CHECK_EXPIRATION = OFF, 
        CHECK_POLICY = OFF;
END
GO

USE o24report;
GO

IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = 'o24report')
BEGIN
    CREATE USER o24report FOR LOGIN o24report;
END
GO

USE master;
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.server_permissions p
    JOIN sys.server_principals sp ON p.grantee_principal_id = sp.principal_id
    WHERE sp.name = 'o24report' AND p.permission_name = 'CONTROL SERVER'
)
BEGIN
    GRANT CONTROL SERVER TO o24report;
END
GO

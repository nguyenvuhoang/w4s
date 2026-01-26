-- o24pmt
USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'o24pmt')
BEGIN
    CREATE DATABASE [o24pmt] CONTAINMENT = NONE;
END
GO

USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.sql_logins WHERE name = 'o24pmt')
BEGIN
    CREATE LOGIN [o24pmt] WITH PASSWORD = N'o24pmt', 
        DEFAULT_DATABASE = [o24pmt], 
        DEFAULT_LANGUAGE = [us_english], 
        CHECK_EXPIRATION = OFF, 
        CHECK_POLICY = OFF;
END
GO

USE o24pmt;
GO

IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = 'o24pmt')
BEGIN
    CREATE USER [o24pmt] FOR LOGIN [o24pmt];
END
GO

USE master;
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.server_permissions p
    JOIN sys.server_principals sp ON p.grantee_principal_id = sp.principal_id
    WHERE sp.name = 'o24pmt' AND p.permission_name = 'CONTROL SERVER'
)
BEGIN
    GRANT CONTROL SERVER TO [o24pmt];
END
GO

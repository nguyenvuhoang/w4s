-- o24cth
USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'o24cth')
BEGIN
    CREATE DATABASE [o24cth] CONTAINMENT = NONE;
END
GO

USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.sql_logins WHERE name = 'o24cth')
BEGIN
    CREATE LOGIN [o24cth] WITH PASSWORD = N'o24cth', 
        DEFAULT_DATABASE = [o24cth], 
        DEFAULT_LANGUAGE = [us_english], 
        CHECK_EXPIRATION = OFF, 
        CHECK_POLICY = OFF;
END
GO

USE o24cth;
GO

IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = 'o24cth')
BEGIN
    CREATE USER [o24cth] FOR LOGIN [o24cth];
END
GO

USE master;
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.server_permissions p
    JOIN sys.server_principals sp ON p.grantee_principal_id = sp.principal_id
    WHERE sp.name = 'o24cth' AND p.permission_name = 'CONTROL SERVER'
)
BEGIN
    GRANT CONTROL SERVER TO [o24cth];
END
GO

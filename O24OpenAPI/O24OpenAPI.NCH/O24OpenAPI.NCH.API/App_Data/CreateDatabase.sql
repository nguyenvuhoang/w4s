-- o24nch
USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'o24nch')
BEGIN
    CREATE DATABASE [o24nch] CONTAINMENT = NONE;
END
GO

USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.sql_logins WHERE name = 'o24nch')
BEGIN
    CREATE LOGIN [o24nch] WITH PASSWORD = N'o24nch', 
        DEFAULT_DATABASE = [o24nch], 
        DEFAULT_LANGUAGE = [us_english], 
        CHECK_EXPIRATION = OFF, 
        CHECK_POLICY = OFF;
END
GO

USE o24nch;
GO

IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = 'o24nch')
BEGIN
    CREATE USER [o24nch] FOR LOGIN [o24nch];
END
GO

USE master;
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.server_permissions p
    JOIN sys.server_principals sp ON p.grantee_principal_id = sp.principal_id
    WHERE sp.name = 'o24nch' AND p.permission_name = 'CONTROL SERVER'
)
BEGIN
    GRANT CONTROL SERVER TO [o24nch];
END
GO

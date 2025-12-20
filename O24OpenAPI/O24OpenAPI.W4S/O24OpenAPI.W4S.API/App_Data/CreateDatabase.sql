-- o24w4s
USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'o24w4s')
BEGIN
    CREATE DATABASE [o24w4s] CONTAINMENT = NONE;
END
GO

USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.sql_logins WHERE name = 'o24w4s')
BEGIN
    CREATE LOGIN [o24w4s] WITH PASSWORD = N'o24w4s', 
        DEFAULT_DATABASE = [o24w4s], 
        DEFAULT_LANGUAGE = [us_english], 
        CHECK_EXPIRATION = OFF, 
        CHECK_POLICY = OFF;
END
GO

USE o24w4s;
GO

IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = 'o24w4s')
BEGIN
    CREATE USER [o24w4s] FOR LOGIN [o24w4s];
END
GO

USE master;
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.server_permissions p
    JOIN sys.server_principals sp ON p.grantee_principal_id = sp.principal_id
    WHERE sp.name = 'o24w4s' AND p.permission_name = 'CONTROL SERVER'
)
BEGIN
    GRANT CONTROL SERVER TO [o24w4s];
END
GO
